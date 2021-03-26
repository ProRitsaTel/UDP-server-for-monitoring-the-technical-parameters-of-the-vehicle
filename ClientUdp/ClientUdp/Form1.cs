using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientUdpNewEraForms
{
    public struct Fsg
    {

        public byte[] pref; // признак начала файла, при разметке проинициализировано "vibr"
        public float K1_skz_yel; // коэф-т для допуска СКЗ диапазон1 граница желтого выхода за норму
        public float K2_skz_yel; // коэф-т для допуска СКЗ диапазон2 граница желтого выхода за норму
        public float K1_pik_yel; // коэф-т для допуска Пик-фактора диапазон1 граница желтого выхода за норму
        public float K2_pik_yel; // коэф-т для допуска Пик-фактора диапазон2 граница желтого выхода за норму
        public float K1_skz_red; // коэф-т для допуска СКЗ диапазон1 граница красного выхода за норму
        public float K2_skz_red; // коэф-т для допуска СКЗ диапазон2 граница красного выхода за норму
        public float n_zub; // количество зубьев для вычисления F_ob_min
        public float Kd_rmk1; // коэф-т для нормализации значений вибродатчика 1 (перевод СКЗ в м/с**2)
        public float Kd_rmk2; // коэф-ты для нормализации значений вибродатчика 2 (перевод СКЗ в м/с**2)
        public float K_Moment_rmk1; // граница желтого выхода за норму вибродатчиков 1 и 2
        public float K_Moment_rmk2; // граница красного выхода за норму вибродатчиков 1 и 2
        public UInt16 graniza_1; // нижняя граница для определения диапазонов по об/мин
        public UInt16 graniza_2; // верхняя граница для определения диапазонов по об/мин
        public UInt16 moment_1; // нижняя граница для определения диапазонов по нагрузке
        public UInt16 moment_2; // верхняя граница для определения диапазонов по нагрузке
        public UInt16 kratnoct_FOB; //кратность записи ФОВ( 1..100 )
        public UInt32 period_recording; // периодичность записи в секундах (сейчас в F_sys2)
        public UInt16 regim_work; // режим работы ( пока не используется)

    }
    public partial class Form1 : Form
    {       
        static Socket listeningSocket;      
        int count = 0;

        #region заголовок массива
        ushort message = 3455;//первое число
        ushort command = 3;
        ushort numberOfCar = 2;
        ushort nenyhnayaInfa = 0;
        ushort typeFile = 5;
        ushort numberOfPartFile = 1;
        ushort sizeOfPartFile = 1024;
        ushort rezerv = 0;

        #endregion
        IPAddress ipAddress = IPAddress.Parse("192.168.100.4");
        int port = Convert.ToInt32(23002);

        public Form1()
        {
            InitializeComponent();
            listeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);//создание сокета
            
        }

       

        private void Listen()// функция для прослушивания входящих сообщений от сервера
        {
            try
            {
                IPEndPoint localIP = new IPEndPoint(ipAddress, port); // Прослушиваем по адресу
                while (true)
                {
                    StringBuilder builder = new StringBuilder();//для вывода
                    int bytes = 0;
                    byte[] data = new byte[1024];
                    byte[] data1 = new byte[1024];//размер массива байтов                   
                    EndPoint remoteIp = new IPEndPoint(IPAddress.Any, 0);
                    
                    do
                    {
                        bytes = listeningSocket.ReceiveFrom(data, ref remoteIp);//чтение данных, которые пришли с сервера
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));//запись данных из byte в строку
                    }   
                    while (listeningSocket.Available > 0);
                    command = BitConverter.ToUInt16(data, 2);
                    IPEndPoint remoteFullIp = remoteIp as IPEndPoint;
                    if((command = BitConverter.ToUInt16(data, 2)) == 2)
                    {
                        
                        Console.WriteLine("2");
                    }
                    if ((command = BitConverter.ToUInt16(data, 2)) == 4)
                    {
                        Console.WriteLine("4");
                    }
                    //printLine(remoteFullIp.Address.ToString() + " : " + remoteFullIp.Port + " - " + "Text : " + Encoding.Unicode.GetString(data, 0, bytes).ToString());
                    if ((command = BitConverter.ToUInt16(data, 2)) == 10)
                    {
                        byte[] xuita = new byte[84];
                        command = 11;
                        BitConverter.GetBytes(message).CopyTo(xuita, 0);
                        BitConverter.GetBytes(command).CopyTo(xuita, 2);
                        BitConverter.GetBytes(numberOfCar).CopyTo(xuita, 4);
                        BitConverter.GetBytes(nenyhnayaInfa).CopyTo(xuita, 6);
                        BitConverter.GetBytes(typeFile).CopyTo(xuita, 8);
                        BitConverter.GetBytes(numberOfPartFile).CopyTo(xuita, 10);
                        BitConverter.GetBytes(sizeOfPartFile).CopyTo(xuita, 12);
                        BitConverter.GetBytes(rezerv).CopyTo(xuita, 14);

                        using (var stream = new System.IO.FileStream("Z:\\F_sys1.bin", System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Read))
                        {
                            //Перемещаемся в файле на 100 байт от начала
                            stream.Seek(0, System.IO.SeekOrigin.Begin);
                            //Записываем буфер             
                            stream.Read(xuita, 16, xuita.Length - 16);
                        }
                        listeningSocket.SendTo(xuita, 0, xuita.Length, 0, remoteIp);
                    }
                    if ((command = BitConverter.ToUInt16(data, 2)) == 12)
                    {
                        byte[] sys2 = new byte[268];
                        command = 13;
                        BitConverter.GetBytes(message).CopyTo(sys2, 0);
                        BitConverter.GetBytes(command).CopyTo(sys2, 2);
                        BitConverter.GetBytes(numberOfCar).CopyTo(sys2, 4);
                        BitConverter.GetBytes(nenyhnayaInfa).CopyTo(sys2, 6);
                        BitConverter.GetBytes(typeFile).CopyTo(sys2, 8);
                        BitConverter.GetBytes(numberOfPartFile).CopyTo(sys2, 10);
                        BitConverter.GetBytes(sizeOfPartFile).CopyTo(sys2, 12);
                        BitConverter.GetBytes(rezerv).CopyTo(sys2, 14);

                        using (var stream = new System.IO.FileStream("Z:\\F_sys2.bin", System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Read))
                        {
                            //Перемещаемся в файле на 100 байт от начала
                            stream.Seek(0, System.IO.SeekOrigin.Begin);
                            //Записываем буфер
                            stream.Read(sys2, 16, sys2.Length - 16);
                        }
                        listeningSocket.SendTo(sys2, 0, sys2.Length, 0, remoteIp);
                    }
                    if ((command = BitConverter.ToUInt16(data, 2)) == 20)//!!!!!!
                    {
                        while (true)
                        {
                            byte[] M1M2 = new byte[1024];
                            command = 21;
                            if(numberOfPartFile==239)
                                sizeOfPartFile = Convert.ToUInt16(240032 - 238 * 1008);
                            BitConverter.GetBytes(message).CopyTo(M1M2, 0);
                            BitConverter.GetBytes(command).CopyTo(M1M2, 2);
                            BitConverter.GetBytes(numberOfCar).CopyTo(M1M2, 4);
                            BitConverter.GetBytes(nenyhnayaInfa).CopyTo(M1M2, 6);
                            BitConverter.GetBytes(typeFile).CopyTo(M1M2, 8);
                            BitConverter.GetBytes(numberOfPartFile).CopyTo(M1M2, 10);
                            BitConverter.GetBytes(sizeOfPartFile).CopyTo(M1M2, 12);
                            BitConverter.GetBytes(rezerv).CopyTo(M1M2, 14);

                            using (var stream = new System.IO.FileStream("Z:\\M1M2.bin", System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Read))
                            {
                                
                                    stream.Seek(1008 * (numberOfPartFile - 1), System.IO.SeekOrigin.Begin);
                                    //Записываем буфер
                                    stream.Read(M1M2, 16, M1M2.Length - 16);
                                
                            }
                           
                            listeningSocket.SendTo(M1M2, 0, M1M2.Length, 0, remoteIp);
                            if (numberOfPartFile == 239)
                            {
                                numberOfPartFile = 0;
                                break;
                            }
                            numberOfPartFile++;
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
       
        private void button2_Click(object sender, EventArgs e)//функция для отправления сообщений на сервер
        {
            try
            {
                byte[] data = new byte[16];
                BitConverter.GetBytes(message).CopyTo(data, 0);
                BitConverter.GetBytes(command).CopyTo(data, 4);
                BitConverter.GetBytes(numberOfCar).CopyTo(data, 2);
                BitConverter.GetBytes(nenyhnayaInfa).CopyTo(data, 6);
                BitConverter.GetBytes(typeFile).CopyTo(data, 8);
                BitConverter.GetBytes(numberOfPartFile).CopyTo(data, 10);
                BitConverter.GetBytes(sizeOfPartFile).CopyTo(data, 12);
                BitConverter.GetBytes(rezerv).CopyTo(data, 14);




                EndPoint remotePoint = new IPEndPoint(ipAddress, port);//определение удалённой точки               
                listeningSocket.SendTo(data, remotePoint);//отправка сообщений на сервер
                Task listeningTask = new Task(Listen); // Создание потока
                listeningTask.Start(); // Запуск потока
                MessageBox.Show("Сообщение отправлено!");
            }catch(Exception p)
            {
                MessageBox.Show("Связь с сервером отсутствует!");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Invoke(new Action(() => textBox1.Text = "Отправьте сообщение для установки связи с сервером."));
        }
       
       
      
    }
}
