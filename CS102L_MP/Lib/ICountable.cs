using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS102L_MP.Lib
{
    interface ICountable<T>
    {
        int Count { get; }
        bool IsEmpty { get; }
        bool Contains(T element);
    }
}
