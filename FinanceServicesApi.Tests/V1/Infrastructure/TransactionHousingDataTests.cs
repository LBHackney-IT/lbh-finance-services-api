using FinanceServicesApi.V1.Domain.TransactionModels;
using Xunit;

namespace FinanceServicesApi.Tests.V1.Infrastructure
{
    public class TransactionHousingDataTests : HousingDataTests<Transaction>
    {
        [Fact]
        public override void DownloadAsyncWithNonExistenceIdThrowsException()
        {
            base.DownloadAsyncWithNonExistenceIdThrowsException();
        }

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
    }
}
