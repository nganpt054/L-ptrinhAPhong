using SV18T1021230.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV18T1021230.DataLayer
{
    public interface ISupplierDAL
    {
        /// <summary>
        /// Tìm kiếm và lấy danh sách nhà cung cấp dưới dạng phân trang
        /// </summary>
        /// <param name="page">Trang cần xem</param>
        /// <param name="pageSize">Số dòng hiển thị trên 1 trang</param>
        /// <param name="searchValue">Giá trị tìm kiếm tương đối (nếu là kiểu rỗng thì hiển thị toàn bộ)</param>
        /// <returns></returns>
        IList<Supplier> List(int page, int pageSize, string searchValue);

        /// <summary>
        /// Đếm số lượng nhà cung cấp thỏa điều kiện
        /// </summary>
        /// <param name="searchValue">Giá trị tìm kiếm tương đối (nếu là kiểu rỗng thì hiển thị toàn bộ)</param>
        /// <returns></returns>
        int Count(string searchValue);
        /// <summary>
        /// Lấy thông tin của 1 khách hàng theo mã khách hàng
        /// </summary>
        /// <param name="supplierID"></param>
        /// <returns></returns>
        Supplier Get(int supplierID);
        /// <summary>
        /// Bổ sung một khách hàng. Hàm trả về mã của khách hàng được bổ sung
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int Add(Supplier data);
        /// <summary>
        /// Cập nhật thông tin khách hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(Supplier data);
        /// <summary>
        /// Xóa thông tin khách hàng
        /// </summary>
        /// <param name="supplierID"></param>
        /// <returns></returns>
        bool Delete(int supplierID);
        /// <summary>
        /// Kiểm tra khách hàng có dữ liệu liên quan hay ko 
        /// </summary>
        /// <param name="supplierID"></param>
        /// <returns></returns>
        bool InUsed(int supplierID);
    }
}
