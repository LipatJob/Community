using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS102L_MP.Lib.Concrete
{
    class TreeMap<TKey, TValue> : IMap<TKey, TValue>
    {
        public IComparer<TKey> Comparer { get; set; }

        public TreeMap()
        {
            this.Comparer = Comparer<TKey>.Default;
        }

        public TreeMap(IComparer<TKey> comparer)
        {
            this.Comparer = comparer;
        }


        public TValue this[TKey key]
        {
            get { return GetValue(key); }
            set { Insert(key, value); }
        }

        public TValue GetValue(TKey key)
        {
            return GetValue(root, key);
        }

        private TValue GetValue(Node node, TKey key)
        {
            if (node == null) { return default(TValue); }


            if (Comparer.Compare(node.Key, key) > 0)
            {
                return GetValue(node.Right, key);
            }
            else if (Comparer.Compare(node.Key, key) < 0)
            {
                return GetValue(node.Left, key);
            }
            else
            {
                return node.Value;
            }
        }

        public class MapElement
        {
            public TKey Key;
            public TValue Value;

            public MapElement(TKey key, TValue value)
            {
                Key = key;
                Value = value;
            }
        }

        // AVL TREE CODE



        public void Remove(TKey element)
        {
            Delete(root, element);
        }

        public int Count => GetHeight(root);

        public bool IsEmpty => root == null;

        public bool Contains(TKey element)
        {
            return Contains(root, element);
        }


        public IEnumerable<Tuple<TKey, TValue>> Preorder()
        {
            List<Tuple<TKey, TValue>> items = new List<Tuple<TKey, TValue>>();
            Preorder(root, items);
            return items;
        }

        public IEnumerable<Tuple<TKey, TValue>> Inorder()
        {
            List<Tuple<TKey, TValue>> items = new List<Tuple<TKey, TValue>>();
            Inorder(root, items);
            return items;
        }

        public IEnumerable<Tuple<TKey, TValue>> Postorder()
        {
            List<Tuple<TKey, TValue>> items = new List<Tuple<TKey, TValue>>();
            Postorder(root, items);
            return items;
        }


        private void Insert(TKey element, TValue value)
        {
            root = Insert(root, element, value);
        }

        private class Node
        {
            public Node Left;
            public Node Right;
            public TKey Key;
            public TValue Value;
            public int Height;
        }

        private Node root;

        private bool Contains(Node node, TKey element)
        {
            if (node == null) { return false; }


            if (Comparer.Compare(node.Key, element) > 0)
            {
                return Contains(node.Right, element);
            }
            else if (Comparer.Compare(node.Key, element) > 0)
            {
                return Contains(node.Left, element);
            }
            else
            {
                return true;
            }
        }

        private Node Insert(Node node, TKey key, TValue value)
        {
            if (node == null) { return new Node() { Left = null, Right = null, Key = key, Value = value, Height = 0 }; }

            if (Comparer.Compare(key, node.Key) > 0) { node.Right = Insert(node.Right, key, value); }
            else if (Comparer.Compare(key, node.Key) < 0) { node.Left = Insert(node.Left, key, value); }
            else { return node; }

            // Set New Height
            node.Height = Math.Max(GetHeight(node.Left), GetHeight(node.Right)) + 1;

            // Balance Node
            int balanceFactor = GetBalance(node);
            // Case 1: Left-Left Case
            if (balanceFactor > 1 && Comparer.Compare(key, node.Left.Key) < 0) { return RightRotate(node); }
            // Case 2: Right-Right Case
            else if (balanceFactor < -1 && Comparer.Compare(key, node.Right.Key) > 0) { return LeftRotate(node); }
            // Case 3: Left-Right Case
            else if (balanceFactor > 1 && Comparer.Compare(key, node.Left.Key) > 0) { node.Left = LeftRotate(node); return RightRotate(node); }
            // Case 4: Right-Left Case
            else if (balanceFactor < -1 && Comparer.Compare(key, node.Right.Key) < 0) { node.Right = RightRotate(node); return LeftRotate(node); }

            return node;
        }

        private Node Delete(Node node, TKey key)
        {
            if (node == null) { return node; }
            else if (Comparer.Compare(key, node.Key) > 0) { node.Right = Delete(node.Right, key); }
            else if (Comparer.Compare(key, node.Key) < 0) { node.Left = Delete(node.Left, key); }
            else
            {
                if (node.Left == null)
                {
                    return node.Right;
                }
                else if (node.Right == null)
                {
                    return node.Left;
                }

                Node temp = GetMin(node.Right);
                node.Key = temp.Key;
                node.Right = Delete(node.Right, temp.Key);
            }

            if (node == null) { return node; }

            node.Height = GetNewHeight(node);

            int balance = GetBalance(node);
            if (balance > 1 && GetBalance(node.Left) >= 0) { return RightRotate(node); }
            if (balance < -1 && GetBalance(node.Right) <= 0) { return LeftRotate(node); }
            if (balance > 1 && GetBalance(node.Left) < 0) { node.Left = LeftRotate(node.Left); return RightRotate(node); }
            if (balance < -1 && GetBalance(node.Right) > 0) { node.Right = RightRotate(node.Right); return LeftRotate(node); }

            return node;
        }


        private Node LeftRotate(Node node)
        {
            Node pivot = node.Right;
            Node corollary = pivot.Left;

            pivot.Left = node;
            node.Right = corollary;

            node.Height = GetNewHeight(node);
            pivot.Height = GetNewHeight(pivot);

            return pivot;
        }

        private Node RightRotate(Node node)
        {
            Node pivot = node.Left;
            Node corollary = pivot.Right;

            pivot.Right = node;
            node.Left = corollary;

            node.Height = GetNewHeight(node);
            pivot.Height = GetNewHeight(pivot);


            return pivot;
        }


        private void Preorder(Node node, ICollection<Tuple<TKey, TValue>> items)
        {
            if (node == null) { return; }

            items.Add(new Tuple<TKey, TValue>(node.Key, node.Value));
            Preorder(node.Left, items);
            Preorder(node.Right, items);
        }

        private void Inorder(Node node, ICollection<Tuple<TKey, TValue>> items)
        {
            if (node == null) { return; }

            Preorder(node.Left, items);
            items.Add(new Tuple<TKey, TValue>(node.Key, node.Value));
            Preorder(node.Right, items);
        }

        private void Postorder(Node node, ICollection<Tuple<TKey, TValue>> items)
        {
            if (node == null) { return; }

            Preorder(node.Left, items);
            Preorder(node.Right, items);
            items.Add(new Tuple<TKey, TValue>(node.Key, node.Value));
        }

        private int GetHeight(Node node)
        {
            if (node == null) { return 0; }

            return node.Height;
        }

        private int GetNewHeight(Node node)
        {
            return Math.Max(GetHeight(node.Left), GetHeight(node.Right)) + 1;
        }

        private int GetBalance(Node node)
        {
            if (node == null) { return 0; }

            return GetHeight(node.Left) - GetHeight(node.Right);
        }

        private Node GetMin(Node node)
        {
            Node temp = node;
            while (temp.Left != null)
            {
                temp = temp.Left;
            }
            return temp;
        }

        public TKey Retrieve<TKey1>(TKey1 key, Func<TKey1, TKey, int> comparer)
        {
            throw new NotImplementedException();
        }
    }
}
