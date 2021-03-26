using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeverUdp_5._0
{
    class ThreadServer
    {

        static Socket listeningSocket; // создание переменной для соккета 
        static IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0); //сетевая конечная точка (IPEndPoint Представляет конечную точку сети в виде IP-адреса и номера порта.)
        static EndPoint Remote = (EndPoint)(sender);//определяет сетевой адрес конечной точки         
        Utilities utilities = new Utilities();
        byte[] partOfMassiv = new byte[1024];
        byte[] withoutZagM1M2 = new byte[1008];
        byte[] withoutZagSys1 = new byte[68];
        byte[] withoutZagSys2 = new byte[252];
        byte[] withoutZagFdp = new byte[24];

        static byte[] data = new byte[1024];
        //--------------------------------------------------------

        int numberBytes = 0;
        int numberOfCommand = 0;
        bool firstCommand = false;
        bool secondCommand = false;
        bool thirdCommand = false;
        bool fourCommand = false;
        byte[] massiv = new byte[1024+16];
        byte[] massivOnlyZag = new byte[16];

        //------O1
        byte[] massivO1 = new byte[120036];
        int allO1 = 118;
        ushort nextO1 = 0;
        byte[] withoutZagO1 = new byte[1024];
        uint O1 = 0;
        //------T1
        byte[] massivT1 = new byte[120036];
        int allT1 = 118;
        ushort nextT1 = 0;
        byte[] withoutZagT1 = new byte[1024];
        uint T1 = 0;
        //------O2
        byte[] massivO2 = new byte[120036];
        int allO2 = 118;
        ushort nextO2 = 0;
        byte[] withoutZagO2 = new byte[1024];
        uint O2 = 0;
        //------T2
        byte[] massivT2 = new byte[120036];
        int allT2 = 118;
        ushort nextT2 = 0;
        byte[] withoutZagT2 = new byte[1024];
        uint T2 = 0;
        //------O31       
        byte[] withoutZagO31 = new byte[480];
        uint O31 = 0;
        //------T31       
        byte[] withoutZagT31 = new byte[480];
        uint T31 = 0;
        //------O32       
        byte[] withoutZagO32 = new byte[480];
        uint O32 = 0;
        //------T32   
        byte[] withoutZagT32 = new byte[480];
        uint T32 = 0;

        string path = "";
        string pathSys1 = "";
        string pathSys2 = "";
        string pathO1 = "";
        string pathT1 = "";
        string pathO2 = "";
        string pathT2 = "";
        string pathO31 = "";
        string pathT31 = "";
        string pathO32 = "";
        string pathT32 = "";

        ushort message = 3855;//переменная,которая обозначает код проекта в заголовке                
        ushort command = 0;//переменная,которая обозначает номер команды в заголовке
        ushort numberOfCar = 0;//переменная,которая обозначает номер машины в заголовке
        ushort nenyhnayaInfa = 0;//переменная,которая обозначает резерв в заголовке
       ushort typeFile = 0;
        ushort numberOfPartFile = 0;
       ushort sizeOfPartFile = 0;
        ushort reserv = 0;


        public void Server(int numberPort)// создание сервера 
        {
            try
            {
                listeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp); // Создание сокета   
                listeningSocket.Bind(new IPEndPoint(IPAddress.Parse("192.168.100.4"), Convert.ToInt32(23000 + numberPort))); // задаём порт и ip нашего сервера                
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
            }
        }

        void searchIP()
        {
            try
            {
                IPAddress ipa = null;
                ipa = findMyIPV4Address();
                string IP = "";
                if (ipa != null)
                {
                    IP = ipa.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void serverStart(int number)
        {
            try
            {
                searchIP();
                Server(number);// инициализация сервера
                         //textOutput.Invoke(new Action(() => textOutput.Text = "Server is running"));
                Task listeningTask = new Task(Listen);//создание потока для постоянного прослушивания входящих сообщений
                listeningTask.Start(); // Запуск потока

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void newListen()
        {
            while(true)
            {
                numberBytes = listeningSocket.ReceiveFrom(massiv, ref Remote);
                utilities.peremenye(ref message, ref numberOfCar, ref command, ref nenyhnayaInfa, ref typeFile, ref numberOfPartFile, ref sizeOfPartFile,ref  reserv, massiv);
                utilities.filePath(massiv, numberOfCar, ref path, ref pathSys1, ref pathSys2,ref pathO1,ref pathO2,ref pathT1,ref pathT2,ref pathO31, ref pathO32, ref pathT31, ref pathT32);
                utilities.initializationMyCountK1();
                utilities.initializationMyCountK2();
                if (message == 3855)
                {
                    if (firstCommand == false)
                    {
                        firstCommand = true;
                        command = 10;
                        utilities.massiv(message, numberOfCar, command, nenyhnayaInfa, typeFile, numberOfPartFile, sizeOfPartFile, reserv, ref massiv);
                        listeningSocket.SendTo(massiv, 0, 16, 0, Remote);
                        if (firstCommand == true)
                            continue;
                    }
                    if (command == 11)
                    {
                        for (int i = 16; i < 58 + 16; ++i)
                        {
                            withoutZagSys1[i - 16] = data[i];
                        }
                        utilities.Sys1_(withoutZagSys1, pathSys1);
                        command = 12;
                        utilities.massiv(message, numberOfCar, command, nenyhnayaInfa, typeFile, numberOfPartFile, sizeOfPartFile, reserv, ref massiv);
                        if (secondCommand == false)
                        {
                            listeningSocket.SendTo(massiv, 0, 16, 0, Remote);
                            secondCommand = true;
                            if (secondCommand == true)
                                continue;
                        }
                    }
                    if (command == 13)
                    {
                        for (int i = 16; i < 252 + 16; ++i)
                        {
                            withoutZagSys2[i - 16] = data[i];
                        }
                        utilities.Sys2_(withoutZagSys2, pathSys2);//заносим полученные данные в структуру sys2


                        utilities.readMyCountK1();
                        utilities.readMyCountK2();
                        if (utilities.myCountK1.O1 < utilities.f_sys2new.count_ALL[0].O1)
                        {
                            command = 20;
                            typeFile = 2;
                            numberOfPartFile = Convert.ToUInt16(utilities.myCountK1.O1);
                            sizeOfPartFile = 0;

                            utilities.massiv(message, numberOfCar, command, nenyhnayaInfa, typeFile, numberOfPartFile, sizeOfPartFile, reserv, ref massiv);
                            if (thirdCommand == false)
                            {
                                listeningSocket.SendTo(massiv, 0, 16, 0, Remote);
                                thirdCommand = true;
                                if (thirdCommand == true)
                                    continue;
                            }
                        }
                        if (utilities.myCountK1.T1 < utilities.f_sys2new.count_ALL[0].T1)
                        {
                            command = 20;
                            typeFile = 3;
                            numberOfPartFile = Convert.ToUInt16(utilities.myCountK1.T1);
                            sizeOfPartFile = 0;

                            utilities.massiv(message, numberOfCar, command, nenyhnayaInfa, typeFile, numberOfPartFile, sizeOfPartFile, reserv, ref massiv);
                            if (thirdCommand == false)
                            {
                                listeningSocket.SendTo(massiv, 0, 16, 0, Remote);
                                thirdCommand = true;
                                if (thirdCommand == true)
                                    continue;
                            }
                        }
                        if (utilities.myCountK1.O2 < utilities.f_sys2new.count_ALL[0].O2)
                        {
                            command = 20;
                            typeFile = 4;
                            numberOfPartFile = Convert.ToUInt16(utilities.myCountK1.O2);
                            sizeOfPartFile = 0;

                            utilities.massiv(message, numberOfCar, command, nenyhnayaInfa, typeFile, numberOfPartFile, sizeOfPartFile, reserv, ref massiv);
                            if (thirdCommand == false)
                            {
                                listeningSocket.SendTo(massiv, 0, 16, 0, Remote);
                                thirdCommand = true;
                                if (thirdCommand == true)
                                    continue;
                            }
                        }
                        if (utilities.myCountK1.T2 < utilities.f_sys2new.count_ALL[0].T2)
                        {
                            command = 20;
                            typeFile = 5;
                            numberOfPartFile = Convert.ToUInt16(utilities.myCountK1.T2);
                            sizeOfPartFile = 0;

                            utilities.massiv(message, numberOfCar, command, nenyhnayaInfa, typeFile, numberOfPartFile, sizeOfPartFile, reserv, ref massiv);
                            if (thirdCommand == false)
                            {
                                listeningSocket.SendTo(massiv, 0, 16, 0, Remote);
                                thirdCommand = true;
                                if (thirdCommand == true)
                                    continue;
                            }
                        }
                    }
                    if (command == 21)
                    {
                        while (true)
                        {
                            utilities.peremenye(ref message, ref numberOfCar, ref command, ref nenyhnayaInfa, ref typeFile, ref numberOfPartFile, ref sizeOfPartFile, ref reserv, massiv);
                            if (typeFile == 2)
                            {
                                for (int i = 16; i < 1024 + 16; ++i)
                                {
                                    withoutZagO1[i - 16] = massiv[i];
                                }
                                Array.Copy(withoutZagO1, 0, massivO1, 1024 * numberOfPartFile, withoutZagO1.Length);
                                nextO1++;
                                utilities.massiv(message, numberOfCar, command, nenyhnayaInfa, typeFile, numberOfPartFile, nextO1, reserv, ref massiv);
                                listeningSocket.SendTo(massiv, 0, 16, 0, Remote);
                                if (nextO1 == allO1)
                                {
                                    utilities.M1M2_(massivO1, pathO1);
                                    O1++;
                                    utilities.upgradeMyCountK1(ref O1, ref utilities.myCountK1.O2, ref utilities.myCountK1.T1, ref utilities.myCountK1.T2, ref utilities.myCountK1.O31, ref utilities.myCountK1.O32, ref utilities.myCountK1.T31, ref utilities.myCountK1.T32);
                                    break;
                                }
                                do
                                {
                                    listeningSocket.ReceiveFrom(massiv, ref Remote);//получаем наши данные, записываем их в data,а в bytes записываем их количество
                                    utilities.filePath(massiv, numberOfCar, ref path, ref pathSys1, ref pathSys2, ref pathO1, ref pathO2, ref pathT1, ref pathT2, ref pathO31, ref pathO32, ref pathT31, ref pathT32);
                                } while (listeningSocket.Available > 0);
                            }
                            if (typeFile == 3)
                            {
                                for (int i = 16; i < 1024 + 16; ++i)
                                {
                                    withoutZagT1[i - 16] = massiv[i];
                                }
                                Array.Copy(withoutZagT1, 0, massivT1, 1024 * numberOfPartFile, withoutZagT1.Length);
                                nextT1++;
                                utilities.massiv(message, numberOfCar, command, nenyhnayaInfa, typeFile, numberOfPartFile, nextT1, reserv, ref massiv);
                                listeningSocket.SendTo(massiv, 0, 16, 0, Remote);
                                if (nextT1 == allT1)
                                {
                                    utilities.M1M2_(massivT1, pathT1);
                                    T1++;
                                    utilities.upgradeMyCountK1(ref utilities.myCountK1.O1, ref utilities.myCountK1.O2, ref T1, ref utilities.myCountK1.T2, ref utilities.myCountK1.O31, ref utilities.myCountK1.O32, ref utilities.myCountK1.T31, ref utilities.myCountK1.T32);
                                    break;
                                }
                                do
                                {
                                    listeningSocket.ReceiveFrom(massiv, ref Remote);//получаем наши данные, записываем их в data,а в bytes записываем их количество
                                    utilities.filePath(massiv, numberOfCar, ref path, ref pathSys1, ref pathSys2, ref pathO1, ref pathO2, ref pathT1, ref pathT2, ref pathO31, ref pathO32, ref pathT31, ref pathT32);
                                } while (listeningSocket.Available > 0);
                            }
                            if (typeFile == 4)
                            {
                                for (int i = 16; i < 1024 + 16; ++i)
                                {
                                    withoutZagO2[i - 16] = massiv[i];
                                }
                                Array.Copy(withoutZagO2, 0, massivO2, 1024 * numberOfPartFile, withoutZagO2.Length);
                                nextO2++;
                                utilities.massiv(message, numberOfCar, command, nenyhnayaInfa, typeFile, numberOfPartFile, nextO2, reserv, ref massiv);
                                listeningSocket.SendTo(massiv, 0, 16, 0, Remote);
                                if (nextO2 == allO2)
                                {
                                    utilities.M1M2_(massivO2, pathO2);
                                    O2++;
                                    utilities.upgradeMyCountK1(ref utilities.myCountK1.O1, ref O2, ref utilities.myCountK1.T1, ref utilities.myCountK1.T2, ref utilities.myCountK1.O31, ref utilities.myCountK1.O32, ref utilities.myCountK1.T31, ref utilities.myCountK1.T32);
                                    break;
                                }
                                do
                                {
                                    listeningSocket.ReceiveFrom(massiv, ref Remote);//получаем наши данные, записываем их в data,а в bytes записываем их количество
                                    utilities.filePath(massiv, numberOfCar, ref path, ref pathSys1, ref pathSys2, ref pathO1, ref pathO2, ref pathT1, ref pathT2, ref pathO31, ref pathO32, ref pathT31, ref pathT32);
                                } while (listeningSocket.Available > 0);
                            }
                            if (typeFile == 5)
                            {
                                for (int i = 16; i < 1024 + 16; ++i)
                                {
                                    withoutZagT2[i - 16] = massiv[i];
                                }
                                Array.Copy(withoutZagT2, 0, massivT2, 1024 * numberOfPartFile, withoutZagT2.Length);
                                nextT2++;
                                utilities.massiv(message, numberOfCar, command, nenyhnayaInfa, typeFile, numberOfPartFile, nextT2, reserv, ref massiv);
                                listeningSocket.SendTo(massiv, 0, 16, 0, Remote);
                                if (nextT2 == allT2)
                                {
                                    utilities.M1M2_(massivT2, pathT2);
                                    T2++;
                                    utilities.upgradeMyCountK1(ref utilities.myCountK1.O1, ref utilities.myCountK1.O2, ref utilities.myCountK1.T1, ref T2, ref utilities.myCountK1.O31, ref utilities.myCountK1.O32, ref utilities.myCountK1.T31, ref utilities.myCountK1.T32);
                                    break;
                                }
                                do
                                {
                                    listeningSocket.ReceiveFrom(massiv, ref Remote);//получаем наши данные, записываем их в data,а в bytes записываем их количество
                                    utilities.filePath(massiv, numberOfCar, ref path, ref pathSys1, ref pathSys2, ref pathO1, ref pathO2, ref pathT1, ref pathT2, ref pathO31, ref pathO32, ref pathT31, ref pathT32);
                                } while (listeningSocket.Available > 0);
                            }
                        }
                        if (utilities.myCountK1.O31 < utilities.f_sys2new.count_ALL[0].O31)
                        {
                            command = 22;
                            typeFile = 6;
                            numberOfPartFile = Convert.ToUInt16(utilities.myCountK1.O31);
                            sizeOfPartFile = 0;

                            utilities.massiv(message, numberOfCar, command, nenyhnayaInfa, typeFile, numberOfPartFile, sizeOfPartFile, reserv, ref massiv);
                            if (fourCommand == false)
                            {
                                listeningSocket.SendTo(massiv, 0, 16, 0, Remote);
                                fourCommand = true;
                                if (fourCommand == true)
                                    continue;
                            }
                        }
                        if (utilities.myCountK1.T31 < utilities.f_sys2new.count_ALL[0].T31)
                        {
                            command = 22;
                            typeFile = 7;
                            numberOfPartFile = Convert.ToUInt16(utilities.myCountK1.T31);
                            sizeOfPartFile = 0;

                            utilities.massiv(message, numberOfCar, command, nenyhnayaInfa, typeFile, numberOfPartFile, sizeOfPartFile, reserv, ref massiv);
                            if (fourCommand == false)
                            {
                                listeningSocket.SendTo(massiv, 0, 16, 0, Remote);
                                fourCommand = true;
                                if (fourCommand == true)
                                    continue;
                            }
                        }
                        if (utilities.myCountK1.O32 < utilities.f_sys2new.count_ALL[0].O32)
                        {
                            command = 22;
                            typeFile = 8;
                            numberOfPartFile = Convert.ToUInt16(utilities.myCountK1.O32);
                            sizeOfPartFile = 0;

                            utilities.massiv(message, numberOfCar, command, nenyhnayaInfa, typeFile, numberOfPartFile, sizeOfPartFile, reserv, ref massiv);
                            if (fourCommand == false)
                            {
                                listeningSocket.SendTo(massiv, 0, 16, 0, Remote);
                                fourCommand = true;
                                if (fourCommand == true)
                                    continue;
                            }
                        }
                        if (utilities.myCountK1.T32 < utilities.f_sys2new.count_ALL[0].T32)
                        {
                            command = 22;
                            typeFile = 9;
                            numberOfPartFile = Convert.ToUInt16(utilities.myCountK1.T32);
                            sizeOfPartFile = 0;

                            utilities.massiv(message, numberOfCar, command, nenyhnayaInfa, typeFile, numberOfPartFile, sizeOfPartFile, reserv, ref massiv);
                            if (fourCommand == false)
                            {
                                listeningSocket.SendTo(massiv, 0, 16, 0, Remote);
                                fourCommand = true;
                                if (fourCommand == true)
                                    continue;
                            }
                        }
                    }
                    if(command == 23)
                    {
                        if (typeFile == 6)
                        {
                            for (int i = 16; i < 480 + 16; ++i)
                            {
                                withoutZagO31[i - 16] = massiv[i];
                            }
                            utilities.Fdp_(withoutZagFdp, pathO31);
                            O31++;
                            utilities.upgradeMyCountK1(ref utilities.myCountK1.O1, ref utilities.myCountK1.O2, ref utilities.myCountK1.T1, ref utilities.myCountK1.T2, ref O31, ref utilities.myCountK1.O32, ref utilities.myCountK1.T31, ref utilities.myCountK1.T32);
                            firstCommand = false;
                            secondCommand = false;
                            thirdCommand = false;
                            fourCommand = false;
                            nextO1 = 0;
                            nextO2 = 0;
                            nextT2 = 0;
                            nextT1 = 0;
                        }
                        if (typeFile == 7)
                        {
                            for (int i = 16; i < 480 + 16; ++i)
                            {
                                withoutZagT31[i - 16] = massiv[i];
                            }
                            utilities.Fdp_(withoutZagFdp, pathT31);
                            T31++;
                            utilities.upgradeMyCountK1(ref utilities.myCountK1.O1, ref utilities.myCountK1.O2, ref utilities.myCountK1.T1, ref utilities.myCountK1.T2, ref utilities.myCountK1.O31, ref utilities.myCountK1.O32, ref T31, ref utilities.myCountK1.T32);
                            firstCommand = false;
                            secondCommand = false;
                            thirdCommand = false;
                            fourCommand = false;
                            nextO1 = 0;
                            nextO2 = 0;
                            nextT2 = 0;
                            nextT1 = 0;
                        }
                        if (typeFile == 8)
                        {
                            for (int i = 16; i < 480 + 16; ++i)
                            {
                                withoutZagO32[i - 16] = massiv[i];
                            }
                            utilities.Fdp_(withoutZagFdp, pathO32);
                            O32++;
                            utilities.upgradeMyCountK1(ref utilities.myCountK1.O1, ref utilities.myCountK1.O2, ref utilities.myCountK1.T1, ref utilities.myCountK1.T2, ref utilities.myCountK1.O31, ref O32, ref utilities.myCountK1.T31, ref utilities.myCountK1.T32);
                            firstCommand = false;
                            secondCommand = false;
                            thirdCommand = false;
                            fourCommand = false;
                            nextO1 = 0;
                            nextO2 = 0;
                            nextT2 = 0;
                            nextT1 = 0;
                        }
                        if (typeFile == 9)
                        {
                            for (int i = 16; i < 480 + 16; ++i)
                            {
                                withoutZagT32[i - 16] = massiv[i];
                            }                          
                            utilities.Fdp_(withoutZagFdp, pathT32);
                            T32++;
                            utilities.upgradeMyCountK1(ref utilities.myCountK1.O1, ref utilities.myCountK1.O2, ref utilities.myCountK1.T1, ref utilities.myCountK1.T2, ref utilities.myCountK1.O31, ref utilities.myCountK1.O32, ref utilities.myCountK1.T31, ref T32);
                            firstCommand = false;
                            secondCommand = false;
                            thirdCommand = false;
                            fourCommand = false;
                            nextO1 = 0;
                            nextO2 = 0;
                            nextT2 = 0;
                            nextT1 = 0;
                        }
                    }

                }
            }
        }



        


        public void Listen() // функция прослушивания входных данных приходящих на сервер
        {
            try
            {
                short message = BitConverter.ToInt16(data, 0);//переменная,которая обозначает код проекта в заголовке                
                short command = BitConverter.ToInt16(data, 2);//переменная,которая обозначает номер команды в заголовке
                short numberOfCar = BitConverter.ToInt16(data, 4);//переменная,которая обозначает номер машины в заголовке
                short nenyhnayaInfa = BitConverter.ToInt16(data, 6);//переменная,которая обозначает резерв в заголовке
                short typeFile = BitConverter.ToInt16(data, 8);
                short numberOfPartFile = BitConverter.ToInt16(data, 10);
                short sizeOfPartFile = BitConverter.ToInt16(data, 12);
                short reserv = BitConverter.ToInt16(data, 14);
                //listeningSocket.LingerState = new LingerOption(true, 10);
                while (true)
                {
                    int bytes = 0;
                    byte[] data = new byte[1024];
                    byte[] data1 = new byte[1024];//размер массива байтов                      
                    EndPoint remoteIp = new IPEndPoint(IPAddress.Any, 0);
                    int number = 0;
                    listeningSocket.ReceiveTimeout = 1000000;


                    while (true)
                    {
                        try
                        {   if (number < 2)
                            {
                                bytes = listeningSocket.ReceiveFrom(data, ref Remote);//получаем наши данные, записываем их в data,а в bytes записываем их количество
                                break;
                            }
                            else
                            {
                                listeningSocket.Close();
                                break;
                            }
                        }
                        catch (Exception ex)
                        {
                            number++;
                            byte[] massiv = new byte[16];
                            if (number == 1)
                            {
                                BitConverter.GetBytes(message).CopyTo(massiv, 0);
                                BitConverter.GetBytes(command).CopyTo(massiv, 4);
                                BitConverter.GetBytes(numberOfCar).CopyTo(massiv, 2);
                                BitConverter.GetBytes(nenyhnayaInfa).CopyTo(massiv, 6);
                                BitConverter.GetBytes(typeFile).CopyTo(massiv, 8);
                                BitConverter.GetBytes(numberOfPartFile).CopyTo(massiv, 10);
                                BitConverter.GetBytes(sizeOfPartFile).CopyTo(massiv, 12);
                                BitConverter.GetBytes(reserv).CopyTo(massiv, 14);
                                listeningSocket.SendTo(massiv, 0, 16, 0, Remote);
                            }
                        }
                    }
                    if (number == 2)
                        break;



                    number = 0;
                    IPEndPoint remoteFullIp = Remote as IPEndPoint;
                    byte[] qerty = new byte[bytes];


                    
                     message = BitConverter.ToInt16(data, 0);//переменная,которая обозначает код проекта в заголовке                
                     command = BitConverter.ToInt16(data, 4);//переменная,которая обозначает номер команды в заголовке
                     numberOfCar = BitConverter.ToInt16(data, 2);//переменная,которая обозначает номер машины в заголовке
                     nenyhnayaInfa = BitConverter.ToInt16(data, 6);//переменная,которая обозначает резерв в заголовке
                     typeFile = BitConverter.ToInt16(data, 8);
                     numberOfPartFile = BitConverter.ToInt16(data, 10);
                     sizeOfPartFile = BitConverter.ToInt16(data, 12);
                     reserv = BitConverter.ToInt16(data, 14);

                    string path = @"Z:\Belaz\";//путь для сохранения файлов
                    string pathSys1 = @"Z:\Belaz\" + numberOfCar + "\\" + "sys1.bin";
                    string pathSys2 = @"Z:\Belaz\" + numberOfCar + "\\" + "sys2.bin";
                    string pathM1M2 = @"Z:\Belaz\" + numberOfCar + "\\" + "K1" + "\\" + "M1M2.bin";
                    string pathFDP = @"Z:\Belaz\" + numberOfCar + "\\" + "K1" + "\\" + "FDP.bin";



                    if (BitConverter.ToInt32(data, 8) == (6) || BitConverter.ToInt32(data, 8) == (7) || BitConverter.ToInt32(data, 8) == (8) || BitConverter.ToInt32(data, 8) == (9))
                    {
                        pathFDP = @"Z:\Belaz\" + numberOfCar + "\\" + "K0" + "\\" + "FDP.bin";
                    }
                    else
                        pathFDP = @"Z:\Belaz\" + numberOfCar + "\\" + "K1" + "\\" + "FDP.bin";



                    if (BitConverter.ToInt32(data, 8) == (2) || BitConverter.ToInt32(data, 8) == (3) || BitConverter.ToInt32(data, 8) == (4) || BitConverter.ToInt32(data, 8) == (5))
                    {
                        pathM1M2 = @"Z:\Belaz\" + numberOfCar + "\\" + "K0" + "\\" + "M1M2.bin";
                    }
                    else
                        pathM1M2 = @"Z:\Belaz\" + numberOfCar + "\\" + "K1" + "\\" + "M1M2.bin";



                    using (StreamWriter sw = new StreamWriter(Path.GetDirectoryName(path) + "\\" + String.Format("{0}.{1}", "log", "txt"), true))//запись в файл
                    {
                        sw.WriteLine(String.Format("{0} {1}", DateTime.Now.ToString() + ":", message));
                    }

                    if (command == 3)//если пришла команда 3 от клиента
                    {
                        byte[] massiv = new byte[16];
                        command = 4;
                        BitConverter.GetBytes(message).CopyTo(massiv, 0);
                        BitConverter.GetBytes(command).CopyTo(massiv, 4);
                        BitConverter.GetBytes(numberOfCar).CopyTo(massiv, 2);
                        BitConverter.GetBytes(nenyhnayaInfa).CopyTo(massiv, 6);
                        BitConverter.GetBytes(typeFile).CopyTo(massiv, 8);
                        BitConverter.GetBytes(numberOfPartFile).CopyTo(massiv, 10);
                        BitConverter.GetBytes(sizeOfPartFile).CopyTo(massiv, 12);
                        BitConverter.GetBytes(reserv).CopyTo(massiv, 14);

                        listeningSocket.SendTo(massiv, 0, 8, 0, Remote);//отправляем OK

                        Thread.Sleep(2000);

                        command = 10;
                        BitConverter.GetBytes(message).CopyTo(massiv, 0);
                        BitConverter.GetBytes(command).CopyTo(massiv, 4);
                        BitConverter.GetBytes(numberOfCar).CopyTo(massiv, 2);
                        BitConverter.GetBytes(nenyhnayaInfa).CopyTo(massiv, 6);
                        BitConverter.GetBytes(typeFile).CopyTo(massiv, 8);
                        BitConverter.GetBytes(numberOfPartFile).CopyTo(massiv, 10);
                        BitConverter.GetBytes(sizeOfPartFile).CopyTo(massiv, 12);
                        BitConverter.GetBytes(reserv).CopyTo(massiv, 14);
                        listeningSocket.SendTo(massiv, 0, 16, 0, Remote);//отправляем запрос на получение sys1
                    }
                    if (command == 11)//команда 11 - получение данных структуры sys1
                    {
                        byte[] massiv = new byte[16];
                        for (int i = 16; i < 58+16 ; ++i)
                        {
                            withoutZagSys1[i-16] = data[i];
                        }
                        utilities.Sys1_(withoutZagSys1, pathSys1);//заносим полученные данные в структуру sys1

                        command = 12;
                        BitConverter.GetBytes(message).CopyTo(massiv, 0);
                        BitConverter.GetBytes(command).CopyTo(massiv, 4);
                        BitConverter.GetBytes(numberOfCar).CopyTo(massiv, 2);
                        BitConverter.GetBytes(nenyhnayaInfa).CopyTo(massiv, 6);
                        BitConverter.GetBytes(typeFile).CopyTo(massiv, 8);
                        BitConverter.GetBytes(numberOfPartFile).CopyTo(massiv, 10);
                        BitConverter.GetBytes(sizeOfPartFile).CopyTo(massiv, 12);
                        BitConverter.GetBytes(reserv).CopyTo(massiv, 14);
                        listeningSocket.SendTo(massiv, 0, 8, 0, Remote);//отправляем запрос на получение sys2
                    }
                    if (command == 13)//команда 13 - получение данных структуры sys2
                    {
                        byte[] massiv = new byte[16];
                        for (int i = 16; i < 252+16 ; ++i)
                        {
                            withoutZagSys2[i - 16] = data[i];
                        }
                        utilities.Sys2_(withoutZagSys2, pathSys2);//заносим полученные данные в структуру sys2

                        command = 20;
                        BitConverter.GetBytes(message).CopyTo(massiv, 0);
                        BitConverter.GetBytes(command).CopyTo(massiv, 4);
                        BitConverter.GetBytes(numberOfCar).CopyTo(massiv, 2);
                        BitConverter.GetBytes(nenyhnayaInfa).CopyTo(massiv, 6);
                        BitConverter.GetBytes(typeFile).CopyTo(massiv, 8);
                        BitConverter.GetBytes(numberOfPartFile).CopyTo(massiv, 10);
                        BitConverter.GetBytes(sizeOfPartFile).CopyTo(massiv, 12);
                        BitConverter.GetBytes(reserv).CopyTo(massiv, 14);
                        listeningSocket.SendTo(massiv, 0, 16, 0, Remote);
                    }
                    if(command == 21)
                    {
                        byte[] M1M2_ = new byte[240032];
                        byte[] massiv = new byte[16];

                        numberOfPartFile = BitConverter.ToInt16(data, 10);
                        

                        while (true)
                        {

                            for (int i = 16; i < 1008+16; ++i)
                            {
                                withoutZagM1M2[i - 16] = data[i];
                            }
                           

                            Array.Copy(withoutZagM1M2, 0, M1M2_, 1008 * (numberOfPartFile - 1), sizeOfPartFile - 16);

                            if (numberOfPartFile == 238)
                            {
                                utilities.M1M2_(M1M2_, pathM1M2);
                                break;
                            }
                            do
                            {
                                bytes = listeningSocket.ReceiveFrom(data, ref Remote);//получаем наши данные, записываем их в data,а в bytes записываем их количество
  
                            } while (listeningSocket.Available > 0);

                            numberOfPartFile = BitConverter.ToInt16(data, 10);
                            sizeOfPartFile = BitConverter.ToInt16(data, 12);
                                                                             
                        }


                        command = 22;
                        BitConverter.GetBytes(message).CopyTo(massiv, 0);
                        BitConverter.GetBytes(command).CopyTo(massiv, 4);
                        BitConverter.GetBytes(numberOfCar).CopyTo(massiv, 2);
                        BitConverter.GetBytes(nenyhnayaInfa).CopyTo(massiv, 6);
                        BitConverter.GetBytes(typeFile).CopyTo(massiv, 8);
                        BitConverter.GetBytes(numberOfPartFile).CopyTo(massiv, 10);
                        BitConverter.GetBytes(sizeOfPartFile).CopyTo(massiv, 12);
                        BitConverter.GetBytes(reserv).CopyTo(massiv, 14);
                        listeningSocket.SendTo(massiv, 0, 16, 0, Remote);

                    }
                    if(command ==23)
                    {
                        for (int i = 8; i < 24 + 8; ++i)
                        {
                            withoutZagFdp[i - 8] = data[i];
                        }
                        utilities.Fdp_(withoutZagFdp, pathFDP);
                    }


                    qerty = new byte[bytes];
                    data = new byte[1024];
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }


        }
  
        IPAddress findMyIPV4Address()// функция для поиска своего ipv4 адреса
        {
            string strThisHostName = string.Empty;
            IPHostEntry thisHostDNSEntry = null;
            IPAddress[] allIPsOfThisHost = null;
            IPAddress ipv4Ret = null;

            try
            {
                strThisHostName = System.Net.Dns.GetHostName();
                thisHostDNSEntry = System.Net.Dns.GetHostEntry(strThisHostName);
                allIPsOfThisHost = thisHostDNSEntry.AddressList;

                for (int idx = allIPsOfThisHost.Length - 1; idx >= 0; idx--)
                {
                    if (allIPsOfThisHost[idx].AddressFamily == AddressFamily.InterNetwork)
                    {
                        ipv4Ret = allIPsOfThisHost[idx];
                        break;
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }

            return ipv4Ret;
        }
        

        
        


    }
    
}

