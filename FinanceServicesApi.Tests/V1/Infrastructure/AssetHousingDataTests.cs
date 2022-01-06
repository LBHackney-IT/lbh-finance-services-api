using System.Threading.Tasks;
using Hackney.Shared.Asset.Domain;
using Xunit;

namespace FinanceServicesApi.Tests.V1.Infrastructure
{
    public class AssetHousingDataTests : HousingDataTests<Asset>
    {
        [Fact]
        public override void DownloadAsyncWithEmptyIdThrowsArgumentException()
        {
            base.DownloadAsyncWithEmptyIdThrowsArgumentException();
        }

        [Fact]
        public override void DownloadAsyncWithNonReachableApiThrowsException()
        {
            base.DownloadAsyncWithNonReachableApiThrowsException();
        }

        [Fact]
        public override Task DownloadAsyncWithNonExistenceIdReturnsNull()
        {
            return base.DownloadAsyncWithNonExistenceIdReturnsNull();
        }

        [Fact]
        public override void DownloadAsyncWithExistenceIdReturnsValidData()
        {
            base.DownloadAsyncWithExistenceIdReturnsValidData();
        }

        [Fact]
        public override void DownloadAsyncWithNonReachableUrlThrowsException()
        {
            base.DownloadAsyncWithNonReachableUrlThrowsException();
        }

        [Fact]
        public override void DownloadAsyncWithoutAuthorizationThrowsInvalidCredentialException()
        {
            base.DownloadAsyncWithoutAuthorizationThrowsInvalidCredentialException();
        }
    }
}
