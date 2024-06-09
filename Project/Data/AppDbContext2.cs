using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data
{
    internal class AppDbContext2 : DbContext
    {
        public AppDbContext2()
           : base("name=application1Entities3")
        {
        }

        public virtual DbSet<Project.Models.userType> userType { get; set; }
        public virtual DbSet<Project.Models.requestStatus> requestStatus { get; set; }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Configuration here, if needed
        }
    }
}
