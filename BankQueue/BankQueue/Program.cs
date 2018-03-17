using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kattis.IO;

namespace BankQueue
{

    class Customer : IComparable
    {
        public int Cash { get; private set; }
        public int Time { get; private set; }

        public Customer(int c, int t)
        {
            Cash = c;
            Time = t;
        }

        public int CompareTo(object obj)
        {
            Customer cus = (Customer)obj;
            if (cus.Time > this.Time)
                return 1;
            else if (cus.Time == this.Time)
            {
                if (cus.Cash > this.Cash)
                    return 1;
                else if (cus.Cash == this.Cash)
                    return 0;
                else
                    return -1;
            }
            else
                return -1;
                
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            List<Customer> customers = new List<Customer>();
            Scanner scanner = new Scanner();
            int opt = 0;
            int N = scanner.NextInt();
            int T = scanner.NextInt();

            for(int i = 0; i < N; i++)
            {
                int c = scanner.NextInt();
                int t = scanner.NextInt();
                customers.Add(new Customer(c, t));
            }

            customers.Sort();

            int j = customers.Count - 1;
            for(int i = 1; i <= T; i++)
            {
                int current = 0;
                while (j >= 0)
                {
                    if (customers[j].Time > T-i && customers[j].Cash > current)
                    {
                        current = customers[j].Cash;
                        j = FindNextTime(customers, j);
                    }
                    else if (customers[j].Time == T-i)
                    {
                        if (customers[j].Cash > current)
                            current = customers[j].Cash;
                        j = FindNextTime(customers, j);
                        break;
                    }
                    else
                }
            }
        }

        static int FindNextTime(List<Customer> customers, int index)
        {
            Customer c1 = customers[index];
            while (index > 0)
            {
                if (customers[index - 1].Time < c1.Time)
                    return index - 1;
                else
                {
                    c1 = customers[index - 1];
                    index--;
                }
            }
            return -1;
        }
    }
}
