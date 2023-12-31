﻿using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using System.Xml;

namespace APP_PG_USERS_ROLES_SERVICE.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<databases> databases => Set<databases>();
        public DbSet<db_grant_privs> db_grant_privs => Set<db_grant_privs>();
        public DbSet<db_grants> db_grants => Set<db_grants>();
        public DbSet<not_typical_grants> not_typical_grants => Set<not_typical_grants>();
        public DbSet<not_typical_grants_exec> not_typical_grants_exec => Set<not_typical_grants_exec>();
        public DbSet<not_typical_grants_exec_log> not_typical_grants_exec_log => Set<not_typical_grants_exec_log>();
        public DbSet<roles> roles => Set<roles>();
        public DbSet<schemas> schemas => Set<schemas>();
        public DbSet<schm_grant_privs> schm_grant_privs => Set<schm_grant_privs>();
        public DbSet<schm_grants> schm_grants => Set<schm_grants>();
        public DbSet<servers> servers => Set<servers>();
        public DbSet<tasks_not_typical_grants> tasks_not_typical_grants => Set<tasks_not_typical_grants>();
        public DbSet<users_roles_relation> users_roles_relation => Set<users_roles_relation>();
        public DbSet<view_servers_connect_checks> view_servers_connect_checks => Set<view_servers_connect_checks>();
		public DbSet<v_users_roles_grants> v_users_roles_grants => Set<v_users_roles_grants>();
		public DbSet<jobs_status> jobs_status => Set<jobs_status>();
		public DbSet<srv_roles_relations> srv_roles_relations => Set<srv_roles_relations>();
		public DbSet<grants_status> grants_status => Set<grants_status>();
		public DbSet<view_log_not_typical_grants> view_log_not_typical_grants => Set<view_log_not_typical_grants>();
		public DbSet<roles_users_servers> roles_users_servers => Set<roles_users_servers>();
		


		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
			modelBuilder.Entity<grants_status>().HasNoKey();
			modelBuilder.Entity<roles>()
                .HasMany(e => e.users_roles_relation1)
                .WithOne(e => e.roles1)
                .HasForeignKey(e => new { e.from_role });

            modelBuilder.Entity<roles>()
                .HasMany(e => e.users_roles_relation2)
                .WithOne(e => e.roles2)
                .HasForeignKey(e => new { e.to_role });

            modelBuilder.Entity<roles>()
                .Property(o => o.id_role)
                .HasDefaultValueSql("select uuid_generate_v4()");

            modelBuilder.Entity<users_roles_relation>()
                .Property(o => o.id_users_roles_relation)
                .HasDefaultValueSql("select uuid_generate_v4()");

            modelBuilder.Entity<servers>()
                .Property(o => o.id_srv)
                .HasDefaultValueSql("select uuid_generate_v4()");

            modelBuilder.Entity<databases>()
                .Property(o => o.id_db)
                .HasDefaultValueSql("select uuid_generate_v4()");

            modelBuilder.Entity<servers>()
                        .HasMany(m => m.databases)
                        .WithOne(m => m.servers)
                        .HasForeignKey(e => e.srv_id)
                        .HasPrincipalKey(e => e.id_srv);

        }
    }
    public class PostgresContext : DbContext
    {
        public PostgresContext(DbContextOptions<PostgresContext> options) : base(options)
        {

        }

        //public DbSet<databases> databases => Set<databases>();
       
    }
}
