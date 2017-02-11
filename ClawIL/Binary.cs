using System;
using System.Collections.Generic;

namespace ClawIL
{
    public class Binary
    {
        public BinaryType Type;
        public MetaHeader Meta;
        public List<byte[]> Constants;
        public Dictionary<byte, ModuleSlot> Slots;
        public Dictionary<byte, byte[]> Symbols;

        public Binary()
        {
            Meta = new MetaHeader();
            Constants = new List<byte[]>();
            Slots = new Dictionary<byte, ModuleSlot>();
            Symbols = new Dictionary<byte, byte[]>();
        }

        public bool Populate(IDataMemory Memory)
        {
            // First, check the magic numbers CAFE + (CWX || CWL)
            byte[] magicNums;

            // Read the magic numbers from the beginning of the stream
            if (!Memory.ReadBytes(6, out magicNums))
                return false;
            
            // Now compare them to what they must be
            if (magicNums[0] != 0xca || magicNums[1] != 0xfe || magicNums[2] != 'C' || magicNums[3] != 'W')
                return false;
            
            // Now determine the executable type...
            switch ((char)magicNums[4])
            {
                case 'X':
                    Type = BinaryType.Executable;
                    break;
                case 'L':
                    Type = BinaryType.Library;
                    break;
                default:
                    return false;
            }

            // ...and the processor bitness
            switch ((char)magicNums[5])
            {
                case 'S':
                    Type |= BinaryType.Bits16;
                    break;
                case 'I':
                    Type |= BinaryType.Bits32;
                    break;
                case 'L':
                    Type |= BinaryType.Bits64;
                    break;
                default:
                    return false;
            }

            // Let's retrieve the meta information now
            if (!(Meta = new MetaHeader()).Populate(Memory))
                return false;

            // Now retrieve the number of module slots
            byte mSlotCount;
            if (!Memory.ReadByte(out mSlotCount))
                return false;

            // Check the number of slots
            if (mSlotCount > GlobalConstants.MaxSlots)
                return false;

            // Prepare the slots
            Slots.Clear();

            // Then read the slots in
            for (int mSlotNum = 0; mSlotNum < mSlotCount; mSlotNum++)
            {
                byte key;
                bool isOptional;
                string value;

                // Read in the raw key
                if (!Memory.ReadByte(out key))
                    return false;

                // Now determine the state of bit 5 (Is Slot Optional?)
                isOptional = (bool)((key >> 5) > 0);
                // And restore the key id
                key &= 0x0F;
                // If the key is out of bounds, the file is invalid
                if (key > GlobalConstants.MaxSlot)
                    return false;

                // Now read in the key
                if (!ReadString(Memory, out value))
                    return false;

                // Now add the slot to the dictionary
                Slots.Add(key, new ModuleSlot(value, isOptional));
            }

            // Now read in the number of constants
            byte constCount;
            if (!Memory.ReadByte(out constCount))
                return false;

            // Check the number of constants
            if (constCount > GlobalConstants.MaxConstants)
                return false;

            // Then read the actual constants in
            for (int constNum = 0; constNum < constCount; constNum++)
            {
                ulong size;
                byte[] data;

                // Read in the size
                if (!ReadUNumber(Memory, out size))
                    return false;

                // Read in the data
                if (!Memory.ReadBytes((uint)size, out data))
                    return false;

                // Now add the constant
                Constants.Add(data);
            }

            // First, clear the symbols
            Symbols.Clear();

            // Now read in the number of symbols
            byte symbolCount;
            if (!Memory.ReadByte(out symbolCount))
                return false;

            // Now check the number of symbols
            if (symbolCount > GlobalConstants.MaxSymbols)
                return false;

            // Then read the actual symbols in
            for (int symbolNum = 0; symbolNum < symbolCount; symbolNum++)
            {
                ulong size;
                byte key;
                byte[] value;

                // Read the key in
                if (!Memory.ReadByte(out key))
                    return false;
                
                // Check the key
                if (key > GlobalConstants.MaxSymbol)
                    return false;

                // Read the symbol size
                if (!ReadUNumber(Memory, out size))
                    return false;

                // Then read the symbol data
                if (!Memory.ReadBytes((uint)size, out value))
                    return false;

                // Finally, add the symbol to the dictionary
                Symbols.Add(key, value);
            }

            // We're done! Everything went well :)
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

        private bool ReadUNumber(IDataMemory Memory, out ulong Value)
        {
            BinaryType bits = Type & BinaryType.Bits;
            bool success;
            byte val8;
            ushort val16;
            uint val32;
            ulong val64;

            switch (bits)
            {
                case BinaryType.Bits8:
                    success = Memory.ReadByte(out val8);
                    Value = (ulong)val8;
                    return success;
                case BinaryType.Bits16:
                    success = Memory.ReadUShort(out val16);
                    Value = (ulong)val16;
                    return success;
                case BinaryType.Bits32:
                    success = Memory.ReadUInt(out val32);
                    Value = (ulong)val32;
                    return success;
                case BinaryType.Bits64:
                    success = Memory.ReadULong(out val64);
                    Value = (ulong)val64;
                    return success;
            }

            Value = 0;
            return false;
        }

        private bool ReadNumber(IDataMemory Memory, out long Value)
        {
            BinaryType bits = Type & BinaryType.Bits;
            bool success;
            sbyte val8;
            short val16;
            int val32;
            long val64;

            switch (bits)
            {
                case BinaryType.Bits8:
                    success = Memory.ReadSByte(out val8);
                    Value = (long)val8;
                    return success;
                case BinaryType.Bits16:
                    success = Memory.ReadShort(out val16);
                    Value = (long)val16;
                    return success;
                case BinaryType.Bits32:
                    success = Memory.ReadInt(out val32);
                    Value = (long)val32;
                    return success;
                case BinaryType.Bits64:
                    success = Memory.ReadLong(out val64);
                    Value = (long)val64;
                    return success;
            }

            Value = 0;
            return false;
        }

        private ulong MaxUNumber()
        {
            BinaryType bits = Type & BinaryType.Bits;

            switch (bits) {
            case BinaryType.Bits8:
                return byte.MaxValue;
            case BinaryType.Bits16:
                return ushort.MaxValue;
            case BinaryType.Bits32:
                return uint.MaxValue;
            case BinaryType.Bits64:
                return ulong.MaxValue;
            }

            return 0;
        }

        private long MaxNumber()
        {
            BinaryType bits = Type & BinaryType.Bits;

            switch (bits) {
            case BinaryType.Bits8:
                return (long)sbyte.MaxValue;
            case BinaryType.Bits16:
                return (long)short.MaxValue;
            case BinaryType.Bits32:
                return (long)int.MaxValue;
            case BinaryType.Bits64:
                return long.MaxValue;
            }
                
            return 0;
        }
    }
}

