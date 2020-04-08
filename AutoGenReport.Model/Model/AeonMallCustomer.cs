namespace AutoGenReport.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AeonMallCustomer")]
    public partial class AeonMallCustomer
    {
        [Key]
        public int AMCustomerID { get; set; }

        [StringLength(50)]
        public string Cards { get; set; }

        [StringLength(50)]
        public string TenantCode { get; set; }

        public string TenantName { get; set; }

        public string TenantShortName { get; set; }

        [StringLength(50)]
        public string OpenningDate { get; set; }

        public string Note { get; set; }

        [StringLength(50)]
        public string Type { get; set; }

        [StringLength(50)]
        public string TimeTag { get; set; }
    }
}
