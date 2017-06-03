using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameStore.Models
{
    public class Order
    {
        [Key, Column(Order = 1)]
        public int Id { get; set; }

        [Column(Order = 2)]
        public string ClientId { get; set; }

        [Column(Order = 3)]
        public int AddressId { get; set; }


        public virtual AppUser Client { get; set; }
        public virtual Address Address { get; set; }

        public virtual ICollection<OrderPosition> Positions { get; set; }
        public virtual ICollection<OrderStatusChange> History { get; set; }
    }

    public class OrderPosition
    {
        [Key, Column(Order = 1)]
        public int Id { get; set; }

        [Column(Order = 2)]
        public int OrderId { get; set; }

        [Column(Order = 3)]
        public int ProductId { get; set; }


        [Column(Order = 4)]
        public double UnitPrice { get; set; }

        [Column(Order = 5)]
        public int Quantity { get; set; }


        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }

    public class OrderStatus
    {
        [Key, Column(Order = 1)]
        public int Id { get; set; }

        [Column(Order = 2)]
        public string Name { get; set; }

        [Column(Order = 3)]
        public string Description { get; set; }

        public virtual ICollection<OrderStatusChange> Changes { get; set; }
    }

    public class OrderStatusChange
    {
        [Key, Column(Order = 1)]
        public int Id { get; set; }

        [Column(Order = 2)]
        public int OrderId { get; set; }

        [Column(Order = 3)]
        public int StatusId { get; set; }


        [Column(Order = 4)]
        public DateTime Date { get; set; }


        public virtual Order Order { get; set; }
        public virtual OrderStatus Status { get; set; }
    }
}