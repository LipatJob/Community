using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS102L_MP.Lib.Concrete
{
    class LinkedQueue<T> : IQueue<T>
    {
        public LinkedQueue()
        {
            this.list = new DoublyLinkedList<T>();
        }
        public LinkedQueue(ILinkedList<T> list)
        {
            this.list = new DoublyLinkedList<T>();
            foreach (var item in list)
            {
                Enqueue(item);
            }
        }

        private ILinkedList<T> list;
        public int Count => list.Count;

        public bool IsEmpty => list.IsEmpty;

        public bool Contains(T element)
        {
            return list.Contains(element);
        }

        public T Dequeue()
        {
            T item = list.GetStartValue();
            list.DeleteStart();
            return item;
        }

        public void Enqueue(T item)
        {
            list.InsertEnd(item);
        }

        public T Front()
        {
            return list.GetStartValue();
        }

        public IStack<T> ToStack()
        {
            return new LinkedStack<T>(this.list);
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
