using System;
using System.Linq;
using System.Data.Entity;
//using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using App.Infrastructure.EntityFramework.Test.OrdersDomain.Mappings;
using System.Reflection;
using System.Data.Entity.ModelConfiguration;
using System.Data;

namespace App.Infrastructure.EntityFramework.Test.OrdersDomain
{
    public class OrdersDomainContext : DbContext
    {
        public OrdersDomainContext()
            : base("TestDb")
        {
        }
        public OrdersDomainContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        //public DbSet<Customer> Customers { get; set; }
        //public DbSet<Order> Orders { get; set; }
        //public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Customer>().HasEntitySetName("Customers");
            //modelBuilder.Entity<Order>().HasEntitySetName("Orders");
            //modelBuilder.Entity<OrderItem>().HasEntitySetName("OrderItems");
            //modelBuilder.Entity<Product>().HasEntitySetName("Products");

            //dynamically load all configuration
            System.Type configType = typeof(CustomerMap);   //any of your configuration classes here
            var typesToRegister = Assembly.GetAssembly(configType).GetTypes()
            .Where(type => !String.IsNullOrEmpty(type.Namespace))
            .Where(type => type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }
            //...or do it manually below. For example,
            //modelBuilder.Configurations.Add(new LanguageMap());
            base.OnModelCreating(modelBuilder);
        }

        public System.Data.Entity.Core.Objects.ObjectContext Context
        {
            get { return ((IObjectContextAdapter)this).ObjectContext; }
        }

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }
    }
}
