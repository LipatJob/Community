using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS102L_MP.Lib.Concrete
{
    class TreeMap2<TKey, TValue> : IMap<TKey, TValue>, IEnumerable<Tuple<TKey, TValue>>
    {
        public IComparer<TKey> comparer;

        private class PairComparer : Comparer<Tuple<TKey, TValue>>
        {
            IComparer<TKey> comparer;
            public PairComparer()
            {
                comparer = Comparer<TKey>.Default;
            }

            public PairComparer(IComparer<TKey> comparer)
            {
                this.comparer = comparer;
            }
            public override int Compare(Tuple<TKey, TValue> x, Tuple<TKey, TValue> y)
            {
                return comparer.Compare(x.Item1, y.Item1);
            }

        }

        public TreeMap2()
        {
            this.comparer = Comparer<TKey>.Default;
            tree = new AVLTree<Tuple<TKey, TValue>>();
            tree.Comparer = new PairComparer(comparer);
        }

        public TreeMap2(Comparer<TKey> comparer)
        {
            this.comparer = comparer;
            tree = new AVLTree<Tuple<TKey, TValue>>();
            tree.Comparer = new PairComparer(comparer);
        }

        AVLTree<Tuple<TKey, TValue>> tree;

        public TValue this[TKey key] { get => tree.Retrieve(key, ContainsComparer).Item2; set => tree.Insert(Tuple.Create(key, value)); }

        public int Count => tree.Count;

        public bool IsEmpty => tree.IsEmpty;

        public bool Contains(TKey element)
        {
            return tree.Retrieve(element, ContainsComparer) != null;
        }

        private int ContainsComparer(TKey key, Tuple<TKey, TValue> treeKey)
        {
            return comparer.Compare(key, treeKey.Item1);
        }

        public void Remove(TKey key)
        {
            tree.Remove(key, ContainsComparer);
        }

        public TKey Retrieve<TKey1>(TKey1 key, Func<TKey1, TKey, int> comparer)
        {
            Func<TKey1, Tuple<TKey, TValue>, int> newCmp = delegate (TKey1 key1, Tuple<TKey, TValue> tup) { return comparer(key1, tup.Item1); };
            return tree.Retrieve(key, newCmp).Item1;
        }

        public IEnumerator<Tuple<TKey, TValue>> GetEnumerator()
        {
            return tree.Inorder().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
