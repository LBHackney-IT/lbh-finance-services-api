using FinanceServicesApi.V1.Gateways.Interfaces;
using Moq;
using System;

namespace FinanceServicesApi.Tests.Extensions
{
    public static class MockExtensions
    {
        public static void VerifyGetAssetById(this Mock<IAssetGateway> assetGateway, Times times)
        {
            assetGateway.Verify(pr => pr.GetById(It.IsAny<Guid>()), times);
        }

        public static void VerifyGetChargesByAssetId(this Mock<IChargesGateway> chargesGateway, Times times)
        {
            chargesGateway.Verify(pr => pr.GetAllByAssetId(It.IsAny<Guid>()), times);
        }

        public static void VerifyGetTenureById(this Mock<ITenureInformationGateway> tenureInfoGateway, Times times)
        {
            tenureInfoGateway.Verify(pr => pr.GetById(It.IsAny<Guid>()), times);
        }
    }
}
