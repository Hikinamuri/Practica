﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Project.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class application1Entities1 : DbContext
    {
        public application1Entities1()
            : base("name=application1Entities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<autoriz> autoriz { get; set; }
        public virtual DbSet<comment> comment { get; set; }
        public virtual DbSet<commentMasterClient> commentMasterClient { get; set; }
        public virtual DbSet<homeTech> homeTech { get; set; }
        public virtual DbSet<request> request { get; set; }
        public virtual DbSet<requestClientMaster> requestClientMaster { get; set; }
        public virtual DbSet<requestHomeTech> requestHomeTech { get; set; }
        public virtual DbSet<status> status { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<type> type { get; set; }
        public virtual DbSet<users> users { get; set; }
    }
}
