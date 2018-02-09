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
        static List<City> linearized;
        private class City
        {
            public string Name { private set; get; }
            public int Toll { private set; get; }

            public bool visited;

            public int cost = int.MaxValue;

            private HashSet<City> highways;
            public City(string name, int toll)
            {
                Name = name;
                Toll = toll;
                highways = new HashSet<City>();
            }

            public void AddHighway(ref City city)
            {
                highways.Add(city);
            }

            public int GetCheapest(ref City dest)
            {
                int sourceIndex = linearized.IndexOf(this);
                int destIndex = linearized.IndexOf(dest);
                if (destIndex < sourceIndex)
                    return int.MaxValue;
                else if (destIndex == sourceIndex)
                    return 0;
                else
                {
                    linearized[sourceIndex].cost = 0;
                    HashSet<int> nodesToCheck = new HashSet<int>();
                    nodesToCheck.Add(sourceIndex);
                    for (int i = sourceIndex; i < destIndex; i++)
                    {
                        if (!nodesToCheck.Contains(i))
                            continue;
                        for (int j = sourceIndex + 1; j <= destIndex; j++)
                        {
                            if (linearized[i].highways.Contains(linearized[j]))
                            {
                                int tempCost = linearized[i].cost + linearized[j].Toll;
                                if (tempCost < linearized[j].cost)
                                    linearized[j].cost = tempCost;
                                nodesToCheck.Add(j);
                            }
                        }
                    }
                    int cheapestCost = linearized[destIndex].cost;
                    for (int i = sourceIndex; i <= destIndex; i++)
                    {
                        linearized[i].cost = int.MaxValue;
                    }
                    return cheapestCost;
                }
            }

            public override int GetHashCode()
            {
                return Name.GetHashCode();
            }

            public void Explore()
            {
                visited = true;

                foreach (City node in highways)
                {
                    if (!node.visited)
                    {
                        node.Explore();
                    }
                }
                linearized.Insert(0, this);
            }

        }

        static void Main(string[] args)
        {
            linearized = new List<City>();
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

            Linearize(cities);
            int t = scanner.NextInt();
            for (int i = 0; i < t; i++)
            {
                string tripStart = scanner.Next();
                string tripEnd = scanner.Next();
                City source, dest;
                cities.TryGetValue(tripStart, out source);
                cities.TryGetValue(tripEnd, out dest);
                int cost = source.GetCheapest(ref dest);
                if (cost == int.MaxValue)
                    output = output + "NO\n";
                else
                    output = output + cost.ToString() + "\n";
            }

            Console.WriteLine(output);
        }

        static void Linearize(Dictionary<string, City> cities)
        {
            foreach (City node in cities.Values)
            {
                if (!node.visited)
                    node.Explore();
            }
        }
    }
}
