using System.Collections.Generic;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Domain.ContactDetails;
using Xunit;

namespace FinanceServicesApi.Tests.V1.Infrastructure
{
    public class ContactDetailsHousingDataTests : HousingDataTests<List<ContactDetail>>
    {
        [Fact]
        public override void DownloadAsyncWithoutAuthorizationThrowsInvalidCredentialException()
        {
            base.DownloadAsyncWithoutAuthorizationThrowsInvalidCredentialException();
        }

        [Fact]
        public override void DownloadAsyncWithNonReachableUrlThrowsException()
        {
            base.DownloadAsyncWithNonReachableUrlThrowsException();
        }

        [Fact]
        public override void DownloadAsyncWithExistenceIdReturnsValidData()
        {
            base.DownloadAsyncWithExistenceIdReturnsValidData();
        }

        [Fact]
        public override void DownloadAsyncWithEmptyIdThrowsArgumentException()
        {
            base.DownloadAsyncWithEmptyIdThrowsArgumentException();
        }

        [Fact]
        public override Task DownloadAsyncWithNonExistenceIdReturnsNull()
        {
            return base.DownloadAsyncWithNonExistenceIdReturnsNull();
        }

        [Fact]
        public override void DownloadAsyncWithNonReachableApiThrowsException()
        {
            base.DownloadAsyncWithNonReachableApiThrowsException();
        }

        [Fact]
        public override void DownloadAsyncWithApiExceptionReturnsException()
        {
            base.DownloadAsyncWithApiExceptionReturnsException();
        }
    }
}
