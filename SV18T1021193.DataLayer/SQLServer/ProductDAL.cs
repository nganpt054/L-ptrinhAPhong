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
    public class ProductDAL : _BaseDAL, ICommomDAL<Product>
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

        public int Count(string searchValue)
        {
            int count = 0;

            using (SqlConnection cn = OpenConnection())
            {
                if (searchValue != "")
                    searchValue = "%" + searchValue + "%";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT    COUNT(*)
                                FROM    Products
                                WHERE    (@searchValue = N'')
                                    OR    (
                                            (ProductName LIKE @searchValue)
                                         
                                        )
                            ";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                cmd.Parameters.AddWithValue("@searchValue", searchValue);
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

        public IList<Product> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            List<Product> data = new List<Product>();
            if (searchValue != "")
                searchValue = "%" + searchValue + "%";
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT *
                                FROM
                                (
                                    SELECT    *, ROW_NUMBER() OVER (ORDER BY ProductName) AS RowNumber
                                    FROM    Products
                                    WHERE    (@searchValue = N'')
                                        OR    (
                                                (ProductName LIKE @searchValue)
                                             
                                            )
                                ) AS t
                                WHERE t.RowNumber BETWEEN (@page - 1) * @pageSize + 1 AND @page * @pageSize;";

                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@page", page);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);
                cmd.Parameters.AddWithValue("@searchValue", searchValue);

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

                cmd.Parameters.AddWithValue("@ProductName", data.ProductName);
                cmd.Parameters.AddWithValue("@SupplierID", data.SupplierID);
                cmd.Parameters.AddWithValue("@CategoryID", data.CategoryID);
                cmd.Parameters.AddWithValue("@Unit", data.Unit);
                cmd.Parameters.AddWithValue("@Price", data.Price);
                cmd.Parameters.AddWithValue("@Photo", data.Photo);
                cmd.Parameters.AddWithValue("@ProductID", data.ProductID);

                result = cmd.ExecuteNonQuery() > 0;

                cn.Close();
            }
            return result;
        }
    }
}
