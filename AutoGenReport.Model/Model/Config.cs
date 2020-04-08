namespace AutoGenReport.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Config")]
    public partial class Config
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ConfigID { get; set; }

        [StringLength(50)]
        public string Parameter { get; set; }

        [StringLength(50)]
        public string Value { get; set; }

        public string Description { get; set; }

        [StringLength(50)]
        public string TimeTag { get; set; }
    }
}
