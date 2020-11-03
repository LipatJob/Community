using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS102L_MP.Lib
{
    class HashMap<TKey, TValue> : IMap<TKey, TValue>
    {
        private struct Entry
        {
            public int hashcode;
            public int next;
            public TKey Key { get; set; }
            public TValue Value{ get; set; }

        }

        private int[] primeTable = { 11, 19, 37, 73, 109, 163, 251, 367, 557, 823, 1237, 1861, 2777, 4177, 6247, 9371, 14057, 21089, 31627, 47431, 71143, 106721, 160073, 240101, 360163, 540217, 810343, 1215497, 1823231, 2734867, 4102283, 6153409, 9230113, 13845163 };

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

        public bool IsEmpty => throw new NotImplementedException();

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
            foreach (var prime in primeTable)
            {
                if (prime >= min) { return prime; };
            }
            return 100000;
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







    }
}
