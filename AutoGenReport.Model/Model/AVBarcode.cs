namespace AutoGenReport.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AVBarcode")]
    public partial class AVBarcode
    {
        public int AVBarcodeID { get; set; }

        public int? AVCustomerID { get; set; }

        [StringLength(50)]
        public string SalesBagBarcode { get; set; }

        [StringLength(50)]
        public string IntermediateBagBarcode { get; set; }
    }
}
