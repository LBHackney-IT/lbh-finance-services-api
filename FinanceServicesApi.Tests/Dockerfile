FROM mcr.microsoft.com/dotnet/core/sdk:3.1

# disable microsoft telematry
ENV DOTNET_CLI_TELEMETRY_OPTOUT='true'

ENV DynamoDb_LocalMode='true'
ARG LBHPACKAGESTOKEN
ENV LBHPACKAGESTOKEN=$LBHPACKAGESTOKEN
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY /NuGet.Config /root/.nuget/NuGet/NuGet.Config
COPY ./FinanceServicesApi.sln ./
COPY ./FinanceServicesApi/FinanceServicesApi.csproj ./FinanceServicesApi/
COPY ./FinanceServicesApi.Tests/FinanceServicesApi.Tests.csproj ./FinanceServicesApi.Tests/

RUN dotnet restore ./FinanceServicesApi/FinanceServicesApi.csproj
RUN dotnet restore ./FinanceServicesApi.Tests/FinanceServicesApi.Tests.csproj

# Copy everything else and build
COPY . .

RUN dotnet build -c debug -o out FinanceServicesApi.Tests/FinanceServicesApi.Tests.csproj

CMD dotnet test
