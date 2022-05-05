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
    /// 
    /// </summary>
    public static class SelectListHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> Countries()
        {
            //SelectListItem : tạo ra option trong thẻ select
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Value = "", Text = "--Chọn quốc gia--" });
            foreach (var c in CommonDataService.ListOfCountries())
            {
                list.Add(new SelectListItem()
                {
                    Value = c.CountryName,
                    Text = c.CountryName
                }
                );

            }
            return list;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> Categories()
        {
            //SelectListItem : tạo ra option trong thẻ select
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Value = "", Text = "-- Loại hàng--" });
            foreach (var c in CommonDataService.ListOfCategories())
            {
                list.Add(new SelectListItem()
                {
                    Value = c.CategoryName,
                    Text = c.CategoryName
                }
                );

            }
            return list;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> Suppliers()
        {
            //SelectListItem : tạo ra option trong thẻ select
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Value = "", Text = "--Nhà cung cấp--" });
            foreach (var c in CommonDataService.ListOfSuppliers())
            {
                list.Add(new SelectListItem()
                {
                    Value = c.SupplierName,
                    Text = c.SupplierName
                }
                );

            }
            return list;
        }
    }
}