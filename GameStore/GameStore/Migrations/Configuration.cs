namespace GameStore.Migrations
{
    using GameStore.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<GameStore.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        private struct TestUserInfo
        {
            public string UserName;
            public string Email;
            public string FirstName;
            public string LastName;
            public string Password;
            public string RoleName;
        }

        protected override void Seed(GameStore.Models.ApplicationDbContext db)
        {
            #region Tworzenie domy�lnych r�l i u�ytkownik�w

            string[] roleNames = new string[] { "User", "Administrator" };
            string standardPassword = "L@jkonik77";

            TestUserInfo[] userInfo = new TestUserInfo[]
            {
                new TestUserInfo
                {
                     UserName = "administrator",
                     Password = standardPassword,
                     RoleName = roleNames[1],
                     Email = "admin@gamestore.pl",
                     FirstName = "Admin",
                     LastName = "Admin"
                },
                new TestUserInfo
                {
                     UserName = "Wombat1995",
                     Password = standardPassword,
                     RoleName = roleNames[0],
                     Email = "wombat1995@example.pl",
                     FirstName = "Krzysztof",
                     LastName = "�apa-Turzyniak"
                },
                new TestUserInfo
                {
                     UserName = "xX_Vizard92_Xx",
                     Password = standardPassword,
                     RoleName = roleNames[0],
                     Email = "vizard92@example.pl",
                     FirstName = "Arkadiusz",
                     LastName = "Kociszewski"
                }
            };

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            foreach (string roleName in roleNames)
            {
                if (roleManager.FindByName(roleName) == null)
                { roleManager.Create(new IdentityRole(roleName)); }
            }

            var userManager = new UserManager<AppUser>(new UserStore<AppUser>(db));
            foreach (var ui in userInfo)
            {
                if (userManager.FindByName(ui.UserName) == null)
                {
                    AppUser user = new AppUser
                    {
                        UserName = ui.UserName,
                        FirstName = ui.FirstName,
                        LastName = ui.LastName,
                        Email = ui.Email
                    };
                    userManager.Create(user, ui.Password);
                    userManager.AddToRole(user.Id, ui.RoleName);
                }
            }

            #endregion

            #region Platformy

            var platforms = new List<Platform>
            {
                new Platform  { Id = 1, Name = "PC" },
                new Platform  { Id = 2, Name = "Playstation 3" },
                new Platform  { Id = 3, Name = "Playstation 4" },
                new Platform  { Id = 4, Name = "Xbox One" },
                new Platform  { Id = 5, Name = "Xbox 360" }
            };
            foreach (var platform in platforms)
            {
                if (!db.Platforms.Any(r => r.Id == platform.Id))
                { db.Platforms.Add(platform); }
            }

            #endregion

            #region PEGI

            var pegi = new List<Pegi>
            {
                new Pegi
                {
                     Id = 1,
                     Name = "PEGI 3",
                     Description = "Gra przeznaczona jest dla os�b powy�ej trzeciego roku �ycia.",
                     IconPath = "PEGI_3.png",
                     Priority = 105,
                     IsAgeRating = true
                },
                new Pegi
                {
                     Id = 2,
                     Name = "PEGI 7",
                     Description = "Gra przeznaczona jest dla os�b powy�ej si�dmego roku �ycia.",
                     IconPath = "PEGI_7.png",
                     Priority = 104,
                     IsAgeRating = true
                },
                new Pegi
                {
                     Id = 3,
                     Name = "PEGI 12",
                     Description = "Gra przeznaczona jest dla os�b powy�ej dwunastego roku �ycia.",
                     IconPath = "PEGI_12.png",
                     Priority = 103,
                     IsAgeRating = true
                },
                new Pegi
                {
                     Id = 4,
                     Name = "PEGI 16",
                     Description = "Gra przeznaczona jest dla os�b powy�ej szesnastego roku �ycia.",
                     IconPath = "PEGI_16.png",
                     Priority = 102,
                     IsAgeRating = true
                },
                new Pegi
                {
                     Id = 5,
                     Name = "PEGI 18",
                     Description = "Gra przeznaczona jest dla os�b pe�noletnich.",
                     IconPath = "PEGI_18.png",
                     Priority = 101,
                     IsAgeRating = true
                },
                new Pegi
                {
                     Id = 11,
                     Name = "Przemoc",
                     Description = "Gra zawiera elementy przemocy.",
                     IconPath = "PEGI_violence.png",
                     Priority = 7,
                     IsAgeRating = false
                },
                new Pegi
                {
                     Id = 12,
                     Name = "Seks",
                     Description = "W grze pojawiaj� si� nago�� lub zachowania seksualne lub nawi�zania do zachowa� o charakterze seksualnym.",
                     IconPath = "PEGI_sex.png",
                     Priority = 6,
                     IsAgeRating = false
                },
                new Pegi
                {
                     Id = 13,
                     Name = "Dyskryminacja",
                     Description = "Gra pokazuje przypadki dyskryminacji lub zawiera materia�y, kt�re mog� do niej zach�ca� osoby nieletnie.",
                     IconPath = "PEGI_discrimination.png",
                     Priority = 5,
                     IsAgeRating = false
                },
                new Pegi
                {
                     Id = 14,
                     Name = "U�ywki",
                     Description = "W grze pojawiaj� si� nawi�zania do u�ywek lub przedstawiono ich za�ywanie.",
                     IconPath = "PEGI_drugs.png",
                     Priority = 4,
                     IsAgeRating = false
                },
                new Pegi
                {
                     Id = 15,
                     Name = "Strach",
                     Description = "Gra mo�e przestraszy� m�odsze dzieci.",
                     IconPath = "PEGI_fear.png",
                     Priority = 3,
                     IsAgeRating = false
                },
                new Pegi
                {
                     Id = 16,
                     Name = "Wulgarny j�zyk",
                     Description = "W grze jest u�ywany wulgarny j�zyk.",
                     IconPath = "PEGI_language.png",
                     Priority = 2,
                     IsAgeRating = false
                },
                new Pegi
                {
                     Id = 17,
                     Name = "Hazard",
                     Description = "Gra uczy lub zach�ca do uprawiania hazardu.",
                     IconPath = "PEGI_gambling.png",
                     Priority = 1,
                     IsAgeRating = false
                }
            };
            foreach (var pg in pegi)
            {
                if (!db.Pegi.Any(p => p.Name == pg.Name))
                { db.Pegi.Add(pg); }
            }

            #endregion

            #region

            var statuses = new List<OrderStatus>
            {
                new OrderStatus
                {
                     Id = 1,
                     Name = "Oczekuj�ce",
                     Description = "Zam�wienie oczekuje na p�atno��."
                },
                new OrderStatus
                {
                     Id = 2,
                     Name = "Op�acone",
                     Description = "Zam�wienie oczekuje na rozpocz�cie realizacji."
                },
                new OrderStatus
                {
                     Id = 3,
                     Name = "Zarejestrowane",
                     Description = "Zam�wienie zosta�o przyj�te i rozpocz�o si� kompletowanie towaru."
                },
                new OrderStatus
                {
                     Id = 4,
                     Name = "Skompletowane",
                     Description = "Zam�wienie oczekuje na wysy�k�."
                },
                new OrderStatus
                {
                     Id = 5,
                     Name = "Wys�ane",
                     Description = "Zam�wienie zosta�o wys�ane."
                },
                new OrderStatus
                {
                     Id = 6,
                     Name = "Zrealizowane",
                     Description = "Zam�wienie zosta�o dostarczone i odebrane."
                },
                new OrderStatus
                {
                     Id = 7,
                     Name = "Anulowane",
                     Description = "Zam�wienie zosta�o anulowane przez u�ytkownika."
                }
            };

            #endregion

        }
    }
}