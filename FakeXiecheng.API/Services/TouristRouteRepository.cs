using FakeXiecheng.API.Database;
using FakeXiecheng.API.Moders;
using FakeXiecheng.API.ResourceParameters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeXiecheng.API.Services
{
    public class TouristRouteRepository:ITouristRouteRepository
    {
        private readonly AppDbContext _context;

        public TouristRouteRepository(AppDbContext context)
        {
            _context = context;
        }

       
        public async  Task<TouristRoute> GetTouristRouteAsync(Guid tourisRouteId)
        {
            return await _context.TouristRoutes.Include(t => t.TouristRoutePictures).FirstOrDefaultAsync(n => n.Id == tourisRouteId);
        }

        public async Task<IEnumerable<TouristRoute>> GetTouristRoutesAsync(TouristRouteResourceParamaters paramaters)
        {
            IQueryable<TouristRoute> result= _context
                .TouristRoutes 
                .Include(t => t.TouristRoutePictures);
            if (!string.IsNullOrWhiteSpace(paramaters.Keyword))
            {
                paramaters.Keyword = paramaters.Keyword.Trim();
                result.Where(t => t.Title.Contains(paramaters.Keyword));
            }
            if (paramaters.RatingValue >= 0)
            {
                result = paramaters.RatingOperator switch
                {
                    "largerThan" => result.Where(t => t.Rating >= paramaters.RatingValue),
                    "lessThan" => result.Where(t => t.Rating <= paramaters.RatingValue),
                    _ => result.Where(t => t.Rating == paramaters.RatingValue),
                };

                //switch (ratingOperator)
                //{
                //    case "largerThan":
                //        result = result.Where(t => t.Rating >= ratingValue);
                //        break;
                //    case "lessThan":
                //        result = result.Where(t => t.Rating <= ratingValue);
                //        break;
                //    case "equalTo":
                //        result = result.Where(t => t.Rating == ratingValue);
                //        break;
                //}
            }
            return await result.ToListAsync();
        }

        public async Task<bool> TrouristRouteExistsAsync(Guid tourisRouteId)
        {
            return await _context.TouristRoutes.AnyAsync(n => n.Id == tourisRouteId);
        }

        public async Task<IEnumerable<TouristRoutePicture>> GetPictureForTouristRouteAsync(Guid tourisRouteId)
        {
            return await _context.touristRoutePictures.Where(p => p.TouristRouteId == tourisRouteId).ToListAsync();
        }

        public async Task<TouristRoutePicture> GetPictureAsync(int pictureId)
        {
            return await _context.touristRoutePictures.Where(p => p.Id == pictureId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TouristRoute>> GetTouritRoutesByIDListAsync(IEnumerable<Guid> ids)
        {
            return await _context.TouristRoutes.Where(t => ids.Contains(t.Id)).ToListAsync();
        }


        public  void AddTouristRoute(TouristRoute touristRoute)
        {
            if (touristRoute==null)
            {
                throw new ArgumentNullException(nameof(touristRoute));
            }
             _context.TouristRoutes.Add(touristRoute);
            
        }

        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }

        public async Task<ShoppingCart> GetShoppingCarByUserIdAsync(string userId)
        {
            return await _context.ShoppingCarts
                .Include(s => s.User)
                .Include(s => s.ShoppingCartItems)
                .ThenInclude(li => li.TouristRoute)
                .Where(s => s.UserId == userId)
                .FirstOrDefaultAsync();
        }

        public async Task CreateShoppingCartAsync(ShoppingCart shoppingCart)
        {
            await _context.ShoppingCarts.AddAsync(shoppingCart);
        }

        public async  Task AddShoppingCartItemAsync(LineItem lineItem)
        {
            await _context.LineItems.AddAsync(lineItem);
        }

        public async Task<LineItem> GetShoppingCartItemByItemIdAsync(int lineItemId)
        {
            return await _context.LineItems
                .Where(li => li.Id == lineItemId)
                .FirstOrDefaultAsync();
        }

        public void DeleteShoppingCartItem(LineItem lineItem)
        {
             _context.LineItems.Remove(lineItem);
        }

        public async Task<IEnumerable<LineItem>> GetShoppingCartByIdListAsync(IEnumerable<int> ids)
        {
            return await _context.LineItems
                .Where(li => ids.Contains(li.Id)).ToListAsync();
        }

        public void DeleteShoppingCartItems(IEnumerable<LineItem> lineItems)
        {
            _context.LineItems.RemoveRange(lineItems);
        }
        public async Task AddOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
        }


        public async  Task<IEnumerable<Order>> GetOrderByUserId(string userId)
        {
           return await _context.Orders.Where(o => o.UserId == userId).ToListAsync();
        }


        public async Task<Order> GetOrderById(Guid orderId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems).ThenInclude(oi => oi.TouristRoute)
                .Where(o => o.Id == orderId)
                .FirstOrDefaultAsync();
        }



        public void AddTouristRoutePicture(Guid touriteRouteId, TouristRoutePicture touristRoutePicture)
        {
            if (touriteRouteId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(touriteRouteId));
            }
            if (touristRoutePicture == null)
            {
                throw new ArgumentNullException(nameof(touristRoutePicture));
            }
            touristRoutePicture.TouristRouteId = touriteRouteId;
            _context.touristRoutePictures.Add(touristRoutePicture);
        }

        public void DeleteTouristRoute(TouristRoute touristRoute)
        {
            _context.TouristRoutes.Remove(touristRoute);
        }

        public void DeleteTouristRoutes(IEnumerable<TouristRoute> touristRoutes)
        {
            _context.TouristRoutes.RemoveRange(touristRoutes);
        }


        public void DeleteTouristRoutePicture(TouristRoutePicture picture)
        {
            _context.touristRoutePictures.Remove(picture);
        }
    }
}
