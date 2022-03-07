using FinanceServicesApi.V1.Boundary.Request;
using FinanceServicesApi.V1.Boundary.Responses;
using FinanceServicesApi.V1.Domain.AccountModels;
using FinanceServicesApi.V1.Gateways.Interfaces;
using FinanceServicesApi.V1.UseCase.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace FinanceServicesApi.V1.UseCase
{
    public class GetPersonListUseCase : IGetPersonListUseCase
    {
        private readonly IHousingSearchGateway _housingSearchGateway;
        private readonly IGetAccountByTargetIdUseCase _getAccountByTargetIdUseCase;

        public GetPersonListUseCase(IHousingSearchGateway housingSearchGateway, IGetAccountByTargetIdUseCase getAccountByTargetIdUseCase)
        {
            _housingSearchGateway = housingSearchGateway;
            _getAccountByTargetIdUseCase = getAccountByTargetIdUseCase;
        }

        public async Task<GetPersonListResponse> ExecuteAsync(GetPersonListRequest housingSearchRequest)
        {
            var personsList = await _housingSearchGateway.GetPersons(housingSearchRequest).ConfigureAwait(false);
            if (personsList.Persons.Count == 0)
            {
                return personsList;
            }

            var tenuresToLoad = personsList.Persons.SelectMany(p => p.Tenures).Select(t => Guid.Parse(t.Id)).ToList();

            var accounts = await GetAccountListFromApi(tenuresToLoad).ConfigureAwait(false);

            personsList.Persons.SelectMany(p => p.Tenures)
                    .Join(accounts,
                          tenure => Guid.Parse(tenure.Id),
                          account => account.TargetId,
                          (tenure, account) => new { Tenure = tenure, Account = account })
                    .ToList()
                    .ForEach(pair =>
                    {
                        pair.Tenure.TotalBalance = pair.Account.AccountBalance;
                    });

            personsList.Persons.ForEach(person =>
            {
                person.TotalBalance = person.Tenures.Sum(_ => _.TotalBalance);
            });

            return personsList;
        }

        private async Task<List<Account>> GetAccountListFromApi(List<Guid> tenureIds)
        {
            ConcurrentBag<Account> accounts = new ConcurrentBag<Account>();

            var degree = Convert.ToInt32(Math.Ceiling(Environment.ProcessorCount * 0.75 * 2.0));
            var block = new ActionBlock<Guid>(
                    async tenureId =>
                    {
                        var account = await _getAccountByTargetIdUseCase.ExecuteAsync(tenureId).ConfigureAwait(false);

                        if (account != null)
                        {
                            accounts.Add(account);
                        }
                    },
                    new ExecutionDataflowBlockOptions
                    {
                        MaxDegreeOfParallelism = degree, // Parallelize on all cores
                    });

            foreach (var id in tenureIds)
            {
                block.Post(id);
            }

            block.Complete();
            await block.Completion.ConfigureAwait(false);

            return accounts.ToList();
        }

    }
}
