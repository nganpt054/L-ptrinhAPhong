using SV18T1021193.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SV18T1021193.Web.Models
{
    public class ProductPaginationResult:BasePaginationResult
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Product> Data { get; set; }
    }
}