//using System;
//using Microsoft.Extensions.DependencyInjection;
//using Xunit;

//namespace GameATron4000.Infrastructure.UnitTest
//{
//    public class Test
//    {
//        [Fact]
//        public void SmokeTest()
//        {
//            var services = new ServiceCollection();
//            services.AddInfrastructure();
//            services.AddDomain();

//            var serviceProvider = services.BuildServiceProvider();

//            var bootstrapper = new Bootstrapper(serviceProvider);
//            bootstrapper.StartGame();
//        }
//    }
//}
