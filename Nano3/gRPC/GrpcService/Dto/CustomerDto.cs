using System;
using System.Collections.Generic;
namespace GrpcService.Dto
{
     public class CustomerDto
        {
            public int Id { get; set; }
            public int? PartnerId { get; set; }
            public string Name { get; set; }
            public decimal Salary { get; set; }
            public DateTime CreatedDate { get; set; }

            public List<ProjectDto> Projects { get; set; }
        }
}
