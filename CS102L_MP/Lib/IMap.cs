using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS102L_MP.Lib
{
    interface IMap<TKey, TValue> : ICountable<TKey>, IRetrievable<TKey>
    {
        TValue this[TKey key] { get; set; }

        void Remove(TKey key);
    }
}
