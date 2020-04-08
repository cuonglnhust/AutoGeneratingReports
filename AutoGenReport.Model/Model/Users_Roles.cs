namespace AutoGenReport.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Users_Roles
    {
        public int ID { get; set; }

        public string Username { get; set; }

        [StringLength(50)]
        public string RoleID { get; set; }
    }
}
