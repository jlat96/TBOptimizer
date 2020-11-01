using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OptimizerTests.TestModels.Evaluable;
using TrailBlazer.TBOptimizer.Comparison;
using TrailBlazer.TBOptimizer.Evaluation;

namespace OptimizerTests.Evaluation
{
    [TestFixture]
    public class ListEvaluation
    {
        [Test]
        public void TestExtremaIntegerMaximumComparer()
        {
            IComparer<TestIntegerEvaluable> comparer = new MaximizingComparer<TestIntegerEvaluable>();
            List<TestIntegerEvaluable> unsortedIntList = new List<TestIntegerEvaluable>()
            {
                new TestIntegerEvaluable(0, 7),
                new TestIntegerEvaluable(1, 1),
                new TestIntegerEvaluable(2, 3),
                new TestIntegerEvaluable(3, 8),
                new TestIntegerEvaluable(4, 2),
                new TestIntegerEvaluable(5, 5),
                new TestIntegerEvaluable(6, 4),
            };

            var max = unsortedIntList.Extrema<TestIntegerEvaluable, int>(comparer);
            Assert.AreEqual(3, max.SequenceNumber);
            Assert.AreEqual(8, max.GetEvaluation());
        }

        [Test]
        public void TestExtremaIntegerMinimumComparer()
        {
            IComparer<TestIntegerEvaluable> comparer = new MinimizingComparer<TestIntegerEvaluable>();
            List<TestIntegerEvaluable> unsortedIntList = new List<TestIntegerEvaluable>()
            {
                new TestIntegerEvaluable(0, 7),
                new TestIntegerEvaluable(1, 1),
                new TestIntegerEvaluable(2, 3),
                new TestIntegerEvaluable(3, 8),
                new TestIntegerEvaluable(4, 2),
                new TestIntegerEvaluable(5, 5),
                new TestIntegerEvaluable(6, 4),
            };

            var max = unsortedIntList.Extrema<TestIntegerEvaluable, int>(comparer);
            Assert.AreEqual(1, max.SequenceNumber);
            Assert.AreEqual(1, max.GetEvaluation());
        }

        private List<int> GetLargeRandomInput()
        {
            List<int> randomNumbers = new List<int>();
            Random randy = new Random(117);
            for (int i = 0; i < 1000; i++)
            {
                randomNumbers.Add(randy.Next() % 1000);
            }

            return randomNumbers;
        }

        [Test]
        public void TestExtremaListInReverseOrder()
        {
            IComparer<TestIntegerEvaluable> comparer = new MaximizingComparer<TestIntegerEvaluable>();
            List<int> numbers = new List<int>();
            for (int i = 0; i < 1000; i++)
            {
                numbers.Add(i);
            }

            List<TestIntegerEvaluable> reversedList = numbers.Select(n => new TestIntegerEvaluable(n, n))
                .OrderByDescending(e => e.Value).ToList();

            TestIntegerEvaluable max = reversedList.Extrema<TestIntegerEvaluable, int>(comparer);
            Assert.AreEqual(999, max.GetEvaluation());
        }

        [Test]
        public void TestExtremaMaximumLargeRandomInput()
        {
            IComparer<TestIntegerEvaluable> comparer = new MaximizingComparer<TestIntegerEvaluable>();
            List<int> randomNumbers = GetLargeRandomInput();

            int j = 0;
            List<TestIntegerEvaluable> unsortedIntList = randomNumbers.Select(n => new TestIntegerEvaluable(j++, n)).ToList();
            var max = unsortedIntList.Extrema<TestIntegerEvaluable, int>(comparer);
            int maxInt = unsortedIntList.Max(s => s.Value);
            Assert.AreEqual(maxInt, max.GetEvaluation());
        }

        [Test]
        public void TestExtremaMinimizingLargeRandomInput()
        {
            IComparer<TestIntegerEvaluable> comparer = new MinimizingComparer<TestIntegerEvaluable>();
            List<int> randomNumbers = GetLargeRandomInput();

            int j = 0;
            List<TestIntegerEvaluable> unsortedIntList = randomNumbers.Select(n => new TestIntegerEvaluable(j++, n)).ToList();
            var max = unsortedIntList.Extrema<TestIntegerEvaluable, int>(comparer);
            int maxInt = unsortedIntList.Min(s => s.Value);
            Assert.AreEqual(maxInt, max.GetEvaluation());
        }

        [Test]
        public void TestExtremaAllValuesEqual()
        {
            IComparer<TestIntegerEvaluable> comparer = new MaximizingComparer<TestIntegerEvaluable>();
            List<TestIntegerEvaluable> unsortedIntList = new List<TestIntegerEvaluable>()
            {
                new TestIntegerEvaluable(0, 10),
                new TestIntegerEvaluable(1, 10),
                new TestIntegerEvaluable(2, 10),
                new TestIntegerEvaluable(3, 10),
                new TestIntegerEvaluable(4, 10),
                new TestIntegerEvaluable(5, 10),
                new TestIntegerEvaluable(6, 10),
            };
            var max = unsortedIntList.Extrema<TestIntegerEvaluable, int>(comparer);
            Assert.AreEqual(0, max.SequenceNumber);
            Assert.AreEqual(10, max.GetEvaluation());
        }
    }
}
