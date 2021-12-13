using System;
using FinanceServicesApi.V1.Infrastructure.Enums;
using FinanceServicesApi.V1.Infrastructure.Interfaces;

namespace FinanceServicesApi.V1.Infrastructure
{
    public class GetHousingSearchEnvironmentVariables : IGetEnvironmentVariables
    {
        public string GetUrl(SearchBy searchBy)
        {
            string result = Environment.GetEnvironmentVariable("SEARCH_API_URL") ?? string.Empty; ;
            if (string.IsNullOrEmpty(result))
                throw new Exception("Search api url shouldn't be null or empty");
            switch (searchBy)
            {
                case SearchBy.ByAccount:
                    return result + $"/search/accounts";
                case SearchBy.ByAsset:
                    return result + $"/search/assets";
                case SearchBy.ByCharge:
                    return result + $"/search/charges";
                case SearchBy.ByPerson:
                    return result + $"/search/persons";
                case SearchBy.ByTenure:
                    return result + $"/search/tenures";
                case SearchBy.ByTransaction:
                    return result + $"/search/transactions";
                default:
                    throw new ArgumentNullException($"{nameof(searchBy).ToString()} is not valid");
            }
        }

        public string GetToken()
        {
            string result = Environment.GetEnvironmentVariable("PERSON_API_TOKEN") ?? string.Empty;
            if (string.IsNullOrEmpty(result))
                throw new Exception("Person api token shouldn't be null or empty.");
            return result;
        }

    }
}
