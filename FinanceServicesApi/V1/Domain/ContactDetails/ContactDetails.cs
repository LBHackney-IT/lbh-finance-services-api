using System;

namespace FinanceServicesApi.V1.Domain.ContactDetails
{
    public class ContactDetail
    {
        public Guid Id { get; set; }

        public Guid TargetId { get; set; }

        public TargetType TargetType { get; set; }

        public ContactInformation ContactInformation { get; set; }

        public SourceServiceArea SourceServiceArea { get; set; }

        public DateTime? RecordValidUntil { get; set; }

        public bool IsActive { get; set; }

        public CreatedBy CreatedBy { get; set; }

        public DateTime? LastModified { get; set; }
    }
}
