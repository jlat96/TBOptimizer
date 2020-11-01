using System;
using TBOptimizer.Types;

namespace TrailBlazer.TBOptimizer.State
{
    /// <summary>
    /// Represents an evaluable that will lazily produce an evaluation.
    /// </summary>
    /// <typeparam name="TState">The type of the state being evaluated</typeparam>
    /// <typeparam name="TEvaluation">The return type of the evaluation, must be IComparable</typeparam>
    public abstract class EvaluableState<TEvaluation> : IEvaluable<TEvaluation>
        where TEvaluation : IComparable<TEvaluation>
    {
        private Lazy<TEvaluation> _evaluation;

        protected EvaluableState()
        {
            _evaluation = new Lazy<TEvaluation>(() => Evaluate());
        }

        /// <summary>
        /// The evaluation score for this state. If the evaluation has not yet been completed, it will be computed and stored.
        /// </summary>
        protected TEvaluation Evaluation {
            get => _evaluation.Value;
        }

        public int CompareTo(IEvaluable<TEvaluation> other)
        {
            return GetEvaluation().CompareTo(other.GetEvaluation());
        }

        /// <summary>
        /// Gets the evaluation of this state. Calculation will occur if the evaluation has not yet been performed.
        /// </summary>
        /// <returns>The evaluation of this state</returns>
        public virtual TEvaluation GetEvaluation()
        {
            return Evaluation;
        }

        /// <summary>
        /// Calculate the evaluation score for this state.
        /// </summary>
        /// <returns></returns>
        protected abstract TEvaluation Evaluate();

        public static bool operator <(EvaluableState<TEvaluation> left, EvaluableState<TEvaluation> right)
        {
            return left.GetEvaluation().CompareTo(right.GetEvaluation()) < 0;
        }

        public static bool operator >(EvaluableState<TEvaluation> left, EvaluableState<TEvaluation> right)
        {
            return left.GetEvaluation().CompareTo(right.GetEvaluation()) > 0;
        }
    }
}
