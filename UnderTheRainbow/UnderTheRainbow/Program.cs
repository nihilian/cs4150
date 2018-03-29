using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kattis.IO;

namespace UnderTheRainbow
{
    class Program
    {
        static void Main(string[] args)
        {
            Scanner scanner = new Scanner();
            int n = scanner.NextInt();
            int[] distance = new int[n + 1];

            for (int i = 0; i <= n; i++)
                distance[i] = scanner.NextInt();

            Console.WriteLine(Penalty(distance));
        }

        private static int Penalty(int[] distance)
        {
            int[] minPenalty = new int[distance.Length];
            int n = distance.Length - 1;
            for (int i=n-1; i >= 0; i--)
            {
                minPenalty[i] = (400 - (distance[i + 1] - distance[i])) * (400 - (distance[i + 1] - distance[i])) + minPenalty[i+1];
                for (int k=i+2; k <= n; k++)
                {
                    int hopPenalty = (400 - (distance[k] - distance[i])) * (400 - (distance[k] - distance[i])) + minPenalty[k];
                    if (hopPenalty < minPenalty[i])
                        minPenalty[i] = hopPenalty;
                }
            }

            return minPenalty[0];
        }

    }
}
