using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace CS102L_MP.Lib.Concrete
{
    class LinkedStack<T> : IStack<T>
    {
        public LinkedStack()
        {
            this.list = new SinglyLinkedList<T>();
        }
        public LinkedStack(ILinkedList<T> list)
        {
            this.list = new SinglyLinkedList<T>();
            foreach (var item in list)
            {
                Push(item);
            }
        }
        public int Count => list.Count;

        public bool IsEmpty => list.IsEmpty;

        public bool Contains(T element)
        {
            return list.Contains(element);
        }

        public T Peek()
        {
            return list.GetStartValue();
        }

        public T Pop()
        {
            T value = list.GetStartValue();
            list.DeleteStart();
            return value;
        }

        public void Push(T item)
        {
            list.InsertStart(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IQueue<T> ToQueue()
        {
            return new LinkedQueue<T>(this.list);
        }

        private ILinkedList<T> list;
    }
}
