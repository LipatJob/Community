using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS102L_MP.Lib
{
    interface ITree<T> : ICountable<T>, IRetrievable<T>
    {
        void Insert(T element);
        void Remove(T element);
        void Remove<TKey>(TKey key, Func<TKey, T, int> comparer);

        IEnumerable<T> Inorder();
        IEnumerable<T> Preorder();
        IEnumerable<T> Postorder();
    }
}
