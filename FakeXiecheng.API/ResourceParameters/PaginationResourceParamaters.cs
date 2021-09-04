using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeXiecheng.API.ResourceParameters
{
    public class PaginationResourceParamaters
    {

        private int _pageNumber = 1;

        public int PageNumber
        {
            get
            {
                return _pageNumber;
            }
            set
            {
                if (value >= 1)
                {
                    _pageNumber = value;
                }
            }
        }

        private int _pageSize = 5;
        const int maxPageSize = 50;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                if (value >= 1)
                {
                    PageSize = (value > maxPageSize) ? maxPageSize : value;
                }
            }
        }

    }
}
