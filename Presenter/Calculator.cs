using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PVK.Control.View;

namespace PVK.Control.Presenter
{
    class Calculator
    {
        public event EventHandler<EventArgs> UpdateWeightAxis;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public double lastWeigth = 0.0;
        private bool _bIncreaseWeight = false;
        /// <summary>
        /// Количество точек в интервале если меньше то игнорируем интервал
        /// </summary>
        private Int32 _minCountPoint4Intraval = 10;
        /// <summary>
        /// Минимальный считываемы вес
        /// </summary>
        private double _trimMinWeight = 0.200;

        /// <summary>
        /// Количество точек в интервале если меньше то игнорируем интервал
        /// </summary>
        public int MinCountPoint4Intraval
        {
            get { return _minCountPoint4Intraval; }
            set { _minCountPoint4Intraval = value; }
        }

        private Dictionary<Int32, List<WeightRecord>> _newArrWeight;

        /// <summary>
        /// Минимальный считываемы вес
        /// </summary>
        public double TrimMinWeight
        {
            get { return _trimMinWeight; }
            set { _trimMinWeight = value; }
        }
        /// <summary>
        /// Новые отпарсенные точки массива
        /// </summary>
        public Dictionary<Int32, List<WeightRecord>> NewArrWeight
        {
            get { return _newArrWeight; }
            set { _newArrWeight = value; }
        }

        public void Calculate(List<WeightRecord> arrWeight)
        {
            try
            {
                int numberInterval = 0;
                var weightCollet = new WeigthCollection();
                double lastlastweight = 1;
                lastWeigth = arrWeight.Count > 0 ? arrWeight[0].Weight : 0.0;
                bool changeInterval = false;
                WeightRecord val;
                weightCollet.IntervalCollection.Add(numberInterval ,new List<WeightRecord>());
                for (int i = 0 ; i< arrWeight.Count; i++)
                {
                    /// Получаем набор интервалов
                    if (arrWeight[i].Weight == TrimMinWeight)
                        continue;
                    val = arrWeight[i];
                    if ((val.Weight > TrimMinWeight && lastWeigth < TrimMinWeight ) ||
                        (val.Weight < TrimMinWeight && lastWeigth > TrimMinWeight ))
                    {
                        numberInterval++;
                        changeInterval = true;
                    }
                    if (changeInterval)
                    {
                        weightCollet.IntervalCollection.Add(numberInterval, new List<WeightRecord>());}
                    weightCollet.IntervalCollection[numberInterval].Add((WeightRecord)val.Clone());
                    // в случаи если точка попала на разделительную прямую, смотри уменшение это или возрастание
                    lastlastweight = lastWeigth;
                    lastWeigth = val.Weight;
                    changeInterval = false;

                }
                WeigthCollection newCollect = new WeigthCollection();
                int num = 0;
               /// Убираем лишние интревалы
                for (int i = 0 ; i <  weightCollet.IntervalCollection.Count; i++)
                {
                    if (weightCollet.IntervalCollection[i].Count <= 1)
                        continue;
                    if (weightCollet.IntervalCollection[i][0].Weight > TrimMinWeight)
                    {
                        /// Фильтруем верхние значения
                        /*if (weightCollet.IntervalCollection[i].Count < MinCountPoint4Intraval)
                        {
                            i++;
                            num++;
                            continue;
                        }*/
                        if (newCollect.IntervalCollection.ContainsKey(num))
                            newCollect.IntervalCollection[num].AddRange(new List<WeightRecord>(weightCollet.IntervalCollection[i]));
                        else
                            newCollect.IntervalCollection.Add(num, new List<WeightRecord>(weightCollet.IntervalCollection[i]));
                        /// Фильтруем нижние значения
                        if (weightCollet.IntervalCollection.Count > i + 1 && weightCollet.IntervalCollection[i + 1].Count < MinCountPoint4Intraval)
                        {
                            /// Объединяем интервалы предыдущий и следующий
                            if (weightCollet.IntervalCollection.Count > i + 2)
                            {
                                foreach (var weightRecord in weightCollet.IntervalCollection[i + 2])
                                {
                                    newCollect.IntervalCollection[num].Add((WeightRecord)weightRecord.Clone());
                                }
                            }
                            i++;
                            continue;
                        }
                        num++;
                    }
                }
                _newArrWeight = newCollect.IntervalCollection;

                foreach (var weightRecord in newCollect.IntervalCollection)
                {
                    lastWeigth = weightRecord.Value.Max(t => t.Weight);
                    if (UpdateWeightAxis != null)
                        UpdateWeightAxis(this, EventArgs.Empty);
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

    }

    public class WeigthCollection 
    {
        public Dictionary<Int32, List<WeightRecord>> IntervalCollection = new Dictionary<Int32, List<WeightRecord>>();
    }
}
