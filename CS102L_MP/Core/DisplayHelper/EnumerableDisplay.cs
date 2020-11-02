using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS102L_MP
{
    class EnumerableDisplay<T>
    {
        IList<T> items;
        Func<T, string> display;

        public EnumerableDisplay(IEnumerable<T> items, int itemsPerPage, Func<T, string> display)
        {
            this.items = items.ToList();
            ItemsPerPage = itemsPerPage;
            this.display = display;
            CurrentPage = 1;
        }

        public void Display()
        {
            for (int i = CurrentPage; i < items.Count && i < CurrentPage + ItemsPerPage; i++)
            {
                display(items[i]);
            }
        }

        public void NextPage()
        {
            if(!HasNextPage)
            {
                throw new IndexOutOfRangeException();
            }

            CurrentPage++;
        }

        public void PreviousPage()
        {
            if (!HasPreviousPage)
            {
                throw new IndexOutOfRangeException();
            }

            CurrentPage--;
        }

        public bool HasNextPage { get { return CurrentPage * ItemsPerPage < items.Count(); } }
        public bool HasPreviousPage { get { return CurrentPage > 1;  } }
        public int ItemsPerPage { get;  }
        public int CurrentPage { get; private set; }
    }
}
