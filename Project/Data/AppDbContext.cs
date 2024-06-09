using Project.Models;
using System.Data.Entity;

namespace Project.Data
{
    internal class AppDbContext : DbContext
    {
        public AppDbContext()
           : base("name=application1Entities")
        {
        }

        public virtual DbSet<users> users { get; set; }
        public virtual DbSet<autoriz> autoriz { get; set; }
        public virtual DbSet<request> request { get; set; }
        public virtual DbSet<requestHomeTech> requestHomeTech { get; set; }
        public virtual DbSet<homeTech> homeTech { get; set; }
        public virtual DbSet<type> type { get; set; }
        public virtual DbSet<status> status { get; set; }
        public virtual DbSet<comment> comment { get; set; }
        public virtual DbSet<requestClientMaster> requestClientMaster { get; set; }
        public virtual DbSet<commentMasterClient> commentMasterClient { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Configuration here, if needed
        }
    }
    
}
