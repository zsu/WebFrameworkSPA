using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Infrastructure
{
    public class PagingOptions
    {
        private int _pageSize;
        private int _pageNumber;

        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                if (value < 1) throw new ArgumentOutOfRangeException("Page size");
                _pageSize = value;
            }
        }

        public int PageNumber
        {
            get { return _pageNumber; }
            set
            {
                if (value < 1) throw new ArgumentOutOfRangeException("Page number");
                _pageNumber = value;
            }
        }

        public int TotalItems { get; set; }

        public int TotalPages
        {
            get { return (int)Math.Ceiling((double)(TotalItems / PageSize)); }
        }

        public PagingOptions()
        {
            _pageSize = 20;
            _pageNumber = 1;
        }
    }
}