using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CSMS.Models
{
    
    [Table("ScChecklist")]
    public partial class ScChecklist
    {
        [Key]
        public int Id { get; set; }
        public int? Number { get; set; }
        [StringLength(255)]
        public string Time { get; set; }
        [StringLength(255)]
        public string Task { get; set; }
        [Column("Detail_task")]
        public string DetailTask { get; set; }
        [Column("cb_id")]
        [StringLength(255)]
        public string CbId { get; set; }
        [Column("Detail_taskG1")]
        public string DetailTaskG1 { get; set; }
        [Column("GroupID")]
        [StringLength(255)]
        public string GroupId { get; set; }
    }
}
