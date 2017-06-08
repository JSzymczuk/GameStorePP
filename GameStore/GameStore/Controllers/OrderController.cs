using GameStore.Helpers;
using GameStore.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GameStore.Controllers
{
    public class OrderController : Controller
    {
        private const string InitialOrderStatusName = "Oczekujące";
        private const string CancelledOrderStatusName = "Anulowane";

        private ApplicationUserManager userManager;
        private ApplicationDbContext db = new ApplicationDbContext();

        public ApplicationUserManager UserManager
        {
            get
            {
                return userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                userManager = value;
            }
        }

        public OrderController() { }
        public OrderController(ApplicationUserManager userManager) { UserManager = userManager; }

        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            var userOrders = db.Orders.Where(o => o.ClientId == userId).ToList();
            var ordersInfo = new List<OrderListItemViewModel>(userOrders.Count);
            foreach (var order in userOrders)
            {
                var orderHistory = order.GetHistory();
                var orderDate = orderHistory[orderHistory.Count - 1].Date;
                ordersInfo.Add(new OrderListItemViewModel
                {
                    Id = order.Id,
                    Price = order.GetTotalPrice(),
                    Address = order.Address.ToDisplayableAddress(),
                    DateCreated = string.Format("{0:D2}:{1:D2} {2}", orderDate.Hour,
                        orderDate.Minute, orderDate.ToDisplayableDate()),
                    Status = orderHistory[0].Name,
                    CanBeCancelled = orderHistory[0].Cancellable,
                    Positions = order.Positions.ToList()
                });
            }
            return View(ordersInfo);
        }

        public ActionResult Manage()
        {
            var orders = db.Orders.ToList();
            var ordersInfo = new List<OrderListItemViewModel>(orders.Count);
            foreach (var order in orders)
            {
                var orderHistory = order.GetHistory();
                var orderDate = orderHistory[orderHistory.Count - 1].Date;
                ordersInfo.Add(new OrderListItemViewModel
                {
                    Id = order.Id,
                    Price = order.GetTotalPrice(),
                    Address = order.Address.ToDisplayableAddress(),
                    DateCreated = string.Format("{0:D2}:{1:D2} {2}", orderDate.Hour,
                        orderDate.Minute, orderDate.ToDisplayableDate()),
                    Status = orderHistory[0].Name,
                    CanBeCancelled = orderHistory[0].Cancellable,
                    Positions = order.Positions.ToList()
                });
            }
            ViewBag.StatusId = new SelectList(db.OrderStatuses.ToList(), "Id", "Name");
            return View(ordersInfo);
        }

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            var cart = Session.Get<Cart>("Cart");
            if (cart != null && !cart.IsEmpty && User.Identity.GetUserId() != null)
            {
                var addresses = await GetAddressSelectionList();
                if (addresses.Count() > 0)
                {
                    ViewBag.AddressId = addresses;
                    return View(cart);
                }
                return RedirectToAction("AddAddress", "Manage");
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Create(int? addressId)
        {
            var cart = Session.Get<Cart>("Cart");
            var address = db.Addresses.Find(addressId);
            var initialStatus = db.OrderStatuses.FirstOrDefault(s => s.Name == InitialOrderStatusName);

            if (cart != null && !cart.IsEmpty && address != null && initialStatus != null)
            {
                Order order = new Order();                
                order.AddressId = address.Id;
                order.Positions = new List<OrderPosition>();
                order.ClientId = User.Identity.GetUserId();
                foreach (var pos in cart)
                {
                    order.Positions.Add(new OrderPosition
                    {
                        ProductId = pos.Product.Id,
                        Quantity = pos.Quantity,
                        UnitPrice = pos.UnitPrice
                    });
                }
                order.History = new List<OrderStatusChange>
                {
                    new OrderStatusChange
                    {
                        Date = DateTime.Now,
                        Status = initialStatus
                    }
                };
                cart.Clear();
                db.Orders.Add(order);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
        
        private bool CanBeCancelled(Order order)
        {
            return order.History.OrderByDescending(s => s.Date)
                .ToList()[0].Status.Cancellable;
        }

        public ActionResult Cancel(Guid id)
        {
            Order order = db.Orders.FirstOrDefault(o => o.Id == id);
            if (order != null && CanBeCancelled(order))
            {
                var status = db.OrderStatuses.FirstOrDefault(s =>
                    s.Name == CancelledOrderStatusName);
                if (status != null)
                {
                    db.OrderStatusChanges.Add(new OrderStatusChange
                    {
                        Date = DateTime.Now,
                        OrderId = id,
                        StatusId = status.Id
                    });
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult Details(Guid id)
        {
            Order order = db.Orders.FirstOrDefault(o => o.Id == id);
            if (order != null)
            {
                return View(new OrderDetailsViewModel
                {
                     Address = order.Address.ToDisplayableAddress(),
                     History = order.GetHistory(),
                     Id = order.Id,
                     Positions = order.Positions.ToList(),
                     Price = order.GetTotalPrice()
                });
            }
            return RedirectToAction("Index");
        }

        public ActionResult ChangeStatus(Guid id, int statusId)
        {
            Order order = db.Orders.FirstOrDefault(o => o.Id == id);
            var status = db.OrderStatuses.Find(statusId);
            if (order != null && status != null)
            {                
                db.OrderStatusChanges.Add(new OrderStatusChange
                {
                    Date = DateTime.Now,
                    OrderId = id,
                    StatusId = statusId
                });
                db.SaveChanges();
            }
            return RedirectToAction("Manage");
        }

        private async Task<SelectList> GetAddressSelectionList()
        {
            AppUser user = await GetCurrentUser();
            var addresses = db.Addresses.Where(a => !a.IsDeleted && a.UserId == user.Id).ToList();
            var displayableAddresses = new List<AddressSelectionViewModel>(addresses.Count);
            foreach (var address in addresses)
            {
                displayableAddresses.Add(AddressSelectionViewModel.FromAddress(address));
            }
            return new SelectList(displayableAddresses, "Id", "Name", user.DefaultAddressId);
        }

        private async Task<AppUser> GetCurrentUser()
        {
            return await UserManager.FindByNameAsync(User.Identity.Name);
        }
    }
}