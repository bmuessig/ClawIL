using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ClawIL
{
    public class StreamMemory : IDataMemory
    {
        MemoryStream stream;
        BinaryReader reader;

        public StreamMemory(MemoryStream Stream)
        {
            this.stream = Stream;
            this.reader = new BinaryReader(stream);
        }

        public bool ReadByte(out byte Output)
        {
            try
            {
                Output = reader.ReadByte();
                return true;
            }
            catch (Exception) { }

            Output = 0;
            return false;
        }

        public bool ReadSByte(out sbyte Output)
        {
            try
            {
                Output = reader.ReadSByte();
                return true;
            }
            catch (Exception) { }

            Output = 0;
            return false;
        }

        public bool ReadShort(out short Output)
        {
            try
            {
                Output = reader.ReadInt16();
                return true;
            }
            catch (Exception) { }

            Output = 0;
            return false;
        }

        public bool ReadUShort(out ushort Output)
        {
            try
            {
                Output = reader.ReadUInt16();
                return true;
            }
            catch (Exception) { }

            Output = 0;
            return false;
        }

        public bool ReadInt(out int Output)
        {
            try
            {
                Output = reader.ReadInt32();
                return true;
            }
            catch (Exception) { }

            Output = 0;
            return false;
        }

        public bool ReadUInt(out uint Output)
        {
            try
            {
                Output = reader.ReadUInt32();
                return true;
            }
            catch (Exception) { }

            Output = 0;
            return false;
        }

        public bool ReadLong(out long Output)
        {
            try
            {
                Output = reader.ReadInt64();
                return true;
            }
            catch (Exception) { }

            Output = 0;
            return false;
        }

        public bool ReadULong(out ulong Output)
        {
            try
            {
                Output = reader.ReadUInt64();
                return true;
            }
            catch (Exception) { }

            Output = 0;
            return false;
        }

        public bool ReadString(uint Length, out string Output)
        {
            StringBuilder builder = new StringBuilder();
            char chr;

            try
            {
                for (; Length > 0; Length-- )
                {
                    if ((chr = reader.ReadChar()) != 0)
                        builder.Append(chr);
                    else
                        break;
                }

                Output = builder.ToString();
                return true;
            }
            catch (Exception) { }

            Output = String.Empty;
            return false;
        }

        public bool ReadString(out string Output)
        {
            StringBuilder builder = new StringBuilder();
            char chr;

            try
            {
                while (true)
                {
                    if ((chr = reader.ReadChar()) != 0)
                        builder.Append(chr);
                    else
                        break;
                }

                Output = builder.ToString();
                return true;
            }
            catch (Exception) { }

            Output = String.Empty;
            return false;
        }

        public bool ReadBytes(uint Count, out byte[] Output)
        {
            try
            {
                Output = reader.ReadBytes((int)Count);
                return true;
            }
            catch { }

            Output = new byte[0];
            return false;
        }

        public bool Seek(long Offset)
        {
            return SeekTo((ulong)((long)Position + Offset));
        }

        public bool SeekTo(ulong Position)
        {
            if (stream != null)
            {
                try
                {
                    Position = (ulong)stream.Position;
                }
                catch (Exception)
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        public ulong Position
        {
            get
            {
                if (stream != null)
                {
                    return (ulong)stream.Position;
                }

                return ulong.MaxValue;
            }
        }

        public void Close()
        {
            if (stream != null)
            {
                try
                {
                    stream.Close();
                }
                catch (Exception) { }
            }
        }

        public void Reset()
        {
            this.SeekTo(0);
        }
    }
}
