using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS102L_MP.Lib
{
    interface IStack<T> : ICountable<T>, IEnumerable<T>
    {
        void Push(T item);
        T Pop();
        T Peek();
        IQueue<T> ToQueue();
    }
}
