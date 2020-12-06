using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab3_client
{
    public partial class Form1 : Form
    {
        public int selected =0;
        
        public static Object ByteArrayToObject(byte[] arrBytes)
        {
            using (var memStream = new MemoryStream())
            {
                var binForm = new BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                var obj = binForm.Deserialize(memStream);
                return obj;
            }
        }
        public static byte[] ObjectToByteArray(Object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        TcpClient tcp_client = new TcpClient("localhost", 5555);
        ASCIIEncoding ae = new ASCIIEncoding();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Debug.WriteLine("exit");
            NetworkStream ns = tcp_client.GetStream();
            String command = "exit";
            String res = command + "|";
            byte[] sent = ae.GetBytes(res);
            ns.Write(sent, 0, sent.Length);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            //Если выбран RadioButton просмотра информции то...
            if (radioButton1.Checked == true)
            {
                //Создаем объект класса NetworkStream и ассоциируем его объектом класса TcpClient
                NetworkStream ns = tcp_client.GetStream();
                String command = "view";
                String res = command + "|";
                //Создаем переменные типа byte[] для отправки запроса и получения результата
                byte[] sent = ae.GetBytes(res);
                byte[] recieved = new byte[256];
                //Отправляем запрос на сервер
                ns.Write(sent, 0, sent.Length);
                //Получаем результат выполнения запроса с сервера
                ns.Read(recieved, 0, recieved.Length);
                //Отображаем полученный результат в клиентском RichTextBox
                string[] ss = ByteArrayToObject(recieved) as string[];
                String status = "=>Command sent:view data";
                //Отображаем служебную информацию в клиентском ListBox
                listBox1.Items.Add(status);
                foreach(string line in ss)
                {
                    listBox2.Items.Add(line);
                }
            }
            if (radioButton2.Checked == true)
            {
                string[] ttd = {richTextBox1.Text,richTextBox2.Text,richTextBox3.Text };
                byte[] dde = ObjectToByteArray(ttd);
                NetworkStream ns = tcp_client.GetStream();
                String command = "add";
                String res = command + "|";
                //Создаем переменные типа byte[] для отправки запроса и получения результата
                byte[] sent = ae.GetBytes(res);
                byte[] recieved = new byte[256];
                //Отправляем запрос на сервер
                ns.Write(sent, 0, sent.Length);
                ns.Read(recieved, 0, recieved.Length);
                string[] ss = ByteArrayToObject(recieved) as string[];
                Debug.WriteLine(ss[0]);
                ns.Write(dde, 0, dde.Length);
                Debug.WriteLine("Otpravil");
                richTextBox1.Clear();
                richTextBox2.Clear();
                richTextBox3.Clear();
                String status = "=>Command sent:add data";
                listBox1.Items.Add(status);
            }
            if (radioButton3.Checked == true)
            {
                string[] ttd = { selected.ToString() };
                byte[] dde = ObjectToByteArray(ttd);
                NetworkStream ns = tcp_client.GetStream();
                String command = "delete";
                String res = command + "|";
                //Создаем переменные типа byte[] для отправки запроса и получения результата
                byte[] sent = ae.GetBytes(res);
                byte[] recieved = new byte[256];
                //Отправляем запрос на сервер
                ns.Write(sent, 0, sent.Length);
                ns.Read(recieved, 0, recieved.Length);
                string[] ss = ByteArrayToObject(recieved) as string[];
                Debug.WriteLine(ss[0]);
                ns.Write(dde, 0, dde.Length);
                Debug.WriteLine("Otpravil");
                richTextBox1.Clear();
                richTextBox2.Clear();
                richTextBox3.Clear();
                String status = "=>Command sent:delete data";
                listBox1.Items.Add(status);
            }
            if (radioButton4.Checked == true)
            {
                string[] ttd = { selected.ToString()};
                string[] ttn = { richTextBox1.Text, richTextBox2.Text, richTextBox3.Text };
                byte[] dde = ObjectToByteArray(ttd);
                byte[] ddn = ObjectToByteArray(ttn);
                NetworkStream ns = tcp_client.GetStream();
                String command = "change";
                String res = command + "|";
                //Создаем переменные типа byte[] для отправки запроса и получения результата
                byte[] sent = ae.GetBytes(res);
                byte[] recieved = new byte[256];
                //Отправляем запрос на сервер
                ns.Write(sent, 0, sent.Length);
                ns.Read(recieved, 0, recieved.Length);
                string[] ss = ByteArrayToObject(recieved) as string[];
                Debug.WriteLine(ss[0]);
                ns.Write(dde, 0, dde.Length);
                Debug.WriteLine("Otpravil");
                ns.Read(recieved, 0, recieved.Length);
                ns.Write(ddn, 0, ddn.Length);
                Debug.WriteLine("Otpravil");
                richTextBox1.Clear();
                richTextBox2.Clear();
                richTextBox3.Clear();
                String status = "=>Command sent:change data";
                listBox1.Items.Add(status);
            }
        }

        private void Form1_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            Debug.WriteLine("exit");
            NetworkStream ns = tcp_client.GetStream();
            String command = "exit";
            String res = command + "|";
            byte[] sent = ae.GetBytes(res);
            ns.Write(sent, 0, sent.Length);
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            selected = listBox2.SelectedIndex;
            Debug.WriteLine("Selected"+selected);
            string sel = listBox2.SelectedItem.ToString();
            string[] sel_array = sel.Split(' ');
            richTextBox1.Text = sel_array[0];
            richTextBox2.Text = sel_array[1];
            richTextBox3.Text = sel_array[2];
            foreach(string line in sel_array)
            {
                Debug.WriteLine(line);
            }
        }
    }
}
