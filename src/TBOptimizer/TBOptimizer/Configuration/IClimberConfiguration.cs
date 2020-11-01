using System;
using System.Collections.Generic;
using TBOptimizer.Climber;
using TrailBlazer.TBOptimizer.Climber;
using TrailBlazer.TBOptimizer.Climber.Algorithm;
using TrailBlazer.TBOptimizer.State;

namespace TrailBlazer.TBOptimizer.Configuration
{
    public interface IClimberConfiguration<TState, TEvaluation>
        where TState : EvaluableState<TEvaluation>
        where TEvaluation : IComparable<TEvaluation>
    {
        ClimberConfiguration<TState, TEvaluation> ComparesUsing(IComparer<TState> comparisonStrategy);
        ClimberConfiguration<TState, TEvaluation> GeneratesSuccessorsWith(Func<TState, IEnumerable<TState>> successorGenerationFunction);
        ClimberConfiguration<TState, TEvaluation> UsingAlgorithm(IClimberAlgorithm<TState, TEvaluation> algorithm);
        IHillClimber<TState, TEvaluation> Build();
    }
}