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
using System.IO;
using System.Drawing;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;

namespace GameStore.Controllers
{
    public class ProductController : Controller
    {
        public const int ProductCoverThumbWidth = 64 * 3;
        public const int ProductCoverThumbHeight = 64 * 4;

        private const string sessionPageNumber = "Manage_Current_Page";
        private const string sessionPageSize = "Manage_Page_Size";

        private ApplicationUserManager _userManager;
        private ApplicationDbContext db = new ApplicationDbContext();

        public ProductController() { }

        public ProductController(ApplicationUserManager userManager) { UserManager = userManager; }
        
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult Manage(int? pageNumber, int? pageSize)
        {
            var pageInfo = ProductPageHelper.GetProducts(db.Products.Include(p => p.Platform), pageNumber, pageSize, Session.Get<int?>(sessionPageNumber), Session.Get<int?>(sessionPageSize), p => true);

            Session.Set(sessionPageSize, pageInfo.PageSize);
            Session.Set(sessionPageNumber, pageInfo.CurrentPage);

            ViewBag.CurrentPage = pageInfo.CurrentPage;
            ViewBag.MaxPages = pageInfo.TotalPages;

            return View(pageInfo.Products);
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
                CoverPath = product.ThumbPath,
                Description = product.Description,
                Price = product.Price,
                ReleaseDate = product.ReleaseDate,
                Pegi = product.Pegi.ToList().OrderByDescending(p => p.Priority).ToList(),
                MinimalRequirements = product.MinimumRequirements,
                RecommendedRequirements = product.RecommendedRequirements,
                Quantity = product.Quantity,
                State = product.State.GetDisplayName(),
                AddedInfo = FormatStateUserDateChange(product.DateAdded, product.AddedBy),
                EditedInfo = FormatStateUserDateChange(product.DateEdited, product.EditedBy),
                DeletedInfo = FormatStateUserDateChange(product.DateDeleted, product.DeletedBy)
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
        public async Task<ActionResult> Create(ProductCreateViewModel product, HttpPostedFileBase cover)
        {
            if (ModelState.IsValid)
            {                
                CoverUploadResult coverUploadResult = FileUpload(cover);

                var pegiIds = product.GetPegiIds().ToList();
                var pegi = db.Pegi.Where(p => pegiIds.Any(id => id == p.Id)).ToList();
                Requirements minReqs = product.MinimalRequirements.IsAnyPropertySet() ? product.MinimalRequirements : null;
                Requirements recReqs = product.RecommendedRequirements.IsAnyPropertySet() ? product.RecommendedRequirements : null;
                string currentUserId = await GetCurrentUserId();

                db.Products.Add(new Product
                {
                    Name = product.Name,
                    PlatformId = product.PlatformId,
                    Description = product.Description,
                    Studio = product.Studio,
                    Price = product.Price,
                    ReleaseDate = product.ReleaseDate,
                    CoverPath = coverUploadResult.Succeeded ? coverUploadResult.Cover : null,
                    ThumbPath = coverUploadResult.Succeeded ? coverUploadResult.Thumb : null,
                    Pegi = pegi,
                    DateAdded = DateTime.Now,
                    State = ProductState.Created,
                    AddedById = currentUserId,
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
                ReleaseDate = product.ReleaseDate,
                Studio = product.Studio,
                Description = product.Description,
                PlatformId = product.PlatformId,
                MinimalRequirements = product.MinimumRequirements,
                RecommendedRequirements = product.RecommendedRequirements,
                PegiAgeId = productPegiContent.FirstOrDefault(p => p.IsAgeRating)?.Id ?? 0,
                PegiContent = pegiContentList,
                CoverPath = product.CoverPath, 
                ThumbPath = product.ThumbPath
            });            
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ProductCreateViewModel productVM, HttpPostedFileBase cover)
        {
            if (ModelState.IsValid)
            {
                Product product = db.Products.Find(productVM.Id);

                if (product != null)
                {
                    product.Name = productVM.Name;
                    product.PlatformId = productVM.PlatformId;
                    product.Description = productVM.Description;
                    product.Studio = productVM.Studio;
                    product.Price = productVM.Price;
                    product.ReleaseDate = productVM.ReleaseDate;
                    product.DateEdited = DateTime.Now;
                    product.EditedById = await GetCurrentUserId();
                    
                    if (cover != null)
                    {
                        var coverUploadResult = FileUpload(cover);
                        if (coverUploadResult.Succeeded)
                        {
                            product.CoverPath = coverUploadResult.Cover;
                            product.ThumbPath = coverUploadResult.Thumb;
                        }
                    }
                    else if (productVM.DeleteCover)
                    {
                        product.CoverPath = null;
                        product.ThumbPath = null;
                    }

                    Requirements newMinReqs = productVM.MinimalRequirements.IsAnyPropertySet() ? productVM.MinimalRequirements : null;
                    UpdateRequirements(product, newMinReqs, true);

                    Requirements newRecReqs = productVM.RecommendedRequirements.IsAnyPropertySet() ? productVM.RecommendedRequirements : null;
                    UpdateRequirements(product, newRecReqs, false);

                    var pegiIds = productVM.GetPegiIds().ToList();
                    var pegi = db.Pegi.Where(p => pegiIds.Any(id => id == p.Id)).ToList();
                    product.Pegi.Clear();
                    foreach (var pg in pegi) { product.Pegi.Add(pg); }

                    db.Entry(product).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Manage");
                }
            }

            var pegiList = db.Pegi.OrderByDescending(p => p.Priority).ToList();
            ViewBag.PegiAge = pegiList.Where(p => p.IsAgeRating);
            ViewBag.PlatformId = new SelectList(db.Platforms, "Id", "Name", productVM.PlatformId);
            return View(productVM);            
        }
        
        public async Task<ActionResult> Delete(int? id)
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
            else
            {
                product.DeletedById = await GetCurrentUserId();
                product.DateDeleted = DateTime.Now;
                product.State = ProductState.Deleted;
                db.SaveChanges();
            }
            return RedirectToAction("Manage");
        }
        
        public PartialViewResult ConfirmAll()
        {
            var some = db.Products.Where(p => p.State == ProductState.Created).ToList();
            some.ForEach(p => p.State = ProductState.Visible);
            db.SaveChanges();
            return RefreshProductTablePage();
        }

        public PartialViewResult Confirm(int? id)
        {
            var product = db.Products.Find(id);
            if (product != null)
            {
                product.State = ProductState.Visible;
                db.SaveChanges();
            }
            return RefreshProductTablePage();
        }

        public PartialViewResult ChangeQuantity(int? id, int quantity)
        {
            var product = db.Products.Find(id);
            if (product != null)
            {
                product.Quantity = Math.Max(product.Quantity + quantity, 0);
                db.SaveChanges();
            }
            return RefreshProductTablePage();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private struct CoverUploadResult
        {
            public bool Succeeded { get; set; }
            public string Cover { get; set; }
            public string Thumb { get; set; }
        }
        
        private string FormatStateUserDateChange(DateTime? date, AppUser user)
        {
            return date.HasValue ? date.Value.ToString("HH:mm:ss ")
                + date.Value.ToDisplayableDate() + " przez "
                + (user != null ? user.UserName : "niezalogowanego użytkownika")
                : string.Empty;
        }

        private async Task<string> GetCurrentUserId()
        {
            var currentUser = await UserManager.FindByNameAsync(User.Identity.Name);
            return currentUser != null ? currentUser.Id : null;
        }

        private CoverUploadResult FileUpload(HttpPostedFileBase file)
        {
            if (file != null) { return FileUpload(file, file.FileName); }
            return new CoverUploadResult { Succeeded = false };
        }

        private CoverUploadResult FileUpload(HttpPostedFileBase file, string fileName)
        {
            CoverUploadResult result = new CoverUploadResult();
            try
            {
                string coverName = "Covers/" + fileName;
                string thumbName = "Covers/Thumbs/" + fileName;
                string fullName = Path.Combine(Server.MapPath("~/Images"), coverName);
                file.SaveAs(fullName);
                Image image = Image.FromFile(fullName);
                Image thumb = image.GetThumbnailImage(
                    ProductCoverThumbWidth, 
                    ProductCoverThumbHeight, 
                    () => false, IntPtr.Zero);
                fullName = Path.Combine(Server.MapPath("~/Images"), thumbName);
                thumb.Save(fullName);
                result.Cover = coverName;
                result.Thumb = thumbName;
                result.Succeeded = true;
            }
            catch (Exception)
            {
                result.Succeeded = false;
            }
            return result;
        }

        private void UpdateRequirements(Product product, Requirements newReqs, bool minReqs)
        {
            Requirements reqs = db.Requirements.Find(minReqs ?
                product.MinimumRequirementsId
                : product.RecommendedRequirementsId);

            if (reqs != null)
            {
                // Było wcześniej.
                if (newReqs != null)
                {
                    // I jest nadal. Ale inne. Zaktualizować.
                    reqs.OS = newReqs.OS;
                    reqs.CPU = newReqs.CPU;
                    reqs.GPU = newReqs.GPU;
                    reqs.RAM = newReqs.RAM;
                    reqs.HDD = newReqs.HDD;
                    reqs.DirectX = newReqs.DirectX;
                    db.Entry(reqs).State = EntityState.Modified;
                }
                else
                {
                    // Ale nie ma. Kasujemy.
                    db.Requirements.Remove(reqs);
                    if (minReqs)
                    { product.MinimumRequirementsId = null; }
                    else
                    { product.RecommendedRequirementsId = null; }
                }
            }
            else if (newReqs != null)
            {
                // Nie było i jest nowe.
                if (minReqs)
                { product.MinimumRequirements = newReqs; }
                else
                { product.RecommendedRequirements = newReqs; }
            }
        }

        private PartialViewResult RefreshProductTablePage()
        {
            var pageInfo = ProductPageHelper.GetProducts(
                db.Products.Include(p => p.Platform),
                null, null, Session.Get<int?>(sessionPageNumber),
                Session.Get<int?>(sessionPageSize), p => true);
            ViewBag.CurrentPage = pageInfo.CurrentPage;
            ViewBag.MaxPages = pageInfo.TotalPages;
            return PartialView("_ProductsTable", pageInfo.Products);
        }
    }
}
