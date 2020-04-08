namespace AutoGenReport.Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class AutoGenReportDbContext : DbContext
    {
        public AutoGenReportDbContext()
            : base("name=AutoGenReportDbContext")
        {
        }

        public virtual DbSet<AeonMallCustomer> AeonMallCustomers { get; set; }
        public virtual DbSet<AeonVNCustomer> AeonVNCustomers { get; set; }
        public virtual DbSet<AeonVNSup> AeonVNSups { get; set; }
        public virtual DbSet<AVBarcode> AVBarcodes { get; set; }
        public virtual DbSet<Config> Configs { get; set; }
        public virtual DbSet<DepositHistory> DepositHistories { get; set; }
        public virtual DbSet<ProcessedFile> ProcessedFiles { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Tenant> Tenants { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Users_Roles> Users_Roles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DepositHistory>()
                .Property(e => e.LastEdit)
                .IsFixedLength();
        }
    }
}
