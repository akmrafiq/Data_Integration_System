using System;
using System.Collections.Generic;
using System.Text;
using Data_Integration.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data_Integration.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ExtendedIdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
    //public class ApplicationDbContext : IdentityDbContext<ExtendedIdentityUser>
    //{
    //    private string _connectionString;
    //    private string _migrationAssemblyName;

    //    public ApplicationDbContext(string connectionString, string migrationAssemblyName)
    //    {
    //        _connectionString = connectionString;
    //        _migrationAssemblyName = migrationAssemblyName;
    //    }

    //    protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
    //    {
    //        if (!dbContextOptionsBuilder.IsConfigured)
    //        {
    //            dbContextOptionsBuilder.UseSqlServer(
    //                _connectionString,
    //                m => m.MigrationsAssembly(_migrationAssemblyName));
    //        }

    //        base.OnConfiguring(dbContextOptionsBuilder);
    //    }
    //}
}
