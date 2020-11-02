using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS102L_MP.Lib
{
    interface IQueue<T> : ICountable<T>, IEnumerable<T>
    {
        void Enqueue(T item);
        T Dequeue();
        T Front();

        IStack<T> ToStack();

    }
}
