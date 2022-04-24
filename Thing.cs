using System;
using System.Collections.Generic;
using System.Text;

namespace Kurs
{
    class Thing
    {
        int weight = 0;
        int index;

        public int Weight
        {
            get
            {
                return weight;
            }
        }
        public int Index
        {
            get
            {
                return index;
            }
        }


        public Thing(int w, int i)
        {
            weight = w;
            index = i;
        }

        public int CompareTo(Thing thing)
        {
            return weight.CompareTo(thing.weight);
        }
    }
}
