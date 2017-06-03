using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.DynamicData;

namespace GameStore.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public int PlatformId { get; set; }
        public int? MinimumRequirementsId { get; set; }
        public int? RecommendedRequirementsId { get; set; }

        [Display(Name = "Widoczne?")]
        public bool IsVisible { get; set; }
        [Display(Name = "Data dodania")]
        public DateTime DateAdded { get; set; }
        
        [Display(Name = "Cena")]
        public decimal? Price { get; set; }
        
        public string CoverPath { get; set; }
        public string ThumbPath { get; set; }
        
        [Display(Name = "Nazwa")]
        public string Name { get; set; }
        [Display(Name = "Producent")]
        public string Studio { get; set; }
        [Display(Name = "Data premiery")]
        public DateTime? ReleaseDate { get; set; }
        [Display(Name = "Opis")]
        public string Description { get; set; }


        public virtual Platform Platform { get; set; }
        public virtual Requirements MinimumRequirements { get; set; }
        public virtual Requirements RecommendedRequirements { get; set; }
        
        public virtual ICollection<Pegi> Pegi { get; set; }
    }
    
    public class Platform
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Platforma")]
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }

    [Table("Pegi")]
    public class Pegi
    {
        [Key]
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string IconPath { get; set; }
        
        public string Description { get; set; }

        public int Priority { get; set; }

        public bool IsAgeRating { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }

    [Table("Requirements")]
    public class Requirements
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "System operacyjny")]
        public string OS { get; set; }
        [Display(Name = "Procesor")]
        public string CPU { get; set; }
        [Display(Name = "Karta graficzna")]
        public string GPU { get; set; }
        [Display(Name = "Pamięć RAM")]
        public string RAM { get; set; }
        [Display(Name = "Przestrzeń dyskowa")]
        public string HDD { get; set; }
        [Display(Name = "DirectX")]
        public string DirectX { get; set; }

        public virtual Product Product { get; set; }
    }
}