using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoFixture;
using FinanceServicesApi.V1.Boundary.Request.Enums;
using FinanceServicesApi.V1.Boundary.Responses;
using FinanceServicesApi.V1.Boundary.Responses.PropertySummary;
using FinanceServicesApi.V1.Controllers;
using FinanceServicesApi.V1.Domain.AccountModels;
using FinanceServicesApi.V1.Domain.Charges;
using FinanceServicesApi.V1.Domain.TransactionModels;
using FinanceServicesApi.V1.Infrastructure;
using FinanceServicesApi.V1.UseCase.Interfaces;
using FluentAssertions;
using Hackney.Shared.Asset.Domain;
using Hackney.Shared.Person;
using Hackney.Shared.Tenure.Domain;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace FinanceServicesApi.Tests.V1.Controllers
{
    [ExcludeFromCodeCoverage]
    public class PropertySummaryControllerTests
    {
        private readonly Fixture _fixture;
        private readonly PropertySummaryController _sutController;
        private readonly Mock<IGetPersonByIdUseCase> _mockPerson;
        private readonly Mock<IGetChargeByAssetIdUseCase> _mockChargeUseCase;
        private readonly Mock<IGetTenureInformationByIdUseCase> _mockTenureUseCase;
        private readonly Mock<IGetContactDetailsByTargetIdUseCase> _mockContactUseCase;
        private readonly Mock<IGetAccountByTargetIdUseCase> _mockAccountByTargetIdUseCase;
        private readonly Mock<IGetLastPaymentTransactionsByTargetIdUseCase> _mockLastPaymentTransactionsByTargetIdUseCase;
        private readonly Mock<IGetAssetByIdUseCase> _mockAssetByIdUseCase;
        private readonly Mock<IGetAssetApportionmentUseCase> _mockGetAssetApportionmentUseCase;

        public PropertySummaryControllerTests()
        {
            _fixture = new Fixture();
            _mockPerson = new Mock<IGetPersonByIdUseCase>();
            _mockChargeUseCase = new Mock<IGetChargeByAssetIdUseCase>();
            _mockTenureUseCase = new Mock<IGetTenureInformationByIdUseCase>();
            _mockContactUseCase = new Mock<IGetContactDetailsByTargetIdUseCase>();
            _mockAccountByTargetIdUseCase = new Mock<IGetAccountByTargetIdUseCase>();
            _mockLastPaymentTransactionsByTargetIdUseCase = new Mock<IGetLastPaymentTransactionsByTargetIdUseCase>();
            _mockAssetByIdUseCase = new Mock<IGetAssetByIdUseCase>();
            _mockGetAssetApportionmentUseCase = new Mock<IGetAssetApportionmentUseCase>();

            _sutController = new PropertySummaryController(_mockPerson.Object,
                _mockChargeUseCase.Object,
                _mockTenureUseCase.Object,
                _mockContactUseCase.Object,
                _mockAccountByTargetIdUseCase.Object,
                _mockLastPaymentTransactionsByTargetIdUseCase.Object,
                _mockAssetByIdUseCase.Object,
                _mockGetAssetApportionmentUseCase.Object);
        }

        [Fact]
        public void GetByIdWithExistenceIdReturnsPropertySummaryResponse()
        {
            // Arrange
            Account account = _fixture.Create<Account>();
            List<Transaction> transactions = _fixture.CreateMany<Transaction>(20).ToList();
            TenureInformation tenureInformation = _fixture.Create<TenureInformation>();
            Asset asset = _fixture.Create<Asset>();
            List<Charge> charges = _fixture.CreateMany<Charge>(20).ToList();
            Person person = _fixture.Create<Person>();
            GetContactDetailsResponse contactDetail = _fixture.Create<GetContactDetailsResponse>();

            _mockAccountByTargetIdUseCase.Setup(p => p.ExecuteAsync(It.IsAny<Guid>()))
                .ReturnsAsync(account);

            _mockLastPaymentTransactionsByTargetIdUseCase.Setup(p => p.ExecuteAsync(It.IsAny<Guid>()))
                .ReturnsAsync(transactions);

            _mockTenureUseCase.Setup(p => p.ExecuteAsync(It.IsAny<Guid>()))
                .ReturnsAsync(tenureInformation);

            _mockAssetByIdUseCase.Setup(p => p.ExecuteAsync(It.IsAny<Guid>()))
                .ReturnsAsync(asset);

            _mockChargeUseCase.Setup(p => p.ExecuteAsync(It.IsAny<Guid>()))
                .ReturnsAsync(charges);

            _mockPerson.Setup(p => p.ExecuteAsync(It.IsAny<Guid>()))
                .ReturnsAsync(person);

            _mockContactUseCase.Setup(p => p.ExecuteAsync(It.IsAny<Guid>()))
                .ReturnsAsync(contactDetail);
            // Act

            Func<Task<IActionResult>> func = async () =>
                await _sutController.GetById(Guid.NewGuid()).ConfigureAwait(false);

            var response = func.Invoke();
            // Assert
            response.Should().NotBeNull();
            response.Result.Should().BeOfType<OkObjectResult>();
            var responseObject = response.Result as OkObjectResult;
            responseObject.Should().NotBeNull();
            var result = responseObject?.Value as PropertySummaryResponse;
            result.Should().NotBeNull();
            result?.CurrentBalance.Should().Be(account.ConsolidatedBalance);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(1969)]
        [InlineData(2050)]
        public async Task GetAssetApportionmentWithWrongYearShouldReturn400(short year)
        {
            var actualResult = await _sutController.GetAssetApportionments(Guid.NewGuid(), year).ConfigureAwait(false);

            actualResult.Should().NotBeNull();
            actualResult.Should().BeOfType<BadRequestObjectResult>();
            var responseObject = actualResult as BadRequestObjectResult;
            responseObject.Should().NotBeNull();

            var badErrorResponse = responseObject?.Value as BaseErrorResponse;

            badErrorResponse.StatusCode.Should().Be((int) HttpStatusCode.BadRequest);
            badErrorResponse.Message.Should().Be("fromYear should be more that 1970 ans less than currect year");
        }

        [Fact]
        public async Task GetAssetApportionmentWithWrongAssetIdShouldReturn400()
        {
            var actualResult = await _sutController.GetAssetApportionments(Guid.Empty, 2022).ConfigureAwait(false);

            actualResult.Should().NotBeNull();
            actualResult.Should().BeOfType<BadRequestObjectResult>();
            var responseObject = actualResult as BadRequestObjectResult;
            responseObject.Should().NotBeNull();

            var badErrorResponse = responseObject?.Value as BaseErrorResponse;

            badErrorResponse.StatusCode.Should().Be((int) HttpStatusCode.BadRequest);
            badErrorResponse.Message.Should().Be("assetId cannot be empty.");
        }

        [Fact]
        public async Task GetAssetApportionmentCallsUseCaseReturnsResponse()
        {
            var expectedResponse = _fixture.Create<AssetApportionmentResponse>();
            _mockGetAssetApportionmentUseCase.Setup(_ => _.ExecuteAsync(It.IsAny<Guid>(), It.IsAny<short>(), ChargeGroupFilter.Both))
                .ReturnsAsync(expectedResponse);

            var actualResult = await _sutController.GetAssetApportionments(Guid.NewGuid(), 2022, ChargeGroupFilter.Both).ConfigureAwait(false);

            actualResult.Should().NotBeNull();
            actualResult.Should().BeOfType<OkObjectResult>();
            var responseObject = actualResult as OkObjectResult;
            responseObject.Should().NotBeNull();

            var actualResponse = responseObject?.Value as AssetApportionmentResponse;
            actualResponse.Should().NotBeNull();
            actualResponse.Should().BeEquivalentTo(expectedResponse);
        }
    }
}
