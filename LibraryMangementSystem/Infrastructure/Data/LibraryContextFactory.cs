//using LMS.Shared.Spec;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Data.Common;
//using System.Data.SqlClient;
//using System.Text;

//namespace LMS.Data
//{
//    public class LibraryContextFactory
//    {
//        private readonly IApplicationAuthorizationContext _applicationAuthorizationContext;

//        public LibraryContextFactory(IApplicationAuthorizationContext applicationAuthorizationContext)
//        {
//            _applicationAuthorizationContext = applicationAuthorizationContext;
//        }

//        public LibraryContext GetLibraryContext()
//        {
//            var optionsBuilder = new DbContextOptionsBuilder<LibraryContext>();
//            var connectionStringBuilder = new SqlConnectionStringBuilder();
//            connectionStringBuilder.DataSource = "";
//            connectionStringBuilder.InitialCatalog = "";
//            connectionStringBuilder.ConnectTimeout = 30;

//            optionsBuilder.UseSqlServer<LibraryContext>()

//            return new LibraryContext(optionsBuilder.Options);
//        }
//    }
//}
