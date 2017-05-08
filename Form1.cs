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
        connect_mysql connect1 = new connect_mysql();
        MySqlCommand comm;
        MySqlConnection conn;

        public void update_text()
        {
            connect1.ip_mysqlserver = textBox1.Text;
            connect1.user_id = textBox4.Text;
            connect1.password = textBox3.Text;
            connect1.database = "css";
        }

        public Form1()            
        {            
            InitializeComponent();
            update_text();           
            conn = new MySqlConnection(connect1.connect_str());
            textBox2.Text = "初始化完成";
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            update_text();            
            if (connect1.connect_sucessfull)
            { label2.Text = "成功"; MessageBox.Show("连接成功"); textBox2.AppendText("\r\n连接数据库成功"); }
            else
            { label2.Text = "失败"; MessageBox.Show("连接失败"); textBox2.AppendText("\r\n连接失败"); }
        }

        private void button2_Click(object sender, EventArgs e)

        {
            update_text();
            if (connect1.connect_sucessfull)
            {
                label2.Text = "成功";
                textBox2.AppendText("\r\n连接数据库成功");
                connect1.sqlstr = "SELECT * FROM  record_item ";
                comm = new MySqlCommand(connect1.sqlstr, conn);
                Console.WriteLine(connect1.sqlstr + connect1.connect_str());
                MySqlDataAdapter adap = new MySqlDataAdapter(comm);
                DataSet ds = new DataSet();
                adap.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0].DefaultView;
                textBox2.AppendText("\r\n" + connect1.sqlstr);
            }

            else
            { label2.Text = "失败"; MessageBox.Show("连接失败"); }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            update_text();
            if (connect1.connect_sucessfull)
            { label2.Text = "成功"; }
            else
            { label2.Text = "失败"; }            
            string timemin=dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm");
            string timemax=dateTimePicker2.Value.ToString("yyyy-MM-dd HH:mm");
            string timenow = string.Format("{0:d}", System.DateTime.Now);
            string table=comboBox1.SelectedItem.ToString();
            if (table == "record_item" && connect1.connect_sucessfull)
            {
                connect1.sqlstr = "SELECT FILE_PATH FROM " + table + " WHERE BEGIN_TIME >=  \"" + timemin + "\" AND BEGIN_TIME <= \"" + timemax + " \"";                
                comm = new MySqlCommand(connect1.sqlstr, conn);
                textBox2.AppendText("\r\n" + connect1.sqlstr);
                conn.Open();
                Console.WriteLine(connect1.sqlstr);                
                MySqlDataReader dateReader = comm.ExecuteReader();
                while (dateReader.Read())
                    {
                        string record_path = (string)dateReader[0].ToString();
                        //record_begin_time = record_begin_time.Remove(4, 1); record_begin_time = record_begin_time.Remove(5, 1);
                       //record_begin_time = record_begin_time.Insert(4, "-"); record_begin_time = record_begin_time.Insert(6, "-");
                       //Console.WriteLine("时间: " + record_begin_time); 
                        try
                        {
                            File.Delete(record_path);
                            Console.WriteLine(record_path);
                        }
                        catch (Exception)
                        { MessageBox.Show("未找到对应文件: "+record_path); }
                    }
                dateReader.Close();
                conn.Close();
                conn.Open();
                connect1.sqlstr = "DELETE FROM " + table + " WHERE BEGIN_TIME >= \"" + timemin + "\" AND BEGIN_TIME <= \"" + timemax + " \"";
                comm = new MySqlCommand(connect1.sqlstr, conn);
                Console.WriteLine(connect1.sqlstr);
                int iRet = comm.ExecuteNonQuery();//这里返回的是受影响的行数，为int值。可以根据返回的值进行判断是否插入成功。
                if (iRet > 0)
                {                    
                    MessageBox.Show("文件与记录删除成功");
                }
                else
                {

                    MessageBox.Show("该时间段内没有数据");
                    connect1.sqlstr = "SELECT * FROM " + table + " WHERE BEGIN_TIME >=  \"" + timemin + "\" AND BEGIN_TIME <= \"" + timemax + " \"";
                    comm = new MySqlCommand(connect1.sqlstr, conn);
                    MySqlDataAdapter adap = new MySqlDataAdapter(comm);
                    DataSet ds = new DataSet();
                    adap.Fill(ds);
                    dataGridView1.DataSource = ds.Tables[0].DefaultView;

                }
                conn.Close();//关闭连接
                textBox2.AppendText("\r\n" + connect1.sqlstr);
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
                    textBox2.AppendText("\r\n" + connect1.sqlstr);

                }
                conn.Close();//关闭连接                
            }
            else { MessageBox.Show("无法连接数据库服务"); }  
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = comboBox1.Items.IndexOf("record_item");
        }

       
    }
}
