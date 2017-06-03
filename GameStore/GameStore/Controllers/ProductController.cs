using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GameStore.Models;
using GameStore.Helpers;

namespace GameStore.Controllers
{
    public class ProductController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Search");
        }
        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(new ProductDetailsViewModel
            {
                Id = product.Id, 
                Name = product.Name,
                PlatformName = product.Platform.Name,
                Studio = product.Studio,
                CoverPath = product.CoverPath,
                Description = product.Description,
                Price = product.Price,
                ReleaseDate = product.ReleaseDate,
                Pegi = product.Pegi.ToList().OrderByDescending(p => p.Priority).ToList(),
                MinimalRequirements = product.MinimumRequirements,
                RecommendedRequirements = product.RecommendedRequirements
            });
        }
        
        public ActionResult Create()
        {
            var pegiList = db.Pegi.OrderByDescending(p => p.Priority).ToList();
            ViewBag.PegiAge = pegiList.Where(p => p.IsAgeRating);
            ViewBag.PlatformId = new SelectList(db.Platforms, "Id", "Name");

            return View(new ProductCreateViewModel
            {
                PegiContent = pegiList.Where(p => !p.IsAgeRating).ToPegiInfo()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductCreateViewModel product)
        {
            if (ModelState.IsValid)
            {
                var pegiIds = product.GetPegiIds().ToList();
                var pegi = db.Pegi.Where(p => pegiIds.Any(id => id == p.Id)).ToList();
                Requirements minReqs = product.MinimalRequirements.IsAnyPropertySet() ? product.MinimalRequirements : null;
                Requirements recReqs = product.RecommendedRequirements.IsAnyPropertySet() ? product.RecommendedRequirements : null;

                db.Products.Add(new Product
                {
                    Name = product.Name,
                    PlatformId = product.PlatformId,
                    Description = product.Description,
                    Studio = product.Studio,
                    Price = product.Price,
                    ReleaseDate = product.ReleaseDate,
                    CoverPath = product.CoverPath,
                    ThumbPath = product.ThumbPath,
                    Pegi = pegi,
                    IsVisible = true,
                    DateAdded = DateTime.Now,
                    MinimumRequirements = minReqs,
                    RecommendedRequirements = recReqs,
                });
                db.SaveChanges();
                return RedirectToAction("Manage");
            }

            ViewBag.PlatformId = new SelectList(db.Platforms, "Id", "Name", product.PlatformId);
            var pegiList = db.Pegi.OrderByDescending(p => p.Priority).ToList();
            ViewBag.PegiAge = pegiList.Where(p => p.IsAgeRating);
            return View(product);
        }
        
        public ActionResult Edit(int? id)
        {
            if (id == null) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

            Product product = db.Products.Include(p => p.MinimumRequirements)
                .Include(p => p.RecommendedRequirements)
                .FirstOrDefault(p => p.Id == id);

            if (product == null) { return HttpNotFound(); }

            var pegiList = db.Pegi.OrderByDescending(p => p.Priority).ToList();
            ViewBag.PlatformSelection = new SelectList(db.Platforms, "Id", "Name", product.PlatformId);            
            ViewBag.PegiAge = pegiList.Where(p => p.IsAgeRating);
            var pegiContentList = pegiList.Where(p => !p.IsAgeRating).ToPegiInfo();
            var productPegiContent = product.Pegi.ToList();
            for (int i = 0; i < pegiContentList.Count; i++)
            {
                pegiContentList[i].Checked = productPegiContent
                    .Any(pg => pg.Id == pegiContentList[i].Id);
            }

            return View(new ProductCreateViewModel
            {
                Id = product.Id,
                Name = product.Name, Price = product.Price,
                DateAdded = product.DateAdded,
                IsVisible = product.IsVisible,
                ReleaseDate = product.ReleaseDate,
                Studio = product.Studio,
                Description = product.Description,
                PlatformId = product.PlatformId,
                MinimalRequirements = product.MinimumRequirements,
                RecommendedRequirements = product.RecommendedRequirements,
                PegiAgeId = productPegiContent.FirstOrDefault(p => p.IsAgeRating).Id,
                PegiContent = pegiContentList,
                CoverPath = product.CoverPath, 
                ThumbPath = product.ThumbPath
            });            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductCreateViewModel product)
        {
            if (ModelState.IsValid)
            {
                var pegiIds = product.GetPegiIds().ToList();
                var pegi = db.Pegi.Where(p => pegiIds.Any(id => id == p.Id)).ToList();
                Requirements minReqs = product.MinimalRequirements.IsAnyPropertySet() ? product.MinimalRequirements : null;
                Requirements recReqs = product.RecommendedRequirements.IsAnyPropertySet() ? product.RecommendedRequirements : null;

                Product productModel = new Product
                {
                    Id = product.Id,
                    Name = product.Name,
                    PlatformId = product.PlatformId,
                    Description = product.Description,
                    Studio = product.Studio,
                    Price = product.Price,
                    ReleaseDate = product.ReleaseDate,
                    CoverPath = product.CoverPath,
                    ThumbPath = product.ThumbPath,
                    Pegi = pegi,
                    IsVisible = true,
                    DateAdded = DateTime.Now,
                    MinimumRequirements = minReqs,
                    RecommendedRequirements = recReqs
                };

                db.Entry(productModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var pegiList = db.Pegi.OrderByDescending(p => p.Priority).ToList();
            ViewBag.PegiAge = pegiList.Where(p => p.IsAgeRating);
            ViewBag.PlatformId = new SelectList(db.Platforms, "Id", "Name", product.PlatformId);
            return View(product);            
        }
        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
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
