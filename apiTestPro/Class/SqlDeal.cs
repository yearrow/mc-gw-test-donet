using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;


namespace apiTestPro
{
    public class SqlDeal
    {
        //创建数据操作类；
        public static SqlConnection conn;
        public static string connection = System.Configuration.ConfigurationSettings.AppSettings["connectionString"];
      
        //执行数据表的增加、删除、修改操作
        public static int NonQuery(string sql)
        {
            conn = new SqlConnection(connection);
            int t = 0;
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                t = cmd.ExecuteNonQuery();
            //    PublicMethod.GroupLog(App, sql);//插入日志方法
            }
            catch
            {
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();

            }
            return t;
        }
       
        //执行数据查询操作；
        public static  DataSet Query(string sql)
        {
            
            conn = new SqlConnection(connection);
            DataSet ds = new DataSet();
            try
            {
                conn.Open();
                SqlDataAdapter dap = new SqlDataAdapter(sql, conn);
                dap.Fill(ds);
                
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
            return ds;
        }

        public static string UpdateStr(string table, string value, int id)
        {
            string[] valuegroup = value.Trim(',').Split(',');

            string str = "" + valuegroup[0].Split('=')[0] + "=" + valuegroup[0].Split('=')[1];

            for (int i = 1; i < valuegroup.Length; i++)
            {
                str += "," + valuegroup[i].Split('=')[0] + "=" + valuegroup[i].Split('=')[1];
            }
            string sql = "update " + table + " set " + str + " where ID=" + id;
            sql = sql.Replace("&sbquo;", ",").Replace("&equal;", "=");

            return sql;
        }
        public static string UpdateStr(string table, string value, string line, string id)
        //更新数据
        {
            string[] valuegroup = value.Trim(',').Split(',');

            string str = "" + valuegroup[0].Split('=')[0] + "=" + valuegroup[0].Split('=')[1];

            for (int i = 1; i < valuegroup.Length; i++)
            {
                str += "," + valuegroup[i].Split('=')[0] + "=" + valuegroup[i].Split('=')[1];
            }
            string sql = "update " + table + " set " + str + " where " + line + "='" + id + "'";
            sql = sql.Replace("&sbquo;", ",").Replace("&equal;", "=");

            return sql;
        }
        public static string UpdateStr(string table, string value, string line, int id)
        {
            string[] valuegroup = value.Trim(',').Split(',');

            string str = "" + valuegroup[0].Split('=')[0] + "=" + valuegroup[0].Split('=')[1];

            for (int i = 1; i < valuegroup.Length; i++)
            {
                str += "," + valuegroup[i].Split('=')[0] + "=" + valuegroup[i].Split('=')[1];
            }
            string sql = "update " + table + " set " + str + " where " + line + "=" + id;
            sql = sql.Replace("&sbquo;", ",").Replace("&equal;", "=");

            return sql;
        }
        public static string InsertStr(string table, string value)
        {
            //传入表名与相应字段插入表中，返回SQL
            string[] valuegroup = value.Trim(',').Split(',');

            string str1 = "";
            string str2 = "";

            for (int i = 0; i < valuegroup.Length; i++)
            {
                str1 += "," + valuegroup[i].Split('=')[0] + "";
                str2 += "," + valuegroup[i].Split('=')[1];
            }
            string sql = "insert into " + table + " (" + str1.Trim(',') + ") values (" + str2.Trim(',') + ")";
            return sql = sql.Replace("&sbquo;", ",").Replace("&equal;", "=");

           
        }
        public static string Build(string[] str1, string[] str2)
        {
            string sql = "";
            for (int i = 0; i < str1.Length; i++)
            {
                sql += str1[i] + "='" + str2[i].Replace(",", "&sbquo;").Replace("=", "&equal;") + "'" + ",";
            }
            return sql;
        }
        public static string Build(string str1, string str2)
        {
            return str1 + "='" + str2.Replace(",", "&sbquo;").Replace("=", "&equal;") + "'" + ",";
        }
        public static string Build(string str1, DateTime str2)
        {
            return str1 + "='" + str2.ToString("yyyy-MM-dd HH:mm:ss") + "'" + ",";
        }
        public static string Build(string str1, int str2)
        {
            return str1 + "=" + str2 + "" + ",";
        }
        public static string Build(string str1, float str2)
        {
            return str1 + "=" + str2 + "" + ",";
        }
        public static string Build(string str1, double str2)
        {
            return str1 + "=" + str2 + "" + ",";
        }
        public static string Build(string str1, decimal str2)
        {
            return str1 + "=" + str2 + "" + ",";
        }
        public static string Build(string str1, int str2, string act)
        {
            if (act == "+")
            {
                return str1 + "=" + str1 + "+" + str2 + ",";
            }
            else
            {
                return str1 + "=" + str1 + "-" + str2 + ",";
            }
        }
        public static string Build(string str1, float str2, string act)
        {
            if (act == "+")
            {
                return str1 + "=" + str1 + "+" + str2 + ",";
            }
            else
            {
                return str1 + "=" + str1 + "-" + str2 + ",";
            }
        }
        public static string Build(string str1, double str2, string act)
        {
            if (act == "+")
            {
                return str1 + "=" + str1 + "+" + str2 + ",";
            }
            else
            {
                return str1 + "=" + str1 + "-" + str2 + ",";
            }
        }

        public static string DeleteStr(string table, int id)
        {
            string sql = "delete from " + table + " where ID=" + id;
            return sql;
        }
        public static string DeleteStr(string table, string user, int userid, string value, int id)
        //
        {
            string sql = "delete from " + table + " where " + user + "=" + userid + " and " + value + "=" + id;
            return sql;
        }

        public static string DeleteStr(string table, string value, string id)
        {
            string sql = "delete from " + table + " where " + value + "='" + id + "'";
            return sql;
        }

        public static string DeleteStr(string table, string value, int id)
        {
            string sql = "delete from " + table + " where " + value + "=" + id;
            return sql;
        }


        public static decimal ConvertToDecimal(string obj)
        {
            decimal result = 0m;
            if (decimal.TryParse(obj, out result))
            {
                return result;
            }
            else
            {
                return 0m;
            }
        }
    }

}
