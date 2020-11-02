using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS102L_MP.Lib
{
    interface ILinkedList<T> : ICountable<T>, IEnumerable<T>
    {
        void InsertEnd(T value);
        void InsertIndex(T value, int index);
        void InsertStart(T value);

        void DeleteEnd();
        void DeleteIndex(int i);
        void DeleteElement(T element);
        void DeleteStart();

        T GetStartValue();
        T GetIndexValue(int i);
        T GetEndValue();
    }
}
