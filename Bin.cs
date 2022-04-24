using System;
using System.Collections.Generic;
using System.Text;

namespace Kurs
{
    class Bin
    {
        int filled = 0;
        List<Thing> things = new List<Thing>();

        public void AddItem(Thing item)
        {
            things.Add(item);
            filled += item.Weight;
        }

        public int Filled
        {
            get
            {
                return filled;
            }
        }

        public List<Thing> Things
        {
            get
            {
                return things;
            }
        }
    }
}
