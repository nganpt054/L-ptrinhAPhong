using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SV18T1021193.Web.Models
{
    /// <summary>
    /// Lưu trữ thông tin đầu vào tìm kiếm phân trang đơn giản
    /// </summary>
    public class PaginationSearchInput
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string SearchValue { get; set; }
    }
}