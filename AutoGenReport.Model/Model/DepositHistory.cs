namespace AutoGenReport.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DepositHistory")]
    public partial class DepositHistory
    {
        [StringLength(50)]
        public string DepositHistoryID { get; set; }

        [StringLength(50)]
        public string Device { get; set; }

        [StringLength(50)]
        public string DepositDate { get; set; }

        [StringLength(50)]
        public string SaleDate { get; set; }

        [StringLength(10)]
        public string CustomerID { get; set; }

        [StringLength(20)]
        public string BarcodeID { get; set; }

        [StringLength(15)]
        public string DeclaredAmount { get; set; }

        [StringLength(15)]
        public string ActualAmount { get; set; }

        [StringLength(15)]
        public string CounterfeitAmount { get; set; }

        [StringLength(15)]
        public string DiscrepancyAmount { get; set; }

        [StringLength(110)]
        public string Quantity { get; set; }

        [StringLength(10)]
        public string LastEdit { get; set; }

        [StringLength(50)]
        public string TimeTag { get; set; }

        [StringLength(50)]
        public string CountingPeople { get; set; }

        [StringLength(1)]
        public string Checked { get; set; }
    }
}
