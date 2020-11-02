using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS102L_MP.Lib
{
    interface IGraph<T> : IRetrievable<T>, ICountable<T>
    {
        void AddVertex(T key);
        void AddEdge(T starting, T destination, int weight);

        void RemoveVertex(T key);
        void RemoveEdge(T starting, T destination);

        IEnumerable<T> Vertices { get; }

        IEnumerable<Tuple<T, int>> Neighbors(T key);
    }
}
