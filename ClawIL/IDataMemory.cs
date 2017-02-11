using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClawIL
{
    public interface IDataMemory
    {
        bool ReadByte(out byte Output);
        bool ReadSByte(out sbyte Output);
        bool ReadShort(out short Output);
        bool ReadUShort(out ushort Output);
        bool ReadInt(out int Output);
        bool ReadUInt(out uint Output);
        bool ReadLong(out long Output);
        bool ReadULong(out ulong Output);
        bool ReadString(out string Output);
        bool ReadString(uint Length, out string Output);
        bool ReadBytes(uint Count, out byte[] Output);
        bool Seek(long Offset);
        bool SeekTo(ulong Position);
        ulong Position { get; }
        void Close();
        void Reset();
    }
}
