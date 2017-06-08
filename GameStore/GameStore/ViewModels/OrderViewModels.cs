using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameStore.Models
{
    public class OrderListItemViewModel
    {
        public Guid Id { get; set; }
        public List<OrderPosition> Positions { get; set; }
        public string Status { get; set; }
        public string DateCreated { get; set; }
        public decimal Price { get; set; }
        public bool CanBeCancelled { get; set; }
        public string Address { get; set; }
    }

    public class OrderDetailsViewModel
    {
        public Guid Id { get; set; }
        public List<OrderPosition> Positions { get; set; }
        public List<OrderStatusInfo> History { get; set; }
        public decimal Price { get; set; }
        public string Address { get; set; }
    }

    public class OrderStatusInfo
    {
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Cancellable { get; set; }
    }
}