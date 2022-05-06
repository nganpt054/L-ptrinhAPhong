﻿using SV18T1021193.DomainModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV18T1021193.DataLayer.SQLServer
{
    public class CountryDAL : _BaseDAL,ICommonDAL<Country>
    {
        public CountryDAL(string connectionString) : base(connectionString)
        {
        }

        public int Add(Country data)
        {
            throw new NotImplementedException();
        }

        public int Count(string searchValue)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Country Get(int id)
        {
            throw new NotImplementedException();
        }

        public bool InUsed(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IList<Country> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            List<Country> data = new List<Country>();
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM Countries";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;

                SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dbReader.Read())
                {
                    data.Add(new Country()
                    {
                        CountryName=Convert.ToString(dbReader["CountryName"])   
                    });
                }
                dbReader.Close();
                cn.Close();
                
            }
            return data;
        }

       

        public bool Update(Country data)
        {
            throw new NotImplementedException();
        }
    }
}
