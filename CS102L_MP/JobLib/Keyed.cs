using JobLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipmentBorrowingSystem.JobLib
{
    interface Keyed<TKey>
    {
        TKey GetKey();
    }
}
