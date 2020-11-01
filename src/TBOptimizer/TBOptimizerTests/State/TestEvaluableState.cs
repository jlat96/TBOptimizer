using System;
using NUnit.Framework;
using OptimizerTests.TestModels.Evaluable;

namespace OptimizerTests.State
{
    [TestFixture]
    public class TestEvaluableState
    {
        [Test]
        public void TestLazyEvaluation()
        {
            TestIntegerEvaluableState state = new TestIntegerEvaluableState(1);
            Assert.AreEqual(0, state.TimesEvaluated);
            int evaluation = state.GetEvaluation();
            Assert.AreEqual(1, evaluation);
            Assert.AreEqual(1, state.TimesEvaluated);
            state.GetEvaluation();
            Assert.AreEqual(1, state.TimesEvaluated);
        }
    }
}
