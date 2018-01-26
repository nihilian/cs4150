using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kattis.IO;

namespace Ceiling
{

    class Node
    {
        private int value;
        private Node lNode, rNode;

        public Node(int _value)
        {
            value = _value;
            lNode = null;
            rNode = null;
        }

        private bool IsLess(int toComp)
        {
            if (toComp < value)
                return true;
            else
                return false;
        }

        private bool Lexist()
        {
            if (lNode != null)
                return true;
            else
                return false;
        }

        private bool Rexist()
        {
            if (rNode != null)
                return true;
            else
                return false;
        }

        public int AddNode(int toAdd, int start)
        {
            if (IsLess(toAdd))
            {
                start = start * 2;
                if (Lexist())
                    start = lNode.AddNode(toAdd, start);
                else
                    lNode = new Node(toAdd);
            }
            else
            {
                start = (start * 2) + 1;
                if (Rexist())
                    start = rNode.AddNode(toAdd, start);
                else
                    rNode = new Node(toAdd);
            }
            return start;
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            HashSet<string> uniqueShapes = new HashSet<string>();
            Scanner scanner = new Scanner();
            int n = scanner.NextInt();
            int k = scanner.NextInt();

            for (int i = 0; i < n; i++)
            {
                List<int> list = new List<int>();
                list.Add(1);
                Node node = new Node(scanner.NextInt());
                for (int j = 1; j < k; j++)
                {
                    list.Add(node.AddNode(scanner.NextInt(), 1));
                }
                list.Sort();
                string shape = "";
                foreach (int val in list)
                    shape = shape + val.ToString();
                if (!uniqueShapes.Contains(shape))
                    uniqueShapes.Add(shape);
            }

            Console.WriteLine(uniqueShapes.Count);
        }
    }
}
