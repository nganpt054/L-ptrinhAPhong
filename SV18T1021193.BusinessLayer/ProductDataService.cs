using SV18T1021193.DataLayer;
using SV18T1021193.DomainModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV18T1021193.BusinessLayer
{
    public static class ProductDataService
    {
        private static readonly IProductDAL productDB;
        static ProductDataService()
        {
            string provider = ConfigurationManager.
                ConnectionStrings["DB"].
                ProviderName;
            string connectionString = ConfigurationManager.
                ConnectionStrings["DB"].
                ConnectionString;
            if (provider == "SQLServer")
            {
                
                productDB = new DataLayer.SQLServer.ProductDAL(connectionString);
                
            }
            
        }
        /// <summary>
        /// Tìm kiếm và lấy danh sách mặt hàng dưới dạng phân trang
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public static List<Product> ListOfProducts(int page,
                                                       int pageSize,
                                                       string searchValue,
                                                       int categoryID,
                                                       int supplierID,
                                                       out int rowCount)
        {
            rowCount = productDB.Count(searchValue,categoryID,supplierID);
            return productDB.List(page, pageSize, searchValue,categoryID,supplierID).ToList();
        }
        /// <summary>
        /// lấy thông tin mặt hàng theo id
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        public static Product GetProduct(int productID)
        {
            return productDB.Get(productID);
        }
        /// <summary>
        /// Thêm mặt hàng theo data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddProduct(Product data)
        {
            return productDB.Add(data);
        }
        /// <summary>
        /// Update mặt hàng theo data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateProduct(Product data)
        {
            return productDB.Update(data);
        }
        /// <summary>
        /// Xóa mặt hàng theo ID,(kiểm tra InUsed)
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        public static bool DeleteProduct(int productID)
        {
            if (productDB.InUsed(productID))
                return false;
            return productDB.Delete(productID);
        }
        public static bool InUsedProduct(int productID)
        {
            return productDB.InUsed(productID);
        }
        public static List<ProductPhoto> ListOfProductPhotos(int productID)
        {
            return productDB.ListPhoto(productID).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="photoID"></param>
        /// <returns></returns>
        public static ProductPhoto GetProductPhoto(int photoID)
        {
            return productDB.GetPhoto(photoID);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddProductPhoto(ProductPhoto data)
        {
            return productDB.AddPhoto(data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateProductPhoto(ProductPhoto data)
        {
            return productDB.UpdatePhoto(data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="photoID"></param>
        /// <returns></returns>
        public static bool DeleteProductPhoto(int photoID)
        {

            return productDB.DeletePhoto(photoID);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<ProductAttribute> ListOfProductAttributes(int productID)
        {
            return productDB.ListAttribute(productID).ToList();
        }


        public static ProductAttribute GetProductAttribute(int attributeID)
        {
            return productDB.GetAttribute(attributeID);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddProductAttribute(ProductAttribute data)
        {
            return productDB.AddAttribute(data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateProductAttribute(ProductAttribute data)
        {
            return productDB.UpdateAttribute(data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="photoID"></param>
        /// <returns></returns>
        public static bool DeleteProductAttribute(int attributeID)
        {

            return productDB.DeleteAttribute(attributeID);
        }
    }
}
