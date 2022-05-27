﻿using Microsoft.EntityFrameworkCore;

namespace Cosmos
{
    public class DatabaseContext : DbContext
    {
        private readonly IConfiguration config;
        private readonly ILoggerFactory loggerFactory;
        private readonly IHttpContextAccessor httpContextAccessor;

        public DatabaseContext (IConfiguration _config, ILoggerFactory _loggerFactory, IHttpContextAccessor _httpContextAccessor)
        {
            config = _config;
            loggerFactory = _loggerFactory;
            httpContextAccessor = _httpContextAccessor;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(loggerFactory);
            optionsBuilder.EnableSensitiveDataLogging();
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}
