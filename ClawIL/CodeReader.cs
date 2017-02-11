using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClawIL
{
    public class ProgramReader
    {
        public Binary Binary { get; private set; }
        public Location Location { get; private set; }
        public bool EndOfSymbol
        {
            get
            {
                return (bool)(Location.Address >= Binary.Symbols[Location.Symbol].Length);
            }
        }

        public ProgramReader(Binary Binary, byte EntrySymbol = 0)
        {
            this.Binary = Binary;
            if (!Binary.Symbols.ContainsKey(EntrySymbol))
                throw new ArgumentException("Symbol cannot be found!", "EntrySymbol");
            Location = new Location(EntrySymbol, 0);
        }

        public bool Goto(uint Address, bool Simulate = false)
        {
            if (Binary.Symbols[Location.Symbol].Length < Address)
                return false;
            if(!Simulate)
                Location.Address = Address;
            return true;
        }

        public bool Goto(byte Symbol, bool Simulate = false)
        {
            if (!Binary.Symbols.ContainsKey(Symbol))
                return false;
            if (!Simulate)
            {
                Location.Symbol = Symbol;
                Location.Address = 0;
            }
            return true;
        }

        public bool Goto(byte Symbol, uint Address, bool Simulate = false)
        {
            if (!Binary.Symbols.ContainsKey(Symbol))
                return false;
            if (Binary.Symbols[Symbol].Length < Address)
                return false;
            if (!Simulate)
            {
                Location.Symbol = Symbol;
                Location.Address = Address;
            }
            return true;
        }

        public bool Goto(Location Location, bool Simulate = false)
        {
            return this.Goto(Location.Symbol, Location.Address, Simulate);
        }

        public bool AdjustAddress(int OffsetBytes, bool Simulate = false)
        {
            if (Binary.Symbols[Location.Symbol].Length < Location.Address + OffsetBytes && Location.Address + OffsetBytes >= 0)
                return false;
            if(!Simulate)
                Location.Address = (uint)((int)Location.Address + OffsetBytes);
            return true;
        }

        public bool AdjustAddressNumbers(int OffsetNumbers, bool Simulate = false)
        {
            BinaryType bits = Binary.Type & BinaryType.Bits;

            switch (bits)
            {
                case BinaryType.Bits16:
                    OffsetNumbers *= 2;
                    break;
                case BinaryType.Bits32:
                    OffsetNumbers *= 4;
                    break;
                case BinaryType.Bits64:
                    OffsetNumbers *= 8;
                    break;
            }
            return AdjustAddress(OffsetNumbers, Simulate);
        }

        public bool ReadByte(out byte Byte, bool AdvancePointer = true)
        {
            // Initialize the return variable
            Byte = 0;

            // Check if we can read a byte
            if (!AdjustAddress(1, true))
                return false;

            Byte = Binary.Symbols[Location.Symbol][Location.Address];
            return true;
        }

        public bool ReadNumber(out long Number, bool AdvancePointer = true)
        {
            BinaryType bits = Binary.Type & BinaryType.Bits;
            uint address = Location.Address;
            Number = 0;

            // Check if we can read that much and advance if desired
            if (!AdjustAddressNumbers(1, !AdvancePointer))
                return false;

            switch (bits)
            {
                case BinaryType.Bits16:
                    Number = (long)BitConverter.ToInt16(Binary.Symbols[Location.Symbol], (int)address);
                    return true;
                case BinaryType.Bits32:
                    Number = (long)BitConverter.ToInt32(Binary.Symbols[Location.Symbol], (int)address);
                    return true;
                case BinaryType.Bits64:
                    Number = (long)BitConverter.ToInt64(Binary.Symbols[Location.Symbol], (int)address);
                    return true;
            }

            return false;
        }

        public bool ReadUNumber(out ulong Number, bool AdvancePointer = true)
        {
            BinaryType bits = Binary.Type & BinaryType.Bits;
            uint address = Location.Address;
            Number = 0;

            // Check if we can read that much and advance if desired
            if (!AdjustAddressNumbers(1, !AdvancePointer))
                return false;

            switch (bits)
            {
                case BinaryType.Bits16:
                    Number = (ulong)BitConverter.ToUInt16(Binary.Symbols[Location.Symbol], (int)address);
                    return true;
                case BinaryType.Bits32:
                    Number = (ulong)BitConverter.ToUInt32(Binary.Symbols[Location.Symbol], (int)address);
                    return true;
                case BinaryType.Bits64:
                    Number = (ulong)BitConverter.ToUInt64(Binary.Symbols[Location.Symbol], (int)address);
                    return true;
            }

            return false;
        }
    }
}
