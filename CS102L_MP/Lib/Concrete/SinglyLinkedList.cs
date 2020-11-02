using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace CS102L_MP.Lib.Concrete
{
    class SinglyLinkedList<T> : ILinkedList<T>
    {
        private class Node
        {
            public T Value;
            public Node Next;
        }

        private Node head;
        public int Count { get; private set; } = 0;
        public bool IsEmpty => Count == 0;

        public SinglyLinkedList()
        {

        }

        public SinglyLinkedList(ILinkedList<T> list)
        {
            foreach (var item in list)
            {
                InsertStart(item);
            }
        }


        public void InsertStart(T value)
        {
            Node temp = head;
            head = new Node { Next = temp, Value = value };
            Count++;
        }

        public void InsertIndex(T value, int index)
        {
            if (index >= Count) { throw new IndexOutOfRangeException(); }
            if (index == 0) { InsertStart(value); return; }

            int currentIndex = 0;
            Node current = head;

            while (currentIndex < index - 1)
            {
                current = current.Next;
                currentIndex++;
            }

            Node newNode = new Node() { Value = value, Next = current.Next };
            current.Next = newNode;

            Count++;
        }

        public void InsertEnd(T value)
        {
            if(head == null)
            {
                InsertStart(value);
                return;
            }
            Node temp = head;
            while (temp.Next != null)
            {
                temp = temp.Next;
            }
            temp.Next = new Node { Next = temp.Next, Value = value };
            Count++;
        }


        public void DeleteStart()
        {
            if (head == null) { throw new IndexOutOfRangeException(); }
            head = head.Next;
            Count--;
        }

        public void DeleteIndex(int index)
        {
            if (head == null) { throw new IndexOutOfRangeException(); }
            if (index >= Count) { throw new IndexOutOfRangeException(); }
            if (index == 0) { DeleteStart(); return; }

            int currentIndex = 0;
            Node current = head;

            while (currentIndex < index - 1)
            {
                current = current.Next;
                currentIndex++;
            }

            current.Next = current.Next.Next;

            Count--;
        }

        public void DeleteElement(T element)
        {
            if (head == null) { throw new IndexOutOfRangeException(); }
            if (element.Equals(head.Value)) { DeleteStart(); return; }

            Node current = head;

            while (current.Next != null && !current.Next.Value.Equals(element))
            {
                current = current.Next;
            }

            if (current.Next == null) { throw new KeyNotFoundException(); }

            current.Next = current.Next.Next;
            Count--;
        }

        public void DeleteEnd()
        {
            if (head == null) { throw new IndexOutOfRangeException(); }

            Node temp = head;
            while (temp.Next != null)
            {
                temp = temp.Next;
            }

            if (temp == head) { DeleteStart(); return; }

            temp.Next = null;
            Count--;
        }


        public T GetStartValue()
        {
            if (head == null) { return default(T); }
            return head.Value;
        }

        public T GetIndexValue(int index)
        {
            if (index >= Count) { throw new IndexOutOfRangeException(); }
            if (index == 0) { return GetStartValue(); }

            int currentIndex = 0;
            Node current = head;

            while (currentIndex < index)
            {
                current = current.Next;
                currentIndex++;
            }

            return current.Value;
        }

        public T GetEndValue()
        {
            if (head == null) { return default(T); }

            Node temp = head;
            while (temp.Next != null)
            {
                temp = temp.Next;
            }
            return temp.Value;
        }


        public bool Contains(T element)
        {
            Node current = head;

            while (current != null)
            {
                if (current.Value.Equals(element))
                {
                    return true;
                }
                current = current.Next;
            }

            return false;
        }

        public IEnumerator<T> GetEnumerator()
        {
            Node current = head;
            while (current != null)
            {
                yield return current.Value;
                current = current.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
