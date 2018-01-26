using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kattis.IO;

namespace MrAnaga
{
    class Program
    {
        static void Main(string[] args)
        {
            int n, k;
            int numOfAna;
            string word, alphabetized;
            HashSet<string> accepted = new HashSet<string>();
            HashSet<string> rejected = new HashSet<string>();

            Scanner scanner = new Scanner();
            n = scanner.NextInt();
            k = scanner.NextInt();
            numOfAna = n;

            for (int i = 0; i < n; i++)
            {
                word = scanner.Next();
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

            Console.WriteLine(numOfAna);
        }

        private static string Alphabetize(string word)
        {
            char[] letters = word.ToCharArray();
            Array.Sort(letters);
            return new string(letters);
        }
    }
}
