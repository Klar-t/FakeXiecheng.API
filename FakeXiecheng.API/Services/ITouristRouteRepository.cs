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
        

        Task<ShoppingCart> GetShoppingCarByUserIdAsync(string userId);

        Task CreateShoppingCartAsync(ShoppingCart shoppingCart);

        Task AddShoppingCartItemAsync(LineItem lineItem);

        Task<LineItem> GetShoppingCartItemByItemIdAsync(int lineItemId);

        void DeleteShoppingCartItem(LineItem lineItem);

        Task<IEnumerable<LineItem>> GetShoppingCartByIdListAsync(IEnumerable<int> ids);

        void DeleteShoppingCartItems(IEnumerable<LineItem> lineItems);

        Task AddOrderAsync(Order order);

        Task<IEnumerable<Order>> GetOrderByUserId(string userId);

        Task<Order> GetOrderById(Guid orderId);

        void DeleteTouristRoute(TouristRoute touristRoute);
         
        void DeleteTouristRoutes(IEnumerable<TouristRoute> touristRoutes);

        void DeleteTouristRoutePicture(TouristRoutePicture picture);
        Task<bool> SaveAsync();
    }
}
