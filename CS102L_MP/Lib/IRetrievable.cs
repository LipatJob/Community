using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS102L_MP.Lib
{
    interface IRetrievable<T>
    {
        T Retrieve<TKey>(TKey key, Func<TKey, T, int> comparer);
    }
}
