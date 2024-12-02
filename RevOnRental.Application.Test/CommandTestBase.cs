//using MediatR;
//using RevOnRental.Infrastructure.Data;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace RevOnRental.Application.Test
//{
//    public class CommandTestBase : IDisposable
//    {
//        public CommandTestBase()
//        {
//            Context = AppDbContextFactory.Create();
//        }

//        public AppDbContext Context { get; }
//        public IMediator Mediator;

//        public void Dispose()
//        {
//            AppDbContextFactory.Destroy(Context);
//        }
//    }
//}
