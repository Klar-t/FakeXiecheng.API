using FakeXiecheng.API.Moders;
using FakeXiecheng.API.ResourceParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeXiecheng.API.Services
{
    public interface ITouristRouteRepository
    {
        Task<IEnumerable<TouristRoute>> GetTouristRoutesAsync(TouristRouteResourceParamaters paramaters);

        Task<TouristRoute> GetTouristRouteAsync(Guid tourisRouteId);

        Task<bool> TrouristRouteExistsAsync(Guid tourisRouteId);

        Task<IEnumerable<TouristRoutePicture>> GetPictureForTouristRouteAsync(Guid tourisRouteId);

        Task<TouristRoutePicture> GetPictureAsync(int pictureId);

        Task<IEnumerable<TouristRoute>> GetTouritRoutesByIDListAsync(IEnumerable<Guid> ids);




        void AddTouristRoute(TouristRoute touristRoute);

        void AddTouristRoutePicture(Guid touriteRouteId, TouristRoutePicture touristRoutePicture);
        

        Task<ShoppingCart> GetShoppingCarByUserId(string userId);

        Task CreateShoppingCart(ShoppingCart shoppingCart);

        Task AddShoppingCartItem(LineItem lineItem);

        Task<LineItem> GetShoppingCartItemByItemId(int lineItemId);

        void DeleteShoppingCartItem(LineItem lineItem);

        Task<IEnumerable<LineItem>> GetShoppingCartByIdListAsync(IEnumerable<int> ids);

        void DeleteShoppingCartItems(IEnumerable<LineItem> lineItems);


        void DeleteTouristRoute(TouristRoute touristRoute);
         
        void DeleteTouristRoutes(IEnumerable<TouristRoute> touristRoutes);

        void DeleteTouristRoutePicture(TouristRoutePicture picture);
        Task<bool> SaveAsync();
    }
}
