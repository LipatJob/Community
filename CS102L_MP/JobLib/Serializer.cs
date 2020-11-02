using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobLib
{
    /// <summary>
    /// Author: Job Lipat
    /// Date: September 24, 2020
    /// 
    /// Allows the user to tell how an object should be serialized and deserialized
    /// </summary>
    /// <typeparam name="T">The object to be serialized</typeparam>
    abstract class Serializer<T>
    {
        /// <summary> Converts object ito string to make it serializable <summary>
        public abstract string ToSerializable(T item);

        /// <summary> Converts serialized item into the desired object <summary>
        public abstract T Deserialize(string serializedItem);
    }
}
