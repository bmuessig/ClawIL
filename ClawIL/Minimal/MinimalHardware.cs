using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ClawIL.Minimal
{
    public class MinimalHardware : IHardware
    {
        public MemoryStream FlashImage { get; private set; }

        public MinimalHardware(MemoryStream FlashImage)
        {
            this.FlashImage = FlashImage;
        }

        public bool OpenDataMemory(string Handle, out IDataMemory DataMemory)
        {
            DataMemory = new StreamMemory(FlashImage);
            return true;
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
