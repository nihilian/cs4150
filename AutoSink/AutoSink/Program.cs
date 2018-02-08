using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kattis.IO;

namespace AutoSink
{
    class Program
    {
        private class City
        {
            public string Name { private set; get; }
            public int Toll { private set; get; }

            public int pre, post;

            private List<City> highways;
            public City(string name, int toll)
            {
                Name = name;
                Toll = toll;
                highways = new List<City>();
            }

            public void AddHighway(ref City city)
            {
                highways.Add(city);
            }

            public int ComputePath(ref City dest)
            {
                List<City> revSorted = Linearize(dest.Name);
                if (!revSorted.Contains(dest))
                    return -1;
                int destIndex = revSorted.IndexOf(dest);
                return GetCheapest(revSorted, destIndex);
            }

            private int GetCheapest(List<City> revSorted, int index)
            {
                if (revSorted[index].Name == Name)
                    return 0;

                int cheapestParent = -1;
                for(int i = index; i < revSorted.Count; i++)
                {
                    if(revSorted[i].highways.Contains(revSorted[index]))
                    {
                        int tempCheap = GetCheapest(revSorted, i);
                        if (cheapestParent == -1 || tempCheap < cheapestParent)
                            cheapestParent = tempCheap;
                    }
                }
                return cheapestParent + revSorted[index].Toll;
            }

            private List<City> Linearize(string dest)
            {
                List<City> pre = new List<City>();
                List<City> post = new List<City>();
                pre.Add(this);

                if (Name == dest)
                    return pre;

                foreach (City node in highways)
                {
                    if (!pre.Contains(node))
                    {
                        node.Explore(ref pre, ref post, dest);
                    }
                }

                post.Add(this);
                return post;
            }

            private void Explore(ref List<City> pre, ref List<City> post, string dest)
            {
                pre.Add(this);

                if (Name == dest)
                {
                    post.Add(this);
                    return;
                }
                foreach(City node in this.highways)
                {
                    if (!pre.Contains(node))
                    {
                        node.Explore(ref pre, ref post, dest);
                    }
                }
                post.Add(this);
                return;
            }

        }

        static void Main(string[] args)
        {
            Dictionary<string, City> cities = new Dictionary<string, City>();
            string output = "";
            Scanner scanner = new Scanner();

            int n = scanner.NextInt();
            for (int i = 0; i < n; i++)
            {
                string name = scanner.Next();
                int toll = scanner.NextInt();
                cities.Add(name, new City(name, toll));
            }

            int h = scanner.NextInt();
            for (int i = 0; i < h; i++)
            {
                string source = scanner.Next();
                string destination = scanner.Next();
                City sCity, dCity;
                cities.TryGetValue(source, out sCity);
                cities.TryGetValue(destination, out dCity);
                sCity.AddHighway(ref dCity);
            }


            int t = scanner.NextInt();
            for (int i = 0; i < t; i++)
            {
                string tripStart = scanner.Next();
                string tripEnd = scanner.Next();
                City source, dest;
                cities.TryGetValue(tripStart, out source);
                cities.TryGetValue(tripEnd, out dest);
                int cost = source.ComputePath(ref dest);
                if (cost == -1)
                    output = output + "NO\n";
                else
                    output = output + cost.ToString() + "\n";
            }

            Console.WriteLine(output);
        }   
    }
}
