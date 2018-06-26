using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CommonModel.StatisticsCollecting;
using CommonModel.RandomStreamProducing;
using CommonModel.Collections;
using CommonModel.Kernel;

namespace Model_Lab
{

    public partial class SmoModel : Model
    {

        #region Параметры модель

        // время прогона имитационной модели(сек).
        double TR;
        //  интенсивность входного потока(простейший поток).
        double IVP;
        // интенсивность экспоненциального ЗР, которому подчинено выделение объема памяти из основной, для создания программных процессов(ПП).
        double IEZR;
        // интенсивность ЗР Пуассона, которому подчинено выделение количества квантов времени, необходимых для выполнения ПП.
        double IZRP;
        // объем основной памяти ВК.
        double MAINMEM;
        // величина кванта процессорного времени(сек).
        double DELKV;

        #endregion

        #region Переменные состояния модели

        // свободный объем основной памяти.
        double FREEMEM;
        // переменная для сбора статистики по времени нахождения заявок в системе.
        TRealVar TNZS;
        // переменная для сбора статистики по времени нахождения заявок в процессе счета.
        TRealVar TNZCALC;
        // переменная для сбора статистики по проценту занятой памяти относительно общего объема основной памяти.
        TRealVar PZPMEM;

        #endregion

        #region Дополнительные структуры

        // Заявки в узлах ВС
        public class Bid
        {
            // Номер заявки
            public int NZ;
            // Количество памяти
            public double MEM;
            // Требуемое количество квантов
            public int KPT;
        }

        // Элемент очереди заявки в узлах ВС 
        class QRec : QueueRecord
        {
            public Bid Z;
        }

        // Группа очередей ПП
        SimpleModelList<QRec> VQ;
        // Группа внешних очередей
        SimpleModelList<QRec> QPP;

        #endregion

        #region Cборщики статистики

        // 	Интенсивность числа полных циклов
        Variance<int>[] Variance_LKPP;
        Variance<int>[] Variance_LSQ;

        #endregion

        #region Генераторы ПСЧ

        // Генератор времени между заявками
        ExpStream GenTime;
        // Требуемый объём основной памяти для выполнение заявки
        ExpStream GenVol;
        // Генератор количества квантов
        PoissonStream GenKK;
        
        #endregion

        #region Инициализация объектов модели

        public SmoModel(Model parent, string name) : base(parent, name)
        {
            //LKPP = InitModelObjectArray<TIntVar>(3, "сборщик времени выполнения ТП_#");
            //LSQ = InitModelObjectArray<TIntVar>(3, "сборщик времени выполнения ТП_#");
            VQ = InitModelObject<SimpleModelList<QRec>>();
			QPP = InitModelObject<SimpleModelList<QRec>>();

			GenKK = InitModelObject<PoissonStream>();
			GenTime = InitModelObject<ExpStream>();
            GenVol = InitModelObject<ExpStream>();

            //Variance_LKPP = InitModelObjectArray<Variance<int>>(3, "сборщик времени выполнения ТП_#");
            //Variance_LSQ = InitModelObjectArray<Variance<int>>(3, "сборщик времени выполнения ТП_#");
            //Variance_LKPP[0].ConnectOnSet(LKPP[0]);
            //Variance_LKPP[1].ConnectOnSet(LKPP[1]);
            //Variance_LKPP[2].ConnectOnSet(LKPP[2]);
            //Variance_LSQ[0].ConnectOnSet(LSQ[0]);
            //Variance_LSQ[1].ConnectOnSet(LSQ[1]);
            //Variance_LSQ[2].ConnectOnSet(LSQ[2]);
        }

        #endregion
    }
}
