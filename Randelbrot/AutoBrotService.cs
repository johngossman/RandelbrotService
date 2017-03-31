//#define USE_PRIORITYLIST

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Randelbrot
{
    public class AutoBrotService
    {
#if USE_PRIORITYLIST
        private PriorityList<MandelbrotSet> candidates;
#else
        private PriorityQueue<MandelbrotSet> candidates;
#endif
        private BeautyEvaluator evaluator;
        private Random random = new Random();


        public AutoBrotService(MandelbrotSet startSet, BeautyEvaluator evaluator)
        {
            this.evaluator = evaluator;
#if USE_PRIORITYLIST
            this.candidates = new PriorityList<MandelbrotSet>(1000, evaluator.Evaluate);
#else
            this.candidates = new PriorityQueue<MandelbrotSet>(10000, evaluator.Evaluate);
#endif
            this.candidates.Push(startSet);
        }

        private MandelbrotSet randomChild(MandelbrotSet set)
        {
            double newSide = (this.random.NextDouble() * set.Side / 4.5) + set.Side / 6;
            double newCX = ((this.random.NextDouble() - 0.5) * set.Side / 2) + set.Center.X;
            double newCY = ((this.random.NextDouble() - 0.5) * set.Side / 2) + set.Center.Y;
            var newSet = new MandelbrotSet(new DoubleComplexNumber(newCX, newCY), newSide);
            return newSet;
        }

        private List<MandelbrotSet> generateCandidates(MandelbrotSet set)
        {
            var retval = new List<MandelbrotSet>(10);

  /*          var newSet = new MandelbrotSet(set.Center, set.Side / 2);
            retval.Add(newSet);
            newSet = new MandelbrotSet(set.Center, set.Side * 0.7);
            retval.Add(newSet); 
            var newSet = new MandelbrotSet(new DoubleComplexNumber(set.Center.X - set.Side / 4, set.Center.Y - set.Side / 4), set.Side / 2);
            retval.Add(newSet);
            newSet = new MandelbrotSet(new DoubleComplexNumber(set.Center.X + set.Side / 4, set.Center.Y - set.Side / 4), set.Side / 2);
            retval.Add(newSet);
            newSet = new MandelbrotSet(new DoubleComplexNumber(set.Center.X - set.Side / 4, set.Center.Y + set.Side / 4), set.Side / 2);
            retval.Add(newSet);
            newSet = new MandelbrotSet(new DoubleComplexNumber(set.Center.X + set.Side / 4, set.Center.Y + set.Side / 4), set.Side / 2);
            retval.Add(newSet);
*/
            for (int i = 0; i < 12; i++)
            {
                retval.Add(this.randomChild(set));
            }
            return retval;
        }

        public MandelbrotSet Pop()
        {
            return this.candidates.Pop();
        }

        public MandelbrotSet Peek()
        {
            return this.candidates.Peek();
        }

        public void Generate()
        {
            var newCandidates = this.generateCandidates(this.candidates.Peek());
            this.candidates.Push(newCandidates);
        }

        public MandelbrotSet PopAndGenerate()
        {
            this.candidates.TrimExcess();
            var newCandidates = this.generateCandidates(this.candidates.Peek());
            var retval = this.candidates.Pop();
            this.candidates.Push(newCandidates);

            return retval;
        }

        public double PeekEvaluation()
        {
            return this.candidates.PeekEvaluation();
        }

        public int Count
        {
            get
            {
                return this.candidates.Count;
            }
        }

        public MandelbrotSet this[int i]
        {
            get
            {
                return this.candidates[i];
            }
        }

        public double Evaluation(int i)
        {
            return this.candidates.Evaluation(i);
        }
    }
}
