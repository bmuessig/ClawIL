using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClawIL
{
    public interface IProcessor
    {
        Exception TryExecuteNext(IDataMemory DataMemory);
        void ExecuteNext(IDataMemory DataMemory);
        void Reset();
    }
}
