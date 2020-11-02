using CS102L_MP.Lib;
using CS102L_MP.Lib.Concrete;
using CS102L_MP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CS102L_MP.Lib
{
    static class Algorithms
    {
        private class MSTcmp<T> : Comparer<Tuple<T, int>>
        {
            public override int Compare(Tuple<T, int> x, Tuple<T, int> y)
            {
                return x.Item2.CompareTo(y.Item2);
            }
        }

        public static IEnumerable<Tuple<T, int>> GenerateMST<T> (IGraph<T> graph, T key, Func<T, T, int> comparator)
        {
            var distance = new TreeMap2<T, int>();
            var visited = new AVLTree<T>();
            foreach (var vertex in graph.Vertices)
            {
                distance[vertex] = int.MaxValue;
            }

            PriorityQueue<Tuple<T, int>> pq = new PriorityQueue<Tuple<T, int>>(new MSTcmp<T>());
            distance[key] = 0;
            pq.Enqueue(Tuple.Create(key, 0));

            while ( pq.Count != 0)
            {
                var u = pq.Front().Item1;
                pq.Dequeue();
                visited.Insert(u);

                foreach (var item in graph.Neighbors(u))
                {
                    T v = item.Item1;
                    int weight = distance[u] + comparator(u, v);

                    if (!visited.Contains(v))
                    {
                        if (weight < distance[v])
                        {
                            distance[v] = weight;
                        }
                        pq.Enqueue(Tuple.Create(v, distance[u]));
                    }
                }
            }

            return distance;
        }

        public static int EditDistance(string s, string t)
        {
            if (string.IsNullOrEmpty(s))
            {
                if (string.IsNullOrEmpty(t))
                    return 0;
                return t.Length;
            }

            if (string.IsNullOrEmpty(t))
            {
                return s.Length;
            }

            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            for (int i = 0; i <= n; d[i, 0] = i++) ;
            for (int j = 1; j <= m; d[0, j] = j++) ;

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                    int min1 = d[i - 1, j] + 1;
                    int min2 = d[i, j - 1] + 1;
                    int min3 = d[i - 1, j - 1] + cost;
                    d[i, j] = Math.Min(Math.Min(min1, min2), min3);
                }
            }
            return d[n, m];
        }

        

        public static void QuickSort<T>(IList<T> collection, Func<T, T, int> Comparator)
        {
            Shuffle(collection);
            QuickSort(collection, Comparator, 0, collection.Count - 1);

        }


        static void QuickSort<T>(IList<T> collection, Func<T, T, int> Comparator, int start, int end)
        {
            if (end <= start) { return; }
            int lessThan = start;
            int greaterThan = end;
            
            T pivot = collection[start];
            int i = start + 1;
            while(i <= greaterThan)
            {
                int cmp = Comparator(collection[i], pivot);
                if (cmp < 0 ) { Swap(collection, lessThan++, i++); }
                else if (cmp > 0) { Swap(collection, greaterThan--, i); }
                else { i++; }
            }

            QuickSort(collection, Comparator, start, lessThan-1);
            QuickSort(collection, Comparator, greaterThan+1, end);
        }
        static int Partition<T>(IList<T> list, Func<T, T, int> Comparator, int left, int right)
        {
            T pivot = list[left];

            while(true)
            {
                while (Comparator(list[left], pivot) < 0) { left++;  }
                while (Comparator(list[right], pivot) > 0) { right--; }
                if(left < right)
                {
                    Swap(list, left, right);
                }
                else
                {
                    return right;
                }
            }

        }

        static void Swap<T>(IList<T> list, int i, int j)
        {
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            Random random = new Random();
            int n = list.Count;

            for (int i = list.Count - 1; i > 1; i--)
            {
                int rnd = random.Next(i + 1);

                T value = list[rnd];
                list[rnd] = list[i];
                list[i] = value;
            }
        }

        public static IQueue<UserPost> MergePosts(IList<IStack<UserPost>> posts)
        {
            LinkedQueue<IQueue<UserPost>> queues = new LinkedQueue<IQueue<UserPost>>();

            foreach (var item in posts)
            {
                queues.Enqueue(item.ToQueue());
            }

            var values = MergeQueues(queues);
            return values;
        }



        public static IQueue<UserPost> MergeQueues(IQueue<IQueue<UserPost>> queues)
        {
            while (queues.Count > 1)
            {
                queues.Enqueue(MergeQueue(queues.Dequeue(), queues.Dequeue()));
            }

            return queues.Dequeue();
        }

        public static IQueue<UserPost> MergeQueue(IQueue<UserPost> queue1, IQueue<UserPost> queue2)
        {
     

            LinkedQueue<UserPost> newPosts = new LinkedQueue<UserPost>();

            while(!(queue1.IsEmpty && queue2.IsEmpty))
            {
                if (queue1.IsEmpty) { newPosts.Enqueue(queue2.Dequeue()); continue; }
                else if (queue2.IsEmpty) { newPosts.Enqueue(queue1.Dequeue()); continue; }
                
                else if (queue1.Front().ID == queue2.Front().ID) { newPosts.Enqueue(queue1.Dequeue()); queue2.Dequeue(); }
                else if (queue1.Front().DatePosted > queue2.Front().DatePosted) { newPosts.Enqueue(queue1.Dequeue()); }
                else if (queue2.Front().DatePosted > queue1.Front().DatePosted) { newPosts.Enqueue(queue2.Dequeue()); }
                else { newPosts.Enqueue(queue1.Dequeue()); newPosts.Enqueue(queue2.Dequeue()); }
            }

            return newPosts;
        }

       
    }
}
