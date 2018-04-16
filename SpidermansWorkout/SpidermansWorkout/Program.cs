using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kattis.IO;

namespace SpidermansWorkout
{

    class Pair
    {
        public int Cur { get; private set; }
        public int Max { get; private set; }
        public Pair(int c, int m)
        {
            Cur = c;
            Max = m;
        }
    }

    class Program
    {
        static Dictionary<string, Pair>[] cache;
        static int M;
        static int last;
        static void Main(string[] args)
        {
            int[] distances;
            Scanner scanner = new Scanner();
            int N = scanner.NextInt();
            string[] output = new string[N];
            for (int i = 0; i < N; i++)
            {
                M = scanner.NextInt();
                if (M == 1)
                {
                    output[i] = "IMPOSSIBLE";
                    continue;
                }
                distances = new int[M];
                cache = new Dictionary<string, Pair>[M-1];
                int total = 0;
                for (int j = 0; j < M; j++)
                {
                    if (j < M - 1)
                        cache[j] = new Dictionary<string, Pair>();
                    distances[j] = scanner.NextInt();
                    total += distances[j];
                }
                cache[0].Add("U", new Pair(distances[0], distances[0]));
                total -= distances[0];
                last = distances[M - 1];
                output[i] = Calculate(distances, 1, total);
            }

            for (int i = 0; i < N; i++)
                Console.WriteLine(output[i]);
            Console.Read();
        }


        static string Calculate(int[] distances, int i, int total)
        {
            while (i < M-1)
            {
                foreach(string key in cache[i-1].Keys)
                {
                    Pair val;
                    cache[i - 1].TryGetValue(key, out val);

                    if (val.Cur - distances[i] >= 0 && val.Cur - distances[i] <= total - distances[i])
                    {
                        cache[i].Add((key + "D"), new Pair(val.Cur - distances[i], val.Max));
                    }

                    if (val.Cur + distances[i] <= total - distances[i])
                    {
                        int newMax = val.Cur + distances[i];
                        if (newMax < val.Max)
                            newMax = val.Max;

                        cache[i].Add((key + "U"), new Pair(val.Cur + distances[i], newMax));
                    }   
                }
                total -= distances[i];
                i++;
            }

            string rtnString = "IMPOSSIBLE";
            int min = int.MaxValue;

            foreach(string key in cache[M-2].Keys)
            {
                Pair val;
                cache[i - 1].TryGetValue(key, out val);

                if (val.Cur == last && val.Max < min)
                {
                    rtnString = key + "D";
                    min = val.Max;
                }
            }

            return rtnString;
        }
    }
}
