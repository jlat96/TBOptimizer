using System;
using System.Collections.Generic;
using TBOptimizer.Climber;
using TrailBlazer.TBOptimizer.Climber;
using TrailBlazer.TBOptimizer.Climber.Algorithm;
using TrailBlazer.TBOptimizer.Comparison;
using TrailBlazer.TBOptimizer.State;

namespace TrailBlazer.TBOptimizer.Configuration
{
    public class ClimberConfiguration<TState, TEvaluation> : IClimberConfiguration<TState, TEvaluation> 
        where TState : EvaluableState<TEvaluation>
        where TEvaluation : IComparable<TEvaluation>
    {
        public IComparer<TState> StateComparer { get; internal set; }
        public Func<TState,IEnumerable<TState>> SuccessorGenerationFunction { get; internal set; }
        public IClimberAlgorithm<TState, TEvaluation> ClimberAlgorithm { get; private set; } 

        public static ClimberConfiguration<TState, TEvaluation> Create()
        {
            return new ClimberConfiguration<TState, TEvaluation>();
        }

        /// <summary>
        /// Configure the climber to optimize in a specified direction
        /// </summary>
        /// <param name="direction">The direction for the climber to optimize in</param>
        /// <returns>the modified configuration</returns>
        public ClimberConfiguration<TState, TEvaluation> ClimbsInDirection(ClimberDirection direction)
        {
            StateComparisonFactory comparisonFactory = new StateComparisonFactory();
            StateComparer = comparisonFactory.Create<TState, TEvaluation>(direction);
            return this;
        }

        /// <summary>
        /// Use a custom comparion strategy to compare states during climbing
        /// </summary>
        /// <param name="comparisonStrategy">The comparison strategy with which to compare states</param>
        /// <returns>The modified configuration</returns>
        public ClimberConfiguration<TState, TEvaluation> ComparesUsing(IComparer<TState> comparisonStrategy)
        {
            StateComparer = comparisonStrategy;
            return this;
        }

        /// <summary>
        /// Use a given function to generate successors for any given state.
        /// </summary>
        /// <param name="successorGenerationFunction">The function that crete neighbor states from any given state 'c'/></param>
        /// <returns>The modified configuration</returns>
        public ClimberConfiguration<TState, TEvaluation> GeneratesSuccessorsWith(
            Func<TState, IEnumerable<TState>> successorGenerationFunction)
        {
            SuccessorGenerationFunction = successorGenerationFunction;
            return this;
        }

        /// <summary>
        /// Use a concrete SuccessorGenerator object to generate successor states.
        /// </summary>
        /// <param name="successorGenerator">A successor generator that will create neighbor states for any given state</param>
        /// <returns>The modified configuration</returns>
        public ClimberConfiguration<TState, TEvaluation> GeneratesSuccessorsWith(ISuccessorGenerator<TState, TEvaluation> successorGenerator)
        {
            if (successorGenerator == null)
            {
                throw new ArgumentNullException("SuccessorGenerator cannot be null");
            }
            return GeneratesSuccessorsWith(c => successorGenerator.GetSuccessors(c));
        }

        /// <summary>
        /// Use a pre-existing <see cref="IClimberAlgorithm{TState, TEvaluation}" to build the <see cref="HillClimber{TState, TEvaluation}"/>/>
        /// </summary>
        /// <param name="algorithm">The <see cref="ClimberAlgorithm"/> to Hill Climb with</param>
        /// <returns>Teh modified configuration</returns>
        public ClimberConfiguration<TState, TEvaluation> UsingAlgorithm(IClimberAlgorithm<TState, TEvaluation> algorithm)
        {
            ClimberAlgorithm = algorithm;
            return this;
        }

        /// <summary>
        /// Creates a <see cref="HillClimber{TState, TEvaluation}"/> from the current configuration
        /// </summary>
        /// <returns>The constructed <see cref="HillClimber{TState, TEvaluation}"/></returns>
        /// <exception cref="ConfigurationException">If the configuration is not valid at the time of execution</exception>
        public IHillClimber<TState, TEvaluation> Build()
        {
            if (!IsValid())
            {
                throw new ConfigurationException();
            }

            IComparer<TState> stateComparer = ResolveStateComperer(StateComparer);

            IClimberAlgorithm<TState, TEvaluation> algorithm = ResolveAlgorithm(stateComparer, SuccessorGenerationFunction);

            HillClimber<TState, TEvaluation> climber = new HillClimber<TState, TEvaluation>(algorithm);

            return climber;
        }

        /// <summary>
        /// Returns the configured <see cref="IComparer{TState}"/> if it is available of creates <see langword="abstract"/> default one
        /// </summary>
        /// <param name="comparer">The potentially configured <see cref="IComparer{TState}"/></param>
        /// <returns>A configured <see cref="IComparer{TState}"/></returns>
        private IComparer<TState> ResolveStateComperer(IComparer<TState> comparer)
        {
            return comparer ?? new MaximizingComparer<TState>();
        }

        /// <summary>
        /// Returns the configured <see cref="IClimberAlgorithm{TState, TEvaluation}"/> if it is available or creates a default one with the given configuration.
        /// </summary>
        /// <returns>The <see cref="IClimberAlgorithm{TState, TEvaluation}"/> for this configuration</returns>
        private IClimberAlgorithm<TState, TEvaluation> ResolveAlgorithm(IComparer<TState> comparer, Func<TState, IEnumerable<TState>> successorGenerationFunction)
        {
            IClimberAlgorithm<TState, TEvaluation> algorithm;

            // create a climber algorithm if one is not already configured
            if (ClimberAlgorithm == null)
            {
                ClimberSuccessorSelector<TState, TEvaluation> successorSelector = new ClimberSuccessorSelector<TState, TEvaluation>(comparer, successorGenerationFunction);
                algorithm = new LocalClimberAlgorithm<TState, TEvaluation>(successorSelector);
            }
            else
            {
                algorithm = ClimberAlgorithm;
            }

            return algorithm;
        }

        /// <summary>
        /// Determines if this climber configuration is in a state to build a hill climber
        /// </summary>
        /// <returns>The validity status of this configuration</returns>
        public bool IsValid()
        {
            if (ClimberAlgorithm == null)
            {
                return HasRequiredComponents();
            }

            return ClimberAlgorithm != null;
        }

        private bool HasRequiredComponents()
        {
            return SuccessorGenerationFunction != null;
        }
    }
}
