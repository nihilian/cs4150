using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kattis.IO;

namespace NarrowArtGallery
{
    class Program
    {
        static int n;
        static int[][] gallery;
        static Dictionary<string, int> op;
        static void Main(string[] args)
        {
            int k;

            Scanner scanner = new Scanner();
            n = scanner.NextInt();
            while (n > 0)
            {
                gallery = new int[n][];
                op = new Dictionary<string, int>();
                k = scanner.NextInt();
                for (int i = 0; i < n; i++)
                {
                    gallery[i] = new int[2];
                    gallery[i][0] = scanner.NextInt();
                    gallery[i][1] = scanner.NextInt();
                }

                int nextN = scanner.NextInt();
                Console.WriteLine(maxValue(0, -1, k));
                n = nextN;
            }
            Console.Read();
        }

        static int maxValue(int r, int c, int k)
        {
            if (r == n)
                return 0;

            if (k == 0)
            {
                int val = 0;
                string key = (r+1).ToString() + " " + (c).ToString() + " " + (k).ToString();
                if (op.ContainsKey(key))
                    op.TryGetValue(key, out val);
                else
                {
                    val = maxValue(r + 1, c, k);
                    op.Add(key, val);
                }

                int max = gallery[r][0] + gallery[r][1] + val;
                return max;
            }

            if (k == n - r)
            {
                if (c == -1)
                {
                    int val0 = computeVal0(r, c, k);
                    int val1 = computeVal1(r, c, k);

                    int max0 = gallery[r][0] + val0;
                    int max1 = gallery[r][1] + val1;
                    if (max0 > max1)
                    {
                        return max0;
                    }
                    else
                    {
                        return max1;
                    }
                }
                else if (c == 0)
                {
                    int val0 = computeVal0(r, c, k);
                    int max0 = gallery[r][0] + val0;
                    return max0;
                }
                else
                {
                    int val1= computeVal1(r, c, k);
                    int max1 = gallery[r][1] + val1;
                    return max1;
                }
            }
            else
            {
                if (c == -1)
                {
                    int val0 = computeVal0(r, c, k);
                    int val1 = computeVal1(r, c, k);
                    int val2 = computeVal2(r, c, k);

                    int max0 = gallery[r][0] + val0;
                    int max1 = gallery[r][1] + val1;
                    int max2 = gallery[r][0] + gallery[r][1] + val2;
                    if (max0 >= max1 && max0 >= max2)
                    {
                        return max0;
                    }
                    else if (max1 >= max0 && max1 >= max2)
                    {
                        return max1;
                    }
                    else
                    {
                        return max2;
                    }
                }
                else if (c == 0)
                {
                    int val0 = computeVal0(r, c, k);
                    int val2 = computeVal2(r, c, k);

                    int max0 = gallery[r][0] + val0;
                    int max2 = gallery[r][0] + gallery[r][1] + val2;
                    if (max0 > max2)
                    {
                        return max0;
                    }
                    else
                    {
                        return max2;
                    }
                }
                else
                {
                    int val1 = computeVal1(r, c, k);
                    int val2 = computeVal2(r, c, k);

                    int max1 = gallery[r][1] + val1;
                    int max2 = gallery[r][0] + gallery[r][1] + val2;
                    if (max1 > max2)
                    {
                        return max1;
                    }
                    else
                    {
                        return max2;
                    }
                }
            }
        }

        static int computeVal0(int r, int c, int k)
        {
            int val0;
            string key0 = (r + 1).ToString() + " " + "0" + " " + (k - 1).ToString();
            if (op.ContainsKey(key0))
                op.TryGetValue(key0, out val0);
            else
            {
                val0 = maxValue(r + 1, 0, k - 1);
                op.Add(key0, val0);
            }
            return val0;
        }

        static int computeVal1(int r, int c, int k)
        {
            int val1;
            string key1 = (r + 1).ToString() + " " + "1" + " " + (k - 1).ToString();
            if (op.ContainsKey(key1))
                op.TryGetValue(key1, out val1);
            else
            {
                val1 = maxValue(r + 1, 1, k - 1);
                op.Add(key1, val1);
            }
            return val1;
        }

        static int computeVal2(int r, int c, int k)
        {
            int val2;
            string key2 = (r + 1).ToString() + " " + "-1" + " " + (k).ToString();
            if (op.ContainsKey(key2))
                op.TryGetValue(key2, out val2);
            else
            {
                val2 = maxValue(r + 1, -1, k);
                op.Add(key2, val2);
            }
            return val2;
        }
    }
}
