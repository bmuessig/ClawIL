using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ClawIL.Standard
{
    public class ComputerHardware : IHardware
    {    
        public string WorkingDirectory { get; private set; }

        public ComputerHardware(string WorkingDirectory)
        {
            this.WorkingDirectory = WorkingDirectory;
        }

        public bool OpenDataMemory(string Handle, out IDataMemory DataMemory)
        {
            if (File.Exists(Path.Combine(WorkingDirectory, Handle)))
            {
                try {
                    MemoryStream ms = new MemoryStream();
                    FileStream fs = File.OpenRead(Path.Combine(WorkingDirectory, Handle));
                    fs.CopyTo(ms);
                    fs.Close();
                    ms.Position = 0;
                    DataMemory = new StreamMemory(ms);
                    return true;
                } catch(Exception) { }
            }

            DataMemory = null;
            return false;
        }

        public void Reset()
        {

        }
    }
}
