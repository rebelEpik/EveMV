using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveMV.Types
{
    public class EveClient
    {
        public IntPtr windowHandle { get; set; }
        public string windowTitle { get; set; }

        public string charName
        {
            get
            {
                return charName;
            }
            set
            {
                charName = windowTitle.Remove(0, 5);
            }
        }

        public int clientID;
    }
}
