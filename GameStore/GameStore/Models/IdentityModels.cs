using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Web.DynamicData;

namespace GameStore.Models
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int DefaultAddressId { get; set; }

        public virtual ICollection<Address> Adresses { get; set; }
        public virtual ICollection<Order> Orders { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<AppUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }

    [TableName("Addresses")]
    public class Address
    {
        [Key, Column(Order = 1)]
        public int Id { get; set; }

        [Column(Order = 2)]
        public string UserId { get; set; }


        [Column(Order = 3)]
        public bool IsDeleted { get; set; }


        [Column(Order = 4)]
        public string Region { get; set; }

        [Column(Order = 5)]
        public string City { get; set; }

        [Column(Order = 6)]
        public string Street { get; set; }

        [Column(Order = 7)]
        public string PostalCode { get; set; }

        [Column(Order = 8)]
        public string AdditionalInfo { get; set; }


        public virtual AppUser User { get; set; }
        
        public virtual ICollection<Order> Orders { get; set; }
    }
}