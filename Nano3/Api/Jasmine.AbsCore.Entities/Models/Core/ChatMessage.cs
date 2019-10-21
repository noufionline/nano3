using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jasmine.AbsCore.Entities.Models.Core
{
    public partial class ChatMessage
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public Guid MessageId { get; set; }
        [Required]
        [StringLength(250)]
        public string Author { get; set; }
        [Required]
        [StringLength(250)]
        public string Receiver { get; set; }
        public string Message { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime MessageTime { get; set; }
        public int Status { get; set; }
    }
}
