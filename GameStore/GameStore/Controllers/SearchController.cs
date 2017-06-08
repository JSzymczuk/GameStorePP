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
        
        private const string sessionPageNumber = "Product_Current_Page";
        private const string sessionPageSize = "Product_Page_Size";        
        
        public ActionResult Index(int? pageNumber, int? pageSize)
        {
            var pageInfo = ProductPageHelper.GetProducts(db.Products.Include(p => p.Platform), pageNumber, pageSize, Session.Get<int?>(sessionPageNumber), Session.Get<int?>(sessionPageSize), p => p.State == ProductState.Visible);
                        
            Session.Set(sessionPageSize, pageInfo.PageSize);
            Session.Set(sessionPageNumber, pageInfo.CurrentPage);

            ViewBag.CurrentPage = pageInfo.CurrentPage;
            ViewBag.MaxPages = pageInfo.TotalPages;
            
            return View(pageInfo.Products);
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
