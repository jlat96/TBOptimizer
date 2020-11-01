using System;
using System.Collections.Generic;
using System.Text;

namespace TrailBlazer.TBOptimizer.State
{
    /// <summary>
    /// Generator for creating a neighborhood of states from a given state
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    /// <typeparam name="TEvaluation"></typeparam>
    public interface ISuccessorGenerator<TState, TEvaluation>
        where TState : IEvaluable<TEvaluation>
        where TEvaluation : IComparable<TEvaluation>
    {
        /// <summary>
        /// Returns the neighborhood of successor states for a given state
        /// </summary>
        /// <param name="state">The state for which to generate successors</param>
        /// <returns></returns>
        IEnumerable<TState> GetSuccessors(TState state);
    }
}
