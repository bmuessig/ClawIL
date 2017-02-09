using System;
using System.Linq;
using System.Text;

namespace System.Collections.Generic
{
    public class IndexableStack<T> : IEnumerable<T>
    {
        List<T> list;

        public IndexableStack()
        {
            list = new List<T>();
        }

        public T this[uint Offset]
        {
            get
            {
                return this.Peek(Offset);
            }

            set
            {
                if (list.Count > Offset)
                    list[list.Count - (int)Offset - 1] = value;
                else
                    throw new IndexOutOfRangeException();
            }
        }

        public uint Count
        {
            get
            {
                return (uint)list.Count;
            }
        }

        public void Clear()
        {
            list.Clear();
        }

        public void Push(T Element)
        {
            list.Add(Element);
        }

        public T Pop(uint Offset = 0)
        {
            if (list.Count > Offset)
            {
                Offset = (uint)list.Count - Offset - 1;
                T value = list[(int)Offset];
                return value;
            }
            else
                throw new StackUnderflowException();
        }

        public T Peek(uint Offset = 0)
        {
            if (list.Count > Offset)
                return list[list.Count - (int)Offset - 1];
            else
                throw new StackUnderflowException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
