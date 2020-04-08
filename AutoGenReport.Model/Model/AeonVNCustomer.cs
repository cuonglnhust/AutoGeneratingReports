namespace AutoGenReport.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AeonVNCustomer")]
    public partial class AeonVNCustomer
    {
        [Key]
        public int AVCustomerID { get; set; }

        [StringLength(50)]
        public string Floor { get; set; }

        [StringLength(50)]
        public string PosLocation { get; set; }

        [StringLength(50)]
        public string PosNumber { get; set; }

        [StringLength(50)]
        public string PosName { get; set; }

        [StringLength(50)]
        public string SalesBagPOS { get; set; }

        [StringLength(50)]
        public string SaleBagBarcode { get; set; }

        [StringLength(50)]
        public string IntermediateBagPOS { get; set; }

        [StringLength(50)]
        public string IntermediateBagBarcode { get; set; }

        [StringLength(50)]
        public string Type { get; set; }

        [StringLength(50)]
        public string TimeTag { get; set; }
    }
}
