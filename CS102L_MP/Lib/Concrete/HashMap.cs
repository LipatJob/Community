using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace CS102L_MP.Lib
{
    class HashMap<TKey, TValue> : IMap<TKey, TValue>, IEnumerable<Tuple<TKey, TValue>>
    {
        private struct Entry
        {
            public int hashcode;
            public int next;
            public TKey Key { get; set; }
            public TValue Value{ get; set; }

        }

        private static readonly int[] primes = {
            3, 7, 11, 17, 23, 29, 37, 47, 59, 71, 89, 107, 131, 163, 197, 239, 293, 353, 431, 521, 631, 761, 919,
            1103, 1327, 1597, 1931, 2333, 2801, 3371, 4049, 4861, 5839, 7013, 8419, 10103, 12143, 14591,
            17519, 21023, 25229, 30293, 36353, 43627, 52361, 62851, 75431, 90523, 108631, 130363, 156437,
            187751, 225307, 270371, 324449, 389357, 467237, 560689, 672827, 807403, 968897, 1162687, 1395263,
            1674319, 2009191, 2411033, 2893249, 3471899, 4166287, 4999559, 5999471, 7199369};

        private Entry[] entries;
        private int[] buckets;
        private IEqualityComparer<TKey> comparer;
        private int freeCount;
        private int freeList;
        public int count;


        public HashMap() : this(null)
        {

        }
        public HashMap(IEqualityComparer<TKey> comparer)
        {
            count = 0;
            Initialize(0);
            this.comparer = comparer ?? EqualityComparer<TKey>.Default;
        }


        public TValue this[TKey key] 
        { 
            get 
            {
                int i = Retrieve(key);
                if (i >= 0) { return entries[i].Value;  }
                return default(TValue);
            }
            set 
            {
                Insert(key, value, false);
            }
        }

        public int Count { get { return count - freeCount; } }

        public bool IsEmpty => Count == 0;

        public bool Contains(TKey element)
        {
            return Retrieve(element) >= 0;
        }

        public TKey Retrieve<TKey1>(TKey1 key, Func<TKey1, TKey, int> comparer)
        {
            throw new NotImplementedException();
        }

        public void Remove(TKey key)
        {
            if (key == null) { throw new ArgumentNullException(); }

            if(buckets != null)
            {
                int hash = comparer.GetHashCode(key) & 0x7FFFFFFF;
                int bucket = hash % buckets.Length;
                int last = -1;

                for (int i = buckets[bucket]; i >= 0; last = i, i = entries[i].next)
                {
                    if (entries[i].hashcode == hash && comparer.Equals(entries[i].Key, key))
                    {
                        if(last > 0)
                        {
                            buckets[bucket] = entries[i].next;
                        }
                        else
                        {
                            entries[last].next = entries[i].next;
                        }
                        
                        entries[i].hashcode = -1;
                        entries[i].next = freeList;
                        entries[i].Key = default(TKey);
                        entries[i].Value = default(TValue);
                        freeList = i;
                    }
                }
            }
        }

        private void Insert(TKey key, TValue value, bool add)
        {
            if(key == null) { throw new ArgumentNullException(); }

            if(buckets == null) { Initialize(0); }

            int hash = comparer.GetHashCode(key) & 0x7FFFFFFF;
            int targetPosition = hash % buckets.Length;

            // Overrite if value exists
            for (int i = buckets[targetPosition]; i >= 0; i = entries[i].next)
            {
                if (entries[i].hashcode == hash && comparer.Equals(entries[i].Key, key))
                {
                    if (add) { throw new ArgumentException(); }
                    entries[i].Value = value;
                    return;
                }
            }

            // Insert
            int index;
            if(freeCount > 0)
            {
                index = freeList;
                freeList = entries[index].next;
                freeCount--;
            }
            else
            {
                if(count == entries.Length)
                {
                    Resize();
                    targetPosition = hash % buckets.Length;
                }
                index = count;
                count++;
            }

            // insert data
            entries[index].hashcode = hash;
            entries[index].next = buckets[targetPosition];
            entries[index].Key = key;
            entries[index].Value = value;
            buckets[targetPosition] = index;
        }

        private int Retrieve(TKey key)
        {
            if (key == null) { throw new ArgumentNullException(); }

            if (buckets != null)
            {
                int hashCode = comparer.GetHashCode(key) & 0x7FFFFFFF;
                for (int i = buckets[hashCode % buckets.Length]; i >= 0; i = entries[i].next)
                {
                    if (entries[i].hashcode == hashCode && comparer.Equals(entries[i].Key, key)) { return i; };
                }
            }
            return -1;
        }


        private void Initialize(int capacity)
        {
            int size = GetPrime(capacity);
            buckets = new int[size];
            for (int i = 0; i < buckets.Length; i++) buckets[i] = -1;
            entries = new Entry[size];
            freeList = -1;
        }

        private int GetPrime(int min)
        {
            foreach (var prime in primes)
            {
                if (prime >= min) { return prime; };
            }
            return 10000000;
        }

        private void Resize()
        {
            Resize(GetPrime(count * 2), false);
        }

        private void Resize(int newsize, bool rehash)
        {
            // Create Arrays
            int[] newbuckets = new int[newsize];
            Entry[] newentries = new Entry[newsize];
            for (int i = 0; i < newsize; i++) { newbuckets[i] = -1; }
            Array.Copy(entries, 0, newentries, 0, count);
            
            // rehash
            if(rehash)
            {
                for (int i = 0; i < count; i++)
                {
                    if(newentries[i].hashcode != - 1) 
                    {
                        newentries[i].hashcode = comparer.GetHashCode(newentries[i].Key) & 0x7FFFFFFF;
                    }
                }
            }

            // get new place
            for (int i = 0; i < count; i++)
            {
                if(newentries[i].hashcode >= 0)
                {
                    int bucket = newentries[i].hashcode % newsize;
                    newentries[i].next = newbuckets[bucket];
                    newbuckets[bucket] = i;
                }
            }

            buckets = newbuckets;
            entries = newentries;
        }


        public IEnumerator<Tuple<TKey, TValue>> GetEnumerator()
        {
            foreach (var value in entries)
            {
                if(value.Key != null)
                {
                    yield return Tuple.Create(value.Key, value.Value);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
