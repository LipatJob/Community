using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace CS102L_MP.Lib.Concrete
{
    class DoublyLinkedList<T> : ILinkedList<T>
    {
        private class Node
        {
            public T Value;
            public Node Next;
            public Node Previous;
        }

        private Node head;
        private Node tail;
        public int Count { get; private set; } = 0;
        public bool IsEmpty => head == null && tail == null;

        public DoublyLinkedList()
        {

        }

        public DoublyLinkedList(ILinkedList<T> list)
        {
            foreach (var item in list)
            {
                InsertStart(item);
            }
        }

        private void InsertEmpty(T value)
        {
            Contract.Requires(head == null && tail == null);
            var node = new Node() { Next = null, Value = value };
            head = node;
            tail = node;
            Count++;
        }

        public void InsertStart(T value)
        {
            if(IsEmpty) { InsertEmpty(value); return; }
            
            Node temp = head;
            head = new Node { Next = temp, Previous = null, Value = value };
            temp.Previous = head;
            Count++;
        }

        public void InsertIndex(T value, int index)
        {
            if (index >= Count) { throw new IndexOutOfRangeException(); }
            if (IsEmpty) { InsertEmpty(value); return; }
            if (index == 0) { InsertStart(value); return; }
            if (index == Count - 1) { InsertEnd(value); return; }


            int currentIndex = 0;
            Node current = head;

            while (currentIndex < index)
            {
                current = current.Next;
                currentIndex++;
            }

            Node newNode = new Node() { Value = value, Next = current, Previous = current.Previous };
            current.Previous.Next = newNode;
            current.Previous = newNode;

            Count++;
        }

        public void InsertEnd(T value)
        {
            if (head == null) { InsertStart(value); return; }
            if (IsEmpty) { InsertEmpty(value); return; }

            var node = new Node() {Previous = tail, Next = null, Value = value};
            tail.Next = node;
            tail = node;

            Count++;
        }

        private bool IsLast => head == tail && head != null && tail != null;

        private void DeleteLastElement()
        {
            Contract.Requires(IsLast);

            head = null;
            tail = null;

            Count--;
        }

        public void DeleteStart()
        {
            if (head == null) { throw new IndexOutOfRangeException(); }
            if (IsLast) { DeleteLastElement(); return; }

            head = head.Next;
            head.Previous = null;

            Count--;
        }

        public void DeleteIndex(int index)
        {
            if (head == null) { throw new IndexOutOfRangeException(); }
            if (index >= Count) { throw new IndexOutOfRangeException(); }
            if (index == 0) { DeleteStart(); return; }
            if (index == Count - 1) { DeleteEnd(); return; }

            int currentIndex = 0;
            Node current = head;

            while (currentIndex < index)
            {
                current = current.Next;
                currentIndex++;
            }

            current.Previous.Next = current.Next;
            current.Next.Previous = current.Previous;

            current.Next = null;
            current.Previous = null;

            Count--;
        }

        public void DeleteElement(T element)
        {
            if (head == null) { throw new IndexOutOfRangeException(); }
            if (element.Equals(head.Value)) { DeleteStart(); return; }

            Node current = head;

            while (current != null && !current.Value.Equals(element))
            {
                current = current.Next;
            }

            if (current == null) { throw new KeyNotFoundException(); }

            current.Previous.Next = current.Next;
            current.Next.Previous = current.Previous;

            current.Next = null;
            current.Previous = null;

            Count--;
        }

        public void DeleteEnd()
        {
            if (head == null) { throw new IndexOutOfRangeException(); }
            if (IsLast) { DeleteLastElement(); return; }

            tail = tail.Previous;
            tail.Previous = null;

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
