using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClawIL
{
    public class ClawInstance
    {
        public IHardware Hardware { get; private set; }
        public MemoryPool SharedMemory { get; private set; }
        public IProcessor Processor { get; private set; }
        public Binary Binary { get; private set; }

        public ClawInstance(IHardware Hardware, MemoryPool SharedMemory, IProcessor Processor)
        {
            this.Hardware = Hardware;
            this.SharedMemory = SharedMemory;
            this.Processor = Processor;
        }

        public void Reset()
        {
            Hardware.Reset();
            Processor.Reset();
        }

        public bool Load(string StorageProgramHandle)
        {
            IDataMemory dataMemory;

            if (!Hardware.OpenDataMemory(StorageProgramHandle, out dataMemory))
                return false;
            if (!(Binary = new Binary()).Populate(dataMemory))
                return false;

            dataMemory.Close();

            return true;
        }

        public void ExecuteNext()
        {
            //Processor.ExecuteNext(dataMemory);
        }

        public Exception TryExecuteNext()
        {
            return null;
            //return Processor.TryExecuteNext(dataMemory);
        }
    }
}
