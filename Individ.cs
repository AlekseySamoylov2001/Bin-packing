using System;
using System.Collections.Generic;
using System.Text;

namespace Kurs
{
    class Individ
    {
        int weight;
        List<Bin> bins = new List<Bin>();

        public Individ(int w = 20)
        {
            weight = w;
        }

        public float Weight
        {
            get
            {
                return weight;
            }
        }

        public List<Bin> Bins
        {
            get
            {
                return bins;
            }
        }

        public void AddBin()
        {
            bins.Add(new Bin());
        }
    }
}
