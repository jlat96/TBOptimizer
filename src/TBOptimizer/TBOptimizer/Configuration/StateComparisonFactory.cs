using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using TrailBlazer.TBOptimizer.Comparison;
using TrailBlazer.TBOptimizer.State;

namespace TrailBlazer.TBOptimizer.Configuration
{
    /// <summary>
    /// Factory for creating climber state <see cref="IComparable"/>s
    /// </summary>
    public class StateComparisonFactory
    {
        /// <summary>
        /// Creates an <see cref="IComparable{TState}"/> for the desired climber direction
        /// </summary>
        /// <typeparam name="TState">The state for which the climber will optimize</typeparam>
        /// <typeparam name="TEvaluation">The type of the climber state's evaluation</typeparam>
        /// <param name="direction">The direction in which the climber will climb</param>
        /// <returns>An <see cref="IComparable{TState, TEvaluation}"/> that will promote climbing in the desired direction</returns>
        public IComparer<TState> Create<TState, TEvaluation>(ClimberDirection direction)
            where TState : EvaluableState<TEvaluation>
            where TEvaluation : IComparable<TEvaluation>
        {
            IComparer<TState> comparer;
            switch (direction)
            {
                case ClimberDirection.Maximize: 
                    comparer = new MaximizingComparer<TState>();
                    break;
                case ClimberDirection.Minimize:
                    comparer = new MinimizingComparer<TState>();
                    break;
                default:
                    throw new ArgumentException("Unknown climber direction");
            }

            return comparer;
        }
    }
}
