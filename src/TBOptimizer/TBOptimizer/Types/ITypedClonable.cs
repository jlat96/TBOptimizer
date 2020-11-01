using System;
using System.Collections.Generic;
using System.Text;

namespace TBOptimizer.Types
{
    public interface ITypedClonable<T>
    {
        T Clone();
    }
}
