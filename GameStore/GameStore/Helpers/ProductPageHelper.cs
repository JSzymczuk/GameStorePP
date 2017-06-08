using GameStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameStore.Helpers
{
    public struct ProductsPageSelection
    {
        public List<Product> Products;
        public int PageSize;
        public int CurrentPage;
        public int TotalPages;
    }

    public static class ProductPageConstants
    {
        public const int DefaultPageNumber = 1;
        public const int DefaultPageSize = 12;

        public const int MinPageSize = 1;
        public const int MaxPageSize = 30;
    }

    public static class ProductPageHelper
    {
        public static ProductsPageSelection GetProducts(IEnumerable<Product> products, 
            int? pageNumber, int? pageSize, int? currentPageNumber, int? currentPageSize,
            Predicate<Product> pred)
        {
            int ps = GetPageSize(pageSize, currentPageSize);
            int mpn;
            int pn = GetPageNumber(products, pageNumber, ps, out mpn, currentPageNumber);

            return new ProductsPageSelection
            {
                 CurrentPage= pn, 
                 PageSize=ps,
                 TotalPages = mpn,
                 Products = products.Where(p => pred(p)).OrderBy(p => p.DateAdded)
                    .Skip((pn - 1) * ps).Take(ps).ToList()
            };
        }
 
        private static int GetPageSize(int? pageSize, int? currentPageSize)
        {
            int size;
            if (pageSize.HasValue && pageSize != 0)
            { size = pageSize.Value; }
            else if (currentPageSize.HasValue)
            { size = currentPageSize.Value; }
            else
            { return ProductPageConstants.DefaultPageSize; }

            if (size < ProductPageConstants.MinPageSize) { return ProductPageConstants.MinPageSize; }
            if (size > ProductPageConstants.MaxPageSize) { return ProductPageConstants.MaxPageSize; }

            return size;
        }

        private static int GetPageNumber(IEnumerable<Product> products, int? pageNumber, int pageSize, out int maxPageNumber, int? currentPageNumber)
        {
            int count = products.Count();

            if (count == 0)
            {
                maxPageNumber = 0;
                return 0;
            }

            if (count % pageSize > 0)
            { maxPageNumber = count / pageSize + 1; }
            else
            { maxPageNumber = count / pageSize; }

            int number;

            if (pageNumber.HasValue && pageNumber != 0)
            { number = pageNumber.Value; }
            else if (currentPageNumber.HasValue)
            { number = currentPageNumber.Value; }
            else
            { return ProductPageConstants.DefaultPageNumber; }

            if (number < 1) { return 1; }
            if (number > maxPageNumber) { return maxPageNumber; }

            return number;
        }
    }
}