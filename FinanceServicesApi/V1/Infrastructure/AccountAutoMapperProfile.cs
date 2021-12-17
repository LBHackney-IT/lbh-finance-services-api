using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FinanceServicesApi.V1.Domain.AccountModels;
using FinanceServicesApi.V1.Infrastructure.Entities;

namespace FinanceServicesApi.V1.Infrastructure
{
    public class AccountAutoMapperProfile : Profile
    {
        public AccountAutoMapperProfile()
        {
            CreateMap<AccountDbEntity, Account>();
        }
    }
}
