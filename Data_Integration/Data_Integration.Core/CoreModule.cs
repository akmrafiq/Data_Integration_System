using Autofac;
using Data_Integration.Core.Contexts;
using Data_Integration.Core.Repositories;
using Data_Integration.Core.UnitOfWorks;
using Microsoft.Extensions.Configuration;
using System;

namespace Data_Integration.Core
{
    public class CoreModule:Module
    {
        private readonly string _connectionString;
        private readonly string _migrationAssemblyName;
        private readonly IConfiguration _configuration;

        public CoreModule(IConfiguration configuration, string connectionStringName, string migrationAssemblyName)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString(connectionStringName);
            _migrationAssemblyName = migrationAssemblyName;
        }
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DataContext>()
                   .WithParameter("connectionString", _connectionString)
                   .WithParameter("migrationAssemblyName", _migrationAssemblyName)
                   .InstancePerLifetimeScope();

            builder.RegisterType<DataContext>().As<DataContext>()
                   .WithParameter("connectionString", _connectionString)
                   .WithParameter("migrationAssemblyName", _migrationAssemblyName)
                   .InstancePerLifetimeScope();

            builder.RegisterType<DataUnitOfWork>().As<IDataUnitOfWork>()
                   .WithParameter("connectionString", _connectionString)
                   .WithParameter("migrationAssemblyName", _migrationAssemblyName)
                   .InstancePerLifetimeScope();

            builder.RegisterType<DataDetailsRepository>().As<IDataDetailsRepository>()
                .InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}
