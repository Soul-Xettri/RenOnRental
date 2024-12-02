//using Microsoft.Data.Sqlite;
//using Microsoft.EntityFrameworkCore;
//using Moq;
//using NUnit.Framework.Constraints;
//using RevOnRental.Domain.Models;
//using RevOnRental.Infrastructure.Data;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace RevOnRental.Application.Test
//{
//    public class AppDbContextFactory
//    {

//        public static AppDbContext Create()
//        {
//            var connection = new SqliteConnection("DataSource=:memory:");
//            connection.Open();

//            var options = new DbContextOptionsBuilder<AppDbContext>()
//            .UseSqlite(connection)
//            .Options;
//            AppDbContext context = new AppDbContext(options);
//            context.Database.EnsureCreated();
//            SeedBusinessData(context);

//            return context;
//        }

//        private static void SeedBusinessData(AppDbContext context)
//        {
//            context.Businesses.AddRange(
//                new Business
//                {
//                    Id = 1,
//                    BusinessName = "Test",
//                    BusinessRegistrationNumber = "3456",
//                    BusinessType = Domain.Enums.BusinessType.Company,
//                    CreatedDate = DateTime.Now
//                }
//            );
//            context.SaveChanges();
//        }


//        public static void Destroy(AppDbContext context)
//        {
//            context.Database.EnsureDeleted();

//            context.Dispose();
//        }
//    }
//}
