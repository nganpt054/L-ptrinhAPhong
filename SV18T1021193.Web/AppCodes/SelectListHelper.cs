using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SV18T1021193.BusinessLayer;
using SV18T1021193.DomainModel;
namespace SV18T1021193.Web
{
    /// <summary>
    /// Cung cấp các hàm tiện ích liên quan đến danh sách chọn trong thẻ select
    /// </summary>
    public static class SelectListHelper
    {
        /// <summary>
        /// Danh sách quốc gia
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> Countries()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Value = "", Text = "--Chọn quốc gia--" });
            foreach (var c in CommonDataService.ListOfCountries())
            {
                list.Add(new SelectListItem()
                {
                    
                    Value = c.CountryName,
                    Text = c.CountryName

                });
            }
            return list;
        }
        /// <summary>
        /// Danh sách các nhà cung cấp
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> SelectSupplier()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Value = "0", Text = "--Nhà cung cấp--" });
           
            foreach (var c in CommonDataService.ListOfSuppliers())
            { 
                
                    list.Add(new SelectListItem()
                    {

                        Value = c.SupplierID.ToString(),
                        Text = c.SupplierName

                    });
                
                
            }
            return list;
        }
        /// <summary>
        /// Danh sách các mặt hàng
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> SelectCategories()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Value = "0", Text = "--Loại hàng--" });
            foreach (var c in CommonDataService.ListOfCategories())
            {
                list.Add(new SelectListItem()
                {

                    Value = c.CategoryID.ToString(),
                    Text = c.CategoryName

                });
            }
            return list;
        }

    }
}