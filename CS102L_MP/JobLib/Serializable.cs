using JobLib;
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
    /// </summary>
    interface Serializable<T>
    {
        Serializer<T> GetSerializer();
    }
}
