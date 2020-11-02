using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobLib
{
    interface Storeable
    {
        /// <summary>
        /// Author: Job Lipat
        /// Date: September 24, 2020
        /// </summary>
        bool SaveState();
        bool RetrieveState();
    }
}
