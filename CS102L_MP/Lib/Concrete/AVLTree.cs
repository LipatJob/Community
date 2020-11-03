using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CS102L_MP.Lib.Concrete
{
    class AVLTree<T> : ITree<T>, IRetrievable<T>
    {
        public Comparer<T> Comparer { get; set; }

        public AVLTree()
        {
            Comparer = Comparer<T>.Default;
        }

        public AVLTree(Comparer<T> comparer)
        {
            this.Comparer = comparer;
        }


        // Public Members
        public int Count { get { return Inorder().Count(); } }

        public bool IsEmpty => root == null;

        public bool Contains(T element)
        {
            return Contains(root, element);
        }   

        public T Retrieve<TKey>(TKey key, Func<TKey, T, int> comparer)
        {
            return Retrieve(key, comparer, root);
        }

        private T Retrieve<TKey>(TKey key, Func<TKey, T, int> comparer, Node node)
        {
            if(node == null)
            {
                return default(T);
            }
            else if (comparer(key, node.Value) > 0)
            {
                return Retrieve(key, comparer, node.Right);
            }
            else if (comparer(key, node.Value) < 0)
            {
                return Retrieve(key, comparer, node.Left);
            }
            else
            {
                return node.Value;
            }
            
        }

        public void Insert(T element)
        {
            root = Insert(root, element);
        }
        
        public void Remove(T element)
        {
            root = Delete(root, element);
        }

        public void Remove<TKey>(TKey key, Func<TKey, T, int> comparer)
        {
            root = Delete(root, key, comparer);
        }


        public IEnumerable<T> Preorder()
        {
            List<T> items = new List<T>();
            Preorder(root, items);
            return items;
        }

        public IEnumerable<T> Inorder()
        {
            List<T> items = new List<T>();
            Inorder(root, items);
            return items;
        }

        public IEnumerable<T> Postorder()
        {
            List<T> items = new List<T>();
            Postorder(root, items);
            return items;
        }



        // Private Members
        private class Node
        {
            public Node Left;
            public Node Right;
            public T Value;
            public int Height;
        }

        private Node root;


        private bool Contains(Node node, T element)
        {
            if (node == null) { return false; }
            
            
            if(Comparer.Compare(element, node.Value) > 0)
            {
                return Contains(node.Right, element);
            }
            else if(Comparer.Compare(element, node.Value) < 0)
            {
                return Contains(node.Left, element);
            }
            else
            {
                return true;
            }
        }


        private void Preorder(Node node, ICollection<T> items)
        {
            if(node == null) { return; }

            items.Add(node.Value);
            Preorder(node.Left, items);
            Preorder(node.Right, items);
        }

        private void Inorder(Node node, ICollection<T> items)
        {
            if (node == null) { return; }

            Inorder(node.Left, items);
            items.Add(node.Value);
            Inorder(node.Right, items);
        }

        private void Postorder(Node node, ICollection<T> items)
        {
            if (node == null) { return; }

            Postorder(node.Left, items);
            Postorder(node.Right, items);
            items.Add(node.Value);
        }


        private Node Insert(Node node, T value)
        {
            if(node == null) { return new Node() { Left = null, Right = null, Value = value, Height = 0 }; }

                 if (Comparer.Compare(value, node.Value) > 0) { node.Right = Insert(node.Right, value); }
            else if (Comparer.Compare(value, node.Value) < 0) { node.Left  = Insert(node.Left, value); }
            else { return new Node() { Left = node.Left, Right = node.Right, Value = value, Height = GetHeight(node) }; }

            // Set New Height
            node.Height = Math.Max(GetHeight(node.Left), GetHeight(node.Right)) + 1;

            // Balance Node
            int balanceFactor = GetBalance(node);
            // Case 1: Left-Left Case
                 if (balanceFactor > 1  && Comparer.Compare(value, node.Left.Value) < 0) { return RightRotate(node); }
            // Case 2: Right-Right Case
            else if (balanceFactor < -1 && Comparer.Compare(value, node.Right.Value) > 0) { return LeftRotate(node); }
            // Case 3: Left-Right Case
            else if (balanceFactor > 1  && Comparer.Compare(value, node.Left.Value) > 0) { node.Left = LeftRotate(node.Left); return RightRotate(node); }
            // Case 4: Right-Left Case
            else if (balanceFactor < -1 && Comparer.Compare(value, node.Right.Value) < 0) { node.Right = RightRotate(node.Right); return LeftRotate(node); } 

            return node;
        }

        private Node Delete(Node node, T Value)
        {
            if (node == null) { return node; }
            else if (Comparer.Compare(Value, node.Value) > 0) { node.Right = Delete(node.Right, Value); }
            else if (Comparer.Compare(Value, node.Value) < 0) { node.Left = Delete(node.Left, Value); }
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
                node.Value = temp.Value;
                node.Right = Delete(node.Right, temp.Value);
            }

            if (node == null) { return node; }

            node.Height = GetNewHeight(node);

            int balance = GetBalance(node);
            if (balance >  1 && GetBalance(node.Left)  >= 0) { return RightRotate(node); }
            if (balance < -1 && GetBalance(node.Right) <= 0) { return LeftRotate(node); }
            if (balance >  1 && GetBalance(node.Left)  < 0)  { node.Left  = LeftRotate(node.Left);   return RightRotate(node); }
            if (balance < -1 && GetBalance(node.Right) > 0)  { node.Right = RightRotate(node.Right); return LeftRotate(node); }

            return node;
        }

        private Node Delete<TKey>(Node node, TKey Value, Func<TKey, T, int> compare)
        {
            if (node == null) { return node; }
            else if (compare(Value, node.Value) > 0) { node.Right = Delete(node.Right, Value, compare); }
            else if (compare(Value, node.Value) < 0) { node.Left = Delete(node.Left, Value, compare); }
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
                node.Value = temp.Value;
                node.Right = Delete(node.Right, temp.Value);
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

        
    }
}
