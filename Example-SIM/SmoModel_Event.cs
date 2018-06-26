using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonModel.Kernel;

namespace Model_Lab
{
    public partial class SmoModel : Model
    {
        // класс для события 1 - приход новой заявки
        public class K1 : TimeModelEvent<SmoModel>
        {
            #region Атрибуты события
            public Bid ZP;
            #endregion

            // алгоритм обработки события            
            protected override void HandleEvent(ModelEventArgs args)
            {
                if (ZP.MEM <= Model.FREEMEM)
                {
                    // Добавление заявки в очередь ПП
                    var rec = new QRec();                          
                    rec.Z = ZP;                                       
                    Model.QPP.Add(rec);

                    // Уменьшения объема свободной памяти
                    Model.FREEMEM -= ZP.MEM;
                }
                else
                {
                    // Добавление заявки во входную очередь
                    var rec = new QRec();
                    rec.Z = ZP;
                    Model.VQ.Add(rec);
                }

                // Планируем событие К1
                double dt1 = Model.GenTime.GenerateValue();
                var ev1 = new K1();                                
                Model.PlanEvent(ev1, dt1);                              
                Model.Tracer.PlanEventTrace(ev1);
            }
        }

        // класс для события 2 - Окончание кванта процессорного времени
        public class K2 : TimeModelEvent<SmoModel>
        {
            // алгоритм обработки события            
            protected override void HandleEvent(ModelEventArgs args)
            {
                if (Model.QPP.Count != 0)
                {
                    // Выбор первой заявки
                    QRec QPPRec = Model.QPP[0];
                    Model.QPP.RemoveAt(0);
                    // Выделение кванта
                    QPPRec.Z.KPT--;

                    if (QPPRec.Z.KPT == 0)
                    {
                        // Увеличение объема свободной ОП 
                        Model.FREEMEM += QPPRec.Z.MEM;

                        if (Model.VQ.Count != 0)
                        {
                            if (Model.FREEMEM >= Model.VQ[0].Z.MEM)
                            {
                                // Перенос заявки из VQ в QPP
                                Model.QPP.Add(Model.VQ[0]);
                                Model.VQ.RemoveAt(0);

                                // Уменьшение свободной ОП
                                Model.FREEMEM -= Model.QPP[Model.QPP.Count-1].Z.MEM;
                            }
                        }
                    }
                }

                // Планируем событие К2
                var ev2 = new K2();
                Model.PlanEvent(ev2, 1.0);
                Model.Tracer.PlanEventTrace(ev2);
            }
        }
    }
}
