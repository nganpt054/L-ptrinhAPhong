using SV18T1021193.DomainModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV18T1021193.DataLayer.SQLServer
{
    public class ProductDAL : _BaseDAL, IProductDAL
    {
        public ProductDAL(string connectionString) : base(connectionString)
        {
        }

        public int Add(Product data)
        {
            int result = 0;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"insert into Products (ProductName,SupplierID,CategoryID,Unit,Price,Photo)
                                        values (@productName,@supplierID,@categoryID,@unit,@price,@photo)
                                        select scope_identity()";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@productName", data.ProductName);
                cmd.Parameters.AddWithValue("@supplierID", data.SupplierID);
                cmd.Parameters.AddWithValue("@categoryID", data.CategoryID);
                cmd.Parameters.AddWithValue("@unit", data.Unit);
                cmd.Parameters.AddWithValue("@price", data.Price);
                cmd.Parameters.AddWithValue("@photo", data.Photo);

                result = Convert.ToInt32(cmd.ExecuteScalar());
                cn.Close();
            }

            return result;
        }

        public int AddAttribute(ProductAttribute data)
        {
            int result = 0;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"insert into ProductAttributes (ProductID,AttributeName,AttributeValue,DisplayOrder)
                                        values (@productID,@attributeName,@attributeValue,@displayOrder)
                                        select scope_identity()";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@productID", data.ProductID);
                cmd.Parameters.AddWithValue("@attributeName", data.AttributeName);
                cmd.Parameters.AddWithValue("@attributeValue", data.AttributeValue);
                cmd.Parameters.AddWithValue("@displayOrder", data.DisplayOrder);


                result = Convert.ToInt32(cmd.ExecuteScalar());
                cn.Close();
            }

            return result;
        }

        public int AddPhoto(ProductPhoto data)
        {
            int result = 0;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"insert into ProductPhotos (ProductID,Photo,Description,DisplayOrder)
                                        values (@productID,@photo,@description,@displayOrder)
                                        select scope_identity()";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@productID", data.ProductID);
                cmd.Parameters.AddWithValue("@photo", data.Photo);
                cmd.Parameters.AddWithValue("@description", data.Description);
                cmd.Parameters.AddWithValue("@displayOrder", data.DisplayOrder);


                result = Convert.ToInt32(cmd.ExecuteScalar());
                cn.Close();
            }

            return result;
        }

        public int Count(string searchValue, int categoryID , int supplierID )
        {
            int count = 0;

            using (SqlConnection cn = OpenConnection())
            {
                if (searchValue != "")
                    searchValue = "%" + searchValue + "%";
                SqlCommand cmd = new SqlCommand();
                if (categoryID==0&&supplierID==0)
                {
                    cmd.CommandText = @"SELECT    COUNT(*)
                                FROM    Products
                                WHERE (ProductName LIKE @searchValue)";
                }
                else
                {
                    if(categoryID==0&&supplierID!=0)
                    {
                        cmd.CommandText = @"SELECT    COUNT(*)
                                FROM    Products
                                WHERE   (ProductName LIKE @searchValue)
                                            and (SupplierID LIKE @supplierID)";
                    }
                    else
                    {
                        if(categoryID!=0&&supplierID==0)
                        {
                            cmd.CommandText = @"SELECT    COUNT(*)
                                FROM    Products
                                WHERE  (ProductName LIKE @searchValue)
                                     and (CategoryID LIKE @categoryID)";
                        }
                        else
                        {
                            cmd.CommandText = @"SELECT    COUNT(*)
                                FROM    Products
                                WHERE  (ProductName LIKE @searchValue)
                                            and (SupplierID LIKE @supplierID)
                                             and (CategoryID LIKE @categoryID)";
                        }    
                    }
                }    
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@searchValue", searchValue);
                cmd.Parameters.AddWithValue("@supplierID", supplierID);
                cmd.Parameters.AddWithValue("@categoryID", categoryID);
                count = Convert.ToInt32(cmd.ExecuteScalar());
                cn.Close();
            }

            return count;
        }

        public bool Delete(int productID)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE FROM Products WHERE ProductID=@productID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@productID", productID);
                result = cmd.ExecuteNonQuery() > 0;
                cn.Close();
            }
            return result;
        }

        public bool DeleteAttribute(int attributeID)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE FROM ProductAttributes WHERE AttributeID=@attributeID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@attributeID", attributeID);
                result = cmd.ExecuteNonQuery() > 0;
                cn.Close();
            }
            return result;
        }

        public bool DeletePhoto(int photoID)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE FROM ProductPhotos WHERE PhotoID=@photoID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@photoID", photoID);
                result = cmd.ExecuteNonQuery() > 0;
                cn.Close();
            }
            return result;
        }

        public Product Get(int productID)
        {
            Product result = null;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM Products WHERE ProductID=@productID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@productID", productID);

                var dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (dbReader.Read())
                {
                    result = new Product()
                    {
                        ProductID = Convert.ToInt32(dbReader["ProductID"]),
                        ProductName = Convert.ToString(dbReader["ProductName"]),
                        SupplierID = Convert.ToInt32(dbReader["SupplierID"]),
                        CategoryID = Convert.ToInt32(dbReader["CategoryID"]),
                        Unit = Convert.ToString(dbReader["Unit"]),
                        Price = Convert.ToString(dbReader["Price"]),
                        Photo = Convert.ToString(dbReader["Photo"])

                    };
                }
                cn.Close();
            }
            return result;
        }

        public ProductAttribute GetAttribute(int attributeID)
        {
            ProductAttribute result = null;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM ProductAttributes WHERE AttributeID=@attributeID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@attributeID", attributeID);

                var dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (dbReader.Read())
                {
                    result = new ProductAttribute()
                    {
                        AttributeID = Convert.ToInt32(dbReader["AttributeID"]),
                        AttributeName = Convert.ToString(dbReader["AttributeName"]),
                        ProductID = Convert.ToInt32(dbReader["ProductID"]),
                        DisplayOrder = Convert.ToInt32(dbReader["DisplayOrder"]),
                        AttributeValue = Convert.ToString(dbReader["AttributeValue"])

                    };
                }
                cn.Close();
            }
            return result;

        }

        public ProductPhoto GetPhoto(int photoID)
        {
            ProductPhoto result = null;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM ProductPhotos WHERE PhotoID=@photoID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@photoID", photoID);

                var dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (dbReader.Read())
                {
                    result = new ProductPhoto()
                    {
                        PhotoID = Convert.ToInt32(dbReader["PhotoID"]),
                        Description = Convert.ToString(dbReader["Description"]),
                        ProductID = Convert.ToInt32(dbReader["ProductID"]),
                        DisplayOrder = Convert.ToInt32(dbReader["DisplayOrder"]),
                        Photo = Convert.ToString(dbReader["Photo"])

                    };
                }
                cn.Close();
            }
            return result;
        }

        public bool InUsed(int productID)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText =
                    @"select case when exists
                    (select ProductID from ProductAttributes where ProductID=@productID
					union
					select ProductID from ProductPhotos where ProductID=@productID
					union
					select ProductID from OrderDetails where ProductID=@productID
                                    ) then 1 else 0 end";

                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@productID", productID);
                result = Convert.ToBoolean(cmd.ExecuteScalar());
                cn.Close();
            }
            return result;
        }

        public IList<Product> List(int page = 1, int pageSize = 0, string searchValue = "", int categoryID = 0, int supplierID = 0)
        {
            List<Product> data = new List<Product>();
            if (searchValue != "")
                searchValue = "%" + searchValue + "%";
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                if(categoryID==0&&supplierID==0)
                {
                    cmd.CommandText = @"SELECT *
                                FROM
                                (
                                    SELECT    *, ROW_NUMBER() OVER (ORDER BY ProductName) AS RowNumber
                                    FROM    Products
                                    WHERE (ProductName LIKE @searchValue)
                                )AS t
                                WHERE t.RowNumber BETWEEN (@page - 1) * @pageSize + 1 AND @page * @pageSize;";
                }   
                else
                {
                    if (categoryID==0&&supplierID!=0)
                    {
                        cmd.CommandText = @"SELECT *
                                FROM
                                (
                                    SELECT    *, ROW_NUMBER() OVER (ORDER BY ProductName) AS RowNumber
                                    FROM    Products
                                    WHERE    (ProductName LIKE @searchValue)
                                             and (SupplierID LIKE @supplierID)
                                            ) AS t
                                WHERE t.RowNumber BETWEEN (@page - 1) * @pageSize + 1 AND @page * @pageSize;";
                    } 
                    else
                    {
                        if(categoryID!=0&&supplierID==0)
                        {
                            cmd.CommandText = @"SELECT *
                                FROM
                                (
                                    SELECT    *, ROW_NUMBER() OVER (ORDER BY ProductName) AS RowNumber
                                    FROM    Products
                                    WHERE (ProductName LIKE @searchValue)
                                          and (CategoryID LIKE @categoryID)
                                  ) AS t
                                WHERE t.RowNumber BETWEEN (@page - 1) * @pageSize + 1 AND @page * @pageSize;";
                        }  
                        else
                        {
                            cmd.CommandText = @"SELECT *
                                FROM
                                (
                                    SELECT    *, ROW_NUMBER() OVER (ORDER BY ProductName) AS RowNumber
                                    FROM    Products
                                    WHERE    (ProductName LIKE @searchValue)
                                             and (SupplierID LIKE @supplierID)
                                             and (CategoryID LIKE @categoryID)
                                            
                                ) AS t
                                WHERE t.RowNumber BETWEEN (@page - 1) * @pageSize + 1 AND @page * @pageSize;";
                        }    
                    }    
                }    

                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@page", page);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);
                cmd.Parameters.AddWithValue("@searchValue", searchValue);
                cmd.Parameters.AddWithValue("@supplierID", supplierID);
                cmd.Parameters.AddWithValue("@categoryID", categoryID);

                SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dbReader.Read())
                {
                    data.Add(new Product()
                    {
                        ProductID = Convert.ToInt32(dbReader["ProductID"]),
                        ProductName = Convert.ToString(dbReader["ProductName"]),
                        SupplierID = Convert.ToInt32(dbReader["SupplierID"]),
                        CategoryID = Convert.ToInt32(dbReader["CategoryID"]),
                        Unit = Convert.ToString(dbReader["Unit"]),
                        Price = Convert.ToString(dbReader["Price"]),
                        Photo = Convert.ToString(dbReader["Photo"])
                    });
                }
                dbReader.Close();
                cn.Close();
            }

            return data;
        }

        

        public IList<ProductAttribute> ListAttribute(int productID)
        {
            List<ProductAttribute> data = new List<ProductAttribute>();
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM ProductAttributes where ProductID=@productID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@productID", productID);

                SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dbReader.Read())
                {
                    data.Add(new ProductAttribute()
                    {
                        AttributeID = Convert.ToInt32(dbReader["AttributeID"]),
                        ProductID = Convert.ToInt32(dbReader["ProductID"]),
                        AttributeName = Convert.ToString(dbReader["AttributeName"]),
                        AttributeValue = Convert.ToString(dbReader["AttributeValue"]),
                        DisplayOrder = Convert.ToInt32(dbReader["DisplayOrder"])

                    });
                }
                dbReader.Close();
                cn.Close();

            }
            return data;
        }

        public IList<ProductPhoto> ListPhoto(int productID)
        {
            List<ProductPhoto> data = new List<ProductPhoto>();
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM ProductPhotos where ProductID=@productID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@productID", productID);

                SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dbReader.Read())
                {
                    data.Add(new ProductPhoto()
                    {
                        PhotoID = Convert.ToInt32(dbReader["PhotoID"]),
                        ProductID = Convert.ToInt32(dbReader["ProductID"]),
                        Photo = Convert.ToString(dbReader["Photo"]),
                        Description = Convert.ToString(dbReader["Description"]),
                        DisplayOrder = Convert.ToInt32(dbReader["DisplayOrder"])

                    });
                }
                dbReader.Close();
                cn.Close();

            }
            return data;
        }

        public bool Update(Product data)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"Update Products
                                    set ProductName=@productName,
                                        SupplierID=@supplierID,
                                        CategoryID=@categoryID,
                                        Unit=@unit,
                                        Price=@price,
                                        Photo=@photo
                                        where ProductID=@productID";


                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@productName", data.ProductName);
                cmd.Parameters.AddWithValue("@supplierID", data.SupplierID);
                cmd.Parameters.AddWithValue("@categoryID", data.CategoryID);
                cmd.Parameters.AddWithValue("@unit", data.Unit);
                cmd.Parameters.AddWithValue("@price", data.Price);
                cmd.Parameters.AddWithValue("@photo", data.Photo);
                cmd.Parameters.AddWithValue("@productID", data.ProductID);

                result = cmd.ExecuteNonQuery() > 0;

                cn.Close();
            }
            return result;
        }

        public bool UpdateAttribute(ProductAttribute data)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"Update ProductAttributes
                                    set ProductID=@productID,
                                        AttributeName=@attributeName,
                                        AttributeValue=@attributeValue,
                                        DisplayOrder=@displayOrder
                                        
                                        where AttributeID=@attributeID";


                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@productID", data.ProductID);
                cmd.Parameters.AddWithValue("@attributeName", data.AttributeName);
                cmd.Parameters.AddWithValue("@displayOrder", data.DisplayOrder);
                cmd.Parameters.AddWithValue("@attributeValue", data.AttributeValue);
                cmd.Parameters.AddWithValue("@attributeID", data.AttributeID);

                result = cmd.ExecuteNonQuery() > 0;

                cn.Close();
            }
            return result;
        }

        public bool UpdatePhoto(ProductPhoto data)
        {
            bool result = false;
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"Update ProductPhotos
                                    set ProductID=@productID,
                                        Photo=@photo,
                                        Description=@description,
                                        DisplayOrder=@displayOrder
                                        
                                        where PhotoID=@photoID";


                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@productID", data.ProductID);
                cmd.Parameters.AddWithValue("@description", data.Description);
                cmd.Parameters.AddWithValue("@displayOrder", data.DisplayOrder);
                cmd.Parameters.AddWithValue("@photo", data.Photo);
                cmd.Parameters.AddWithValue("@photoID", data.PhotoID);

                result = cmd.ExecuteNonQuery() > 0;

                cn.Close();
            }
            return result;
        }
    }
}
