using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV18T1021230.DataLayer
{
    /// <summary>
    /// Định nghĩa các phép dữ liệu chung chung
    /// </summary>
    public interface ICommomDAL <T> where T:class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        IList<T> List(int page=1, int pageSize=0, string searchValue="");
        

        int Count(string searchValue);
        T Get(int id);
        int Add(T data);
        bool Update(T data);
        bool Delete(int id);
        bool InUsed(int id);
    }
}
