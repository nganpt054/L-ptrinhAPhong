using SV18T1021193.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV18T1021193.DataLayer
{
    public interface IProductDAL
    {
        IList<Product> List(int page = 1, int pageSize = 0, string searchValue = "",int categoryID=0,int supplierID=0);


        int Count(string searchValue, int categoryID = 0, int supplierID = 0);
        Product Get(int productID);
        int Add(Product data);
        bool Update(Product data);
        bool Delete(int productID);
        bool InUsed(int productID);
        IList<ProductPhoto> ListPhoto(int productID);
        ProductPhoto GetPhoto(int photoID);
        int AddPhoto(ProductPhoto data);
        bool UpdatePhoto(ProductPhoto data);
        bool DeletePhoto(int photoID);
        IList<ProductAttribute> ListAttribute(int productID);
        ProductAttribute GetAttribute(int attributeID);
        int AddAttribute(ProductAttribute data);
        bool UpdateAttribute(ProductAttribute data);
        bool DeleteAttribute(int attributeID);

    }
}
