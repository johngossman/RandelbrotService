using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Randelbrot
{
    
    public class PriorityList<T>
    {
        List<Tuple<double, T>> list;
        Func<T, double> evaluator;

        public PriorityList(int size, Func<T,double> evaluator)
        {
            this.list = new List<Tuple<double, T>>(size);
            this.evaluator = evaluator;
        }

        private void Sort()
        {
            this.list.Sort((x, y) =>
                {
                    return x.Item1.CompareTo(y.Item1);
                });
        }

        public void Push(T element)
        {
            double evaluation = this.evaluator(element);
            if (evaluation > double.NegativeInfinity)
            {
                this.list.Add(new Tuple<double, T>(evaluation, element));
                this.Sort();
            }
        }

        public void Push(List<T> elements)
        {
            foreach (T element in elements)
            {
                this.list.Add(new Tuple<double, T>(this.evaluator(element), element));
            }
            this.Sort();
        }

        public T Pop()
        {
            T retval = this.list[this.list.Count - 1].Item2;
            this.list.RemoveAt(this.list.Count - 1);
            return retval;
        }

        public T Peek()
        {
            return this.list[this.list.Count - 1].Item2;
        }

        public double PeekEvaluation()
        {
            return this.list[this.list.Count - 1].Item1;
        }

        public int Count
        {
            get
            {
                return this.list.Count;
            }
        }

        public T this[int i]
        {
            get
            {
                return this.list[this.list.Count - 1 - i].Item2;
            }
        }

        public double Evaluation(int i)
        {
            return this.list[this.list.Count - 1 - i].Item1;
        }

        public void TrimExcess()
        {
        }
    }
}
