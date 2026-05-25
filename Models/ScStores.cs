using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CSMS.Models
{
    [Table("SC_Stores")]
    public partial class ScStores
    {
        public ScStores()
        {
            SurveyStore = new HashSet<SurveyStore>();
        }

        [Key]
        public int Id { get; set; }
        [Column("DS_ID")]
        [StringLength(6)]
        public string DsId { get; set; }
        [Column("Stores_Id")]
        [StringLength(4)]
        public string StoresId { get; set; }
        [Column("Stores_Name")]
        [StringLength(75)]
        public string StoresName { get; set; }
        [Column("Stores_Description")]
        [StringLength(150)]
        public string StoresDescription { get; set; }
        [Column("Store_Address")]
        [StringLength(500)]
        public string StoreAddress { get; set; }
        [Column("Web_Address")]
        [StringLength(200)]
        public string WebAddress { get; set; }
        [Column("GPS_Coordinates")]
        [StringLength(500)]
        public string GpsCoordinates { get; set; }
        [Column("Outlet_Type_Id")]
        public short? OutletTypeId { get; set; }

        [InverseProperty("Store")]
        public virtual ICollection<SurveyStore> SurveyStore { get; set; }
    }
}
