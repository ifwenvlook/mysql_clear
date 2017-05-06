using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MySql.Data;
using System.IO;
namespace mysql
{
    public partial class Form1 : Form
    {
        connect_mysql connect1;
        MySqlCommand comm;
        MySqlConnection conn;
        file_io file1;
        public Form1()            
        {            
            InitializeComponent();
            connect1 = new connect_mysql { database = "css" };           
            string connect_str = connect1.connect_str();
            conn = new MySqlConnection(connect_str);
            comm = new MySqlCommand(connect1.sqlstr, conn);
            file1 = new file_io { creat_path = @"C:\Users\wlz\Desktop\test.txt", record_path = @"C:\Users\wlz\Desktop\test.txt" };

            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            connect1.ip_mysqlserver = textBox1.Text;
            connect1.user_id = textBox4.Text;
            connect1.password = textBox3.Text;
            if (connect1.connect_sucessfull)
            { label2.Text = "成功"; MessageBox.Show("连接成功"); }
            else
            { label2.Text = "失败"; MessageBox.Show("连接失败"); }            
        }

        private void button2_Click(object sender, EventArgs e)

        {
            connect1.ip_mysqlserver = textBox1.Text;
            connect1.user_id = textBox4.Text;
            connect1.password = textBox3.Text;
            if (connect1.connect_sucessfull)
            {
                label2.Text = "成功";
                string connect_str = connect1.connect_str();
                connect1.sqlstr = "SELECT * FROM  record_item ";
                conn = new MySqlConnection(connect_str);
                comm = new MySqlCommand(connect1.sqlstr, conn);
                MySqlDataAdapter adap = new MySqlDataAdapter(comm);
                DataSet ds = new DataSet();
                adap.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0].DefaultView;
                textBox2.Text = connect1.sqlstr;
            }

            else
            { label2.Text = "失败"; MessageBox.Show("连接失败"); }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            connect1.ip_mysqlserver = textBox1.Text;
            connect1.user_id = textBox4.Text;
            connect1.password = textBox3.Text;
            if (connect1.connect_sucessfull)
            { label2.Text = "成功"; }
            else
            { label2.Text = "失败"; }
            string connect_str = connect1.connect_str();
            MySqlConnection conn = new MySqlConnection(connect_str);
            string timemin=dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm");
            string timemax=dateTimePicker2.Value.ToString("yyyy-MM-dd HH:mm");
            string timenow = string.Format("{0:d}", System.DateTime.Now);
            string table=comboBox1.SelectedItem.ToString();
            if (table == "record_item" && connect1.connect_sucessfull)
            {
                connect1.sqlstr = "DELETE FROM " + table + " WHERE \"" + timemin + "\" < BEGIN_TIME < \"" + timemax + "\"";                
                comm = new MySqlCommand(connect1.sqlstr, conn);
                conn.Open();
                Console.WriteLine(connect1.sqlstr);
                int iRet = comm.ExecuteNonQuery();//这里返回的是受影响的行数，为int值。可以根据返回的值进行判断是否插入成功。
                if (iRet > 0)
                {

                    MessageBox.Show("删除成功");

                }
                else
                {

                    MessageBox.Show("该时间段内没有数据");
                    connect1.sqlstr = "SELECT * FROM " + table + " WHERE \"" + timemin + "\" < BEGIN_TIME < \"" + timemax + "\"";
                    comm = new MySqlCommand(connect1.sqlstr, conn);
                    MySqlDataAdapter adap = new MySqlDataAdapter(comm);
                    DataSet ds = new DataSet();
                    adap.Fill(ds);
                    dataGridView1.DataSource = ds.Tables[0].DefaultView;

                }
                conn.Close();//关闭连接
                textBox2.Text = connect1.sqlstr;
            }
            else if (table != "record_item" && connect1.connect_sucessfull)
            {
                connect1.sqlstr = "DELETE FROM  " + table;
                comm = new MySqlCommand(connect1.sqlstr, conn);
                conn.Open();
                int iRet = comm.ExecuteNonQuery();//这里返回的是受影响的行数，为int值。可以根据返回的值进行判断是否插入成功。
                if (iRet > 0)
                {
                    Console.WriteLine("this is table: " + table);
                    Console.WriteLine("清空了 " + table + "中的数据");
                    MessageBox.Show("删除表成功");

                }
                else
                {
                    connect1.sqlstr = "SELECT *  FROM " + table;
                    comm = new MySqlCommand(connect1.sqlstr, conn);
                    MySqlDataAdapter adap = new MySqlDataAdapter(comm);
                    DataSet ds = new DataSet();
                    adap.Fill(ds);
                    dataGridView1.DataSource = ds.Tables[0].DefaultView;
                    Console.WriteLine("this is table: " + table);
                    Console.WriteLine(table + "中的数据为空");
                    MessageBox.Show("删除失败," + table + " 表是空的");

                }
                conn.Close();//关闭连接
                textBox2.Text = connect1.sqlstr;
            }
            else { MessageBox.Show("无法连接数据库服务"); }  
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = comboBox1.Items.IndexOf("record_item");
        }

        /*private void button4_Click(object sender, EventArgs e)//创建文件示例
        {
            //参数1：指定要判断的文件路径
            if (!File.Exists(file1.creat_path))
            {
                //参数1：要创建的文件路径，包含文件名称、后缀等
                FileStream fs = File.Create(file1.creat_path);
                fs.Close();
                MessageBox.Show("文件创建成功！");
            }
            else
            {
                MessageBox.Show("文件已经存在！");
            }
        }*/
        private void button4_Click(object sender,EventArgs E)
        {
            if (File.Exists(file1.record_path))
            {
                File.Delete(file1.record_path);
                MessageBox.Show("删除成功");
            }
            else
            { MessageBox.Show("文件不存在！"); }
        }


        
    }
}
