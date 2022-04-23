using SV18T1021230.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SV18T1021230.Web.Models
{
    public class CustomerPaginationResult:BasePaginationResult
    {
        /// <summary>
        /// Danh sách các khách hàng tìm được
        /// </summary>
        public List<Customer> Data { get; set; }
    }
}