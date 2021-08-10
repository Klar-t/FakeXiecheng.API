//using FakeXiecheng.API.Moders;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace FakeXiecheng.API.Services
//{
//    public class MockTouristRouteRepository : ITouristRouteRepository
//    {
//        private List<TouristRoute> _routes;

//        public MockTouristRouteRepository() 
//        {
//            if (_routes == null)
//            {
//                InitiaalizeTourisRoute();
//            }
//        }

//        private void InitiaalizeTourisRoute()
//        {
//            _routes = new List<TouristRoute>
//            {
//                new TouristRoute{
//                    Id=Guid.NewGuid(),
//                    Title="黄山",
//                    Description="黄山真好玩",
//                    OriginalPrice=1299,
//                    Features="<p>吃住行都方便<p>",
//                    Fees="<p>交通费自理<p>",
//                    Notes="<p>小心危险<p>"
//                },
//                new TouristRoute{
//                    Id=Guid.NewGuid(),
//                    Title="华山",
//                    Description="黄山真好玩",
//                    OriginalPrice=1299,
//                    Features="<p>吃住行都方便<p>",
//                    Fees="<p>交通费自理<p>",
//                    Notes="<p>小心危险<p>"
//                }
//            };
//        }



//        public TouristRoute GetTouristRoute(Guid tourisRouteId)
//        {
//            return _routes.FirstOrDefault(n => n.Id == tourisRouteId);
//        }

//        public IEnumerable<TouristRoute> GetTouristRoutes()
//        {
//            return _routes;
//        }
//    }
//}
