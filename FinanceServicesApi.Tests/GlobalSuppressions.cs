// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Design", "CA1063:Implement IDisposable Correctly", Justification = "<Pending>", Scope = "type", Target = "~T:FinanceServicesApi.Tests.IntegrationTests`1")]
[assembly: SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "<Pending>", Scope = "member", Target = "~M:FinanceServicesApi.Tests.IntegrationTests`1.Dispose")]
[assembly: SuppressMessage("Design", "CA1063:Implement IDisposable Correctly", Justification = "<Pending>", Scope = "type", Target = "~T:FinanceServicesApi.Tests.DynamoDbIntegrationTests`1")]
[assembly: SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "<Pending>", Scope = "member", Target = "~M:FinanceServicesApi.Tests.DynamoDbIntegrationTests`1.Dispose")]
[assembly: SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "<Pending>", Scope = "member", Target = "~M:FinanceServicesApi.Tests.V1.Gateways.TransactionGatewayTests.ConstructorGetsApiUrlAndApiTokenFromEnvironment")]
[assembly: SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "<Pending>", Scope = "member", Target = "~M:FinanceServicesApi.Tests.V1.Gateways.AccountGatewayTests.ConstructorGetsApiUrlAndApiTokenFromEnvironment")]
[assembly: SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "<Pending>", Scope = "member", Target = "~M:FinanceServicesApi.Tests.V1.Gateways.AccountGatewayTests.GetByIdReturnsBadRequestThrowsException~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "<Pending>", Scope = "member", Target = "~M:FinanceServicesApi.Tests.V1.Gateways.AccountGatewayTests.GetByIdWithValidOutputReturnsAccount~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "<Pending>", Scope = "member", Target = "~M:FinanceServicesApi.Tests.V1.Gateways.TransactionGatewayTests.GetByIdReturnsBadRequestThrowsException~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "<Pending>", Scope = "member", Target = "~M:FinanceServicesApi.Tests.V1.Gateways.TransactionGatewayTests.GetByIdWithValidOutputReturnsTransaction~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "<Pending>", Scope = "member", Target = "~M:FinanceServicesApi.Tests.V1.Infrastructure.HousingDataTests`1.DownloadAsyncWithExistenceIdReturnsValidData")]
[assembly: SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "<Pending>", Scope = "member", Target = "~M:FinanceServicesApi.Tests.V1.Infrastructure.HousingDataTests.DownloadAsyncWithExistenceIdReturnsValidData")]
[assembly: SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "<Pending>", Scope = "member", Target = "~M:FinanceServicesApi.Tests.V1.Infrastructure.HousingDataTests`1.DownloadAsyncWithNonExistenceIdThrowsException")]
[assembly: SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "<Pending>", Scope = "member", Target = "~M:FinanceServicesApi.Tests.V1.Infrastructure.HousingDataTests`1.DownloadAsyncWithNonReachableApiThrowsException")]
[assembly: SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "<Pending>", Scope = "member", Target = "~M:FinanceServicesApi.Tests.V1.Infrastructure.HousingDataTests`1.DownloadAsyncWithNonExistenceIdReturnsNull~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "<Pending>", Scope = "member", Target = "~M:FinanceServicesApi.Tests.V1.Infrastructure.HousingDataTests`1.DownloadAsyncWithApiExceptionReturnsException~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "<Pending>", Scope = "member", Target = "~M:FinanceServicesApi.Tests.V1.Infrastructure.HousingDataTests`1.DownloadAsyncWithApiExceptionReturnsException")]
[assembly: SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "<Pending>", Scope = "member", Target = "~M:FinanceServicesApi.Tests.V1.Infrastructure.HousingDataTests`1.DownloadAsyncWithoutApiTokenThrowsInvalidCredentialException")]
