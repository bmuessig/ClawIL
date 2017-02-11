using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClawIL
{
    public interface IHardware
    {
        bool OpenDataMemory(string Handle, out IDataMemory DataMemory);
        void Reset();
    }
}
