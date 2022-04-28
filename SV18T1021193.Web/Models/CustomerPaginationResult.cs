using SV18T1021193.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SV18T1021193.Web.Models
{
    /// <summary>
    /// Kết quả tìm kiếm, phân trang cho khách hàng
    /// </summary>
    public class CustomerPaginationResult :BasePaginationResult
    {
        /// <summary>
        /// Danh sách các khách hàng tìm được
        /// </summary>
        public List<Customer> Data { get; set; }
    }
}