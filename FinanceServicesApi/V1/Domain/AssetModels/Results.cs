using Hackney.Shared.Asset.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceServicesApi.V1.Domain.AssetModels
{
    public class Results
    {
        public List<Asset> Assets { get; set; }
    }
}
