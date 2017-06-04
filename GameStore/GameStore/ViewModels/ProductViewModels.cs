using GameStore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GameStore.Models
{
    public class PegiInfo
    {
        public int Id { get; set; }
        public bool Checked { get; set; }
        public string IconPath { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public static PegiInfo FromPegi(Pegi pegi)
        {
            return new PegiInfo
            {
                Checked = false,
                Id = pegi.Id,
                Description = pegi.Description,
                IconPath = pegi.IconPath,
                Name = pegi.Name
            };
        }
    }
    
    public class ProductCreateViewModel
    {
        [Key]
        public int Id { get; set; }

        public string CoverPath { get; set; }        
        public string ThumbPath { get; set; }
        [Display(Name = "Usunąć okładkę?")]
        public bool DeleteCover { get; set; }

        [Required]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }
        [Display(Name = "Platforma")]
        public int PlatformId { get; set; }
        [Required]
        [Display(Name = "Producent")]
        public string Studio { get; set; }        
        [Display(Name = "Data premiery")]
        public DateTime? ReleaseDate { get; set; }
        [Required]
        [Display(Name = "Cena")]
        public decimal? Price { get; set; }        
        [Display(Name = "Opis")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "PEGI")]
        public int PegiAgeId { get; set; }
        public List<PegiInfo> PegiContent { get; set; }
        
        public virtual Platform Platform { get; set; }

        [Display(Name = "Minimalne wymagania sprzętowe")]
        public Requirements MinimalRequirements { get; set; }
        [Display(Name = "Zalecane wymagania sprzętowe")]
        public Requirements RecommendedRequirements { get; set; }
        
    }

    public class ProductDetailsViewModel
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Okładka")]
        public string CoverPath { get; set; }
        
        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [Display(Name = "Platforma")]
        public string PlatformName { get; set; }
        
        [Display(Name = "Producent")]
        public string Studio { get; set; }

        [Display(Name = "Data premiery")]
        public DateTime? ReleaseDate { get; set; }
        
        [Display(Name = "Cena")]
        public decimal? Price { get; set; }

        [Display(Name = "Opis")]
        public string Description { get; set; }

        [Display(Name = "PEGI")]
        public List<Pegi> Pegi { get; set; }

        [Display(Name = "Minimalne wymagania sprzętowe")]
        public Requirements MinimalRequirements { get; set; }

        [Display(Name = "Zalecane wymagania sprzętowe")]
        public Requirements RecommendedRequirements { get; set; }

        [Display(Name = "Stan")]
        public string State { get; set; }

        [Display(Name = "Dodano")]
        public string AddedInfo { get; set; }

        [Display(Name = "Edytowano")]
        public string EditedInfo { get; set; }

        [Display(Name = "Usunieto")]
        public string DeletedInfo { get; set; }

    }

}