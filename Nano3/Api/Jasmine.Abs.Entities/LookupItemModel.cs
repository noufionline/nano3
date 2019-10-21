using System;
using System.ComponentModel.DataAnnotations;

namespace Jasmine.Abs.Entities
{
    public class LookupItemModel:ILookupItemModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public byte[] RowVersion { get; set; }
    }
}