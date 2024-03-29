﻿using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ModelLib
{
    public class DatabaseAccess
    {
        // Cake CRUD
        public static List<CakeModel> LoadCake()
        {
            using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                var output = connection.Query<CakeModel>($"Select cakeId, cakeCode, cakeName, cakeName2, cakeCat, cakePrice, cakeDesc, cakeImage, cakeQuantity from cake JOIN category on cake.cakeCat = category.cateId", new DynamicParameters());
                return output.ToList();
            }
        }

        public static CakeModel FindCakeById(int cakeId)
        {
            using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                var output = connection.QuerySingle<CakeModel>($"Select cakeId, cakeCode, cakeName, cakeName2, cakeCat, cakePrice, cakeDesc, cakeImage, cakeQuantity, cateName from cake JOIN category on cake.cakeCat = category.cateId where cakeId = {cakeId}", new DynamicParameters());
                return output;
            }
        }

        public static List<CakeModel> FindCakeByCategory(int categoryId)
        {
            using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                var output = connection.Query<CakeModel>($"Select cakeId, cakeCode, cakeName, cakeName2, cakeCat, cakePrice, cakeDesc, cakeImage, cakeQuantity, cateName from cake JOIN category on cake.cakeCat = category.cateId where cakeCat = {categoryId}");
                return output.ToList();
            }
        }

        public static int SaveCake(CakeModel cake)
        {
            cake.CakeName2 = ConvertToUnSign(cake.CakeName);
            using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                int cakeId = connection.ExecuteScalar<int>("INSERT INTO cake(cakeCode, cakeName, cakeName2, cakeCat, cakePrice, cakeDesc, cakeImage, cakeQuantity) VALUES(@CakeCode, @CakeName, @CakeName2, @CakeCat, @CakePrice, @CakeDesc, @CakeImage, @CakeQuantity); SELECT last_insert_rowid()", cake);
                return cakeId;
            }
        }

        public static int UpdateCake(CakeModel cake)
        {
            cake.CakeName2 = ConvertToUnSign(cake.CakeName);
            using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                int cakeId = connection.ExecuteScalar<int>("UPDATE cake SET cakeCode = @CakeCode, cakeName = @CakeName, cakeName2 = @CakeName2, cakeCat = @CakeCat, cakePrice = @CakePrice, cakeDesc = @CakeDesc, cakeImage = @CakeImage, cakeQuantity = @CakeQuantity WHERE cakeId = @CakeId; SELECT last_insert_rowid()", cake);
                return cakeId;
            }
        }

        //Customer CRUD
        public static List<CustomerModel> LoadCustomer()
        {
            using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                var output = connection.Query<CustomerModel>($"Select * from customer", new DynamicParameters());
                return output.ToList();
            }
        }

        public static CustomerModel FindCustomerById(int customerId)
        {
            using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                var output = connection.QuerySingle<CustomerModel>($"Select * from customer where customerId = {customerId}");
                return output;
            }
        }

        public static int SaveCustomer(CustomerModel customer)
        {
            customer.CustomerName2 = ConvertToUnSign(customer.CustomerName);
            using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                int customId = connection.ExecuteScalar<int>("INSERT INTO customer(customerName, customerName2, customerTel) VALUES(@CustomerName, @CustomerName2, @CustomerTel); SELECT last_insert_rowid()", customer);
                return customId;
            }
        }

        //Order CRUD
        public static List<OrderModel> LoadOrder()
        {
            using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                var output = connection.Query<OrderModel>($"SELECT orderId, order_cake.customerId, orderStatus, shippingFee, totalPrice, orderDate, shippingAddress, isDirect, customerName, customerTel  FROM order_cake JOIN customer on order_cake.customerId = customer.customerId", new DynamicParameters());
                return output.ToList();
            }
        }

        public static OrderModel FindOrderById(int orderId)
        {
            using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                var output = connection.QuerySingle<OrderModel>($"Select * from order where orderId = {orderId}");
                return output;
            }
        }

        public static int SaveOrder(OrderModel order)
        {
            using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                int orderId = connection.ExecuteScalar<int>("INSERT INTO order_cake(customerId, orderStatus, shippingFee, totalPrice, orderDate, shippingAddress, isDirect) VALUES(@CustomerId, @OrderStatus, @ShippingFee, @TotalPrice, @OrderDate, @ShippingAddress, @IsDirect); SELECT last_insert_rowid()", order);
                return orderId;
            }
        }

        //Order Item CRUD
        public static List<OrderItem> LoadOrderItem(int orderId)
        {
            using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                var output = connection.Query<OrderItem>($"SELECT order_cake.orderId, order_item.cakeId, cake.cakeName, cake.cakeCode, order_item.cartQuantity FROM order_item JOIN order_cake ON order_item.orderId = order_cake.orderId JOIN cake on order_item.cakeId = cake.cakeId WHERE order_cake.orderId = {orderId}");
                return output.ToList();
            }
        }

        public static int SaveOrderItem(int orderId, int cakeId, int cartQuantity)
        {
            using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                int output = connection.ExecuteScalar<int>("INSERT INTO order_item(orderId, cakeId, cartQuantity) VALUES(@orderId, @cakeId, @cartQuantity); SELECT last_insert_rowid()", new { orderId, cakeId, cartQuantity});
                return output;
            }
        }

        public static int CompleteOrder(int orderId)
        {
            using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                int output = connection.ExecuteScalar<int>("UPDATE order_cake SET orderStatus = 1 WHERE orderId = @orderId; SELECT last_insert_rowid()", new { orderId });
                return output;
            }
        }

        //Category CRUD
        public static List<CategoryModel> LoadAllCategories()
        {
            using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                var output = connection.Query<CategoryModel>($"Select * from category", new DynamicParameters());
                return output.ToList();
            }
        }

        public static int SaveCategory(string categoryName)
        {
            using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                int output = connection.ExecuteScalar<int>("INSERT INTO category(cateName) VALUES(@categoryName); SELECT last_insert_rowid()", new { categoryName });
                return output;
            }
        }

        public static List<TotalPerCategory> LoadTotalByCategory()
        {
            using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                var output = connection.Query<TotalPerCategory>($"SELECT category.cateId, category.cateName, sum(order_item.cartQuantity*cake.cakePrice) AS totalCate FROM order_item JOIN order_cake ON order_item.orderId = order_cake.orderId JOIN cake on order_item.cakeId = cake.cakeId JOIN category on cake.cakeCat = category.cateId GROUP BY cateId, cateName", new DynamicParameters());
                return output.ToList();
            }
        }

        private static string LoadConnectionString(string id = "default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }

        public static string ConvertToUnSign(string input)
        {
            input = input.Trim();
            for (int i = 0x20; i < 0x30; i++)
            {
                input = input.Replace(((char)i).ToString(), " ");
            }
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string str = input.Normalize(NormalizationForm.FormD);
            string str2 = regex.Replace(str, string.Empty).Replace('đ', 'd').Replace('Đ', 'D');
            while (str2.IndexOf("?") >= 0)
            {
                str2 = str2.Remove(str2.IndexOf("?"), 1);
            }
            return str2.ToLower();
        }
    }
}
