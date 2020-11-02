using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CS102L_MP.Lib.Concrete
{
    class PriorityQueue<T> : IQueue<T>
    {
        private Comparer<T> comparer;
        public PriorityQueue()
        {
            values = new T[1];
            comparer = Comparer<T>.Default;
        }

        public PriorityQueue(Comparer<T> comparer)
        {
            values = new T[1];
            this.comparer = comparer;
        }

        // Public Members
        public int Count { get; private set; } = 0;

        public bool IsEmpty => Count == 0;

        public bool Contains(T element)
        {
            foreach (var item in values)
            {
                if (item.Equals(element))
                {
                    return true;
                }
            }
            return false;
        }


        public void Enqueue(T item)
        {
            if (Count >= values.Length) { Grow(); }
            values[Count] = item;
            int i = Count;
            Count++;
            BubbleUp(i);

        }

        public T Dequeue()
        {
            if(Count == 0) { return default(T); }

            if(Count == 1)
            {
                T val = values[0]; 
                values[0] = default(T); 
                Count--; 
                return val; 
            }

            T value = values[0];
            Swap(0, Count - 1);
            values[Count - 1] = default(T);
            Count--;
            BubbleDown(0);

            if(Count < values.Length / 4) { Shrink(); }

            return value;
        }

        public T Front()
        {
            if(Count == 0) { return default(T); }
            return values[0];
        }


        // Private Members 
        private T[] values;

        private int Parent(int index)
        {
            return (index - 1) / 2;
        }

        private int Left(int index)
        {
            return index * 2 + 1;
        }

        private int Right(int index)
        {
            return index * 2 + 2;
        }


        private void BubbleUp(int index)
        {
            int currentIndex = index;

            while (currentIndex != 0 && comparer.Compare(values[currentIndex], values[Parent(currentIndex)]) > 0)
            {
                Swap(Parent(currentIndex), currentIndex);
                currentIndex = Parent(currentIndex);
            }
        }

        private void BubbleDown(int index)
        {
            int smallest = index;

            if (Left(index) < Count && comparer.Compare(values[smallest], values[Left(index)]) > 0)
            {
                smallest = Left(index);
            }

            if (Right(index) < Count && comparer.Compare(values[smallest], values[Right(index)]) > 0)
            {
                smallest = Right(index);
            }

            if (smallest != index)
            {
                Swap(index, smallest);
                BubbleDown(smallest);
            }
        }



        private void Grow()
        {
            var newValues = new T[values.Length * 2];
            for (int i = 0; i < values.Length; i++)
            {
                newValues[i] = values[i];
            }
            values = newValues;
        }

        private void Shrink()
        {
            var newValues = new T[(values.Length / 4) + 1];
            for (int i = 0; i < newValues.Length; i++)
            {
                newValues[i] = values[i];
            }
            values = newValues;
        }


        private void Swap(int index1, int index2)
        {
            T temp = values[index1];
            values[index1] = values[index2];
            values[index2] = temp;
        }

        public IStack<T> ToStack()
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
