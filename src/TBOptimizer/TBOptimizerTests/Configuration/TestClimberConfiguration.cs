using NUnit.Framework;
using OptimizerTests.TestModels.Evaluable;
using OptimizerTests.TestModels.State;
using System;
using System.Collections.Generic;
using System.Text;
using TBOptimizer.Climber;
using TrailBlazer.TBOptimizer.Comparison;
using TrailBlazer.TBOptimizer.Configuration;
using TrailBlazer.TBOptimizer.State;

namespace TBOptimizerTests.Configuration
{
    [TestFixture]
    public class TestClimberConfiguration
    {
        private ISuccessorGenerator<TestIntegerEvaluableState, int> generator;

        [SetUp]
        public void SetUp()
        {
            generator = new TestLinearIntegerSuccessorGenerator();
        }

        [Test]
        public void TestIsValidComponentsAreMissing()
        {
            ClimberConfiguration<TestIntegerEvaluableState, int> config = ClimberConfiguration<TestIntegerEvaluableState, int>.Create();


            Assert.IsNotNull(config);
            Assert.IsFalse(config.IsValid());
        }

        [Test]
        public void TestCreateFromConfigurationWithDefaults()
        {
            ClimberConfiguration<TestIntegerEvaluableState, int> config = ClimberConfiguration<TestIntegerEvaluableState, int>.Create()
                .GeneratesSuccessorsWith(generator);

            IComparer<TestIntegerEvaluableState> expectedComparer = new MaximizingComparer<TestIntegerEvaluableState>();

            Assert.IsNotNull(config);
            Assert.IsTrue(config.IsValid());
        }

        [Test]
        public void TestCreateFromConfigurationWithDefinedDirection()
        {
            ClimberConfiguration<TestIntegerEvaluableState, int> config = ClimberConfiguration<TestIntegerEvaluableState, int>.Create()
                .GeneratesSuccessorsWith(generator)
                .ClimbsInDirection(ClimberDirection.Maximize);

            Assert.IsNotNull(config);
            Assert.IsTrue(config.IsValid());
        }

        [Test]
        public void TestCreateFromConfigurationWithCustomComparer()
        {
            IComparer<TestIntegerEvaluableState> comparer = new MinimizingComparer<TestIntegerEvaluableState>();
            ClimberConfiguration<TestIntegerEvaluableState, int> config = ClimberConfiguration<TestIntegerEvaluableState, int>.Create()
                .GeneratesSuccessorsWith(generator)
                .ComparesUsing(comparer);

            Assert.IsNotNull(config);
            Assert.IsTrue(config.IsValid());
        }

        [Test]
        public void TestBuildCreatesClimberWhenValid()
        {
            IComparer<TestIntegerEvaluableState> comparer = new MinimizingComparer<TestIntegerEvaluableState>();
            ClimberConfiguration<TestIntegerEvaluableState, int> config = ClimberConfiguration<TestIntegerEvaluableState, int>.Create()
                .GeneratesSuccessorsWith(generator)
                .ComparesUsing(comparer);

            IHillClimber<TestIntegerEvaluableState, int> climber = null;

            try
            {
                climber = config.Build();
            }
            catch
            {
                Assert.Fail("Build threw an exception");
            }

            Assert.IsNotNull(climber);
        }

        [Test]
        public void TestBuildThrowsCorrectExceptionWhenInvalid()
        {
            IComparer<TestIntegerEvaluableState> comparer = new MinimizingComparer<TestIntegerEvaluableState>();
            ClimberConfiguration<TestIntegerEvaluableState, int> config = ClimberConfiguration<TestIntegerEvaluableState, int>.Create();

            IHillClimber<TestIntegerEvaluableState, int> climber = null;

            bool caught = false;

            try
            {
                climber = config.Build();
            }
            catch (ConfigurationException)
            {
                caught = true;
            }

            Assert.IsTrue(caught);
            Assert.IsNull(climber);
        }
    }
}
