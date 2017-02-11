using System;

namespace ClawIL
{
    public struct ModuleSlot
    {
        public string Name;
        public bool IsOptional;

        public ModuleSlot(string Name, bool IsOptional = false)
        {
            this.Name = Name;
            this.IsOptional = IsOptional;
        }
    }
}

