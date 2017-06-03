using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using GameStore.Models;
using GameStore.Helpers;

namespace GameStore.Controllers
{
    public class SearchController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        public const int DefaultPageNumber = 1;
        public const int DefaultPageSize = 3;

        private const int minPageSize = 1;
        private const int maxPageSize = 30;
        private const string sessionPageNumber = "Product_Current_Page";
        private const string sessionPageSize = "Product_Page_Size";

        private int GetPageSize(int? pageSize)
        {
            int size;
            if (pageSize.HasValue)
            { size = pageSize.Value; }
            else if (Session.IsSet(sessionPageSize))
            { size = Session.Get<int>(sessionPageSize); }
            else
            { return DefaultPageSize; }

            if (size < minPageSize) { return minPageSize; }
            if (size > maxPageSize) { return maxPageSize; }

            return size;
        }

        private int GetPageNumber(int? pageNumber, int pageSize, out int maxPageNumber)
        {
            int count = db.Products.Count();

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

            if (pageNumber.HasValue)
            { number = pageNumber.Value; }
            else if (Session.IsSet(sessionPageNumber))
            { number = Session.Get<int>(sessionPageNumber); }
            else
            { return DefaultPageNumber; }

            if (number < 1) { return 1; }
            if (number > maxPageNumber) { return maxPageNumber; }

            return number;
        }
        
        public ActionResult Index(int? pageNumber, int? pageSize)
        {
            int ps = GetPageSize(pageSize);
            int maxPageNumber;
            int pn = GetPageNumber(pageNumber, ps, out maxPageNumber);

            Session.Set(sessionPageSize, ps);
            Session.Set(sessionPageNumber, pn);

            ViewBag.CurrentPage = pn;
            ViewBag.MaxPages = maxPageNumber;

            if (maxPageNumber > 0)
            {
                var products = db.Products.Include(p => p.Platform)
                    .OrderBy(p => p.DateAdded)
                    .Skip((pn - 1) * ps)
                    .Take(ps);

                return View(products.ToList());
            }

            return View(new List<Product>());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
