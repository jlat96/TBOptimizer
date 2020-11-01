using System;
using TrailBlazer.TBOptimizer.State;

namespace TBOptimizer.Climber.Events
{
    public interface IClimberEventHandler<TState, TEvaluation>
        where TState : EvaluableState<TEvaluation>
        where TEvaluation : IComparable<TEvaluation>
    {
        void OnClimberStepEvent(object sender, ClimberStepEvent<TState, TEvaluation> e);
    }
}
