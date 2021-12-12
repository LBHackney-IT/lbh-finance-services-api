using System;
using System.Collections.Generic;
using FinanceServicesApi.V1.Infrastructure.Enums;

namespace FinanceServicesApi.V1.Domain.Charges
{
    public class Charge
    {
        public Guid Id { get; set; }
        public Guid TargetId { get; set; }
        public TargetType TargetType { get; set; }
        public ChargeGroup ChargeGroup { get; set; }
        public IEnumerable<DetailedCharges> DetailedCharges { get; set; }
    }
}
