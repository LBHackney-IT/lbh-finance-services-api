using System;
using System.ComponentModel.DataAnnotations;
using BaseApi.V1.Infrastructure;

namespace BaseApi.V1.Domain
{
    public class Person
    {
        [NonEmptyGuid("PersonId")]
        public Guid Id { get; set; }

        [Required]
        public string FullName { get; set; }
    }
}
