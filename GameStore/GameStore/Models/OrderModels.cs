using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameStore.Models
{
    public class Order
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }        

        public string ClientId { get; set; }        
        public int AddressId { get; set; }

        public virtual AppUser Client { get; set; }
        public virtual Address Address { get; set; }

        public virtual ICollection<OrderPosition> Positions { get; set; }
        public virtual ICollection<OrderStatusChange> History { get; set; }
    }

    public class OrderPosition
    {
        public int Id { get; set; }        
        public Guid? OrderId { get; set; }        
        public int ProductId { get; set; }
        
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }

        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }

    public class OrderStatus
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }

        public bool Cancellable { get; set; }

        public virtual ICollection<OrderStatusChange> Changes { get; set; }
    }

    public class OrderStatusChange
    {
        public int Id { get; set; }
        
        public Guid OrderId { get; set; }
        
        public int StatusId { get; set; }
        
        public DateTime Date { get; set; }


        public virtual Order Order { get; set; }
        public virtual OrderStatus Status { get; set; }
    }
}