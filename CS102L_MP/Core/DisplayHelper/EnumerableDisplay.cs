using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS102L_MP
{
    class EnumerableDisplay<T>
    {
        public IList<T> items { get; set; }
        public Action<T> display { get; set; }

        public EnumerableDisplay(IEnumerable<T> items, int itemsPerPage, Action<T> display)
        {
            this.items = items.ToList();
            ItemsPerPage = itemsPerPage;
            this.display = display;
            CurrentPage = 0;
        }

        public void Display()
        {
            if(items.Count == 0) { Console.WriteLine("\nLooks like nothing's here :/\n"); }
            for (int i = (CurrentPage * ItemsPerPage); i < items.Count && i < (CurrentPage * ItemsPerPage) + ItemsPerPage; i++)
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

        public bool HasNextPage { get { return CurrentPage < (items.Count()/ItemsPerPage); } }
        public bool HasPreviousPage { get { return CurrentPage > 0;  } }
        public int ItemsPerPage { get;  }
        public int CurrentPage { get; private set; }
    }
}
