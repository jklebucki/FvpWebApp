using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FvpWebApp.Infrastructure
{
    public static class ConstData
    {
        public static List<SelectListItem> MonthsSelectList()
        {
            return new List<SelectListItem> {
                new SelectListItem { Text = "1", Value = "1" },
                new SelectListItem { Text = "2", Value = "2" },
                new SelectListItem { Text = "3", Value = "3" },
                new SelectListItem { Text = "4", Value = "4" },
                new SelectListItem { Text = "5", Value = "5" },
                new SelectListItem { Text = "6", Value = "6" },
                new SelectListItem { Text = "7", Value = "7" },
                new SelectListItem { Text = "8", Value = "8" },
                new SelectListItem { Text = "9", Value = "9" },
                new SelectListItem { Text = "10", Value = "10" },
                new SelectListItem { Text = "11", Value = "11" },
                new SelectListItem { Text = "12", Value = "12" }
            };
        }
    }
}
