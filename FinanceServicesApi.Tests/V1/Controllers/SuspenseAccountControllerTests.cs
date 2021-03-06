using AutoFixture;
using FinanceServicesApi.Tests.V1.Helper;
using FinanceServicesApi.V1.Controllers;
using FinanceServicesApi.V1.Domain.AccountModels;
using FinanceServicesApi.V1.UseCase.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace FinanceServicesApi.Tests.V1.Controllers
{
    public class SuspenseAccountControllerTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<IGetAccountByTargetIdUseCase> _getAccountByTargetIdUseCase;
        private readonly Mock<IGetTransactionByIdUseCase> _getTransactionByIdUseCase;
        private readonly SuspenseAccountController _sut;

        public SuspenseAccountControllerTests()
        {
            _fixture = new Fixture();
            _getAccountByTargetIdUseCase = new Mock<IGetAccountByTargetIdUseCase>();
            _getTransactionByIdUseCase = new Mock<IGetTransactionByIdUseCase>();
            _sut = new SuspenseAccountController(_getTransactionByIdUseCase.Object, _getAccountByTargetIdUseCase.Object);

            /*var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                It.IsAny<ValidationStateDictionary>(),
                It.IsAny<string>(),
                It.IsAny<Object>()));

            _sut.ObjectValidator = objectValidator.Object;*/

        }

        [Theory]
        [MemberData(nameof(SuspenseAccountGetByIdArgument.GetTestData), MemberType = typeof(SuspenseAccountGetByIdArgument))]
        public async Task GetByIdWithEmptyParametersThrowsArgumentNullException(Guid transactionId, Guid accountId)
        {
            Func<Task<IActionResult>> func = async () => await _sut.GetById(transactionId, accountId).ConfigureAwait(false);
            await func.Should().ThrowAsync<ArgumentException>().ConfigureAwait(false);
        }

        /*[Fact]
        public async Task GetAccountByIdWithInvalidResponseThrowsBadRequest()
        {
            Account accountResponse = _fixture.Build<Account>()
                .Without(p => p.CreatedBy)
                .Create();
            _getAccountByIdUseCase.Setup(p => p.ExecuteAsync(It.IsAny<Guid>()))
                .ReturnsAsync(accountResponse);

            var result = await _sut.GetById(Guid.NewGuid(), Guid.NewGuid()).ConfigureAwait(false);
            result.Should().BeOfType(typeof(BadRequestObjectResult));
        }*/

        [Fact]
        public async Task GetAccountByTargetIdWithNullResponseThrowsBadRequest()
        {
            Account accountResponse = _fixture.Build<Account>()
                .Without(p => p.CreatedBy)
                .Create();
            _getAccountByTargetIdUseCase.Setup(p => p.ExecuteAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Account) null);

            var result = await _sut.GetById(Guid.NewGuid(), Guid.NewGuid()).ConfigureAwait(false);
            result.Should().BeOfType(typeof(NotFoundObjectResult));
        }

        [Fact]
        public async Task GetAccountByIdWithNullResponseThrowsBadRequest()
        {
            Account accountResponse = _fixture.Build<Account>()
                .Without(p => p.CreatedBy)
                .Create();
            _getAccountByTargetIdUseCase.Setup(p => p.ExecuteAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Account) null);

            var result = await _sut.GetById(Guid.NewGuid(), Guid.NewGuid()).ConfigureAwait(false);
            result.Should().BeOfType(typeof(NotFoundObjectResult));
        }

        /*[Fact]
        public async Task GetTransactionByIdWithInvalidResponseThrowsBadRequest()
        {
            Account accountResponse = _fixture.Build<Account>().Create();
            _getAccountByIdUseCase.Setup(p => p.ExecuteAsync(It.IsAny<Guid>()))
                .ReturnsAsync(accountResponse);

            Transaction transactionResponse = _fixture.Build<Transaction>()
                .Without(p => p.TransactionDate)
                .Create();
            _getTransactionByIdUseCase.Setup(p => p.ExecuteAsync(It.IsAny<Guid>()))
                .ReturnsAsync(transactionResponse);

            var result = await _sut.GetById(Guid.NewGuid(), Guid.NewGuid()).ConfigureAwait(false);
            result.Should().BeOfType(typeof(BadRequestObjectResult));
        }*/

        /*        [Fact]
                public async Task GetTransactionByIdWithNullResponseThrowsBadRequest()
                {
                    Account accountResponse = _fixture.Build<Account>().Create();
                    _getAccountByIdUseCase.Setup(p => p.ExecuteAsync(It.IsAny<Guid>()))
                        .ReturnsAsync(accountResponse);

                    Transaction transactionResponse = _fixture.Build<Transaction>().Create();
                    _getTransactionByIdUseCase.Setup(p => p.ExecuteAsync(It.IsAny<Guid>()))
                        .ReturnsAsync((Transaction) null);

                    var result = await _sut.GetById(Guid.NewGuid(), Guid.NewGuid()).ConfigureAwait(false);
                    result.Should().BeOfType(typeof(NotFoundObjectResult));
                }*/

        /*[Fact]
        public async Task GetByIdWithValidInputReturnsOk()
        {
            Account accountResponse = _fixture.Build<Account>().Create();
            _getAccountByIdUseCase.Setup(p => p.ExecuteAsync(It.IsAny<Guid>()))
                .ReturnsAsync(accountResponse);

            Transaction transactionResponse = _fixture.Build<Transaction>().Create();
            _getTransactionByIdUseCase.Setup(p => p.ExecuteAsync(It.IsAny<Guid>()))
                .ReturnsAsync(transactionResponse);

            var result = await _sut.GetById(Guid.NewGuid(), Guid.NewGuid()).ConfigureAwait(false);
            result.Should().BeOfType(typeof(OkObjectResult));
        }*/
    }
}
