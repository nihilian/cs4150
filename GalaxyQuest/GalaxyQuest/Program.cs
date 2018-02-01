using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kattis.IO;

namespace GalaxyQuest
{
    class Program
    {
        static long dSqr;
        private class Star
        {
            public long X { private set; get; }
            public long Y { private set; get; }
            public Star(long x, long y)
            {
                X = x;
                Y = y;
            }
        }
        static void Main(string[] args)
        {
            Scanner scanner = new Scanner();
            long d = scanner.NextLong();
            long k = scanner.NextLong();
            dSqr = d * d;
            List<Star> stars = new List<Star>();

            for (int i = 0; i < k; i++)
            {
                long x = scanner.NextLong();
                long y = scanner.NextLong();
                stars.Add(new Star(x, y));
            }

            long majorityCount = 0;
            FindMajority(stars, ref majorityCount);
            if (majorityCount == 0)
                Console.WriteLine("NO");
            else
                Console.WriteLine(majorityCount);
            Console.Read();
        }

        private static Star FindMajority(List<Star> stars, ref long count)
        {
            if (stars.Count == 0)
            {
                count = 0;
                return null;
            }
                
            else if (stars.Count == 1)
            {
                count = 1;
                return stars[0];
            }
                
            else
            {
                List<Star> starsPrime = new List<Star>();

                for (int i = 0; i < stars.Count-1; i += 2)
                {
                    if (InSameGalaxy(stars[i], stars[i + 1]))
                    {
                        starsPrime.Add(stars[i]);
                    }    
                }

                Star x = FindMajority(starsPrime, ref count);
                
                if (x == null)
                {
                    if (stars.Count % 2 == 1)
                    {
                        count = 1;
                        for (int i = 0; i < stars.Count-1; i++)
                        {
                            if (InSameGalaxy(stars[i], stars[stars.Count - 1]))
                                count++;
                        }
                        if (count > stars.Count / 2)
                        {
                            return stars[stars.Count - 1];
                        }   
                        else
                        {
                            count = 0;
                            return null;
                        }
                    }
                    else
                    {
                        count = 0;
                        return null;
                    }
                }
                else
                {
                    count = 0;
                    for (int i = 0; i < stars.Count; i++)
                    {
                        if (InSameGalaxy(stars[i], x))
                            count++;
                    }
                    if (count > stars.Count / 2)
                    {
                        return x;
                    }
                    else
                    {
                        count = 0;
                        return null;
                    }
                }
            }
        }

        private static bool InSameGalaxy(Star a, Star b)
        {
            long _dSqr = (a.X - b.X)*(a.X - b.X) + (a.Y - b.Y)*(a.Y - b.Y);
            if (_dSqr > dSqr)
                return false;
            else
                return true;
        }
    }
}
