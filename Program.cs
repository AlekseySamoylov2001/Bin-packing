using System;
using System.IO;
using System.Collections.Generic;


namespace Kurs
{
    class Program
    {
        static int weight = 0;
        static List<Thing> things = new List<Thing>();
        static void Main(string[] args)
        {
            Population population = new Population();

            int index = 0;

            try
            {
                StreamReader sr = new StreamReader("test.txt");

                string line = sr.ReadLine();

                weight = Convert.ToInt32(line);

                while ((line = sr.ReadLine()) != null)
                {
                    things.Add(new Thing(Convert.ToInt32(line), index));

                    Console.WriteLine(things[index].Weight.ToString());

                    index++;
                }

                sr.Close();
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }

            for (int i = 0; i < 40; i++)
            {
                population.AddIndivid(RandomIndivid());
            }
            //}
            //Console.Read();
            //Console.WriteLine(NF().Bins.Count);
            //Console.Read();
            //Console.WriteLine(FF().Bins.Count);
            //Console.Read();
            //Console.WriteLine(BF().Bins.Count);
            //Console.Read();
            //Console.WriteLine(NFD().Bins.Count);
            //Console.Read();
            //Console.WriteLine(FFD().Bins.Count);
            //Console.Read();
            //Console.WriteLine(BFD().Bins.Count);
            //Console.Read();
            //Console.WriteLine(DJD().Bins.Count);

            //Population parents = new Population();

            Random random = new Random();

            for (int i = 0; i < 40; i++)
            {
                int parent = random.Next(40);
                population.Inbreeding(parent);

                //скрещивание
            }
        }

        static Individ NF() // каждый следующий предмет в последнюю корзину
        {
            Individ task = new Individ(weight);
            int binIndex = 0; // индекс последней корзинки

            task.AddBin(); // добавляем первую корзинку

            for (int i = 0; i < things.Count; i++)
            {
                if (task.Bins[binIndex].Filled + things[i].Weight > weight)
                {
                    task.AddBin(); // если места на предмет не хватает, то открываем новую корзинку
                    binIndex++;
                }

                task.Bins[binIndex].AddItem(things[i]); // помещаем предмет в корзину
            }

            return task;
        }

        static Individ FF() // каждый следующий предмет в корзину с наименьшим индексом
        {
            Individ task = new Individ(weight);
            int binIndex = -1; // корзинка с наименьшим индексом
            int binCount = 1;

            task.AddBin(); // добавляем первую корзинку

            for (int i = 0; i < things.Count; i++)
            {
                for (int j = 0; j < binCount; j++)
                {
                    if (task.Bins[j].Filled + things[i].Weight < weight) // если предмет поместился в корзинку
                    {
                        binIndex = j;
                        break;
                    }
                }

                if (binIndex == -1) // если ни в одной корзине не хватило места, то открываем новую
                {
                    task.AddBin();
                    binIndex = binCount;
                    binCount++;
                }

                task.Bins[binIndex].AddItem(things[i]); // добавляем предмет в корзину
                binIndex = -1;
            }

            return task;
        }

        static Individ BF() // каждый следующий предмет впомещается в корзину с наибольшей заполненностью
        {
            Individ task = new Individ(weight);
            int binMaxFilled = -1; // индекс корзинки с наибольшей заполненностью
            int binCount = 0;

            for (int i = 0; i < things.Count; i++)
            {
                int minDiff = -1;

                for (int j = 0; j < binCount; j++)
                {
                    int difference = weight - task.Bins[j].Filled - things[i].Weight; // оставшееся место после помещения предмета в корзину

                    if (difference > 0)
                    {
                        if (difference < minDiff || minDiff == -1) // поиск минимальной разницы
                        {
                            minDiff = difference;
                            binMaxFilled = j;
                        }
                    }
                    else if (difference == 0) // если предмет заполнил корзинку полностью, то от дальнейшего поиска нет смысла
                    {
                        binMaxFilled = j;
                        break;
                    }
                }

                if (binMaxFilled == -1) // если ни в одну корзинку предмет не помещается
                {
                    task.AddBin();
                    binMaxFilled = binCount;
                    binCount++;
                }

                task.Bins[binMaxFilled].AddItem(things[i]); // добавляем предмет в корзину
                binMaxFilled = -1;
            }

            return task;
        }

        private static int Compare(Thing x, Thing y)
        {
            return x.CompareTo(y);
        }

        static Individ NFD()
        {
            things.Sort(Compare);

            return NF();
        }

        static Individ FFD()
        {
            things.Sort(Compare);

            return FF();
        }

        static Individ BFD()
        {
            things.Sort(Compare);

            return BF();
        }

        static Individ DJD()
        {
            List<Thing> temp = new List<Thing>(things);
            temp.Sort(Compare);

            Individ task = new Individ(weight);

            while (temp.Count != 0)
            {
                task.AddBin();

                while (task.Bins[task.Bins.Count - 1].Filled < weight / 3 + (weight % 3 == 0 ? 0 : 1))
                {
                    task.Bins[task.Bins.Count - 1].AddItem(temp[0]);

                    temp.RemoveAt(0);
                }

                while (temp.Count >= 3)
                    if (task.Bins[task.Bins.Count - 1].Filled + temp[0].Weight + temp[1].Weight + temp[2].Weight < weight)
                    {
                        task.Bins[task.Bins.Count - 1].AddItem(temp[0]);
                        task.Bins[task.Bins.Count - 1].AddItem(temp[1]);
                        task.Bins[task.Bins.Count - 1].AddItem(temp[2]);

                        temp.RemoveAt(0);
                        temp.RemoveAt(1);
                        temp.RemoveAt(2);
                    }
                    else
                        break;

                List<int> ans = DJDSearch(temp, weight - task.Bins[task.Bins.Count - 1].Filled);

                foreach (int i in ans)
                {
                    task.Bins[task.Bins.Count - 1].AddItem(temp[i]);

                    temp.RemoveAt(i);
                }
            }

            return task;
        }

        static private List<int> DJDSearch(List<Thing> t, int needed)
        {
            List<int> optimum = new List<int>();

            int min = -1;

            int one = -1;
            int two = -1;
            int three = -1;

            for (int i = 0; i < t.Count; i++)
            {
                if (needed > t[i].Weight && (needed - t[i].Weight < min || min == -1))
                {
                    min = needed - t[i].Weight;

                    one = i;

                    if (min == 0)
                    {
                        optimum.Add(i);
                        return optimum;
                    }
                }
            }

            for (int i = 0; i < t.Count - 1; i++)
            {
                for (int j = i + 1; j < t.Count; j++)
                {
                    int weight = t[i].Weight + t[j].Weight;

                    if (needed > weight && (needed - weight < min || min == -1))
                    {
                        min = needed - weight;

                        one = i;
                        two = j;

                        if (min == 0)
                        {
                            optimum.Add(i);
                            optimum.Add(j);
                            return optimum;
                        }
                    }
                }
            }

            for (int i = 0; i < t.Count - 2; i++)
            {
                for (int j = i + 1; j < t.Count - 1; j++)
                {
                    for (int k = j + 1; k < t.Count; k++)
                    {
                        int weight = t[i].Weight + t[j].Weight + t[k].Weight;

                        if (needed > weight && (needed - weight < min || min == -1))
                        {
                            min = needed - weight;

                            one = i;
                            two = j;
                            three = k;

                            if (min == 0)
                            {
                                optimum.Add(i);
                                optimum.Add(j);
                                optimum.Add(k);
                                return optimum;
                            }
                        }
                    }
                }
            }

            if (one != -1)
                optimum.Add(one);
            if (two != -1)
                optimum.Add(two);
            if (three != -1)
                optimum.Add(three);

            return optimum;
        }

        static Individ RandomIndivid()
        {
            Individ task = new Individ(weight);

            task.AddBin();

            List<Thing> temp = new List<Thing>(things);

            while (temp.Count > 0)
            {
                var rand = new Random();

                int index = rand.Next(temp.Count);

                if (task.Bins[task.Bins.Count - 1].Filled + temp[index].Weight > weight)
                    task.AddBin();

                task.Bins[task.Bins.Count - 1].AddItem(temp[index]);
                temp.RemoveAt(index);
            }

            return task;
        }
    }
}
