// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Design", "CA1063:Implement IDisposable Correctly", Justification = "<Pending>", Scope = "type", Target = "~T:BaseApi.Tests.IntegrationTests`1")]
[assembly: SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "<Pending>", Scope = "member", Target = "~M:BaseApi.Tests.IntegrationTests`1.Dispose")]
[assembly: SuppressMessage("Design", "CA1063:Implement IDisposable Correctly", Justification = "<Pending>", Scope = "type", Target = "~T:BaseApi.Tests.DynamoDbIntegrationTests`1")]
[assembly: SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "<Pending>", Scope = "member", Target = "~M:BaseApi.Tests.DynamoDbIntegrationTests`1.Dispose")]
[assembly: SuppressMessage("Design", "CA1063:Implement IDisposable Correctly", Justification = "<Pending>", Scope = "type", Target = "~T:BaseApi.Tests.DatabaseTests")]
[assembly: SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "<Pending>", Scope = "member", Target = "~M:BaseApi.Tests.DatabaseTests.Dispose")]
[assembly: SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "<Pending>", Scope = "member", Target = "~M:BaseApi.Tests.V1.Gateways.SuspenseTransaction.AccountGatewayTests.ConstructorGetsApiUrlAndApiTokenFromEnvironment")]
[assembly: SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "<Pending>", Scope = "member", Target = "~M:BaseApi.Tests.V1.Gateways.SuspenseTransaction.AccountGatewayTests.GetByIdReturnsBadRequestThrowsException~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "<Pending>", Scope = "member", Target = "~M:BaseApi.Tests.V1.Gateways.SuspenseTransaction.AccountGatewayTests.GetByIdWithValidOutputReturnsAccountResponse~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "<Pending>", Scope = "member", Target = "~M:BaseApi.Tests.V1.Gateways.SuspenseTransaction.TransactionGatewayTests.ConstructorGetsApiUrlAndApiTokenFromEnvironment")]
[assembly: SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "<Pending>", Scope = "member", Target = "~M:BaseApi.Tests.V1.Gateways.SuspenseTransaction.TransactionGatewayTests.GetByIdReturnsBadRequestThrowsException~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "<Pending>", Scope = "member", Target = "~M:BaseApi.Tests.V1.Gateways.SuspenseTransaction.TransactionGatewayTests.GetByIdWithValidOutputReturnsTransactionResponse~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>", Scope = "member", Target = "~M:BaseApi.Tests.V1.Gateways.SuspenseTransaction.AccountGatewayTests.ConstructorWithoutApiUrlThrowsError")]
