﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.Abs.Entities.Models.Core
{
    public partial class SalesCondition
    {
        [Key]
        public int Id { get; set; }
        public bool? AllowEdit { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        [Required]
        [StringLength(150)]
        public string CreatedUser { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedDate { get; set; }
        [StringLength(150)]
        public string ModifiedUser { get; set; }
        [Required]
        public string Name { get; set; }
        [StringLength(500)]
        public string ViewName { get; set; }
        public byte[] RowVersion { get; set; }
    }
}