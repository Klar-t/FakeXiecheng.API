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

       
        public TouristRoute GetTouristRoute(Guid tourisRouteId)
        {
            return _context.TouristRoutes.Include(t => t.TouristRoutePictures).FirstOrDefault(n => n.Id == tourisRouteId);
        }

        public IEnumerable<TouristRoute> GetTouristRoutes(TouristRouteResourceParamaters paramaters)
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
            return result.ToList();
        }

        public bool TrouristRouteExists(Guid tourisRouteId)
        {
            return _context.TouristRoutes.Any(n => n.Id == tourisRouteId);
        }

        public IEnumerable<TouristRoutePicture> GetPictureForTouristRoute(Guid tourisRouteId)
        {
            return _context.touristRoutePictures.Where(p => p.TouristRouteId == tourisRouteId);
        }

        public TouristRoutePicture GetPicture(int pictureId)
        {
            return _context.touristRoutePictures.Where(p => p.Id == pictureId).FirstOrDefault();
        }

        public void AddTouristRoute(TouristRoute touristRoute)
        {
            if (touristRoute==null)
            {
                throw new ArgumentNullException(nameof(touristRoute));
            }
            _context.TouristRoutes.Add(touristRoute);
            
        }

        public bool Save()
        {
            return (_context.SaveChanges()>=0);
        }

        public async Task<ShoppingCart> GetShoppingCarByUserId(string userId)
        {
            return await _context.ShoppingCarts
                .Include(s => s.User)
                .Include(s => s.ShoppingCartItems)
                .ThenInclude(li => li.TouristRoute)
                .Where(s => s.UserId == userId)
                .FirstOrDefaultAsync();
        }

        public async Task CreateShoppingCart(ShoppingCart shoppingCart)
        {
            await _context.ShoppingCarts.AddAsync(shoppingCart);
        }

        public async  Task AddShoppingCartItem(LineItem lineItem)
        {
            await _context.LineItems.AddAsync(lineItem);
        }

        public async Task<LineItem> GetShoppingCartItemByItemId(int lineItemId)
        {
            return await _context.LineItems
                .Where(li => li.Id == lineItemId)
                .FirstOrDefaultAsync();
        }

        public async void DeleteShoppingCartItem(LineItem lineItem)
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
    }
}
