using Amazon.XRay.Recorder.Handlers.AwsSdk;
using FinanceServicesApi.V1;
using FinanceServicesApi.V1.Boundary.Responses;
using FinanceServicesApi.V1.Domain.AccountModels;
using FinanceServicesApi.V1.Domain.Charges;
using FinanceServicesApi.V1.Domain.TransactionModels;
using FinanceServicesApi.V1.Gateways;
using FinanceServicesApi.V1.Gateways.Common;
using FinanceServicesApi.V1.Gateways.Interfaces;
using FinanceServicesApi.V1.Infrastructure;
using FinanceServicesApi.V1.Infrastructure.Environments;
using FinanceServicesApi.V1.Infrastructure.Interfaces;
using FinanceServicesApi.V1.Infrastructure.UrlGenerators;
using FinanceServicesApi.V1.UseCase;
using FinanceServicesApi.V1.UseCase.Interfaces;
using FinanceServicesApi.Versioning;
using FluentValidation.AspNetCore;
using Hackney.Core.Authorization;
using Hackney.Core.DynamoDb;
using Hackney.Core.JWT;
using Hackney.Core.Logging;
using Hackney.Shared.Asset.Domain;
using Hackney.Shared.Person;
using Hackney.Shared.Tenure.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;

namespace FinanceServicesApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            AWSSDKHandler.RegisterXRayForAllServices();
        }

        public IConfiguration Configuration { get; }
        private static List<ApiVersionDescription> _apiVersions { get; set; }

        //TODO update the below to the name of your API
        private const string ApiName = "FinanceServicesApi";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc()
                .AddFluentValidation(fv =>
                {
                    fv.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
                    fv.LocalizationEnabled = false;
                });
            services.AddApiVersioning(o =>
            {
                o.DefaultApiVersion = new ApiVersion(1, 0);
                o.AssumeDefaultVersionWhenUnspecified = true; // assume that the caller wants the default version if they don't specify
                o.ApiVersionReader = new UrlSegmentApiVersionReader(); // read the version number from the url segment header)
            });

            services.AddSingleton<IApiVersionDescriptionProvider, DefaultApiVersionDescriptionProvider>();

            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Token",
                    new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Your Hackney API Key",
                        Name = "X-Api-Key",
                        Type = SecuritySchemeType.ApiKey
                    });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Token" }
                        },
                        new List<string>()
                    }
                });

                //Looks at the APIVersionAttribute [ApiVersion("x")] on controllers and decides whether or not
                //to include it in that version of the swagger document
                //Controllers must have this [ApiVersion("x")] to be included in swagger documentation!!
                c.DocInclusionPredicate((docName, apiDesc) =>
                {
                    apiDesc.TryGetMethodInfo(out var methodInfo);

                    var versions = methodInfo?
                        .DeclaringType?.GetCustomAttributes()
                        .OfType<ApiVersionAttribute>()
                        .SelectMany(attr => attr.Versions).ToList();

                    return versions?.Any(v => $"{v.GetFormattedApiVersion()}" == docName) ?? false;
                });

                //Get every ApiVersion attribute specified and create swagger docs for them
                foreach (var apiVersion in _apiVersions)
                {
                    var version = $"v{apiVersion.ApiVersion.ToString()}";
                    c.SwaggerDoc(version, new OpenApiInfo
                    {
                        Title = $"{ApiName}-api {version}",
                        Version = version,
                        Description = $"{ApiName} version {version}. Please check older versions for depreciated endpoints."
                    });
                }

                c.CustomSchemaIds(x => x.FullName);
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                    c.IncludeXmlComments(xmlPath);
            });

            services.AddTokenFactory();
            ConfigureLogging(services, Configuration);
            services.ConfigureDynamoDB();
            RegisterGateways(services);
            RegisterUseCases(services);
            RegisterInfraService(services);
            services.ConfigureLambdaLogging(Configuration);
            services.AddCors();
            services.AddHttpContextAccessor();
        }

        private static void RegisterInfraService(IServiceCollection services)
        {
            services.AddScoped<ICustomeHttpClient, CustomeHttpClient>();

            services.AddScoped<IGetEnvironmentVariables<TenureInformation>, GetTenureEnvironmentVariables>();
            services.AddScoped<IGetEnvironmentVariables<Asset>, GetAssetEnvironmentVariables>();
            services.AddScoped<IGetEnvironmentVariables<Person>, GetPersonEnvironmentVariables>();
            services.AddScoped<IGetEnvironmentVariables<GetContactDetailsResponse>, GetContactEnvironmentVariables>();
            services.AddScoped<IGetEnvironmentVariables<Account>, GetAccountEnvironmentVariables>();
            services.AddScoped<IGetEnvironmentVariables<GetAccountListResponse>, GetAccountsEnvironmentVariables>();
            services.AddScoped<IGetEnvironmentVariables<Transaction>, GetTransactionEnvironmentVariable>();
            services.AddScoped<IGetEnvironmentVariables<List<Transaction>>, GetTransactionsEnvironmentVariable>();
            services.AddScoped<IGetEnvironmentVariables<List<Charge>>, GetChargesEnvironmentVariables>();

            services.AddScoped<IFinanceDomainApiData<TenureInformation>, FinanceDomainApiData<TenureInformation>>();
            services.AddScoped<IFinanceDomainApiData<Asset>, FinanceDomainApiData<Asset>>();
            services.AddScoped<IFinanceDomainApiData<Person>, FinanceDomainApiData<Person>>();
            services.AddScoped<IFinanceDomainApiData<GetContactDetailsResponse>, FinanceDomainApiData<GetContactDetailsResponse>>();
            services.AddScoped<IFinanceDomainApiData<List<Charge>>, FinanceDomainApiData<List<Charge>>>();
            services.AddScoped<IFinanceDomainApiData<List<Transaction>>, FinanceDomainApiData<List<Transaction>>>();
            services.AddScoped<IFinanceDomainApiData<Transaction>, FinanceDomainApiData<Transaction>>();
            services.AddScoped<IFinanceDomainApiData<Account>, FinanceDomainApiData<Account>>();
            services.AddScoped<IFinanceDomainApiData<GetAccountListResponse>, FinanceDomainApiData<GetAccountListResponse>>();

            services.AddScoped<IGenerateUrl<TenureInformation>, TenureUrlGenerator>();
            services.AddScoped<IGenerateUrl<Account>, AccountUrlGenerator>();
            services.AddScoped<IGenerateUrl<GetAccountListResponse>, AccountsUrlGenerator>();
            services.AddScoped<IGenerateUrl<Transaction>, TransactionUrlGenerator>();
            services.AddScoped<IGenerateUrl<List<Transaction>>, TransactionTargetIdUrlGenerator>();
            services.AddScoped<IGenerateUrl<Asset>, AssetUrlGenerator>();
            services.AddScoped<IGenerateUrl<Person>, PersonUrlGenerator>();
            services.AddScoped<IGenerateUrl<GetContactDetailsResponse>, ContactDetailUrlGenerator>();
            services.AddScoped<IGenerateUrl<List<Charge>>, ChargesUrlGenerator>();

            services.AddAutoMapper(cnf =>
            {
                cnf.AddProfile<AccountAutoMapperProfile>();
            });
        }

        private static void ConfigureLogging(IServiceCollection services, IConfiguration configuration)
        {
            // We rebuild the logging stack so as to ensure the console logger is not used in production.
            // See here: https://weblog.west-wind.com/posts/2018/Dec/31/Dont-let-ASPNET-Core-Default-Console-Logging-Slow-your-App-down
            services.AddLogging(config =>
            {
                // clear out default configuration
                config.ClearProviders();

                config.AddConfiguration(configuration.GetSection("Logging"));
                config.AddDebug();
                config.AddEventSourceLogger();

                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Development)
                {
                    config.AddConsole();
                }
            });
        }

        private static void RegisterGateways(IServiceCollection services)
        {
            services.AddScoped<IAccountGateway, AccountGateway>();
            services.AddScoped<ITransactionGateway, TransactionGateway>();
            services.AddScoped<IPersonGateway, PersonGateway>();
            services.AddScoped<IContactDetailsGateway, ContactDetailsGateway>();
            services.AddScoped<ITenureInformationGateway, TenureInformationGateway>();
            services.AddScoped<IChargesGateway, ChargesGateway>();
            services.AddScoped<IAssetGateway, AssetGateway>();

            services.AddTransient<LoggingDelegatingHandler>();
            var housingSearchApiUrl = Environment.GetEnvironmentVariable("SEARCH_API_URL") ?? String.Empty;
            var housingSearchApiToken = Environment.GetEnvironmentVariable("HOUSING_SEARCH_API_TOKEN") ?? String.Empty;

            services.AddHttpClient<IHousingSearchGateway, HousingSearchGateway>(c =>
            {
                c.BaseAddress = new Uri(housingSearchApiUrl);
                c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(housingSearchApiToken);
            })
           .AddHttpMessageHandler<LoggingDelegatingHandler>();
        }

        private static void RegisterUseCases(IServiceCollection services)
        {
            services.AddScoped<IGetAccountByTargetIdUseCase, GetAccountByTargetIdUseCase>();
            services.AddScoped<IGetAccountByIdUseCase, GetAccountByIdUseCase>();
            services.AddScoped<IGetTransactionByIdUseCase, GetTransactionByIdUseCase>();
            services.AddScoped<IGetLastPaymentTransactionsByTargetIdUseCase, GetLastPaymentTransactionsByTargetIdUseCase>();
            services.AddScoped<IGetPersonByIdUseCase, GetPersonByIdUseCase>();
            services.AddScoped<IGetContactDetailsByTargetIdUseCase, GetContactDetailsByTargetIdUseCase>();
            services.AddScoped<IGetTenureInformationByIdUseCase, GetTenureInformationByIdUseCase>();
            services.AddScoped<IGetChargeByAssetIdUseCase, GetChargeByAssetIdUseCase>();
            services.AddScoped<IGetAssetByIdUseCase, GetAssetByIdUseCase>();
            services.AddScoped<IGetLeaseholdAssetsListUseCase, GetLeaseholdAssetsListUseCase>();
            services.AddScoped<IGetAssetApportionmentUseCase, GetAssetApportionmentUseCase>();
            services.AddScoped<IGetPersonListUseCase, GetPersonListUseCase>();
            services.AddScoped<IGetYearlyRentDebitsUseCase, GetYearlyRentDebitsUseCase>();
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());

            app.UseCorrelation();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseXRay("finance-services-api");

            var api = app.ApplicationServices.GetService<IApiVersionDescriptionProvider>();
            _apiVersions = api.ApiVersionDescriptions.ToList();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                foreach (var apiVersionDescription in _apiVersions)
                {
                    c.SwaggerEndpoint($"{apiVersionDescription.GetFormattedApiVersion()}/swagger.json",
                        $"{ApiName}-api {apiVersionDescription.GetFormattedApiVersion()}");
                }
            });
            app.UseSwagger();
            app.UseRouting();
            app.UseGoogleGroupAuthorization();
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
