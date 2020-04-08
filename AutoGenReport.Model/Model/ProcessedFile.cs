namespace AutoGenReport.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProcessedFile
    {
        [Key]
        public string FileName { get; set; }

        public string Status { get; set; }

        [StringLength(50)]
        public string TimeTag { get; set; }
    }
}
