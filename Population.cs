using System;
using System.Collections.Generic;
using System.Text;

namespace Kurs
{
    class Population
    {
        List<Individ> individs = new List<Individ>();

        public void AddIndivid(Individ item)
        {
            individs.Add(item);
        }

        public List<Individ> Individs
        {
            get
            {
                return individs;
            }
        }

        public int Autobreeding(int index)
        {
            int sum = 0;
            int max = 0;

            List<int> similar = new List<int>();
            for (int i = 0; i < individs.Count; i++)
            {
                if (i != index)
                {
                    similar.Add(0);

                    for (int j = 0; j < Math.Min(individs[index].Bins.Count, individs[i].Bins.Count); j++)
                        for (int k = 0; k < individs[index].Bins[j].Things.Count; k++)
                            similar[i] += individs[i].Bins[j].Things.IndexOf(individs[index].Bins[j].Things[k]) >= 0 ? 1 : 0;

                    sum += similar[similar.Count - 1] + 1;

                    if (similar[similar.Count - 1] > max)
                        max = similar[similar.Count - 1];
                }

            }

            for (int i = 0; i < similar.Count; i++)
                similar[i] = max - similar[i] + 1;

            List<double> prob = new List<double>();

            for (int i = 0; i < similar.Count; i++)
                prob.Add((double)similar[i] / sum);

            return Probability(prob);
        }
        
        public int Inbreeding(int index)
        {
            int sum = 0;

            List<int> similar = new List<int>();
            for (int i = 0; i < individs.Count; i++)
            {
                if (i != index)
                {
                    similar.Add(1);

                    for (int j = 0; j < Math.Min(individs[index].Bins.Count, individs[i].Bins.Count); j++)
                        for (int k = 0; k < individs[index].Bins[j].Things.Count; k++)
                            similar[i] += individs[i].Bins[j].Things.IndexOf(individs[index].Bins[j].Things[k]) >= 0 ? 1 : 0;

                    sum += similar[similar.Count - 1];
                }

            }

            List<double> prob = new List<double>();

            for (int i = 0; i < similar.Count; i++)
                prob.Add((double)similar[i] / sum);

            return Probability(prob);
        }

        public int PositiveAssociativeMating(int index)
        {
            int sum = 0;
            int max = 0;

            List<int> similar = new List<int>();
            for (int i = 0; i < individs.Count; i++)
            {
                if (i != index)
                {
                    similar.Add(Math.Abs(individs[i].Bins.Count - individs[index].Bins.Count));
                    sum += similar[similar.Count - 1] + 1;

                    if (similar[similar.Count - 1] > max)
                        max = similar[similar.Count - 1];
                }
            }

            for (int i = 0; i < similar.Count; i++)
                similar[i] = max - similar[i] + 1;


            List<double> prob = new List<double>();

            for (int i = 0; i < similar.Count; i++)
                prob.Add((double)similar[i] / sum);

            return Probability(prob);
        }

        public int NegativeAssociativeMating(int index)
        {
            int sum = 0;

            List<int> similar = new List<int>();

            for (int i = 0; i < individs.Count; i++)
            {
                if (i != index)
                {
                    similar.Add(Math.Abs(individs[i].Bins.Count - individs[index].Bins.Count) + 1);
                    sum += similar[similar.Count - 1];
                }
            }

            List<double> prob = new List<double>();

            for (int i = 0; i < similar.Count; i++)
                prob.Add((double)similar[i] / sum);

            return Probability(prob);
        }

        public int Panmixia(int index)
        {
            Random random = new Random(individs.Count);
            int rand;

            do
                rand = random.Next();
            while (rand == index);

            return rand;
        }

        int Probability(List<double> prob)
        {
            int index = 0;

            Random random = new Random();

            double rand = random.NextDouble();

            while ((rand -= prob[index]) > 0)
                index++;

            return index;
        }
    }
}
