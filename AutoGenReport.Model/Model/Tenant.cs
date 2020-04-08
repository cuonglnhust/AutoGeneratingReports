namespace AutoGenReport.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tenant
    {
        [Key]
        [StringLength(50)]
        public string TenantCode { get; set; }

        [StringLength(50)]
        public string TenantName { get; set; }

        [StringLength(50)]
        public string CardNumber { get; set; }
    }
}
