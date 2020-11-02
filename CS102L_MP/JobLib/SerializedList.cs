using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;


namespace JobLib
{
    /// <summary>
    ///  Author: Job Lipat
    ///  Date: September 24, 2020
    ///  
    ///  Description:
    ///  A list where the items of the list are to be serialized in one file.
    /// </summary>
    /// <typeparam name="T"> The type of object to serialize</typeparam>
    class SerializedList<T> : List<T>, Storeable
    {
        protected string filename;
        protected string location;

        protected Serializer<T> serializer;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename">Name of the file to be saved</param>
        /// <param name="serializer"> The serializing strategy of the object </param>
        public SerializedList(string filename, Serializer<T> serializer)
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

        /// <summary>
        /// Append an item at the end of the list
        /// </summary>
        /// <param name="item"> item to be appended </param>
        public void Add(T item)
        {
            base.Add(item);
            AppendEntity(item);
        }

        /// <summary>
        /// Retreieve or sets the item at index i
        /// </summary>
        /// <param name="i"> index of the item to be retreieved or set</param>
        /// <returns> returns the item at index i</returns>
        public T this[int i] 
        {
            get { return base[i]; }
            set { base[i] = value; SaveState(); }
        }

        /// <summary>
        /// Removed the item at index i
        /// </summary>
        /// <param name="index"> index of the item to be removed </param>
        public void RemoveAt(int index)
        {
            base.RemoveAt(index);
            RemoveEntity(index);
        }

        /// <summary>
        /// Appends the serialized value of the item at the end of the file
        /// </summary>
        /// <param name="item"> item to be serialied and appended </param>
        /// <returns> returns true when successfuly appeended.  </returns>
        private bool AppendEntity(T item)
        {
            try
            {
                using (StreamWriter streamWriter = File.AppendText(filename))
                {
                    streamWriter.WriteLine(serializer.ToSerializable(item));
                }
            }
            catch (IOException)
            {
                Console.WriteLine("> IOException Occured");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Serializes all the items in the list and puts in in the file.
        /// </summary>
        /// <returns> returns whether the state is successfuly saved </returns>
        public bool SaveState()
        {
            try
            {
                using (StreamWriter fileStream = File.CreateText(filename))
                {
                    foreach (T item in this)
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

        /// <summary>
        /// Gets all the serialized values and puts the deserialized items in the list
        /// </summary>
        /// <returns> Returns whether state has successfuly been retreieved </returns>
        public bool RetrieveState()
        {
            try
            {
                using (StreamReader fileStream = File.OpenText(filename))
                {
                    string line;
                    while ((line = fileStream.ReadLine()) != null)
                    {
                        base.Add(serializer.Deserialize(line));
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

        /// <summary>
        /// Removes the value at specified index
        /// </summary>
        /// <param name="index"> Line of item to be deleted</param>
        /// <returns> Returns whether the item is successfuly removed</returns>
        private bool RemoveEntity(int index)
        {
            bool success = false;
            try
            {
                int current = 0;
                using (StreamWriter fileStream = File.CreateText(filename))
                {
                    foreach (T item in this)
                    {
                        if (current == index) { success = true;  continue; }
                        current++;

                        fileStream.WriteLine(serializer.ToSerializable(item));
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
