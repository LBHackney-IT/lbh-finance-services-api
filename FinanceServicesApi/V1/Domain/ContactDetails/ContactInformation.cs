using Amazon.DynamoDBv2.DataModel;
using FinanceServicesApi.V1.Infrastructure.Enums;
using Hackney.Core.DynamoDb.Converters;

namespace FinanceServicesApi.V1.Domain.ContactDetails
{
    public class ContactInformation
    {
        public ContactType ContactType { get; set; }

        public SubType? SubType { get; set; }

        public string Value { get; set; }

        public string Description { get; set; }

        public AddressExtended AddressExtended { get; set; }
    }
}
