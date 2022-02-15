using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TryMVC.Models
{
    public class ProductModels
    {
        public string pId { get; set; }
        public string pName { get; set; }
        public decimal Price { get; set; }

        public static ProductModels GetDefault()
        {
            return new ProductModels()
            {
                pId = string.Empty,
                pName = string.Empty,
                Price = 10,
            };
        }
    }
}