namespace AutoGenReport.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AeonVNSup")]
    public partial class AeonVNSup
    {
        [Key]
        public int AVSupID { get; set; }

        [StringLength(50)]
        public string SupCode { get; set; }

        [StringLength(50)]
        public string SupName { get; set; }

        [StringLength(50)]
        public string RegisterDay { get; set; }

        public string Note { get; set; }

        [StringLength(50)]
        public string Type { get; set; }

        [StringLength(50)]
        public string TimeTag { get; set; }
    }
}
