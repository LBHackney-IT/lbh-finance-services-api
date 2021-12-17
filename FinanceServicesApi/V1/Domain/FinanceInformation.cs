using System.Collections.Generic;
using FinanceServicesApi.V1.Domain.Charges;
using FinanceServicesApi.V1.Domain.TransactionModels;
using Hackney.Shared.Tenure.Domain;

namespace FinanceServicesApi.V1.Domain
{
    public class FinanceInformation
    {
        public List<TenureInformation> Tenures { get; set; }
        public List<Charge> Charges { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}
