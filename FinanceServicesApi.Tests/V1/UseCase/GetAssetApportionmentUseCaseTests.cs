using AutoFixture;
using FinanceServicesApi.V1.Boundary.Responses.PropertySummary;
using FinanceServicesApi.V1.Domain.Charges;
using FinanceServicesApi.V1.Infrastructure.Enums;
using FinanceServicesApi.V1.UseCase;
using FinanceServicesApi.V1.UseCase.Interfaces;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace FinanceServicesApi.Tests.V1.UseCase
{
    public class GetAssetApportionmentUseCaseTests
    {
        private Mock<IGetChargeByAssetIdUseCase> _chargeUseCase;
        private Fixture _fixture;

        private GetAssetApportionmentUseCase _sut { get; set; }

        public GetAssetApportionmentUseCaseTests()
        {
            _chargeUseCase = new Mock<IGetChargeByAssetIdUseCase>();
            _fixture = new Fixture();

            _sut = new GetAssetApportionmentUseCase(_chargeUseCase.Object);
        }

        [Fact]
        public async Task ChargesGatewayThrowsShouldRethrow()
        {
            var expectedException = new Exception("Some exception message");
            _chargeUseCase.Setup(_ => _.ExecuteAsync(It.IsAny<Guid>()))
                .ThrowsAsync(expectedException);

            Func<Task> action = () => _sut.ExecuteAsync(Guid.NewGuid(), 2022);

            var actualException = await Assert.ThrowsAsync<Exception>(action).ConfigureAwait(false);
            actualException.Should().BeEquivalentTo(expectedException);
        }

        [Fact]
        public async Task ChargesGatewayReturnsNullShouldThrow()
        {
            Guid assetId = Guid.NewGuid();
            var expectedException = new ArgumentException($"No charges was loaded from Charges API for asset id: [{assetId}]");
            _chargeUseCase.Setup(_ => _.ExecuteAsync(It.IsAny<Guid>()))
                .ReturnsAsync((List<Charge>) null);

            Func<Task> action = () => _sut.ExecuteAsync(assetId, 2022);

            var actualException = await Assert.ThrowsAsync<ArgumentException>(action).ConfigureAwait(false);
            actualException.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public async Task ChargesGatewayReturnsZeroItemsShouldThrow()
        {
            Guid assetId = Guid.NewGuid();
            var expectedException = new ArgumentException($"No charges was loaded from Charges API for asset id: [{assetId}]");
            _chargeUseCase.Setup(_ => _.ExecuteAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new List<Charge>());

            Func<Task> action = () => _sut.ExecuteAsync(assetId, 2022);

            var actualException = await Assert.ThrowsAsync<ArgumentException>(action).ConfigureAwait(false);
            actualException.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public async Task GatewatReturnsChargesCalculateApportionmentFrom2019()
        {
            Guid assetId = Guid.NewGuid();
            List<Charge> charges = new List<Charge>()
            {
                CreateCharge(DateTime.Now.Year - 3, ChargeSubGroup.Estimate, ChargeType.Estate, "Estate Cleaning", 100),
                CreateCharge(DateTime.Now.Year - 3, ChargeSubGroup.Actual, ChargeType.Estate, "Estate Cleaning", 100),
                CreateCharge(DateTime.Now.Year - 3, ChargeSubGroup.Actual, ChargeType.Estate, "Estate Cleaning", 200),
                CreateCharge(DateTime.Now.Year - 1, ChargeSubGroup.Estimate, ChargeType.Estate, "Estate Cleaning", 300),
                CreateCharge(DateTime.Now.Year - 1, ChargeSubGroup.Estimate, ChargeType.Estate, "Estate Cleaning", 50),
                CreateCharge(DateTime.Now.Year, ChargeSubGroup.Estimate, ChargeType.Estate, "Estate Cleaning", 800),
                CreateCharge(DateTime.Now.Year - 2, ChargeSubGroup.Actual, ChargeType.Estate, "Grounds Maintenance", 50),
                CreateCharge(DateTime.Now.Year, ChargeSubGroup.Actual, ChargeType.Estate, "Grounds Maintenance", 50),
                CreateCharge(DateTime.Now.Year, ChargeSubGroup.Estimate, ChargeType.Estate, "Grounds Maintenance", 300),
                CreateCharge(DateTime.Now.Year, ChargeSubGroup.Estimate, ChargeType.Estate, "Grounds Maintenance", 150),
                CreateCharge(DateTime.Now.Year - 3, ChargeSubGroup.Estimate, ChargeType.Estate, "Ground rent", 300),
                CreateCharge(DateTime.Now.Year - 3, ChargeSubGroup.Actual, ChargeType.Estate, "Ground rent", 300),

                CreateCharge(DateTime.Now.Year - 3, ChargeSubGroup.Estimate, ChargeType.Block, "Block Cleaning", 100),
                CreateCharge(DateTime.Now.Year - 4, ChargeSubGroup.Actual, ChargeType.Block, "Block Cleaning", 200),
                CreateCharge(DateTime.Now.Year - 3, ChargeSubGroup.Actual, ChargeType.Block, "Block Cleaning", 100),
                CreateCharge(DateTime.Now.Year - 1, ChargeSubGroup.Estimate, ChargeType.Block, "Grounds Maintenance", 50),
                CreateCharge(DateTime.Now.Year - 1, ChargeSubGroup.Estimate, ChargeType.Block, "Grounds Maintenance", 50),
                CreateCharge(DateTime.Now.Year, ChargeSubGroup.Estimate, ChargeType.Block, "Heating Fuel", 500),
                CreateCharge(DateTime.Now.Year, ChargeSubGroup.Estimate, ChargeType.Block, "Heating Fuel", 100),
                CreateCharge(DateTime.Now.Year, ChargeSubGroup.Estimate, ChargeType.Block, "Heating Fuel", 50),
                CreateCharge(DateTime.Now.Year, ChargeSubGroup.Actual, ChargeType.Block, "Heating Fuel", 50),

                CreateCharge(DateTime.Now.Year - 3, ChargeSubGroup.Estimate, ChargeType.Property, "Building insurance", 100),
                CreateCharge(DateTime.Now.Year - 3, ChargeSubGroup.Actual, ChargeType.Property, "Building insurance", 100),
                CreateCharge(DateTime.Now.Year - 3, ChargeSubGroup.Actual, ChargeType.Property, "Building insurance", 50),
                CreateCharge(DateTime.Now.Year - 1, ChargeSubGroup.Estimate, ChargeType.Property, "Building insurance", 200),
                CreateCharge(DateTime.Now.Year - 1, ChargeSubGroup.Estimate, ChargeType.Property, "Building insurance", 100),
                CreateCharge(DateTime.Now.Year - 1, ChargeSubGroup.Actual, ChargeType.Property, "Building insurance", 100),
                CreateCharge(DateTime.Now.Year, ChargeSubGroup.Estimate, ChargeType.Property, "Building insurance", 100),
                CreateCharge(DateTime.Now.Year, ChargeSubGroup.Estimate, ChargeType.Property, "Building insurance", 50),
                CreateCharge(DateTime.Now.Year, ChargeSubGroup.Actual, ChargeType.Property, "Building insurance", 50),
                CreateCharge(DateTime.Now.Year - 3, ChargeSubGroup.Actual, ChargeType.Property, "Management Fee", 200),
                CreateCharge(DateTime.Now.Year - 4, ChargeSubGroup.Actual, ChargeType.Property, "Management Fee", 200),
                CreateCharge(DateTime.Now.Year - 2, ChargeSubGroup.Actual, ChargeType.Property, "Management Fee", 200),
                CreateCharge(DateTime.Now.Year - 2, ChargeSubGroup.Actual, ChargeType.Property, "Management Fee", 800),
                CreateCharge(DateTime.Now.Year - 2, ChargeSubGroup.Estimate, ChargeType.Property, "Management Fee", 200),
                CreateCharge(DateTime.Now.Year - 1, ChargeSubGroup.Actual, ChargeType.Property, "Management Fee", 200),
                CreateCharge(DateTime.Now.Year - 1, ChargeSubGroup.Estimate, ChargeType.Property, "Management Fee", 150),
                CreateCharge(DateTime.Now.Year - 1, ChargeSubGroup.Estimate, ChargeType.Property, "Management Fee", 300),
                CreateCharge(DateTime.Now.Year, ChargeSubGroup.Estimate, ChargeType.Property, "Management Fee", 300),
                CreateCharge(DateTime.Now.Year, ChargeSubGroup.Estimate, ChargeType.Property, "Management Fee", 50),
                CreateCharge(DateTime.Now.Year, ChargeSubGroup.Actual, ChargeType.Property, "Management Fee", 50),
            };

            AssetApportionmentResponse expectedResponse = new AssetApportionmentResponse()
            {
                AssetId = assetId,
                EstateCosts = new List<PropertyCostTotals>
                {
                    new PropertyCostTotals()
                    {
                        ChargeGroup = "Estate Cleaning",
                        Totals = new List<ChargesTotalResponse>
                        {
                            CreateTotal(800, DateTime.Now.Year, ChargeSubGroup.Estimate),
                            CreateTotal(350, DateTime.Now.Year - 1, ChargeSubGroup.Estimate),
                            CreateTotal(0, DateTime.Now.Year - 2, ChargeSubGroup.Actual),
                            CreateTotal(300, DateTime.Now.Year - 3, ChargeSubGroup.Actual)
                        }
                    },
                    new PropertyCostTotals()
                    {
                        ChargeGroup = "Grounds Maintenance",
                        Totals = new List<ChargesTotalResponse>
                        {
                            CreateTotal(450, DateTime.Now.Year, ChargeSubGroup.Estimate),
                            CreateTotal(0, DateTime.Now.Year - 1, ChargeSubGroup.Estimate),
                            CreateTotal(50, DateTime.Now.Year - 2, ChargeSubGroup.Actual),
                            CreateTotal(0, DateTime.Now.Year - 3, ChargeSubGroup.Actual)
                        }
                    },
                    new PropertyCostTotals()
                    {
                        ChargeGroup = "Ground rent",
                        Totals = new List<ChargesTotalResponse>
                        {
                            CreateTotal(0, DateTime.Now.Year, ChargeSubGroup.Estimate),
                            CreateTotal(0, DateTime.Now.Year - 1, ChargeSubGroup.Estimate),
                            CreateTotal(0, DateTime.Now.Year - 2, ChargeSubGroup.Actual),
                            CreateTotal(300, DateTime.Now.Year - 3, ChargeSubGroup.Actual)
                        }
                    }
                },
                EstateCostTotal = new List<ChargesTotalResponse>
                {
                    CreateTotal(1250, DateTime.Now.Year, ChargeSubGroup.Estimate),
                    CreateTotal(350, DateTime.Now.Year - 1, ChargeSubGroup.Estimate),
                    CreateTotal(50, DateTime.Now.Year - 2, ChargeSubGroup.Actual),
                    CreateTotal(600, DateTime.Now.Year - 3, ChargeSubGroup.Actual)
                },
                BlockCosts = new List<PropertyCostTotals>
                {
                    new PropertyCostTotals()
                    {
                        ChargeGroup = "Block Cleaning",
                        Totals = new List<ChargesTotalResponse>()
                        {
                            CreateTotal(0, DateTime.Now.Year, ChargeSubGroup.Estimate),
                            CreateTotal(0, DateTime.Now.Year - 1, ChargeSubGroup.Estimate),
                            CreateTotal(0, DateTime.Now.Year - 2, ChargeSubGroup.Actual),
                            CreateTotal(100, DateTime.Now.Year - 3, ChargeSubGroup.Actual)
                        }
                    },
                    new PropertyCostTotals()
                    {
                        ChargeGroup = "Grounds Maintenance",
                        Totals = new List<ChargesTotalResponse>()
                        {
                            CreateTotal(0, DateTime.Now.Year, ChargeSubGroup.Estimate),
                            CreateTotal(100, DateTime.Now.Year - 1, ChargeSubGroup.Estimate),
                            CreateTotal(0, DateTime.Now.Year - 2, ChargeSubGroup.Actual),
                            CreateTotal(0, DateTime.Now.Year - 3, ChargeSubGroup.Actual)
                        }
                    },
                    new PropertyCostTotals()
                    {
                        ChargeGroup = "Heating Fuel",
                        Totals = new List<ChargesTotalResponse>()
                        {
                            CreateTotal(650, DateTime.Now.Year, ChargeSubGroup.Estimate),
                            CreateTotal(0, DateTime.Now.Year - 1, ChargeSubGroup.Estimate),
                            CreateTotal(0, DateTime.Now.Year - 2, ChargeSubGroup.Actual),
                            CreateTotal(0, DateTime.Now.Year - 3, ChargeSubGroup.Actual)
                        }
                    }
                    //new PropertyCostTotals()
                    //{
                    //    ChargeGroup = "",
                    //    Totals = new List<ChargesTotalResponse>()
                    //    {
                    //        CreateTotal(0, DateTime.Now.Year, ChargeSubGroup.Estimate),
                    //        CreateTotal(0, DateTime.Now.Year - 1, ChargeSubGroup.Estimate),
                    //        CreateTotal(0, DateTime.Now.Year - 2, ChargeSubGroup.Actual),
                    //        CreateTotal(0, DateTime.Now.Year - 3, ChargeSubGroup.Actual)
                    //    }
                    //}
                },
                BlockCostTotal = new List<ChargesTotalResponse>
                {
                    CreateTotal(650, DateTime.Now.Year, ChargeSubGroup.Estimate),
                    CreateTotal(100, DateTime.Now.Year - 1, ChargeSubGroup.Estimate),
                    CreateTotal(0, DateTime.Now.Year - 2, ChargeSubGroup.Actual),
                    CreateTotal(100, DateTime.Now.Year - 3, ChargeSubGroup.Actual)
                },
                PropertyCosts = new List<PropertyCostTotals>
                {
                    new PropertyCostTotals
                    {
                        ChargeGroup = "Building insurance",
                        Totals = new List<ChargesTotalResponse>()
                        {
                            CreateTotal(150, DateTime.Now.Year, ChargeSubGroup.Estimate),
                            CreateTotal(300, DateTime.Now.Year - 1, ChargeSubGroup.Estimate),
                            CreateTotal(0, DateTime.Now.Year - 2, ChargeSubGroup.Actual),
                            CreateTotal(150, DateTime.Now.Year - 3, ChargeSubGroup.Actual)
                        }
                    },
                    new PropertyCostTotals
                    {
                        ChargeGroup = "Management Fee",
                        Totals = new List<ChargesTotalResponse>()
                        {
                            CreateTotal(350, DateTime.Now.Year, ChargeSubGroup.Estimate),
                            CreateTotal(450, DateTime.Now.Year - 1, ChargeSubGroup.Estimate),
                            CreateTotal(1000, DateTime.Now.Year - 2, ChargeSubGroup.Actual),
                            CreateTotal(200, DateTime.Now.Year - 3, ChargeSubGroup.Actual)
                        }
                    }
                },
                PropertyCostTotal = new List<ChargesTotalResponse>
                {
                    CreateTotal(500, DateTime.Now.Year, ChargeSubGroup.Estimate),
                    CreateTotal(750, DateTime.Now.Year - 1, ChargeSubGroup.Estimate),
                    CreateTotal(1000, DateTime.Now.Year - 2, ChargeSubGroup.Actual),
                    CreateTotal(350, DateTime.Now.Year - 3, ChargeSubGroup.Actual)
                },
                BlockName = "Marcon Court",
                EstateName = "Estate 1"
            };
            _chargeUseCase.Setup(_ => _.ExecuteAsync(assetId))
                .ReturnsAsync(charges);

            var actualResponse = await _sut.ExecuteAsync(assetId, (short) (DateTime.Now.Year - 3)).ConfigureAwait(false);

            actualResponse.Should().NotBeNull();
            actualResponse.Should().BeEquivalentTo(expectedResponse);
        }

        private Charge CreateCharge(int year, ChargeSubGroup chargeSubGroup, ChargeType chargeType, string subType, decimal amount)
        {
            var detailerCharge = _fixture.Build<DetailedCharges>()
                .With(_ => _.SubType, subType)
                .With(_ => _.ChargeType, chargeType)
                .With(_ => _.Amount, amount)
                .CreateMany(1);

            return _fixture.Build<Charge>()
                .With(_ => _.ChargeYear, year)
                .With(_ => _.ChargeSubGroup, chargeSubGroup)
                .With(_ => _.DetailedCharges, detailerCharge)
                .Create();
        }

        private ChargesTotalResponse CreateTotal(decimal amount, int year, ChargeSubGroup chargeSubGroup)
        {
            return _fixture.Build<ChargesTotalResponse>()
                 .With(_ => _.Amount, amount)
                 .With(_ => _.Type, chargeSubGroup)
                 .With(_ => _.Year, year)
                 .Create();
        }
    }
}
