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

        public int? DefaultAddressId { get; set; }
        
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
        public int Id { get; set; }

        public string UserId { get; set; }

        [Display(Name = "Aktywny?")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Województwo")]
        public string Region { get; set; }

        [Display(Name = "Miejscowość")]
        public string City { get; set; }

        [Display(Name = "Ulica")]
        public string Street { get; set; }

        [Display(Name = "Kod pocztowy")]
        public string PostalCode { get; set; }

        [Display(Name = "Dodatkowe informacje")]
        public string AdditionalInfo { get; set; }

        public virtual AppUser User { get; set; }
        
        public virtual ICollection<Order> Orders { get; set; }
    }
}