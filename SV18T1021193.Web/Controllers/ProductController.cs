using SV18T1021193.BusinessLayer;
using SV18T1021193.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SV18T1021193.Web.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    [RoutePrefix("product")]
    public class ProductController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            Models.ProductSearchInput model = Session["PRODUCT_SEARCH"] as Models.ProductSearchInput;
            if (model == null)
            {
                model = new Models.ProductSearchInput()
                {
                    Page = 1,
                    PageSize = 10,
                    SearchValue = ""
                };
            }
            return View(model);
        }
        public ActionResult Search(Models.ProductSearchInput input)
        {

            int rowCount = 0;

            var data = CommonDataService.ListOfProducts(input.Page,
                input.PageSize,
                input.SearchValue,
                out rowCount);
            Models.BasePaginationResult model = new Models.ProductpaginationReult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                RowCount = rowCount,
                SearchValue = input.SearchValue,
                Data = data
            };

            Session["PRODUCT_SEARCH"] = input;

            return View(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            Product model = new Product()
            {
                ProductID = 0
            };

            ViewBag.Title = "Bổ sung mặt hàng";
            return View(model);

        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        [Route("edit/{productID?}")]
        public ActionResult Edit(string productID)
        {
            int id = 0;
            try
            {
                id = Convert.ToInt32(productID);
            }
            catch
            {
                return RedirectToAction("Index");
            }
            Product model = CommonDataService.GetProduct(id);
            if (model == null)
                return RedirectToAction("Index");

            ViewBag.Title = "Cập nhật khách hàng";
            return View("Create", model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        [Route("delete/{productID?}")]
        public ActionResult Delete(string productID)
        {
            int id = 0;
            try
            {
                id = Convert.ToInt32(productID);
            }
            catch
            {
                return RedirectToAction("Index");
            }
            if (Request.HttpMethod == "POST")
            {
                CommonDataService.DeleteProduct(id);
                return RedirectToAction("Index");
            }

            var model = CommonDataService.GetProduct(id);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }
        /// <summary>
        /// Lấy giữ liệu cần thiết của model và file ảnh để save vào database
        /// </summary>
        /// <param name="model"></param>
        /// <param name="photo"></param>
        /// <returns></returns>
        [HttpPost]

        public ActionResult Save(Product model, HttpPostedFileBase uploadPhoto)
        {
            

            //Xử lý ảnh
            if (uploadPhoto != null)
            {

                string physicalPath = Server.MapPath("~/images/products");
                string fileName = $"{DateTime.Now.Ticks}_{ uploadPhoto.FileName }";
                string filePath = System.IO.Path.Combine(physicalPath, fileName);
                uploadPhoto.SaveAs(filePath);

                model.Photo = $"images/products/{fileName}";
            }
            //TODO: Kiểm tra tên, email,....
            if (string.IsNullOrWhiteSpace(model.Photo))
            {
                ModelState.AddModelError("Photo", "Ảnh không được để trống");
            }

            if (string.IsNullOrWhiteSpace(model.ProductName))
            {
                ModelState.AddModelError("ProductName", "Tên mặt hàng không được để trống");
            }
            if (string.IsNullOrWhiteSpace(model.SupplierID.ToString()))
            {
                ModelState.AddModelError("SupplierID", "SupplierID không được để trống");
            }

            if (string.IsNullOrWhiteSpace(model.CategoryID.ToString()))
            {
                ModelState.AddModelError("CategoryID", "CategoryID không được để trống");

            }
            if (string.IsNullOrWhiteSpace(model.Unit))
            {

                model.Unit = "";
            }
            if (string.IsNullOrWhiteSpace(model.Price))
            {
                ModelState.AddModelError("Price", "Price không được để trống");

            }
            if (!ModelState.IsValid)
            {
                ViewBag.Title = model.ProductID == 0 ? "Bổ sung mặt hàng" : "Cập nhật mặt hàng";
                return View("Create", model);
            }
            if (model.ProductID == 0)
            {
                CommonDataService.AddProduct(model);

            }
            else
            {
                CommonDataService.UpdateProduct(model);

            }
            return RedirectToAction("Index");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <param name="productID"></param>
        /// <param name="photoID"></param>
        /// <returns></returns>
        [Route("photo/{method}/{productID}/{photoID?}")]
        public ActionResult Photo(string method, int productID, int? photoID)
        {
            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung ảnh";
                    break;
                case "edit":
                    ViewBag.Title = "Thay đổi ảnh";
                    break;
                case "delete":
                    return RedirectToAction("Edit", new { productID = productID });
                default:
                    return RedirectToAction("Index");
            }
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <param name="productID"></param>
        /// <param name="attributeID"></param>
        /// <returns></returns>
        [Route("attribute/{method}/{productID}/{attributeID?}")]
        public ActionResult Attribute(string method, int productID, int? attributeID)
        {
            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung thuộc tính";
                    break;
                case "edit":
                    ViewBag.Title = "Thay đổi thuộc tính";
                    break;
                case "delete":
                    return RedirectToAction("Edit", new { productID = productID });
                default:
                    return RedirectToAction("Index");
            }
            return View();
        }
    }
}