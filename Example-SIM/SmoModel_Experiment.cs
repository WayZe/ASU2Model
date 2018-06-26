using System;
using CommonModel.Kernel;
using CommonModel.RandomStreamProducing;
using System.Collections.Generic;

namespace Model_Lab
{

    public partial class SmoModel : Model
    {
        //Условие завершения прогона модели True - завершить прогон. По умолчанию false. </summary>
        public override bool MustStopRun(int variantCount, int runCount)
        {
            return (Time >= TR);
        }

        //установка метода перебора вариантов модели
        public override bool MustPerformNextVariant(int variantCount)
        {
            //используем один вариант модели
            return variantCount < 1;
        }

        //true - продолжить выполнение ПРОГОНОВ модели;
        //false - прекратить выполнение ПРОГОНОВ модели. по умолчению false.
        public override bool MustPerformNextRun(int variantCount, int runCount)
        {
            return runCount < 1; //выполняем 1 прогон модели
        }

        //Задание начального состояния модели для нового варианта модели
        public override void SetNextVariant(int variantCount)
        {
            #region Параметры модели

            MAINMEM = 100.0;
            FREEMEM = MAINMEM;

			TR = 100;

            #endregion

            #region Установка параметров законов распределения

			(GenKK.BPN as GeneratedBaseRandomStream).Seed = 1;
			(GenTime.BPN as GeneratedBaseRandomStream).Seed = 2;
            (GenVol.BPN as GeneratedBaseRandomStream).Seed = 3;

            GenVol.Lyambda = IEZR;

            #endregion
        }

        public override void StartModelling(int variantCount, int runCount)
        {
            #region Задание начальных значений модельных переменных и объектов
            #endregion

            #region Cброс сборщиков статистики

            #endregion

            //Печать заголовка строки состояния модели
            TraceModelHeader();

			#region Планирование начальных событий      
            var ev1 = new K1();                         
			Bid Z1 = new Bid();
            Z1.NZ = 1;
			Z1.MEM = GenVol.GenerateValue();
			Z1.KPT = GenKK.GenerateValue();
			ev1.ZP = Z1;     
			var rec1 = new QRec();
            rec1.Z = Z1;
            PlanEvent(ev1, 0.0);                        
			Tracer.PlanEventTrace(ev1);

            var ev2 = new K1();                         
            Bid Z2 = new Bid();
            PlanEvent(ev2, 1.0);                        
            Tracer.PlanEventTrace(ev2);
            TraceModel();

            #endregion
        }

        //Действия по окончанию прогона
        public override void FinishModelling(int variantCount, int runCount)
        {
            Tracer.AnyTrace("");
            Tracer.TraceOut("==============================================================");
            Tracer.TraceOut("============Статистические результаты моделирования===========");
            Tracer.TraceOut("==============================================================");
            Tracer.AnyTrace("");
            Tracer.TraceOut("Время моделирования: " + String.Format("{0:0.00}", Time));

            Tracer.TraceOut("\r\nИнтенсивность числа полных прогонов: ");
            //Tracer.TraceOut("Заявка 1: " + KC[0] / TP);
            //Tracer.TraceOut("Заявка 2: " + KC[1] / TP);

            //Tracer.TraceOut("\r\nВероятность загрузки узлов: ");
            //Tracer.TraceOut("Узел 1: " + TSZ[0] / (TSZ[0] + TSZ[1] + TSZ[2]));
            //Tracer.TraceOut("Узел 2: " + TSZ[1] / (TSZ[0] + TSZ[1] + TSZ[2]));
            //Tracer.TraceOut("Узел 3: " + TSZ[2] / (TSZ[0] + TSZ[1] + TSZ[2]));

            //Tracer.TraceOut("\r\nСтатистические характеристики длин очередей: ");
            //Tracer.TraceOut("Очереди KPP: ");
            //Tracer.TraceOut("МО = " + String.Format("{0:0.000}", Variance_LKPP[0].Mx));
            //Tracer.TraceOut("Дисперсия = " + String.Format("{0:0.000}", Variance_LKPP[0].Stat));
            //Tracer.TraceOut("МО = " + String.Format("{0:0.000}", Variance_LKPP[1].Mx));
            //Tracer.TraceOut("Дисперсия = " + String.Format("{0:0.000}", Variance_LKPP[1].Stat));
            //Tracer.TraceOut("МО = " + String.Format("{0:0.000}", Variance_LKPP[2].Mx));
            //Tracer.TraceOut("Дисперсия = " + String.Format("{0:0.000}", Variance_LKPP[2].Stat));
            //Tracer.AnyTrace("");
            //Tracer.TraceOut("Очереди SQ: ");
            //Tracer.TraceOut("МО = " + String.Format("{0:0.000}", Variance_LSQ[0].Mx));
            //Tracer.TraceOut("Дисперсия = " + String.Format("{0:0.000}", Variance_LSQ[0].Stat));
            //Tracer.TraceOut("МО = " + String.Format("{0:0.000}", Variance_LSQ[1].Mx));
            //Tracer.TraceOut("Дисперсия = " + String.Format("{0:0.000}", Variance_LSQ[1].Stat));
            //Tracer.TraceOut("МО = " + String.Format("{0:0.000}", Variance_LSQ[2].Mx));
            //Tracer.TraceOut("Дисперсия = " + String.Format("{0:0.000}", Variance_LSQ[2].Stat));
        }

        //Печать заголовка
        void TraceModelHeader()
        {
            Tracer.TraceOut("==============================================================");
            Tracer.TraceOut("======================= Запущена модель ======================");
            Tracer.TraceOut("==============================================================");
            //вывод заголовка трассировки
            Tracer.AnyTrace("");
            Tracer.AnyTrace("Параметры модели:");
            Tracer.AnyTrace("");

            //Tracer.AnyTrace("Количество узлов: ");
            //Tracer.AnyTrace("KUVS = " + KUVS);
            //Tracer.AnyTrace("");
            //Tracer.AnyTrace("Количество заявок: ");
            //Tracer.AnyTrace("KZ = " + KZ);
            //Tracer.AnyTrace("");
            //Tracer.AnyTrace("Маршрут:");
            //Tracer.AnyTrace(MZ[0, 0] + " " + MZ[0, 1] + " " + MZ[0, 2]);
            //Tracer.AnyTrace(MZ[1, 0] + " " + MZ[1, 1] + " " + MZ[1, 2]);
            //Tracer.AnyTrace("");
            //Tracer.AnyTrace("МО:");
            //Tracer.AnyTrace(MOKK[0, 0] + " " + MOKK[0, 1] + " " + MOKK[0, 2]);
            //Tracer.AnyTrace(MOKK[1, 0] + " " + MOKK[1, 1] + " " + MOKK[1, 2]);
            //Tracer.AnyTrace("");
            //Tracer.AnyTrace("Максимальное количество ПП в каждом узле:");
            //Tracer.AnyTrace(MAXKPP[0] + " " + MAXKPP[0] + " " + MAXKPP[0]);
            Tracer.AnyTrace("");
            Tracer.AnyTrace("Время прогона: ");
            Tracer.AnyTrace("TP = " + TR);
            Tracer.AnyTrace("");
            Tracer.AnyTrace("Начальное состояние модели:");
            TraceModel();
            Tracer.AnyTrace("");

            Tracer.TraceOut("==============================================================");
            Tracer.AnyTrace("");
        }

        //Печать строки состояния
        void TraceModel()
        {
            Tracer.AnyTrace("LVQ = " + VQ.Count + " LQPP = " + QPP.Count + " FREEMEM = " + FREEMEM + " FREEMEM(%) = " + (int)FREEMEM/MAINMEM*100);
            Tracer.AnyTrace("");
        }

    }
}

