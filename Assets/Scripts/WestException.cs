using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    class WestException : Exception
    {
        public WestException(string message): base(message)
        {

        }
    }
}
