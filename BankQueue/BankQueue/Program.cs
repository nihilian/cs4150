using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kattis.IO;

namespace BankQueue
{
    class Program
    {
        static void Main(string[] args)
        {
            Scanner scanner = new Scanner();
            int N = scanner.NextInt();
            int T = scanner.NextInt();            

            int[][] canidates = new int[T][];
            int[] counters = new int[T];
            int optSol = 0;
            for (int i = 0; i < T; i++)
            {
                canidates[i] = new int[i + 1];
                counters[i] = i;
            }
                

            for (int i = 0; i < N; i++)
            {
                int c = scanner.NextInt();
                int t = scanner.NextInt();

                if (t >= T)
                    t = T - 1;
                int j = t;
                while (j >= 0)
                {
                    if (canidates[t][j] == 0)
                    {
                        canidates[t][j] = c;
                        break;
                    }
                    else if (canidates[t][j] <= c)
                    {
                        int temp = canidates[t][j];
                        canidates[t][j] = c;
                        c = temp;
                    }
                    j--;
                }
            }

            for (int i = T-1; i >= 0; i--)
            {
                int maxIndex = -1;
                int maxVal = 0;
                for (int j = T-1; j >= i; j--)
                {
                    int k = counters[j];
                    if (k >= 0 && canidates[j][k] > maxVal)
                    {
                        maxVal = canidates[j][k];
                        maxIndex = j;
                    }
                }
                if (maxIndex != -1)
                {
                    optSol += maxVal;
                    counters[maxIndex]--;
                }
            }

            Console.Write(optSol);
            Console.Read();

        }
    }
}
