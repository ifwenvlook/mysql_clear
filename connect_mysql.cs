using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;


namespace mysql
{
    class connect_mysql
    {        
        public string ip_mysqlserver;
        public string user_id;
        public string password;
        public string database;
        public string sqlstr;
        public string connect_str()
        {
            string x = "server=" + ip_mysqlserver + ";user id=" +
                user_id + ";password=" + password + ";database=" + database;
            return x;
        }

        private bool Connect_sucessfull()
        {
            //string mysql_connect_str = "server=127.0.0.1;user id=root;password=;database=css"; //根据自己的设置
           
            string mysql_connect_str = "server=" + ip_mysqlserver + ";user id=" +
                user_id + ";password=" + password + ";database=" + database;
            MySqlConnection conn = new MySqlConnection(mysql_connect_str);
            try
            {
                conn.Open();
                Console.WriteLine("连接成功！");
                conn.Close();
                return true;
            }
            catch (Exception)
            {
                Console.WriteLine("连接失败！");
                return false; 
            }
            finally
            {
                conn.Close();
                Console.WriteLine("连接关闭！");
            }
            
        }
        //此时外部只能调用public方法，且调用时不加()
        public bool connect_sucessfull{ get {return Connect_sucessfull();}}    
    }
}