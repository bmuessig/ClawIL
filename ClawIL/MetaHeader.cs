using System;
using System.IO;

namespace ClawIL
{
    public class MetaHeader
    {
        public string Title;
        public string Description;
        public string Author;
        public Version Version;

        public Version MinRuntimeVersion;
        public byte MinVarstackSize;
        public byte MinCallstackSize;
        public byte MinPoolSize;

        public MetaHeader()
        {
            Title = "";
            Description = "";
            Author = "";
            Version = new Version(0, 0, 0);
        }

        public MetaHeader(string Title, string Author, Version Version, string Description = "")
        {
            this.Title = Title;
            this.Author = Author;
            this.Version = Version;
            this.Description = Description;
        }

        public bool Populate(IDataMemory Memory)
        {
            if (!ReadString(Memory, out Title))
                return false;
            if (!ReadString(Memory, out Description))
                return false;
            if (!ReadString(Memory, out Author))
                return false;
            if (!ReadVersion(Memory, out Version))
                return false;
            if (!ReadVersion(Memory, out MinRuntimeVersion))
                return false;
            if (!Memory.ReadByte(out MinVarstackSize))
                return false;
            if (!Memory.ReadByte(out MinCallstackSize))
                return false;
            if (!Memory.ReadByte(out MinPoolSize))
                return false;
            return true;
        }

        private bool ReadString(IDataMemory Memory, out string String)
        {
            byte len;
            if (!Memory.ReadByte(out len))
            {
                String = String.Empty;
                return false;
            }

            if (!Memory.ReadString(len, out String))
                return false;

            return true;
        }

        private bool ReadVersion(IDataMemory Memory, out Version Version)
        {
            byte major, minor, rev;
            
            if (!Memory.ReadByte(out major))
            {
                Version = null;
                return false;
            }

            if (!Memory.ReadByte(out minor))
            {
                Version = null;
                return false;
            }

            if (!Memory.ReadByte(out rev))
            {
                Version = null;
                return false;
            }

            Version = new Version(major, minor, rev);
            return true;
        }
    }
}