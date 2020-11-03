using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS102L_MP.Lib.Concrete
{
    class ListDirectedGraph<T> : IGraph<T>, IEnumerable<T>
    {

        public int Count => adjacencyList.Count;

        public bool IsEmpty => adjacencyList.IsEmpty;

        public IEnumerable<T> Vertices => adjacencyList.Select(e=>e.Item1);

        private class Edge
        {
            public T Destination;
            public int Weight;
        }

        TreeMap<T, ILinkedList<Tuple<T, int>>> adjacencyList;

        public ListDirectedGraph()
        {
            adjacencyList = new TreeMap<T, ILinkedList<Tuple<T, int>>>();
        }
        public void AddEdge(T starting, T destination, int weight)
        {
            adjacencyList[starting].InsertEnd(Tuple.Create(destination, weight));
        }

        public void AddVertex(T key)
        {
            adjacencyList[key] = new SinglyLinkedList<Tuple<T, int>>();
        }

        public IEnumerable<Tuple<T, int>> Neighbors(T key)
        {
            return adjacencyList[key];
        }

        public void RemoveVertex(T key)
        {
            foreach (var itemKey in adjacencyList)
            {
                int index = 0;
                foreach(var item in itemKey.Item2)
                {
                    adjacencyList[itemKey.Item1].DeleteIndex(index);
                    index++;
                }
            }
            adjacencyList.Remove(key);
        }

        public void RemoveEdge(T starting, T destination)
        {
            int index = 0;
            foreach(var neigbor in Neighbors(starting))
            {
                if(neigbor.Item1.Equals(destination))
                {
                    break;
                }
                index++;
            }
            adjacencyList[starting].DeleteIndex(index);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return adjacencyList.Select(e => e.Item1).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public T Retrieve<TKey>(TKey key, Func<TKey, T, int> comparer)
        {
            return adjacencyList.Retrieve(key, comparer);
        }

        public bool Contains(T element)
        {
            throw new NotImplementedException();
        }
    }
}
