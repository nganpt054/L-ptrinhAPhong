using SV18T1021230.BusinessLayer;
using SV18T1021230.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SV18T1021230.Web.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    [RoutePrefix("customer")]
    public class CustomerController : Controller
    {
        // GET: Customer
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int page = 1, string searchValue = "")
        {
            int pageSize = 10;
            int rowCount = 0;
            var data = CommonDataService.ListOfCustomers(page,
                pageSize,
                searchValue,
                out rowCount);
            Models.BasePaginationResult model = new Models.CustomerPaginationResult()
            {
                Page = page,
                PageSize = pageSize,
                RowCount = rowCount,
                SearchValue = searchValue,
                Data = data
            };
            return View(model);

            //int pageSize = 10;
            //int rowCount = 0;

            //var model = CommonDataService.ListOfCustomers(
            //                                    page,
            //                                    pageSize,
            //                                    searchValue,
            //                                    out rowCount);
            ////tính số trang
            //int pageCount = rowCount / pageSize;
            //if (rowCount % pageSize > 0) pageCount += 1;

            //ViewBag.RowCount = rowCount;
            //ViewBag.PageCount = pageCount;
            //ViewBag.Page = page;
            //ViewBag.SearchValue = searchValue;

            //return View(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {


            Customer model = new Customer()
            {
                CustomerID = 0
            };

            ViewBag.Title = "Bổ sung khách hàng";
            return View(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        [Route("edit/{customerID}")]
        public ActionResult Edit(int customerID)
        {
            Customer model = CommonDataService.GetCustomer(customerID);
            if (model == null)
                return RedirectToAction("Index");

            ViewBag.Title = "Cập nhật khách hàng";
            return View("Create", model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Save(Customer model)
        {
            if (string.IsNullOrWhiteSpace(model.CustomerName))
            {
                ModelState.AddModelError("CustomerName", "Tên không được để trống");
            }
            if (string.IsNullOrWhiteSpace(model.ContactName))
            {
                ModelState.AddModelError("ContactName", "Tên giao dịch không được để trống");
            }
            if (string.IsNullOrWhiteSpace(model.Address))
            {
                ModelState.AddModelError("Address", "Địa chỉ không được để trống");
            }
            if (string.IsNullOrWhiteSpace(model.City))
            {
                ModelState.AddModelError("City", "Thành phố không được để trống");
            }
            if (string.IsNullOrWhiteSpace(model.Country))
            {
                ModelState.AddModelError("Country", "Country không được để trống");
            }
            if (string.IsNullOrWhiteSpace(model.PostalCode))
            {
                ModelState.AddModelError("PostalCode", "Mã bưu chính không được để trống");
            }
                if (!ModelState.IsValid)
                {
                    ViewBag.Title =model.CustomerID==0?"Bổ sung khách hàng": "Cập nhật khách hàng";
                    return View("Create", model);
                }

                if (model.CustomerID == 0)
                {
                    CommonDataService.AddCustomer(model);
                    return RedirectToAction("Index");
                }
                else
                {
                    CommonDataService.UpdateCustomer(model);
                    return RedirectToAction("Index");
                }
            }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        /// 
        [Route("delete/{customerID}")]
        public ActionResult Delete(int customerID)
        {
            if (Request.HttpMethod == "POST")
            {
                CommonDataService.DeleteCustomer(customerID);
                return RedirectToAction("Index");
            }

            var model = CommonDataService.GetCustomer(customerID);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }
    }
}