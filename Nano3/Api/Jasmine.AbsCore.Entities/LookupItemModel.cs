using System;
using System.ComponentModel.DataAnnotations;

namespace Jasmine.AbsCore.Entities
{
    public class LookupItemModel:ILookupItemModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public byte[] RowVersion { get; set; }
    }
}