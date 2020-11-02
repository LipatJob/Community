using JobLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipmentBorrowingSystem.JobLib
{
    class SerializedDictionary<TKey, TVal> : Dictionary<TKey, TVal>, Storeable where TVal : Keyed<TKey>
    {
        protected string filename;

        protected Serializer<TVal> serializer;
        public SerializedDictionary(string filename, Serializer<TVal> serializer)
        {
            // Create file if file does not exist
            Directory.CreateDirectory(Path.GetDirectoryName(filename));
            using (FileStream fileStream = File.Open(filename, FileMode.OpenOrCreate)) { }

            // Set members
            this.serializer = serializer;
            this.filename = filename;

            // Retreieve Data from text file
            RetrieveState();
        }

        public TVal this[TKey i]
        {
            get { return base[i]; }
            set { base[i] = value; SaveState(); }
        }

        public void Remove(TKey index)
        {
            base.Remove(index);
            RemoveEntity(index);
        }

        public bool RetrieveState()
        {
            try
            {
                using (StreamReader fileStream = File.OpenText(filename))
                {
                    string line;
                    while ((line = fileStream.ReadLine()) != null)
                    {
                        TVal val = serializer.Deserialize(line);
                        base[val.GetKey()] = val;
                    }
                }
            }
            catch (IOException)
            {
                Console.WriteLine("> IOException Occured");
                return false;
            }
            return true;
        }

        public bool SaveState()
        {
            try
            {
                using (StreamWriter fileStream = File.CreateText(filename))
                {
                    foreach (TVal item in this.Values)
                    {
                        fileStream.WriteLine(serializer.ToSerializable(item));
                    }
                }
            }
            catch (IOException)
            {
                Console.WriteLine("> IOException Occured");
                return false;
            }
            return true;
        }

        private bool RemoveEntity(TKey key)
        {
            bool success = false;
            try
            {
                using (StreamWriter fileStream = File.CreateText(filename))
                {
                    foreach (KeyValuePair<TKey, TVal> item in this)
                    {
                        if (item.Key.Equals(key)) { success = true; continue; }

                        fileStream.WriteLine(serializer.ToSerializable(item.Value));
                    }
                }
            }
            catch (IOException)
            {
                Console.WriteLine("> IOException Occured");
                return false;
            }
            return success;
        }
    }
}
