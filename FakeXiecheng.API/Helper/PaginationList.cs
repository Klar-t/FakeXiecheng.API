using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeXiecheng.API.Helper
{
    public class PaginationList<T>:List<T>
    {
        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public PaginationList(int currentpage, int pagesize,List<T> items)
        {
            CurrentPage = currentpage;
            PageSize = pagesize;
            AddRange(items);
        }


        public static async Task<PaginationList<T>> CreateAsync(int currentpage, int pagesize,IQueryable<T> result)
        {
            var skip = (currentpage - 1) * pagesize;
            result = result.Skip(skip);
            //以pagesize为标准显示一定量的数据
            result = result.Take(pagesize);
            var items= await result.ToListAsync();


            return new PaginationList<T>(currentpage, pagesize, items);
        }


    }
}
