using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CSMS.Models
{
    
    [Table("FAQs")]
    public partial class Faq
    {
        [Required]
        [Key]
        [Column("PK")]
        [StringLength(450)]
        public string Pk { get; set; }
        [Column("ID")]
        public int? ID { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        [StringLength(255)]
        public string Category { get; set; }
        [Column("QType")]
        public double? Qtype { get; set; }
        [Column("GroupID")]
        public int? GroupId { get; set; }
    }
}
