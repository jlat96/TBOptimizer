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
        IHillClimber<TState, TEvaluation> Build();
    }
}