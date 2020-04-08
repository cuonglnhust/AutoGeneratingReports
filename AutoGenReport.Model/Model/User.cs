namespace AutoGenReport.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class User
    {
        [Key]
        public string Username { get; set; }

        [StringLength(128)]
        public string Password { get; set; }

        [StringLength(128)]
        public string Time2GenCheckList { get; set; }

        [StringLength(128)]
        public string TimeStart2Edit { get; set; }

        [StringLength(128)]
        public string TimeEnd2Edit { get; set; }

        public string IP { get; set; }  

        public string Description { get; set; }
    }
}
