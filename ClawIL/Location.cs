using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClawIL
{
    public class Location
    {
        public byte File { get; set; }
        public byte Symbol { get; set; }
        public uint Address { get; set; }

        public Location(byte Symbol, uint Address, byte File = 0)
        {
            this.File = File;
            this.Symbol = Symbol;
            this.Address = Address;
        }
    }
}
