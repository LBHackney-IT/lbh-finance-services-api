using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;

namespace FinanceServicesApi.Tests.V1.Helper
{
    public static class SuspenseAccountGetByIdArgument
    {
        public static List<object[]> GetTestData { get; } = new List<object[]> {
            new object[] { Guid.Empty,Guid.NewGuid() },
            new object[]{Guid.NewGuid(),Guid.Empty},
            new object[]{Guid.Empty,Guid.Empty}
        };
    }
}
