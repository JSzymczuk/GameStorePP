using GameStore.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace GameStore.Helpers
{
    public static class ExtensionMethods
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType().GetMember(enumValue.ToString())
                           .First()
                           .GetCustomAttribute<DisplayAttribute>()
                           .GetName();
        }

        public static string ToDisplayableDate(this DateTime date)
        {
            string month;
            switch (date.Month)
            {
                case 1: month = "stycznia"; break;
                case 2: month = "lutego"; break;
                case 3: month = "marca"; break;
                case 4: month = "kwietnia"; break;
                case 5: month = "maja"; break;
                case 6: month = "czerwca"; break;
                case 7: month = "lipca"; break;
                case 8: month = "sierpnia"; break;
                case 9: month = "września"; break;
                case 10: month = "października"; break;
                case 11: month = "listopada"; break;
                case 12: month = "grudnia"; break;
                default: month = "error"; break;
            }
            return string.Format("{0} {1} {2}", date.Day, month, date.Year);
        }

        public static string ToDisplayableAddress(this Address address)
        {
            StringBuilder result = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(address.Street))
            { result.Append(address.Street + " "); }
            if (!string.IsNullOrWhiteSpace(address.City))
            { result.Append(address.City + " "); }
            if (!string.IsNullOrWhiteSpace(address.PostalCode))
            { result.Append(address.PostalCode + " "); }
            if (!string.IsNullOrWhiteSpace(address.Region))
            { result.Append(address.Region); }
            return result.ToString();
        }

        public static List<PegiInfo> ToPegiInfo(this IEnumerable<Pegi> collection)
        {
            var result = new List<PegiInfo>(collection.Count());
            foreach (var pg in collection)
            { result.Add(PegiInfo.FromPegi(pg)); }
            return result;
        }

        public static bool IsAnyPropertySet(this Requirements reqs)
        {
            return !(string.IsNullOrWhiteSpace(reqs.OS)
                && string.IsNullOrWhiteSpace(reqs.CPU)
                && string.IsNullOrWhiteSpace(reqs.GPU)
                && string.IsNullOrWhiteSpace(reqs.HDD)
                && string.IsNullOrWhiteSpace(reqs.RAM)
                && string.IsNullOrWhiteSpace(reqs.DirectX));
        }

        public static List<int> GetPegiIds(this ProductCreateViewModel model)
        {
            List<int> result = new List<int> { model.PegiAgeId };
            foreach (var pegi in model.PegiContent)
            {
                if (pegi.Checked)
                { result.Add(pegi.Id); }
            }
            return result;
        }

        public static decimal GetTotalPrice(this Order order)
        {
            decimal total = 0;
            foreach (var op in order.Positions)
            { total += op.Quantity * op.UnitPrice; }
            return total;
        }

        public static List<OrderStatusInfo> GetHistory(this Order order)
        {
            List<OrderStatusInfo> result = new List<OrderStatusInfo>(order.History.Count);
            var hist = order.History.ToList();
            foreach (var change in hist)
            {
                result.Add(new OrderStatusInfo
                {
                    Date = change.Date,
                    Name = change.Status.Name,
                    Description = change.Status.Description,
                    Cancellable = change.Status.Cancellable
                });
            }
            return result.OrderByDescending(p => p.Date).ToList();
        }
    }
}