using SV18T1021193.BusinessLayer;
using SV18T1021193.DomainModel;
using SV18T1021193.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

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
        //GET:Product
        public ActionResult Index()
        {
            Models.ProductSearchInput model = Session["PRODUCT_SEARCH"] as Models.ProductSearchInput;
            if (model == null)
            {
                model = new Models.ProductSearchInput()
                {
                    Page = 1,
                    PageSize = 10,
                    SearchValue = "",
                    CategoryName="0",
                    SupplierName="0"
                };

            }
            return View(model);
        }
        public ActionResult Search(Models.ProductSearchInput input)
        {
            int rowCount = 0;
            var data = ProductDataService.ListOfProducts(input.Page,
                input.PageSize,
                input.SearchValue,
                Convert.ToInt32(input.CategoryName),
                Convert.ToInt32(input.SupplierName),
                out rowCount);
            Models.BasePaginationResult model = new Models.ProductPaginationResult()
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
        [Route("edit/{productID}")]
        public ActionResult Edit(string productID, string attributeID, string photoID)
        {
            int id = 0, aID, pID;
            try
            {
                id = Convert.ToInt32(productID);
                aID = Convert.ToInt32(attributeID);
                pID = Convert.ToInt32(photoID);
            }
            catch
            {
                return RedirectToAction("Index");
            }
            ProductDataService.DeleteProductAttribute(aID);
            ProductDataService.DeleteProductPhoto(pID);
            Product model = ProductDataService.GetProduct(id);
            if (model == null)
                return RedirectToAction("Index");

            ViewBag.Title = "Cập nhật mặt hàng";
            return View("Edit", model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="uploadPhoto"></param>
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
            //TODO: Kiểm tra tên mặt hàng, giá,....
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
                ModelState.AddModelError("SupplierID", "Tên nhà cung cấp không được để trống");
            }

            if (string.IsNullOrWhiteSpace(model.CategoryID.ToString()))
            {
                ModelState.AddModelError("CategoryID", "Tên loại hàng không được để trống");
            }
            if (string.IsNullOrWhiteSpace(model.Unit))
            {
                ModelState.AddModelError("Unit", "Đơn vị tính không được để trống");
            }
            if (string.IsNullOrWhiteSpace(model.Price))
            {
                ModelState.AddModelError("Price", "Giá không được để trống");
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Title = model.ProductID == 0 ? "Bổ sung mặt hàng" : "Cập nhật mặt hàng";
                return View("Create", model);
            }
            if (model.ProductID == 0)
            {
                ProductDataService.AddProduct(model);

            }
            else
            {
                ProductDataService.UpdateProduct(model);

            }
            return RedirectToAction("Index");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        [Route("delete/{productID}")]
        public ActionResult Delete(int productID)
        {
            if (Request.HttpMethod == "POST")
            {
                ProductDataService.DeleteProduct(productID);
                return RedirectToAction("Index");
            }

            var model = ProductDataService.GetProduct(productID);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
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

            if (photoID != null)
            {
                ProductPhoto model = ProductDataService.GetProductPhoto(Convert.ToInt32(photoID));
                switch (method)
                {
                    
                    case "edit":
                        ViewBag.Title = "Thay đổi ảnh";
                        break;
                    case "delete":
                        return RedirectToAction("Edit", new { productID = productID, photoID = photoID });
                    default:
                        return RedirectToAction("Index");
                }

                return View(model);
            }
            else
            {
                ProductPhoto model = new ProductPhoto()
                {
                    PhotoID = 0,
                    ProductID = productID
                };
                switch (method)
                {
                    case "add":
                        ViewBag.Title = "Bổ sung ảnh";
                        break;
                    
                    default:
                        return RedirectToAction("Index");
                }

                return View(model);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="uploadPhoto"></param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult SavePhoto(ProductPhoto model, HttpPostedFileBase uploadPhoto)
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
            //TODO: Kiểm tra tên mặt hàng, giá,....
            if (string.IsNullOrWhiteSpace(model.Photo))
            {
                ModelState.AddModelError("Photo", "Ảnh không được để trống");
            }


            if (string.IsNullOrWhiteSpace(model.Description))
            {
                ModelState.AddModelError("Description", "Mô tả không được để trống");
            }

            if (string.IsNullOrWhiteSpace(model.DisplayOrder.ToString()))
            {
                ModelState.AddModelError("DisplayOrder", "Thứ tự không được để trống");
            }

            if (!ModelState.IsValid)
            {

                return View("Photo", model);
            }
            if (model.PhotoID == 0)
            {
                ProductDataService.AddProductPhoto(model);

            }
            else
            {
                ProductDataService.UpdateProductPhoto(model);

            }
            return RedirectToAction("Edit",new { productID=model.ProductID});
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
            if (attributeID!=null)
            {
                ProductAttribute model = ProductDataService.GetProductAttribute(Convert.ToInt32(attributeID));
                switch (method)
                {
                    
                    case "edit":
                        ViewBag.Title = "Thay đổi thuộc tính";
                        break;
                    case "delete":
                        return RedirectToAction("Edit", new { productID = productID, attributeID = attributeID });
                    default:
                        return RedirectToAction("Index");
                }
                return View(model);
            }
            else
            {
                ProductAttribute model = new ProductAttribute()
                {
                    AttributeID = Convert.ToInt32(attributeID),
                    ProductID = productID
                };
                switch (method)
                {
                    case "add":
                        ViewBag.Title = "Bổ sung thuộc tính";
                        break;
                    
                    default:
                        return RedirectToAction("Index");
                }
                return View(model);
            } 
  
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public ActionResult SaveAttribute(ProductAttribute model)
        {
            if (string.IsNullOrWhiteSpace(model.ProductID.ToString()))
            {
                ModelState.AddModelError("ProductID", "Tên không được để trống");
            }
            if (string.IsNullOrWhiteSpace(model.AttributeName))
            {
                ModelState.AddModelError("AttributeName", "Tên thuộc tính không được để trống");
            }
            if (string.IsNullOrWhiteSpace(model.AttributeValue))
            {
                ModelState.AddModelError("AttributeValue", "Giá trị không được để trống");
            }
            if (string.IsNullOrWhiteSpace(model.DisplayOrder.ToString()))
            {
                ModelState.AddModelError("DisplayOrder", "Thứ tự không được để trống");
            }

            if (!ModelState.IsValid)
            {
                
                return View("Attribute", model);
            }

            if (model.AttributeID == 0)
            {
                ProductDataService.AddProductAttribute(model);
                return RedirectToAction("Edit/{productID}");
            }
            else
            {
                ProductDataService.UpdateProductAttribute(model);
                return RedirectToAction("Edit",new { productID=model.ProductID});
            }
        }
    }
}