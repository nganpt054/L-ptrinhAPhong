using SV18T1021193.BusinessLayer;
using SV18T1021193.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SV18T1021193.Web.Controllers
{
    [Authorize]
    [RoutePrefix("category")]
    public class CategoryController : Controller
    {
        // GET: Category
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            Models.PaginationSearchInput model = Session["CATEGORY_SEARCH"] as Models.PaginationSearchInput;
            if (model == null)
            {
                model = new Models.PaginationSearchInput()
                {
                    Page = 1,
                    PageSize = 10,
                    SearchValue = ""
                };
            }
            return View(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ActionResult Search(Models.PaginationSearchInput input)
        {

            int rowCount = 0;

            var data = CommonDataService.ListOfCategories(input.Page,
                input.PageSize,
                input.SearchValue,
                out rowCount);
            Models.BasePaginationResult model = new Models.CategoryPaginationResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                RowCount = rowCount,
                SearchValue = input.SearchValue,
                Data = data
            };

            Session["CATEGORY_SEARCH"] = input;

            return View(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            Category model = new Category()
            {
                CategoryID = 0
            };

            ViewBag.Title = "Bổ sung loại hàng";
            return View(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        [Route("edit/{categoryID?}")]
        public ActionResult Edit(string categoryID)
        {
            int id = 0;
            try
            {
                id = Convert.ToInt32(categoryID);
            }
            catch
            {
                return RedirectToAction("Index");
            }
            Category model = CommonDataService.GetCategory(id);
            if (model == null)
                return RedirectToAction("Index");

            ViewBag.Title = "Cập nhật loại hàng";
            return View("Create", model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Save(Category model)
        {
            model.Description = "";
            if (string.IsNullOrWhiteSpace(model.CategoryName))
            {
                ModelState.AddModelError("CategoryName", "Tên không được để trống");
            }


            if (!ModelState.IsValid)
            {
                ViewBag.Title = model.CategoryID == 0 ? "Bổ sung loại hàng" : "Cập nhậtloại hàng";
                return View("Create", model);
            }

            if (model.CategoryID == 0)
            {
                CommonDataService.AddCategory(model);
                return RedirectToAction("Index");
            }
            else
            {
                CommonDataService.UpdateCategory(model);
                return RedirectToAction("Index");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        [Route("delete/{categoryID?}")]
        public ActionResult Delete(string categoryID)
        {
            int id = 0;
            try
            {
                id = Convert.ToInt32(categoryID);
            }
            catch
            {
                return RedirectToAction("Index");
            }
            if (Request.HttpMethod == "POST")
            {
                CommonDataService.DeleteCategory(id);
                return RedirectToAction("Index");
            }

            var model = CommonDataService.GetCategory(id);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }
    }
}