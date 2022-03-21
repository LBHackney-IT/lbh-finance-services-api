using FinanceServicesApi.V1.Gateways.Interfaces;
using FinanceServicesApi.V1.UseCase;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FinanceServicesApi.Tests.Extensions;
using FinanceServicesApi.V1.Domain.Charges;
using FinanceServicesApi.V1.Infrastructure.Exceptions;
using FluentAssertions;
using Hackney.Shared.Asset.Domain;
using Microsoft.AspNetCore.Http;
using Xunit;
using Bogus;
using FinanceServicesApi.V1.Infrastructure.Enums;
using Hackney.Shared.Tenure.Domain;

namespace FinanceServicesApi.Tests.V1.UseCase
{
    public class GetYearlyRentDebitsUseCaseTests
    {
        private readonly Bogus.Faker _faker;
        private readonly Fixture _fixture;
        private readonly GetYearlyRentDebitsUseCase _useCase;
        private readonly Mock<IAssetGateway> _assetGateway;
        private readonly Mock<IChargesGateway> _chargesGateway;
        private readonly Mock<ITenureInformationGateway> _tenureInformationGateway;

        public GetYearlyRentDebitsUseCaseTests()
        {
            _faker = new Bogus.Faker();
            _fixture = new Fixture();
            _assetGateway = new Mock<IAssetGateway>();
            _chargesGateway = new Mock<IChargesGateway>();
            _tenureInformationGateway = new Mock<ITenureInformationGateway>();
            _useCase = new GetYearlyRentDebitsUseCase(_assetGateway.Object, _chargesGateway.Object,
                _tenureInformationGateway.Object);
        }

        [Fact]
        public void ShouldRaiseExceptionIfAssetIdIsEmpty()
        {
            var assetId = Guid.Empty;
            _useCase
                .Invoking(useCase => useCase.ExecuteAsync(assetId))
                .Should().Throw<ArgumentException>();
            _assetGateway.VerifyGetAssetById(Times.Never());
            _chargesGateway.VerifyGetChargesByAssetId(Times.Never());
            _tenureInformationGateway.VerifyGetTenureById(Times.Never());
        }

        [Fact]
        public void ShouldRaiseExceptionIfAssetDoesNotExist()
        {
            var assetId = Guid.NewGuid();
            _assetGateway.Setup(g => g.GetById(It.IsAny<Guid>())).ReturnsAsync((Asset) null);
            _useCase
                .Invoking(useCase => useCase.ExecuteAsync(assetId))
                .Should().Throw<AssetNotFoundException>();
            _assetGateway.VerifyGetAssetById(Times.Once());
            _chargesGateway.VerifyGetChargesByAssetId(Times.Never());
            _tenureInformationGateway.VerifyGetTenureById(Times.Never());
        }

        [Fact]
        public async Task ShouldReturnYearlyRentDebits()
        {
            var assetId = Guid.NewGuid();
            var asset = _fixture.Build<Asset>()
                .With(a => a.Id, assetId)
                .With(a => a.AssetId, assetId.ToString)
                .With(a => a.Tenure, new AssetTenure
                {
                    Id = Guid.NewGuid().ToString(),
                    PaymentReference = "12345678",
                    Type = "asset"
                })
                .Create();
            var years = Enumerable.Range(2011, 12).Select(Convert.ToInt16).ToList();
            var charges = CreateCharges(assetId, years).ToList();
            charges.AddRange(CreateCharges(assetId, Enumerable.Range(2000, 11).Select(Convert.ToInt16).ToList(), ChargeGroup.Leaseholders));
            var tenureInfo = CreateTenureInfo();

            _assetGateway.Setup(g => g.GetById(It.IsAny<Guid>())).ReturnsAsync(asset);
            _chargesGateway.Setup(g => g.GetAllByAssetId(It.IsAny<Guid>())).ReturnsAsync(charges.ToList);
            _tenureInformationGateway.Setup(g => g.GetById(It.IsAny<Guid>())).ReturnsAsync(tenureInfo);

            var response = await _useCase.ExecuteAsync(assetId).ConfigureAwait(false);
            _assetGateway.VerifyGetAssetById(Times.Once());
            _chargesGateway.VerifyGetChargesByAssetId(Times.Once());
            _tenureInformationGateway.VerifyGetTenureById(Times.Once());
            response.Count.Should().Be(12); // Because leaseholder charge group is ignored
            Assert.All(response,
                item => Assert.Equal(160m, item.RentCharge)
            );
            Assert.All(response,
                item => Assert.Equal(160m, item.ServiceCharge)
            );
        }

        private IList<Charge> CreateCharges(Guid assetId, List<short> years, ChargeGroup chargeGroup = ChargeGroup.Tenants)
        {
            var charges = new List<Charge>();
            var chargeFaker = new Faker<Charge>()
                .RuleFor(p => p.Id, f => f.Random.Uuid())
                .RuleFor(p => p.TargetId, f => assetId)
                .RuleFor(p => p.TargetType, f => TargetType.Asset)
                .RuleFor(p => p.ChargeGroup, f => chargeGroup)
                .RuleFor(p => p.ChargeYear, f => 2020);
            foreach (var year in years)
            {
                chargeFaker.RuleFor(p => p.ChargeYear, year);
                var weeklyRentCharges = CreateDetailedCharges(Convert.ToInt16(year), "rent", "weekly");
                var weeklyServiceCharges = CreateDetailedCharges(Convert.ToInt16(year), "service", "weekly");
                var detailedCharges = new List<DetailedCharges>();
                detailedCharges.AddRange(weeklyRentCharges);
                detailedCharges.AddRange(weeklyServiceCharges);
                chargeFaker.RuleFor(p => p.DetailedCharges, detailedCharges);
                charges.Add(chargeFaker.Generate());
            }

            return charges;
        }

        private IList<DetailedCharges> CreateDetailedCharges(short year = 2020, string type = "rent", string frequency = "weekly")
        {
            var startDate = new DateTime(year, 01, 01, 00, 00, 00, DateTimeKind.Utc);
            var detailedCharges = _fixture.Build<DetailedCharges>()
                .OmitAutoProperties()
                .With(p => p.Amount, 10)
                .With(p => p.Type, type)
                .With(p => p.Frequency, frequency)
                .With(p => p.StartDate, startDate)
                .With(p => p.EndDate, startDate.AddDays(28))
                .CreateMany(4);

            return detailedCharges.ToList();
        }

        private TenureInformation CreateTenureInfo()
        {
            var houseHoldMembers = new Faker<HouseholdMembers>()
                .RuleFor(p => p.IsResponsible, false)
                .RuleFor(p => p.FullName, f => f.Person.FullName)
                .Generate(2);
            houseHoldMembers.First().IsResponsible = true;
            var tenureInfo = _fixture.Build<TenureInformation>()
                .With(t => t.HouseholdMembers, houseHoldMembers)
                .With(t => t.StartOfTenureDate, new DateTime(2022, 01, 01, 00, 00, 00, DateTimeKind.Utc))
                .With(t => t.EndOfTenureDate, new DateTime(2022, 01, 28, 00, 00, 00, DateTimeKind.Utc))
                .Create();
            return tenureInfo;
        }
    }
}
