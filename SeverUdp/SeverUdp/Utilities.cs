using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace SeverUdp_5._0
{
    public class Utilities
    {

        [Serializable]
        

        public struct Sys1
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


        public struct COUNT_F
        {

            public byte O1; // счетчик образцового файла массива диапазона1
            public byte O2; // счетчик образцового файла массива диапазона2
            public UInt16 T1; // счетчик текущего файла массива диапазона1
            public UInt16 T2; // счетчик текущего файла массива диапазона2
            public byte O31; // счетчик элементов образцового файла ФДП диапазона1
            public byte O32; // счетчик элементов образцового файла ФДП диапазона2
            public UInt16 T31; // счетчик элементов текущего файла ФДП диапазона1
            public UInt16 T32; // счетчик элементов текущего файла ФДП диапазона2

        }//12
        public struct sys2
        {

            public COUNT_F count; // счетчики файлов 12 byte

            public float Sr_skz_d1, Sr_skz_d2, Sr_pik_d1, Sr_pik_d2; // средние значения ФДП

            public float pred_skz_d1, pred_skz_d2, pred_pik_d1, pred_pik_d2; // предыдущие значения ФДП

            public float Sr_M_d1, Sr_M_d2; // средние значения ФДП --мощность

            public UInt32 Cnt_Bad_red, Cnt_Bad_yel; // счетчики "хорошо"- "плохо" для индикации лампочкой

            public float Sum_skz_d1, Sum_pik_d1, Sum_skz_d2, Sum_pik_d2; // суммы для вычисления средних скз, пик

            public float Prozent_yel, Prozent_red; // процент уровня состояния желтый, красный

            public UInt16 cnt_d1, cnt_d2; // счетчики для вычисления средних

            public UInt16 defect_file; // кол-во бракованных файлов

        }//85


        public struct COUNT_F_ALL // структура счетчиков всех файлов для сервера, используется в f_Sys2

        {
            public UInt32 O1; // счетчик образцового файла массива диапазона1

            public UInt32 O2; // счетчик образцового файла массива диапазона2

            public UInt32 T1; // счетчик текущего файла массива диапазона1

            public UInt32 T2; // счетчик текущего файла массива диапазона2

            public UInt32 O31; // счетчик элементов образцового файла ФДП диапазона1

            public UInt32 O32; // счетчик элементов образцового файла ФДП диапазона2

            public UInt32 T31; // счетчик элементов текущего файла ФДП диапазона1

            public UInt32 T32; // счетчик элементов текущего файла ФДП диапазона2
        }//32
        public struct F_sys2
        {
            public UInt32 cnt; // абсолютный счетчик записи, изменяется функцией записи
                               // при чтении f_Sys2 если cnt=0 то это первое чтение после разметки 
            public sys2[] rmk;
            public UInt16 nomer_rmk; // номер РМК, с которым работаем // 0 -- rmk1, 1 -- rmk2
            public UInt16 regim_work_tek; // периодичность записи в секундах 
            public COUNT_F_ALL[] count_ALL;
        }



        public struct M1M2
        {
           public byte year, month, day; // календарное время
            public byte hour, min, sec;
            public UInt16 reserv;
            public float skz, pik, F_ob_min, moment; // значения*/
            public UInt16[] res; //зарезервированные значения
                                 // размер заголовка =32
            public UInt16[] m1; //сам рабочий файл 
        }
        public struct FDP
        {
            public byte year, month, day; // календарное время
            public byte hour, min, sec;
            public UInt16 reserv;
            public float skz, pik, F_ob_min, moment; // значения*/
        }
        
        public void My_count_write_first()
        {
            

            byte[] byteArray = new byte[32];


            
                /*my_Count.O1 = 0;
                my_Count.O2 = 0;
                my_Count.T1 = 0;
                my_Count.T2 = 0;
                my_Count.O31 = 0;
                my_Count.O32 = 0;
                my_Count.T31 = 0;
                my_Count.T32 = 0;
            
                BitConverter.GetBytes(my_Count.O1).CopyTo(byteArray, 0);
                BitConverter.GetBytes(my_Count.O2).CopyTo(byteArray, 4);
                BitConverter.GetBytes(my_Count.T1).CopyTo(byteArray, 8);
                BitConverter.GetBytes(my_Count.T2).CopyTo(byteArray, 12);
                BitConverter.GetBytes(my_Count.O31).CopyTo(byteArray, 16);
                BitConverter.GetBytes(my_Count.O32).CopyTo(byteArray, 20);
                BitConverter.GetBytes(my_Count.T31).CopyTo(byteArray, 24);
                BitConverter.GetBytes(my_Count.T32).CopyTo(byteArray, 28);*/
            
            using (var stream = new System.IO.FileStream("Z:\\Belaz\\Mycount.txt", System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write))//запись в файл
            {
                //Перемещаемся в файле на 100 байт от начала
                stream.Seek(0, System.IO.SeekOrigin.Begin);
                //Записываем буфер
                stream.Write(byteArray, 0, byteArray.Length);
            }

        }
        public void My_count_write()
        {
           
            /*BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream("people.dat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, my_Count);

                Console.WriteLine("Объект сериализован");
            }*/
        }
        public void My_count_read()
        {
            
            /*byte[] byteArray = new byte[32];
            using (var stream = new System.IO.FileStream("Z:\\Belaz\\Mycount.txt", System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write))//запись в файл
            {
                //Перемещаемся в файле на 100 байт от начала
                stream.Seek(0, System.IO.SeekOrigin.Begin);
                //Записываем буфер
                stream.Read(byteArray, 0, byteArray.Length);
            }

            my_Count.O1 = BitConverter.ToUInt32(byteArray, 0);
            my_Count.O2 = BitConverter.ToUInt32(byteArray, 4);
            my_Count.T1 = BitConverter.ToUInt32(byteArray, 8);
            my_Count.T2 = BitConverter.ToUInt32(byteArray, 12);
            my_Count.O31 = BitConverter.ToUInt32(byteArray, 16);
            my_Count.O32 = BitConverter.ToUInt32(byteArray, 20);
            my_Count.T31 = BitConverter.ToUInt32(byteArray, 24);
            my_Count.T32 = BitConverter.ToUInt32(byteArray, 28);*/

        }
        public void Fdp_(byte[] byteArray, string path)
        {
            FDP fdp;
            fdp.year = byteArray[0];
            fdp.month = byteArray[1];
            fdp.day = byteArray[2];
            fdp.hour = byteArray[3];
            fdp.min = byteArray[4];
            fdp.sec = byteArray[5];

            fdp.reserv = BitConverter.ToUInt16(byteArray, 6);
            fdp.skz = BitConverter.ToSingle(byteArray, 8);
            fdp.pik = BitConverter.ToSingle(byteArray, 12);
            fdp.F_ob_min = BitConverter.ToSingle(byteArray, 16);
            fdp.moment = BitConverter.ToSingle(byteArray, 20);


            BitConverter.GetBytes(fdp.year).CopyTo(byteArray, 0);
            BitConverter.GetBytes(fdp.month).CopyTo(byteArray, 1);
            BitConverter.GetBytes(fdp.day).CopyTo(byteArray, 2);
            BitConverter.GetBytes(fdp.hour).CopyTo(byteArray, 3);
            BitConverter.GetBytes(fdp.min).CopyTo(byteArray, 4);
            BitConverter.GetBytes(fdp.sec).CopyTo(byteArray, 5);
            BitConverter.GetBytes(fdp.reserv).CopyTo(byteArray, 6);
            BitConverter.GetBytes(fdp.skz).CopyTo(byteArray, 8);
            BitConverter.GetBytes(fdp.pik).CopyTo(byteArray, 12);
            BitConverter.GetBytes(fdp.F_ob_min).CopyTo(byteArray, 16);
            BitConverter.GetBytes(fdp.moment).CopyTo(byteArray, 20);

            using (var stream = new System.IO.FileStream(path, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write))//запись в файл
                                                                                                                            //FileMode.OpenOrCreate- ОС открывает файл,если он существует,                                                                                                                            //в противном случае создаёт файл
            {
                //Записываем буфер
                stream.Write(byteArray, 0, byteArray.Length);
            }


        }
        public void M1M2_(byte[] byteArray, string path)
        {
            M1M2 m1m2;
             m1m2.res = new UInt16[4];
             m1m2.res = new UInt16[4];
             m1m2.m1 = new UInt16[120000];

             m1m2.year = byteArray[14-14];
             m1m2.month = byteArray[15-14];
             m1m2.day = byteArray[16-14];
             m1m2.hour = byteArray[17-14];
             m1m2.min = byteArray[18-14];
             m1m2.sec = byteArray[19-14];
             m1m2.reserv = BitConverter.ToUInt16(byteArray, 20-14);
             m1m2.skz = BitConverter.ToSingle(byteArray, 22-14);
             m1m2.pik = BitConverter.ToSingle(byteArray, 26-14);
             m1m2.F_ob_min = BitConverter.ToSingle(byteArray, 30-14);
             m1m2.moment = BitConverter.ToSingle(byteArray, 34-14);

            for (int i = 0; i < 4; i++)
            {
                m1m2.res[i] = BitConverter.ToUInt16(byteArray, 38-14  + (i * 2));
            }

            for (int i = 0; i < 120000; i++)
            {
                m1m2.m1[i] = BitConverter.ToUInt16(byteArray, 46-14 + (i * 2));
            }


            BitConverter.GetBytes(m1m2.year).CopyTo(byteArray, 0);
             BitConverter.GetBytes(m1m2.month).CopyTo(byteArray, 1);
             BitConverter.GetBytes(m1m2.day).CopyTo(byteArray, 2);
             BitConverter.GetBytes(m1m2.hour).CopyTo(byteArray, 3);
             BitConverter.GetBytes(m1m2.min).CopyTo(byteArray, 4);
             BitConverter.GetBytes(m1m2.sec).CopyTo(byteArray, 5);
             BitConverter.GetBytes(m1m2.reserv).CopyTo(byteArray, 6);
             BitConverter.GetBytes(m1m2.skz).CopyTo(byteArray, 8);
             BitConverter.GetBytes(m1m2.pik).CopyTo(byteArray, 12);
             BitConverter.GetBytes(m1m2.F_ob_min).CopyTo(byteArray, 16);
             BitConverter.GetBytes(m1m2.moment).CopyTo(byteArray, 20);         

            for(int i = 0; i < 4; i++)
            {
                BitConverter.GetBytes(m1m2.res[i]).CopyTo(byteArray, 24 + (i * 2));
            }

            for (int i = 0; i < 120000; i++)
            {
                BitConverter.GetBytes(m1m2.m1[i]).CopyTo(byteArray, 32 + (i * 2));
            }

            using (var stream = new System.IO.FileStream(path, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write))//запись в файл
                                                                                                                            //FileMode.OpenOrCreate- ОС открывает файл,если он существует,                                                                                                                            //в противном случае создаёт файл
            {
                //Записываем буфер
                stream.Write(byteArray, 0, byteArray.Length);
            }



        }

        public void Sys1_(byte[] byteArray, string path)
        {
            Sys1 sys1;
            sys1.pref = new byte[8];

            //заносим полученные данные в структуру sys1
            Array.Copy(byteArray, 0, sys1.pref, 0, 8);
            sys1.K1_skz_yel = BitConverter.ToSingle(byteArray, 22-14);
            sys1.K2_skz_yel = BitConverter.ToSingle(byteArray, 26-14);
            sys1.K1_pik_yel = BitConverter.ToSingle(byteArray, 30-14);
            sys1.K2_pik_yel = BitConverter.ToSingle(byteArray, 34-14);
            sys1.K1_skz_red = BitConverter.ToSingle(byteArray, 38-14);
            sys1.K2_skz_red = BitConverter.ToSingle(byteArray, 42-14);
            sys1.n_zub = BitConverter.ToSingle(byteArray, 46-14);
            sys1.Kd_rmk1 = BitConverter.ToSingle(byteArray, 50-14);
            sys1.Kd_rmk2 = BitConverter.ToSingle(byteArray, 54-14);
            sys1.K_Moment_rmk1 = BitConverter.ToSingle(byteArray, 58-14);
            sys1.K_Moment_rmk2 = BitConverter.ToSingle(byteArray, 62-14);
            sys1.graniza_1 = BitConverter.ToUInt16(byteArray, 66-14);
            sys1.graniza_2 = BitConverter.ToUInt16(byteArray, 68-14);
            sys1.moment_1 = BitConverter.ToUInt16(byteArray, 70-14);
            sys1.moment_2 = BitConverter.ToUInt16(byteArray, 72-14);
            sys1.kratnoct_FOB = BitConverter.ToUInt16(byteArray, 74-14);
            sys1.period_recording = BitConverter.ToUInt32(byteArray, 76-14);
            sys1.regim_work = BitConverter.ToUInt16(byteArray, 80-14);

            //достаём данные с структуры Sys1
            Array.Copy(sys1.pref, 0, byteArray, 0, 8);
            BitConverter.GetBytes(sys1.K1_skz_yel).CopyTo(byteArray, 14-6);
            BitConverter.GetBytes(sys1.K2_skz_yel).CopyTo(byteArray, 18-6);
            BitConverter.GetBytes(sys1.K1_pik_yel).CopyTo(byteArray, 22-6);
            BitConverter.GetBytes(sys1.K2_pik_yel).CopyTo(byteArray, 26-6);
            BitConverter.GetBytes(sys1.K1_skz_red).CopyTo(byteArray, 30-6);
            BitConverter.GetBytes(sys1.K2_skz_red).CopyTo(byteArray, 34-6);
            BitConverter.GetBytes(sys1.n_zub).CopyTo(byteArray, 38-6);
            BitConverter.GetBytes(sys1.Kd_rmk1).CopyTo(byteArray, 42-6);
            BitConverter.GetBytes(sys1.Kd_rmk2).CopyTo(byteArray, 46-6);
            BitConverter.GetBytes(sys1.K_Moment_rmk1).CopyTo(byteArray, 50-6);
            BitConverter.GetBytes(sys1.K_Moment_rmk2).CopyTo(byteArray, 54-6);

            BitConverter.GetBytes(sys1.graniza_1).CopyTo(byteArray, 58-6);
            BitConverter.GetBytes(sys1.graniza_2).CopyTo(byteArray, 60-6);
            BitConverter.GetBytes(sys1.moment_1).CopyTo(byteArray, 62-6);
            BitConverter.GetBytes(sys1.moment_2).CopyTo(byteArray, 64-6);
            BitConverter.GetBytes(sys1.kratnoct_FOB).CopyTo(byteArray, 66-6);
            BitConverter.GetBytes(sys1.period_recording).CopyTo(byteArray, 68-6);
            BitConverter.GetBytes(sys1.regim_work).CopyTo(byteArray, 72-6);

            using (var stream = new System.IO.FileStream(path, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write))//запись в файл
                                                                                                                            //FileMode.OpenOrCreate- ОС открывает файл,если он существует,
                                                                                                                            //в противном случае создаёт файл
            {
                //Перемещаемся в файле на 100 байт от начала
                //stream.Seek(0, System.IO.SeekOrigin.Begin);
                //Записываем буфер
                stream.Write(byteArray, 0, byteArray.Length);
            }
        }
        public F_sys2 f_sys2new;
        public void Sys2_(byte[] byteArray, string path)
        {
            
            f_sys2new.rmk = new sys2[2];
            f_sys2new.count_ALL = new COUNT_F_ALL[2];

            //заносим полученные данные в структуру sys2
            f_sys2new.cnt = BitConverter.ToUInt32(byteArray, 14-14);
            f_sys2new.rmk[0].count.O1 = byteArray[18-14];
            f_sys2new.rmk[0].count.O2 = byteArray[19-14];
            f_sys2new.rmk[0].count.T1 = BitConverter.ToUInt16(byteArray, 20-14);
            f_sys2new.rmk[0].count.T2 = BitConverter.ToUInt16(byteArray, 22-14);
            f_sys2new.rmk[0].count.O31 = byteArray[24-14];
            f_sys2new.rmk[0].count.O32 = byteArray[25-14];
            f_sys2new.rmk[0].count.T31 = BitConverter.ToUInt16(byteArray, 26-14);
            f_sys2new.rmk[0].count.T32 = BitConverter.ToUInt16(byteArray, 28-14);

            f_sys2new.rmk[0].Sr_skz_d1 = BitConverter.ToSingle(byteArray, 30-14);
            f_sys2new.rmk[0].Sr_skz_d2 = BitConverter.ToSingle(byteArray, 34-14);
            f_sys2new.rmk[0].Sr_pik_d1 = BitConverter.ToSingle(byteArray, 38-14);
            f_sys2new.rmk[0].Sr_pik_d2 = BitConverter.ToSingle(byteArray, 42-14);

            f_sys2new.rmk[0].pred_skz_d1 = BitConverter.ToSingle(byteArray, 46-14);
            f_sys2new.rmk[0].pred_skz_d2 = BitConverter.ToSingle(byteArray, 50-14);
            f_sys2new.rmk[0].pred_pik_d1 = BitConverter.ToSingle(byteArray, 54-14);
            f_sys2new.rmk[0].pred_pik_d2 = BitConverter.ToSingle(byteArray, 58-14);

            f_sys2new.rmk[0].Sr_M_d1 = BitConverter.ToSingle(byteArray, 62-14);
            f_sys2new.rmk[0].Sr_M_d2 = BitConverter.ToSingle(byteArray, 66-14);

            f_sys2new.rmk[0].Cnt_Bad_red = BitConverter.ToUInt32(byteArray, 70-14);
            f_sys2new.rmk[0].Cnt_Bad_yel = BitConverter.ToUInt32(byteArray, 74-14);

            f_sys2new.rmk[0].Sum_skz_d1 = BitConverter.ToSingle(byteArray, 78-14);
            f_sys2new.rmk[0].Sum_pik_d1 = BitConverter.ToSingle(byteArray, 82-14);
            f_sys2new.rmk[0].Sum_skz_d2 = BitConverter.ToSingle(byteArray, 86-14);
            f_sys2new.rmk[0].Sum_pik_d2 = BitConverter.ToSingle(byteArray, 90-14);

            f_sys2new.rmk[0].Prozent_yel = BitConverter.ToSingle(byteArray, 94-14);
            f_sys2new.rmk[0].Prozent_red = BitConverter.ToSingle(byteArray, 98-14);

            f_sys2new.rmk[0].cnt_d1 = BitConverter.ToUInt16(byteArray, 102-14);
            f_sys2new.rmk[0].cnt_d2 = BitConverter.ToUInt16(byteArray, 104-14);

            f_sys2new.rmk[0].defect_file = BitConverter.ToUInt16(byteArray, 106-14);

            f_sys2new.rmk[1].count.O1 = byteArray[108-14];
            f_sys2new.rmk[1].count.O2 = byteArray[109-14];
            f_sys2new.rmk[1].count.T1 = BitConverter.ToUInt16(byteArray, 110-14);
            f_sys2new.rmk[1].count.T2 = BitConverter.ToUInt16(byteArray, 112-14);
            f_sys2new.rmk[1].count.O31 = byteArray[114-14];
            f_sys2new.rmk[1].count.O32 = byteArray[115-14];
            f_sys2new.rmk[1].count.T31 = BitConverter.ToUInt16(byteArray, 116-14);
            f_sys2new.rmk[1].count.T32 = BitConverter.ToUInt16(byteArray, 118-14);

            f_sys2new.rmk[1].Sr_skz_d1 = BitConverter.ToSingle(byteArray, 120-14);
            f_sys2new.rmk[1].Sr_skz_d2 = BitConverter.ToSingle(byteArray, 124-14);
            f_sys2new.rmk[1].Sr_pik_d1 = BitConverter.ToSingle(byteArray, 128-14);
            f_sys2new.rmk[1].Sr_pik_d2 = BitConverter.ToSingle(byteArray, 132-14);

            f_sys2new.rmk[1].pred_skz_d1 = BitConverter.ToSingle(byteArray, 136-14);
            f_sys2new.rmk[1].pred_skz_d2 = BitConverter.ToSingle(byteArray, 140-14);
            f_sys2new.rmk[1].pred_pik_d1 = BitConverter.ToSingle(byteArray, 144-14);
            f_sys2new.rmk[1].pred_pik_d2 = BitConverter.ToSingle(byteArray, 148-14);

            f_sys2new.rmk[1].Sr_M_d1 = BitConverter.ToSingle(byteArray, 152-14);
            f_sys2new.rmk[1].Sr_M_d2 = BitConverter.ToSingle(byteArray, 156-14);

            f_sys2new.rmk[1].Cnt_Bad_red = BitConverter.ToUInt32(byteArray, 160-14);
            f_sys2new.rmk[1].Cnt_Bad_yel = BitConverter.ToUInt32(byteArray, 164-14);

            f_sys2new.rmk[1].Sum_skz_d1 = BitConverter.ToSingle(byteArray, 168-14);
            f_sys2new.rmk[1].Sum_pik_d1 = BitConverter.ToSingle(byteArray, 172-14);
            f_sys2new.rmk[1].Sum_skz_d2 = BitConverter.ToSingle(byteArray, 176-14);
            f_sys2new.rmk[1].Sum_pik_d2 = BitConverter.ToSingle(byteArray, 180-14);

            f_sys2new.rmk[1].Prozent_yel = BitConverter.ToSingle(byteArray, 184-14);
            f_sys2new.rmk[1].Prozent_red = BitConverter.ToSingle(byteArray, 188-14);

            f_sys2new.rmk[1].cnt_d1 = BitConverter.ToUInt16(byteArray, 192-14);
            f_sys2new.rmk[1].cnt_d2 = BitConverter.ToUInt16(byteArray, 194-14);

            f_sys2new.rmk[1].defect_file = BitConverter.ToUInt16(byteArray, 196-14);


            f_sys2new.nomer_rmk = BitConverter.ToUInt16(byteArray, 198-14);

            f_sys2new.regim_work_tek = BitConverter.ToUInt16(byteArray, 200-14);

            f_sys2new.count_ALL[0].O1 = BitConverter.ToUInt32(byteArray, 202-14);
            f_sys2new.count_ALL[0].O2 = BitConverter.ToUInt32(byteArray, 206-14);
            f_sys2new.count_ALL[0].T1 = BitConverter.ToUInt32(byteArray, 210-14);
            f_sys2new.count_ALL[0].T2 = BitConverter.ToUInt32(byteArray, 214-14);
            f_sys2new.count_ALL[0].O31 = BitConverter.ToUInt32(byteArray, 218-14);
            f_sys2new.count_ALL[0].O32 = BitConverter.ToUInt32(byteArray, 222-14);
            f_sys2new.count_ALL[0].T31 = BitConverter.ToUInt32(byteArray, 226-14);
            f_sys2new.count_ALL[0].T32 = BitConverter.ToUInt32(byteArray, 230-14);

            //2


            f_sys2new.count_ALL[1].O1 = BitConverter.ToUInt32(byteArray, 234-14);
            f_sys2new.count_ALL[1].O2 = BitConverter.ToUInt32(byteArray, 238-14);
            f_sys2new.count_ALL[1].T1 = BitConverter.ToUInt32(byteArray, 242-14);
            f_sys2new.count_ALL[1].T2 = BitConverter.ToUInt32(byteArray, 246-14);
            f_sys2new.count_ALL[1].O31 = BitConverter.ToUInt32(byteArray, 250-14);
            f_sys2new.count_ALL[1].O32 = BitConverter.ToUInt32(byteArray, 254-14);
            f_sys2new.count_ALL[1].T31 = BitConverter.ToUInt32(byteArray, 258-14);
            f_sys2new.count_ALL[1].T32 = BitConverter.ToUInt32(byteArray, 262-14);


            //достаём данные с структуры Sys2
            BitConverter.GetBytes(f_sys2new.cnt).CopyTo(byteArray, 14-14);//8

            BitConverter.GetBytes(f_sys2new.rmk[0].count.O1).CopyTo(byteArray, 18-4);
            BitConverter.GetBytes(f_sys2new.rmk[0].count.O2).CopyTo(byteArray, 19-14);
            BitConverter.GetBytes(f_sys2new.rmk[0].count.T1).CopyTo(byteArray, 20-14);
            BitConverter.GetBytes(f_sys2new.rmk[0].count.T2).CopyTo(byteArray, 22-14);
            BitConverter.GetBytes(f_sys2new.rmk[0].count.O31).CopyTo(byteArray, 24-14);
            BitConverter.GetBytes(f_sys2new.rmk[0].count.O32).CopyTo(byteArray, 25-14);
            BitConverter.GetBytes(f_sys2new.rmk[0].count.T31).CopyTo(byteArray, 26-14);
            BitConverter.GetBytes(f_sys2new.rmk[0].count.T32).CopyTo(byteArray, 28-14);

            BitConverter.GetBytes(f_sys2new.rmk[0].Sr_skz_d1).CopyTo(byteArray, 30-14);
            BitConverter.GetBytes(f_sys2new.rmk[0].Sr_skz_d2).CopyTo(byteArray, 34-14);
            BitConverter.GetBytes(f_sys2new.rmk[0].Sr_pik_d1).CopyTo(byteArray, 38-14);
            BitConverter.GetBytes(f_sys2new.rmk[0].Sr_pik_d2).CopyTo(byteArray, 42-14);

            BitConverter.GetBytes(f_sys2new.rmk[0].pred_skz_d1).CopyTo(byteArray, 46-14);
            BitConverter.GetBytes(f_sys2new.rmk[0].pred_skz_d2).CopyTo(byteArray, 50-14);
            BitConverter.GetBytes(f_sys2new.rmk[0].pred_pik_d1).CopyTo(byteArray, 54-14);
            BitConverter.GetBytes(f_sys2new.rmk[0].pred_pik_d2).CopyTo(byteArray, 58-14);

            BitConverter.GetBytes(f_sys2new.rmk[0].Sr_M_d1).CopyTo(byteArray, 62-14);
            BitConverter.GetBytes(f_sys2new.rmk[0].Sr_M_d2).CopyTo(byteArray, 66-14);

            BitConverter.GetBytes(f_sys2new.rmk[0].Cnt_Bad_red).CopyTo(byteArray, 70-14);
            BitConverter.GetBytes(f_sys2new.rmk[0].Cnt_Bad_yel).CopyTo(byteArray, 76-14);

            BitConverter.GetBytes(f_sys2new.rmk[0].Sum_skz_d1).CopyTo(byteArray, 78-14);
            BitConverter.GetBytes(f_sys2new.rmk[0].Sum_pik_d1).CopyTo(byteArray, 82-14);
            BitConverter.GetBytes(f_sys2new.rmk[0].Sum_skz_d2).CopyTo(byteArray, 86-14);
            BitConverter.GetBytes(f_sys2new.rmk[0].Sum_pik_d2).CopyTo(byteArray, 90-14);

            BitConverter.GetBytes(f_sys2new.rmk[0].Prozent_yel).CopyTo(byteArray, 94-14);
            BitConverter.GetBytes(f_sys2new.rmk[0].Prozent_red).CopyTo(byteArray, 98-14);

            BitConverter.GetBytes(f_sys2new.rmk[0].cnt_d1).CopyTo(byteArray, 102-14);
            BitConverter.GetBytes(f_sys2new.rmk[0].cnt_d2).CopyTo(byteArray, 104-14);

            BitConverter.GetBytes(f_sys2new.rmk[0].defect_file).CopyTo(byteArray, 106-14);

            BitConverter.GetBytes(f_sys2new.nomer_rmk).CopyTo(byteArray, 108-14);

            BitConverter.GetBytes(f_sys2new.regim_work_tek).CopyTo(byteArray, 110-14);

            BitConverter.GetBytes(f_sys2new.count_ALL[0].O1).CopyTo(byteArray, 112-14);
            BitConverter.GetBytes(f_sys2new.count_ALL[0].O2).CopyTo(byteArray, 116-14);
            BitConverter.GetBytes(f_sys2new.count_ALL[0].T1).CopyTo(byteArray, 120-14);
            BitConverter.GetBytes(f_sys2new.count_ALL[0].T2).CopyTo(byteArray, 124-14);
            BitConverter.GetBytes(f_sys2new.count_ALL[0].O31).CopyTo(byteArray, 128-14);
            BitConverter.GetBytes(f_sys2new.count_ALL[0].O32).CopyTo(byteArray, 132-14);
            BitConverter.GetBytes(f_sys2new.count_ALL[0].T31).CopyTo(byteArray, 136-14);
            BitConverter.GetBytes(f_sys2new.count_ALL[0].T32).CopyTo(byteArray, 140-14);

            //2
            BitConverter.GetBytes(f_sys2new.rmk[1].count.O1).CopyTo(byteArray, 144-14);
            BitConverter.GetBytes(f_sys2new.rmk[1].count.O2).CopyTo(byteArray, 145-14);
            BitConverter.GetBytes(f_sys2new.rmk[1].count.T1).CopyTo(byteArray, 146-14);
            BitConverter.GetBytes(f_sys2new.rmk[1].count.T2).CopyTo(byteArray, 148-14);
            BitConverter.GetBytes(f_sys2new.rmk[1].count.O31).CopyTo(byteArray, 150-14);
            BitConverter.GetBytes(f_sys2new.rmk[1].count.O32).CopyTo(byteArray, 151-14);
            BitConverter.GetBytes(f_sys2new.rmk[1].count.T31).CopyTo(byteArray, 152-14);
            BitConverter.GetBytes(f_sys2new.rmk[1].count.T32).CopyTo(byteArray, 154-14);

            BitConverter.GetBytes(f_sys2new.rmk[1].Sr_skz_d1).CopyTo(byteArray, 156-14);
            BitConverter.GetBytes(f_sys2new.rmk[1].Sr_skz_d2).CopyTo(byteArray, 160-14);
            BitConverter.GetBytes(f_sys2new.rmk[1].Sr_pik_d1).CopyTo(byteArray, 164-14);
            BitConverter.GetBytes(f_sys2new.rmk[1].Sr_pik_d2).CopyTo(byteArray, 168-14);

            BitConverter.GetBytes(f_sys2new.rmk[1].pred_skz_d1).CopyTo(byteArray, 172-14);
            BitConverter.GetBytes(f_sys2new.rmk[1].pred_skz_d2).CopyTo(byteArray, 176-14);
            BitConverter.GetBytes(f_sys2new.rmk[1].pred_pik_d1).CopyTo(byteArray, 180-14);
            BitConverter.GetBytes(f_sys2new.rmk[1].pred_pik_d2).CopyTo(byteArray, 184-14);

            BitConverter.GetBytes(f_sys2new.rmk[1].Sr_M_d1).CopyTo(byteArray, 188-14);
            BitConverter.GetBytes(f_sys2new.rmk[1].Sr_M_d2).CopyTo(byteArray, 192-14);

            BitConverter.GetBytes(f_sys2new.rmk[1].Cnt_Bad_red).CopyTo(byteArray, 196-14);
            BitConverter.GetBytes(f_sys2new.rmk[1].Cnt_Bad_yel).CopyTo(byteArray, 200-14);

            BitConverter.GetBytes(f_sys2new.rmk[1].Sum_skz_d1).CopyTo(byteArray, 204-14);
            BitConverter.GetBytes(f_sys2new.rmk[1].Sum_pik_d1).CopyTo(byteArray, 208-14);
            BitConverter.GetBytes(f_sys2new.rmk[1].Sum_skz_d2).CopyTo(byteArray, 212-14);
            BitConverter.GetBytes(f_sys2new.rmk[1].Sum_pik_d2).CopyTo(byteArray, 216-14);

            BitConverter.GetBytes(f_sys2new.rmk[1].Prozent_yel).CopyTo(byteArray, 220-14);
            BitConverter.GetBytes(f_sys2new.rmk[1].Prozent_red).CopyTo(byteArray, 224-14);

            BitConverter.GetBytes(f_sys2new.rmk[1].cnt_d1).CopyTo(byteArray, 228-14);
            BitConverter.GetBytes(f_sys2new.rmk[1].cnt_d2).CopyTo(byteArray, 230-14);

            BitConverter.GetBytes(f_sys2new.rmk[1].defect_file).CopyTo(byteArray, 232-14);

            BitConverter.GetBytes(f_sys2new.count_ALL[1].O1).CopyTo(byteArray, 234-14);
            BitConverter.GetBytes(f_sys2new.count_ALL[1].O2).CopyTo(byteArray, 238-14);
            BitConverter.GetBytes(f_sys2new.count_ALL[1].T1).CopyTo(byteArray, 242-14);
            BitConverter.GetBytes(f_sys2new.count_ALL[1].T2).CopyTo(byteArray, 246-14);
            BitConverter.GetBytes(f_sys2new.count_ALL[1].O31).CopyTo(byteArray, 250-14);
            BitConverter.GetBytes(f_sys2new.count_ALL[1].O32).CopyTo(byteArray, 254-14);
            BitConverter.GetBytes(f_sys2new.count_ALL[1].T31).CopyTo(byteArray, 258-14);
            BitConverter.GetBytes(f_sys2new.count_ALL[1].T32).CopyTo(byteArray, 262-14);

            using (var stream = new System.IO.FileStream(path, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write))//запись в файл
            {
                //Перемещаемся в файле на 100 байт от начала
                //stream.Seek(0, System.IO.SeekOrigin.Begin);
                //Записываем буфер
                stream.Write(byteArray, 0, byteArray.Length);
            }

        }


        public void filePath(byte[] data, int numberOfCar,ref string path, ref string pathSys1, ref string pathSys2,ref string pathO1, ref string pathO2, ref string pathT1, ref string pathT2, ref string pathO31, ref string pathO32, ref string pathT31, ref string pathT32)
        {

            path = @"Z:\Belaz\";//путь для сохранения файлов
            pathSys1 = @"Z:\Belaz\" + numberOfCar + "\\" + "sys1.bin";
            pathSys2 = @"Z:\Belaz\" + numberOfCar + "\\" + "sys2.bin";          

            if (BitConverter.ToInt32(data, 8) == (2))
                pathO1 = @"Z:\Belaz\" + numberOfCar + "\\" + "K0" + "\\" + "O1.bin";
            else if(BitConverter.ToInt32(data, 8) == (10))
                pathO1 = @"Z:\Belaz\" + numberOfCar + "\\" + "K1" + "\\" + "O1.bin";

            if (BitConverter.ToInt32(data, 8) == (3))
                pathT1 = @"Z:\Belaz\" + numberOfCar + "\\" + "K0" + "\\" + "T1.bin";
            else if (BitConverter.ToInt32(data, 8) == (11))
                pathT1 = @"Z:\Belaz\" + numberOfCar + "\\" + "K1" + "\\" + "T1.bin";

            if (BitConverter.ToInt32(data, 8) == (4))
                pathO2 = @"Z:\Belaz\" + numberOfCar + "\\" + "K0" + "\\" + "O2.bin";
            else if (BitConverter.ToInt32(data, 8) == (12))
                pathO2 = @"Z:\Belaz\" + numberOfCar + "\\" + "K1" + "\\" + "O2.bin";

            if (BitConverter.ToInt32(data, 8) == (5))
                pathT2 = @"Z:\Belaz\" + numberOfCar + "\\" + "K0" + "\\" + "T2.bin";
            else if (BitConverter.ToInt32(data, 8) == (13))
                pathT2 = @"Z:\Belaz\" + numberOfCar + "\\" + "K1" + "\\" + "T2.bin";

            if (BitConverter.ToInt32(data, 8) == (6))
                pathO31 = @"Z:\Belaz\" + numberOfCar + "\\" + "K0" + "\\" + "O31.bin";
            else if (BitConverter.ToInt32(data, 8) == (14))
                pathO31 = @"Z:\Belaz\" + numberOfCar + "\\" + "K1" + "\\" + "O31.bin";

            if (BitConverter.ToInt32(data, 8) == (7))
                pathT31 = @"Z:\Belaz\" + numberOfCar + "\\" + "K0" + "\\" + "T31.bin";
            else if (BitConverter.ToInt32(data, 8) == (15))
                pathT31 = @"Z:\Belaz\" + numberOfCar + "\\" + "K1" + "\\" + "T31.bin";

            if (BitConverter.ToInt32(data, 8) == (8))
                pathO32 = @"Z:\Belaz\" + numberOfCar + "\\" + "K0" + "\\" + "O32.bin";
            else if (BitConverter.ToInt32(data, 8) == (16))
                pathO32 = @"Z:\Belaz\" + numberOfCar + "\\" + "K1" + "\\" + "O32.bin";

            if (BitConverter.ToInt32(data, 8) == (9))
                pathT32 = @"Z:\Belaz\" + numberOfCar + "\\" + "K0" + "\\" + "T32.bin";
            else if (BitConverter.ToInt32(data, 8) == (17))
                pathT32 = @"Z:\Belaz\" + numberOfCar + "\\" + "K1" + "\\" + "T32.bin";

        }

        public void massiv(ushort message, ushort numberOfCar, ushort command, ushort nenyhnayaInfa, ushort typeFile, ushort numberOfPartFile, ushort sizeOfPartFile, ushort reserv, ref byte[] massiv)
        {
            BitConverter.GetBytes(message).CopyTo(massiv, 0);
            BitConverter.GetBytes(numberOfCar).CopyTo(massiv, 2);
            BitConverter.GetBytes(command).CopyTo(massiv, 4);
            BitConverter.GetBytes(nenyhnayaInfa).CopyTo(massiv, 6);
            BitConverter.GetBytes(typeFile).CopyTo(massiv, 8);
            BitConverter.GetBytes(numberOfPartFile).CopyTo(massiv, 10);
            BitConverter.GetBytes(sizeOfPartFile).CopyTo(massiv, 12);
            BitConverter.GetBytes(reserv).CopyTo(massiv, 14);        
        }
       
        public void peremenye( ref ushort message, ref ushort numberOfCar, ref ushort command, ref ushort nenyhnayaInfa, ref ushort typeFile,ref ushort numberOfPartFile, ref ushort sizeOfPartFile, ref ushort reserv, byte[] massiv)
        {
             message = BitConverter.ToUInt16(massiv, 0);//переменная,которая обозначает код проекта в заголовке                
             command = BitConverter.ToUInt16(massiv, 2);//переменная,которая обозначает номер команды в заголовке
             numberOfCar = BitConverter.ToUInt16(massiv, 4);//переменная,которая обозначает номер машины в заголовке
             nenyhnayaInfa = BitConverter.ToUInt16(massiv, 6);//переменная,которая обозначает резерв в заголовке
             typeFile = BitConverter.ToUInt16(massiv, 8);
             numberOfPartFile = BitConverter.ToUInt16(massiv, 10);
             sizeOfPartFile = BitConverter.ToUInt16(massiv, 12);
             reserv = BitConverter.ToUInt16(massiv, 14);       
        }
        public struct MyCount // структура счетчиков всех файлов на сервере

        {
            public UInt32 O1; // счетчик образцового файла массива диапазона1

            public UInt32 O2; // счетчик образцового файла массива диапазона2

            public UInt32 T1; // счетчик текущего файла массива диапазона1

            public UInt32 T2; // счетчик текущего файла массива диапазона2

            public UInt32 O31; // счетчик элементов образцового файла ФДП диапазона1

            public UInt32 O32; // счетчик элементов образцового файла ФДП диапазона2

            public UInt32 T31; // счетчик элементов текущего файла ФДП диапазона1

            public UInt32 T32; // счетчик элементов текущего файла ФДП диапазона2
        }//32

        public MyCount myCountK1;
        public MyCount myCountK2;
        public void initializationMyCountK1()
        {
            byte[] byteArray = new byte[32];

            myCountK1.O1 = 0;
            myCountK1.O2 = 0;
            myCountK1.T1 = 0;
            myCountK1.T2 = 0;
            myCountK1.O31 = 0;
            myCountK1.O32 = 0;
            myCountK1.T31 = 0;
            myCountK1.T32 = 0;

            BitConverter.GetBytes(myCountK1.O1).CopyTo(byteArray, 0);
            BitConverter.GetBytes(myCountK1.O2).CopyTo(byteArray, 4);
            BitConverter.GetBytes(myCountK1.T1).CopyTo(byteArray, 8);
            BitConverter.GetBytes(myCountK1.T2).CopyTo(byteArray, 12);
            BitConverter.GetBytes(myCountK1.O31).CopyTo(byteArray, 16);
            BitConverter.GetBytes(myCountK1.O32).CopyTo(byteArray, 20);
            BitConverter.GetBytes(myCountK1.T31).CopyTo(byteArray, 24);
            BitConverter.GetBytes(myCountK1.T32).CopyTo(byteArray, 28);

            using (var stream = new System.IO.FileStream("Z:\\Belaz\\MycountK1.txt", System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write))//запись в файл
            {
                //Перемещаемся в файле на 100 байт от начала
                stream.Seek(0, System.IO.SeekOrigin.Begin);
                //Записываем буфер
                stream.Write(byteArray, 0, byteArray.Length);
            }

        }
        public void readMyCountK1()
        {
            byte[] byteArray = new byte[32];
            using (var stream = new System.IO.FileStream("Z:\\Belaz\\MycountK1.txt", System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write))//запись в файл
            {
                //Перемещаемся в файле на 100 байт от начала
                stream.Seek(0, System.IO.SeekOrigin.Begin);
                //Записываем буфер
                stream.Read(byteArray, 0, byteArray.Length);
            }

            myCountK1.O1 = BitConverter.ToUInt32(byteArray, 0);
            myCountK1.O2 = BitConverter.ToUInt32(byteArray, 4);
            myCountK1.T1 = BitConverter.ToUInt32(byteArray, 8);
            myCountK1.T2 = BitConverter.ToUInt32(byteArray, 12);
            myCountK1.O31 = BitConverter.ToUInt32(byteArray, 16);
            myCountK1.O32 = BitConverter.ToUInt32(byteArray, 20);
            myCountK1.T31 = BitConverter.ToUInt32(byteArray, 24);
            myCountK1.T32 = BitConverter.ToUInt32(byteArray, 28);

        }

        public void upgradeMyCountK1(ref UInt32 O1, ref UInt32 O2, ref UInt32 T1, ref UInt32 T2, ref UInt32 O31, ref UInt32 O32, ref UInt32 T31, ref UInt32 T32)
        {
            byte[] byteArray = new byte[32];
            myCountK1.O1 = O1;
            myCountK1.O2 = O2;
            myCountK1.T1 = T1;
            myCountK1.T2 = T2;
            myCountK1.O31 = O31;
            myCountK1.O32 = O32;
            myCountK1.T31 = T31;
            myCountK1.T32 = T32;

            using (var stream = new System.IO.FileStream("Z:\\Belaz\\MycountK1.txt", System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write))//запись в файл
            {
                //Перемещаемся в файле на 100 байт от начала
                stream.Seek(0, System.IO.SeekOrigin.Begin);
                //Записываем буфер
                stream.Write(byteArray, 0, byteArray.Length);
            }
        }
        public void initializationMyCountK2()
        {
            byte[] byteArray = new byte[32];
            myCountK2.O1 = 0;
            myCountK2.O2 = 0;
            myCountK2.T1 = 0;
            myCountK2.T2 = 0;
            myCountK2.O31 = 0;
            myCountK2.O32 = 0;
            myCountK2.T31 = 0;
            myCountK2.T32 = 0;

            BitConverter.GetBytes(myCountK2.O1).CopyTo(byteArray, 0);
            BitConverter.GetBytes(myCountK2.O2).CopyTo(byteArray, 4);
            BitConverter.GetBytes(myCountK2.T1).CopyTo(byteArray, 8);
            BitConverter.GetBytes(myCountK2.T2).CopyTo(byteArray, 12);
            BitConverter.GetBytes(myCountK2.O31).CopyTo(byteArray, 16);
            BitConverter.GetBytes(myCountK2.O32).CopyTo(byteArray, 20);
            BitConverter.GetBytes(myCountK2.T31).CopyTo(byteArray, 24);
            BitConverter.GetBytes(myCountK2.T32).CopyTo(byteArray, 28);

            using (var stream = new System.IO.FileStream("Z:\\Belaz\\MycountK2.txt", System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write))//запись в файл
            {
                //Перемещаемся в файле на 100 байт от начала
                stream.Seek(0, System.IO.SeekOrigin.Begin);
                //Записываем буфер
                stream.Write(byteArray, 0, byteArray.Length);
            }
        }
        public void readMyCountK2()
        {
            byte[] byteArray = new byte[32];
            using (var stream = new System.IO.FileStream("Z:\\Belaz\\MycountK2.txt", System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write))//запись в файл
            {
                //Перемещаемся в файле на 100 байт от начала
                stream.Seek(0, System.IO.SeekOrigin.Begin);
                //Записываем буфер
                stream.Read(byteArray, 0, byteArray.Length);
            }

            myCountK1.O1 = BitConverter.ToUInt32(byteArray, 0);
            myCountK1.O2 = BitConverter.ToUInt32(byteArray, 4);
            myCountK1.T1 = BitConverter.ToUInt32(byteArray, 8);
            myCountK1.T2 = BitConverter.ToUInt32(byteArray, 12);
            myCountK1.O31 = BitConverter.ToUInt32(byteArray, 16);
            myCountK1.O32 = BitConverter.ToUInt32(byteArray, 20);
            myCountK1.T31 = BitConverter.ToUInt32(byteArray, 24);
            myCountK1.T32 = BitConverter.ToUInt32(byteArray, 28);
        }
        public void upgradeMyCountK2(ref UInt32 O1, ref UInt32 O2, ref UInt32 T1, ref UInt32 T2, ref UInt32 O31, ref UInt32 O32, ref UInt32 T31, ref UInt32 T32)
        {
            byte[] byteArray = new byte[32];
            myCountK2.O1 = O1;
            myCountK2.O2 = O2;
            myCountK2.T1 = T1;
            myCountK2.T2 = T2;
            myCountK2.O31 = O31;
            myCountK2.O32 = O32;
            myCountK2.T31 = T31;
            myCountK2.T32 = T32;

            using (var stream = new System.IO.FileStream("Z:\\Belaz\\MycountK2.txt", System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write))//запись в файл
            {
                //Перемещаемся в файле на 100 байт от начала
                stream.Seek(0, System.IO.SeekOrigin.Begin);
                //Записываем буфер
                stream.Write(byteArray, 0, byteArray.Length);
            }
        }
    }
}
