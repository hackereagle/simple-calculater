using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace SimpleCalculator.BusinessLogic
{
    class Calculate
    {
        System.Reactive.Subjects.ISubject<string> result = new System.Reactive.Subjects.Subject<string>();

        #region Methods
        public void Add(double firstValue, double secondValue)
        {
            result.OnNext((firstValue + secondValue).ToString());
        }

        public void Substract(double firstValue, double secondValue)
        {
            result.OnNext((firstValue - secondValue).ToString());
        }

        public void Mutiply(double firstValue, double secondValue)
        {
            result.OnNext((firstValue * secondValue).ToString());
        }

        public void Divide(double firstValue, double secondValue)
        {
            result.OnNext((firstValue / secondValue).ToString());
        }

        public IObservable<string> Result => result.AsObservable();
        #endregion
    }
}
