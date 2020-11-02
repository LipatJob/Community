using JobLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lipat_Job_Module2_Assignment3.JobLib
{
    /// <summary>
    /// Author: Job Lipat
    /// Date: September 24, 2020
    /// </summary>
    class SerializedWrapper<T> where T : Serializable<T>
    {
        protected string filename;
        protected string location;
        protected string filedirectory;
        protected T item;
        private Serializer<T> serializer;

        public SerializedWrapper(string filename, string location, T item)
        {
            // Create file if file does not exist
            using (FileStream fileStream = File.Open(filename, FileMode.OpenOrCreate)) { }

            // Set Members
            this.filename = filename;
            this.location = location;
            this.item = item;

            filedirectory = location + filename;
            serializer = item.GetSerializer();
        }

        public bool RetrieveState()
        {
            try
            {
                using (StreamReader fileStream = File.OpenText(filedirectory))
                {
                    string line;
                    while ((line = fileStream.ReadLine()) != null)
                    {
                        item = serializer.Deserialize(line);
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
                using (StreamWriter fileStream = File.CreateText(filedirectory))
                {
                    fileStream.WriteLine(serializer.ToSerializable(item));
                }
            }
            catch (IOException)
            {
                Console.WriteLine("> IOException Occured");
                return false;
            }
            return true;
        }
    }
}
