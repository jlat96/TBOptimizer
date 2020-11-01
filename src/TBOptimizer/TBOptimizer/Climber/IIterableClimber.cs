using System;
using TBOptimizer.Climber.Events;
using TrailBlazer.TBOptimizer.State;

namespace TBOptimizer.Climber
{
    public interface IIterableClimber<TState, TEvaluation>
        where TState : EvaluableState<TEvaluation>
        where TEvaluation : IComparable<TEvaluation>
    {
        EventHandler<ClimberCompleteEvent<TState, TEvaluation>> ClimberCompleteEvent { get; set; }
    }
}
