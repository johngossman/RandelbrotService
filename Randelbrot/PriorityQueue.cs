using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Randelbrot
{
    struct PriorityQueueItem<T>
    {
        public PriorityQueueItem(T element, double evaluationValue)
        {
            this.element = element;
            this.evaluationValue = evaluationValue;
        }
        public double evaluationValue;
        public T element;
    }

    public class PriorityQueue<T>
    {
        List<PriorityQueueItem<T>> theHeap;
        Func<T, double> evaluator;
        int size;
        public PriorityQueue(int size, Func<T,double> evaluator)
        {
            this.theHeap = new List<PriorityQueueItem<T>>(size + 1);
            this.theHeap.Add(new PriorityQueueItem<T>(default(T), 0.0));  // Reserve the 0th element
            this.evaluator = evaluator;
            this.size = size;
        }

        private int Parent(int i)
        {
            if (i == 1)
                return 1;
            return (i / 2);
        }

        private int LessChild(int i)
        {
            return Math.Min(i * 2, this.theHeap.Count-1);
        }

        private int GreaterChild(int i)
        {
            return Math.Min((i * 2) + 1, this.theHeap.Count-1);
        }

        private void Swap(int i, int j)
        {
            PriorityQueueItem<T> t = this.theHeap[i];
            this.theHeap[i] = this.theHeap[j];
            this.theHeap[j] = t;
        }

        private bool Compare(int i, int j)
        {
            return (this.theHeap[i].evaluationValue > this.theHeap[j].evaluationValue);
        }


        // FloatUp and SinkDown raise or lower items into their correct
        // position in the PriorityQueue
        private void FloatUp(int item)
        {
            int i = item;
            int j = this.Parent(i);
            while (this.Compare(i,j))
            {
                this.Swap(i,j);
                i = j;
                j = this.Parent(i);
            }
        }

        private void SinkDown(int item)
        {
            int len = this.theHeap.Count;
            int i = item;
            while (i < len)
            {
                int c1 = this.LessChild(i);
                int c2 = this.GreaterChild(i);
                int c = c2;
                if (this.Compare(c1, c))
                {
                    c = c1;
                }
                if (this.Compare(c, i))
                {
                    this.Swap(c, i);
                    i = c;
                }
                else
                {
                    break;
                }
            }
        }

        private void PushItem(PriorityQueueItem<T> item)
        {
            this.theHeap.Add(item);
            this.FloatUp(this.theHeap.Count - 1);
        }

        public void Push(T element)
        {
            double evaluation = this.evaluator(element);
            if (evaluation > Double.NegativeInfinity)
            {
                var item = new PriorityQueueItem<T>(element, evaluation);
                this.PushItem(item);
            }
        }

        public void Push(List<T> elements)
        {
            foreach (T t in elements)
            {
                this.Push(t);
            }
        }

        private PriorityQueueItem<T> PopItem()
        {
            var retval = this.theHeap[1];
            var last = this.theHeap[this.theHeap.Count - 1];
            this.theHeap.RemoveAt(this.theHeap.Count - 1);
            if (this.theHeap.Count > 1)
            {
                this.theHeap[1] = last;
                this.SinkDown(1);
            }
            return retval;
        }

        public T Pop()
        {
            T retval = PopItem().element;
            return retval;
        }

        public T Peek()
        {
            return this.theHeap[1].element;
        }

        public double PeekEvaluation()
        {
            return this.theHeap[1].evaluationValue;
        }

        public bool IsEmpty()
        {
            // Account for the 0th element
            return (this.theHeap.Count < 2);
        }

        public void TrimExcess()
        {
            if (this.theHeap.Count + 1 > 2 * this.size)
            {
                var newPQ = new PriorityQueue<T>(this.size, this.evaluator);
                for (int i = 0; i < this.size; i++)
                {
                    newPQ.PushItem(this.PopItem());
                }
                this.theHeap = newPQ.theHeap;
            }
        }

        public int Count
        {
            get
            {
                return this.theHeap.Count - 1;
            }
        }

        public T this[int i]
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public double Evaluation(int i)
        {
            throw new NotImplementedException();
        }
    }
}
