using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;

namespace Lab3
{


    public partial class Form1 : Form
    {

        public static byte[] ObjectToByteArray(Object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

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

        public class ThreadClass
        {
            String fileName = "d:\\file1.txt";
            int fileCount = 0;
            TcpListener listener = null;
            Socket socket = null;
            NetworkStream ns = null;
            ASCIIEncoding ae = null;
            Form1 form = null;
            public void ThreadOperations()
            {
                String cmd = "";
                while (!(cmd.CompareTo("exit") == 0))
                {
                    //Создаем новую переменную типа byte[]
                    byte[] received = new byte[256];
                    //С помощью сетевого потока считываем в переменную received данные от клиента
                    ns.Read(received, 0, received.Length);
                    String s1 = ae.GetString(received);
                    int i = s1.IndexOf("|", 0);
                    cmd = s1.Substring(0, i);

                    if (cmd.CompareTo("view") == 0)
                    {
                        Debug.WriteLine("view");
                        // Создаем переменную типа byte[] для отправки ответа клиенту
                        byte[] sent = new byte[256];
                        //Создаем объект класса FileStream для последующего чтения информации из файла
                        FileStream fstr = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                        string[] astr = File.ReadAllLines(fileName);
                        StreamReader sr = new StreamReader(fstr);
                        //Запись в переменную sent содержания прочитанного файла
                        /*sent = ae.GetBytes(sr.ReadToEnd());*/
                        byte[] stroki = ObjectToByteArray(astr);
                        Array.Copy(stroki, sent, stroki.Length);
                        Debug.WriteLine(sent.Length);
                        sr.Close();
                        fstr.Close();
                        //Отправка информации клиенту
                        ns.Write(sent, 0, sent.Length);
                        
                    }
                    if (cmd.CompareTo("add") == 0)
                    {
                        string[] ans = { "get ready" };
                        byte[] sent = new byte[256];
                        byte[] rev = new byte[256];
                        byte[] stroki = ObjectToByteArray(ans);
                        Array.Copy(stroki, sent, stroki.Length);
                        Debug.WriteLine(sent.Length);
                        ns.Write(sent, 0, sent.Length);
                        Debug.WriteLine("Wait");
                        ns.Read(rev, 0, rev.Length);
                        Debug.WriteLine("Wait2");
                        var reds = ByteArrayToObject(rev) as string[];
                        Debug.WriteLine(reds[0]);

                        FileStream fstr = new FileStream("d:\\file1.txt", FileMode.OpenOrCreate, FileAccess.Write);
                        fstr.Seek(0, SeekOrigin.End);
                        StreamWriter sw = new StreamWriter(fstr);
                        //запись в файл
                        sw.WriteLine(reds[0] + " " + reds[1] + " " + reds[2]);
                        sw.Close();
                        fstr.Close();
                    }
                    if (cmd.CompareTo("delete") == 0)
                    {
                        string[] ans = { "get ready" };
                        byte[] sent = new byte[256];
                        byte[] rev = new byte[256];
                        byte[] stroki = ObjectToByteArray(ans);
                        Array.Copy(stroki, sent, stroki.Length);
                        Debug.WriteLine(sent.Length);
                        ns.Write(sent, 0, sent.Length);
                        Debug.WriteLine("Wait");
                        ns.Read(rev, 0, rev.Length);
                        Debug.WriteLine("Wait2");
                        var reds = ByteArrayToObject(rev) as string[];
                        Debug.WriteLine(reds[0]);



                        string[] astr = File.ReadAllLines(fileName);
                        var new_astr = new List<string>();

                        for(int j = 0; j < astr.Length; j++)
                        {
                            if (j!=System.Int32.Parse( reds[0]))
                            {
                                new_astr.Add(astr[j]);
                            }
                         
                        }

                        FileStream fstr = new FileStream("d:\\file1.txt", FileMode.Create, FileAccess.Write);
                        fstr.Seek(0, SeekOrigin.End);
                        StreamWriter sw = new StreamWriter(fstr);
                        //запись в файл
                       foreach(string line in new_astr)
                        {
                            sw.WriteLine(line);
                        }
                        sw.Close();
                        fstr.Close();
                    }
                    if (cmd.CompareTo("change") == 0)
                    {
                        string[] ans = { "get ready" };
                        byte[] sent = new byte[256];
                        byte[] rev = new byte[256];
                        byte[] rev_2 = new byte[256];
                        byte[] stroki = ObjectToByteArray(ans);
                        Array.Copy(stroki, sent, stroki.Length);
                        Debug.WriteLine(sent.Length);
                        ns.Write(sent, 0, sent.Length);
                        Debug.WriteLine("Wait");
                        ns.Read(rev, 0, rev.Length);
                        Debug.WriteLine("Wait2");
                        ns.Write(sent, 0, sent.Length);
                        ns.Read(rev_2, 0, rev_2.Length);
                        var reds = ByteArrayToObject(rev) as string[];
                        var rads_2 = ByteArrayToObject(rev_2) as string[];
                        Debug.WriteLine(reds[0]);



                        string[] astr = File.ReadAllLines(fileName);
                        var new_astr = new List<string>();

                        for (int j = 0; j < astr.Length; j++)
                        {
                            if (j != System.Int32.Parse(reds[0]))
                            {
                                new_astr.Add(astr[j]);
                            }
                            else
                            {
                                new_astr.Add(rads_2[0] + " "+rads_2[1]+" "+rads_2[2]);
                            }

                        }

                        FileStream fstr = new FileStream("d:\\file1.txt", FileMode.Create, FileAccess.Write);
                        fstr.Seek(0, SeekOrigin.End);
                        StreamWriter sw = new StreamWriter(fstr);
                        //запись в файл
                        foreach (string line in new_astr)
                        {
                            sw.WriteLine(line);
                        }
                        sw.Close();
                        fstr.Close();
                    }


                }
            }
            public Thread Start(NetworkStream ns, String fileName, int fileCount, Form1 form)
            {
                this.ns = ns;
                ae = new ASCIIEncoding();
                this.fileName = fileName;
                this.fileCount = fileCount;
                this.form = form;
                //Создание нового экземпляра класса Thread
                Thread thread = new Thread(new ThreadStart(ThreadOperations));
                //Запуск потока
                thread.Start();
                return thread;
            }
        }

        String fileName = "d:\\file1.txt";
        int fileCount = 0;
        TcpListener listener = null;
        Socket socket = null;
        NetworkStream ns = null;
        ASCIIEncoding ae = null;

        public Form1()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
      
        private void button1_Click(object sender, EventArgs e)
        {
            listener = new TcpListener(IPAddress.Any, 5555);
            listener.Start();
            socket = listener.AcceptSocket();

            if (socket.Connected)
            {
                ns = new NetworkStream(socket);
                ae = new ASCIIEncoding();
                //Создаем новый экземпляр класса ThreadClass
                ThreadClass threadClass = new ThreadClass();
                //Создаем новый поток
                Thread thread = threadClass.Start(ns, fileName, fileCount, this);
            }
        }
    }
}
