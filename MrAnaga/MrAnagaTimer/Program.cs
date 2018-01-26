// Written by Joe Zachary for CS 4150, January 2016

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ArraySearch
{
    /// <summary>
    /// Provides a timing demo.
    /// </summary>
    public class Timer
    {
        public static void Anaga(int n, string[] wordList)
        {
            int numOfAna;
            string word, alphabetized;
            HashSet<string> accepted = new HashSet<string>();
            HashSet<string> rejected = new HashSet<string>();

            numOfAna = n;

            for (int i = 0; i < n; i++)
            {
                word = wordList[i];

                alphabetized = Alphabetize(word);


                if (accepted.Contains(alphabetized))
                {
                    if (rejected.Contains(alphabetized))
                    {
                        numOfAna--;
                    }
                    else
                    {
                        rejected.Add(alphabetized);
                        numOfAna -= 2;
                    }
                }
                else
                {
                    accepted.Add(alphabetized);
                }
            }


        }

        private static string Alphabetize(string word)
        {
            char[] letters = word.ToCharArray();
            Array.Sort(letters);
            return new string(letters);
        }


        /// <summary>
        /// The minimum duration of a timing eperiment (in msecs) in versions 4 and 5
        /// </summary>
        public const int DURATION = 1000;

        /// <summary>
        /// Drives the timing demo.
        /// </summary>
        public static void Main()
        {
            // Let's look at precise the Stopwatch is
            Console.WriteLine("Is high resolution: " + Stopwatch.IsHighResolution);
            Console.WriteLine("Ticks per second: " + Stopwatch.Frequency);
            Console.WriteLine();

            // Now do an experiment.
            Console.Write("Enter choice (1-2): ");
            int choice = Convert.ToInt32(Console.ReadLine());
            RunExperiment(choice);
            //Console.Read();
        }

        /// <summary>
        /// Runs different experiments depending on the value of approach.
        /// </summary>
        public static void RunExperiment(int approach)
        {
            if (approach == 1)
            {
                // Report the average time required to do a binary search for various sizes
                // of arrays, using a different timing measurement.
                int n = 32;
                string[] list = GetRandomList(n, 5);
                Debug.WriteLine("\nN\tTime (msec)\tRatio (msec)");
                double previousTime = 0;
                for (int i = 0; i <= 17; i++)
                {
                    list = CatList(list, GetRandomList(n, 5));
                    n = n * 2;
                    double currentTime = TimeFunction(n, list);
                    Debug.Write((n) + "\t" + currentTime.ToString("G3"));
                    if (i > 0)
                    {
                        Debug.WriteLine("   \t" + (currentTime / previousTime).ToString("G3"));
                    }
                    else
                    {
                        Debug.WriteLine("");
                    }
                    previousTime = currentTime;
                }
            }
            else if (approach == 2)
            {
                int k = 1;
                string[] list = GetRandomList(2000, k);
                Debug.WriteLine("\nK\tTime (msec)\tRatio (msec)");
                double previousTime = 0;
                for (int i = 0; i <= 17; i++)
                {
                    list = GetRandomList(2000, k);
                    k = k * 2;
                    double currentTime = TimeFunction(2000, list);
                    Debug.Write((k - 1) + "\t" + currentTime.ToString("G3"));
                    if (i > 0)
                    {
                        Debug.WriteLine("   \t" + (currentTime / previousTime).ToString("G3"));
                    }
                    else
                    {
                        Debug.WriteLine("");
                    }
                    previousTime = currentTime;
                }
            }
        }

        /// <summary>
        /// Returns the average time required to find an element in an array of
        /// the given size using binary search, assuming that the element actually
        /// appears in the array.  Uses a different timer than Search5.
        /// </summary>
        public static double TimeFunction(int n, string[] list)
        {
            // Warm up
            int[] data = new int[2000];
            for (int i = 0; i < 2000; i++)
            {
                data[i] = i;
            }

            // Get the process
            Process p = Process.GetCurrentProcess();

            // Keep increasing the number of repetitions until one second elapses.
            double elapsed = 0;
            long REPT = 1;
            do
            {
                REPT *= 2;
                TimeSpan start = p.TotalProcessorTime;
                for (long i = 0; i < REPT; i++)
                {
                    Anaga(n, list);
                }
                TimeSpan stop = p.TotalProcessorTime;
                elapsed = stop.TotalMilliseconds - start.TotalMilliseconds;
            } while (elapsed < DURATION);
            double totalAverage = elapsed / REPT;

            // Keep increasing the number of repetitions until one second elapses.
            elapsed = 0;
            REPT = 1;
            do
            {
                REPT *= 2;
                TimeSpan start = p.TotalProcessorTime;
                for (long i = 0; i < REPT; i++)
                {

                }
                TimeSpan stop = p.TotalProcessorTime;
                elapsed = stop.TotalMilliseconds - start.TotalMilliseconds;
            } while (elapsed < DURATION);
            double overheadAverage = elapsed / REPT;

            // Return the difference
            return totalAverage - overheadAverage;
        }


        private static string[] GetRandomList(int n, int k)
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            string[] list = new string[n];
            for (int i = 0; i < n; i++)
            {
                list[i] = RandomString(k, random);
            }
            return list;
        }

        private static string[] CatList(string[] l1, string[] l2)
        {
            string[] list = new string[l1.Length * 2];
            l1.CopyTo(list, 0);
            l2.CopyTo(list, l1.Length);
            return list;
        }



        private static string RandomString(int k, Random random)
        {
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < k; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }
    }
}




