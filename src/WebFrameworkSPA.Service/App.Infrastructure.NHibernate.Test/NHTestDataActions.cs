﻿using System;
using NHibernate.Criterion;
using Order = App.Infrastructure.NHibernate.Test.OrdersDomain.Order;
using App.Infrastructure.NHibernate.Test.OrdersDomain;
using App.Infrastructure.NHibernate.Test.Domain;
using App.Infrastructure.NHibernate.Test.HRDomain.Domain;

namespace App.Infrastructure.NHibernate.Test
{
    public class NHTestDataActions
    {
        readonly NHTestData _generator;
        readonly Random _random = new Random();

        public NHTestDataActions(NHTestData generator)
        {
            _generator = generator;
        }

        public Customer CreateCustomer()
        {
            var customer = new Customer
            {
                FirstName = "John" + RandomString(),
                LastName = "Doe" + RandomString()
            };
            _generator.Session.Save(customer);
            _generator.EntitiesPersisted.Add(customer);
            return customer;
        }

        public Address CreateAddress()
        {
            return new Address
            {
                StreetAddress1 = "123 Main St " + RandomString(),
                StreetAddress2 = "4th Floor " + RandomString(),
                City = "Sunshine Valley",
                State = "NY",
                ZipCode = "10001"
            };
        }

        public Customer CreateCustomerInState(string state)
        {
            var customer = CreateCustomer();
            customer.Address = CreateAddress();
            customer.Address.State = state;
            return customer;
        }

        public Customer[] CreateCustomersInState(string state, int count)
        {
            var customers = new Customer[count];
            for (var i = 0; i < count; i++)
                customers[i] = CreateCustomerInState(state);
            return customers;
        }

        public Order CreateOrderForCustomer(Customer customer)
        {
            var order = new Order
            {
                Customer = customer,
                OrderDate = DateTime.Now.AddDays(-5),
                ShipDate = DateTime.Now.AddDays(5)
            };
            _generator.Session.Save(order);
            _generator.EntitiesPersisted.Add(order);
            return order;
        }

        public Order CreateOrderForProducts(Product[] products)
        {
            var order = CreateOrderForCustomer(CreateCustomer());
            foreach (var product in products)
                order.Items.Add(CreateItem(order, product));
            return order;
        }

        public Order[] CreateOrdersForCustomers(params Customer[] customers)
        {
            var orders = new Order[customers.Length];
            for (var i = 0; i < customers.Length; i++)
                orders[i] = CreateOrderForCustomer(customers[i]);
            return orders;
        }

        public Product CreateProduct()
        {
            var product = new Product
            {
                Name = "Product" + RandomString(),
                Description = "Product Description" + RandomString()
            };
            _generator.Session.Save(product);
            _generator.EntitiesPersisted.Add(product);
            return product;
        }

        public Product[] CreateProducts(int count)
        {
            var products = new Product[count];
            for (var i = 0; i < count; i++)
                products[i] = CreateProduct();
            return products;
        }

        public OrderItem CreateItem(Order order, Product product)
        {
            return new OrderItem
            {
                Order = order,
                Price = 1,
                Product = product,
                Quantity = 3,
                Store = "Internet"
            };
        }

        public MonthlySalesSummary CreateMonthlySalesSummaryForMonth(int month)
        {
            var summary = new MonthlySalesSummary
            {
                Month = month,
                SalesPersonFirstName = "Joe" + RandomString(),
                SalesPersonLastName = "Doe" + RandomString(),
                Year = 2009,
                SalesPersonId = 1,
                TotalSale = new Money
                {
                    Amount = 100,
                    Currency = "USD"
                }
            };
            _generator.Session.Save(summary);
            _generator.EntitiesPersisted.Add(summary);
            return summary;
        }

        public MonthlySalesSummary CreateMonthlySalesSummaryWithAmount(Money amount)
        {
            var summary = new MonthlySalesSummary
            {
                Month = 1,
                SalesPersonFirstName = "Joe" + RandomString(),
                SalesPersonLastName = "Doe" + RandomString(),
                Year = 2009,
                SalesPersonId = 1,
                TotalSale = amount
            };
            _generator.Session.Save(summary);
            _generator.EntitiesPersisted.Add(summary);
            return summary;
        }

        public MonthlySalesSummary CreateMonthlySalesSummaryForSalesPerson(int salespersonId)
        {
            var summary = new MonthlySalesSummary
            {
                Month = 1,
                SalesPersonFirstName = "Joe" + RandomString(),
                SalesPersonLastName = "Doe" + RandomString(),
                Year = 2009,
                SalesPersonId = salespersonId,
                TotalSale = new Money
                {
                    Amount = 100,
                    Currency = "USD"
                }
            };
            _generator.Session.Save(summary);
            _generator.EntitiesPersisted.Add(summary);
            return summary;
        }

        public SalesPerson CreateSalesPerson()
        {
            var salesPerson = new SalesPerson
            {
                FirstName = "John" + RandomString(),
                LastName = "Doe" + RandomString(),
                SalesQuota = 200,
                SalesYTD = 45000,
            };
            _generator.Session.Save(salesPerson);
            _generator.EntitiesPersisted.Add(salesPerson);
            return salesPerson;
        }

        public Customer GetCustomerById(int customerId)
        {
            var customer = _generator.Session
                .CreateCriteria<Customer>()
                .Add(Restrictions.Eq("CustomerID", customerId))
                .SetMaxResults(1)
                .UniqueResult<Customer>();

            if (customer != null)
                _generator.EntitiesPersisted.Add(customer);
            return customer;
        }

        protected string RandomString()
        {
            return _random.Next(int.MaxValue).ToString();
        }

        public Order GetOrderById(int orderId)
        {
            var order = _generator.Session
                .CreateCriteria<Order>()
                .Add(Restrictions.Eq("OrderID", orderId))
                .UniqueResult<Order>();

            if (order != null)
                _generator.EntitiesPersisted.Add(order);
            return order;
        }

        public SalesPerson GetSalesPersonById(int id)
        {
            var salesPerson = _generator.Session
                .CreateCriteria<SalesPerson>()
                .Add(Restrictions.Eq("Id", id))
                .UniqueResult<SalesPerson>();

            if (salesPerson != null)
                _generator.EntitiesPersisted.Add(salesPerson);
            return salesPerson;
        }
    }
}