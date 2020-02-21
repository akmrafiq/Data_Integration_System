using Autofac;
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
            //builder.RegisterType<StockContext>()
            //       .WithParameter("connectionString", _connectionString)
            //       .WithParameter("migrationAssemblyName", _migrationAssemblyName)
            //       .InstancePerLifetimeScope();

            //builder.RegisterType<StockContext>().As<StockContext>()
            //       .WithParameter("connectionString", _connectionString)
            //       .WithParameter("migrationAssemblyName", _migrationAssemblyName)
            //       .InstancePerLifetimeScope();

            //builder.RegisterType<StockUnitOfWork>().As<IStockUnitOfWork>()
            //       .WithParameter("connectionString", _connectionString)
            //       .WithParameter("migrationAssemblyName", _migrationAssemblyName)
            //       .InstancePerLifetimeScope();

            //builder.RegisterType<CategoryRepository>().As<ICategoryRepository>()
            //    .InstancePerLifetimeScope();
          

            base.Load(builder);
        }
    }
}
