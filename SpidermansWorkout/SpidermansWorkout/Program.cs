using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kattis.IO;

namespace SpidermansWorkout
{

    class Program
    {
        class Node
        {
            public string Path { get; private set; }
            public int Max { get; private set; }
            public bool IsPath { get; private set; }

            public Node(string p, int m, bool i)
            {
                Path = p;
                Max = m;
                IsPath = i;
            }
        }


        static Dictionary<int, Node>[] cache;
        static int M;
        static void Main(string[] args)
        {
            int[] distances;
            Scanner scanner = new Scanner();
            int N = scanner.NextInt();
            string[] output = new string[N];
            for (int i = 0; i < N; i++)
            {
                M = scanner.NextInt();
                
                distances = new int[M];
                cache = new Dictionary<int, Node>[M];
                int total = 0;
                for (int j = 0; j < M; j++)
                {
                    cache[j] = new Dictionary<int, Node>();
                    distances[j] = scanner.NextInt();
                    total += distances[j];
                }

                if (M == 1)
                {
                    output[i] = "IMPOSSIBLE";
                    continue;
                }
                output[i] = Calculate(distances, total);
            }

            for (int i = 0; i < N; i++)
                Console.WriteLine(output[i]);
            Console.Read();
        }


        static string Calculate(int[] distances, int total)
        {
            int start = distances[0];
            total -= distances[0];
            Node up, down;
            string rtn = "IMPOSSIBLE";
            up = GoUp(distances, 1, start, total);
            down = GoDown(distances, 1, start, total);

            if (up.IsPath && down.IsPath)
            {
                if (up.Max > down.Max)
                    rtn = "U" + down.Path;
                else
                    rtn = "U" + up.Path;
            }
            else if (up.IsPath)
                rtn = "U" + up.Path;
            else if (down.IsPath)
                rtn = "U" + down.Path;

            return rtn;
        }


        static Node GoUp(int[] distances, int i, int cur, int remain)
        {
            if (i == M-1)
            {
                Node rtn = new Node("U", int.MaxValue, false);

                return rtn;
            }

            cur = cur + distances[i];
            remain = remain - distances[i];

            if (cache[i].ContainsKey(cur))
            {
                Node node;
                cache[i].TryGetValue(cur, out node);
                return new Node("U"+node.Path, node.Max, node.IsPath);
            }

            if (cur <= remain)
            {
                Node up, down, rtn;
                int max;
                up = GoUp(distances, i+1, cur, remain);
                down = GoDown(distances, i+1, cur, remain);

                if (up.IsPath && down.IsPath)
                {
                    if (up.Max > down.Max)
                    {
                        if (cur > down.Max)
                            max = cur;
                        else
                            max = down.Max;
                        rtn = new Node(down.Path, max, true);
                    }
                    else
                    {
                        if (cur > up.Max)
                            max = cur;
                        else
                            max = up.Max;
                        rtn = new Node(up.Path, max, true);
                    }
                }
                else if (up.IsPath)
                {
                    if (cur > up.Max)
                        max = cur;
                    else
                        max = up.Max;
                    rtn = new Node(up.Path, max, true);
                }
                else
                {
                    if (cur > down.Max)
                        max = cur;
                    else
                        max = down.Max;
                    rtn = new Node(down.Path, max, down.IsPath);
                }

                cache[i].Add(cur, rtn);
                return new Node("U"+rtn.Path, rtn.Max, rtn.IsPath);
            }

            Node badNode = new Node("", int.MaxValue, false);
            cache[i].Add(cur, badNode);

            return badNode;
        }

        static Node GoDown(int[] distances, int i, int cur, int remain)
        {
            if (i == M - 1)
            {
                Node rtn;
                if (cur - distances[i] == 0)
                    rtn = new Node("D", 0, true);
                else
                    rtn = new Node("D", int.MaxValue, false);

                return rtn;
            }

            cur = cur - distances[i];
            remain = remain - distances[i];

            if (cache[i].ContainsKey(cur))
            {
                Node node;
                cache[i].TryGetValue(cur, out node);
                return new Node("D"+node.Path, node.Max, node.IsPath);
            }

            if (cur <= remain && cur >= 0)
            {
                Node up, down, rtn;
                int max;
                up = GoUp(distances, i + 1, cur, remain);
                down = GoDown(distances, i + 1, cur, remain);

                if (up.IsPath && down.IsPath)
                {
                    if (up.Max > down.Max)
                    {
                        if (cur > down.Max)
                            max = cur;
                        else
                            max = down.Max;
                        rtn = new Node(down.Path, max, true);
                    }
                    else
                    {
                        if (cur > up.Max)
                            max = cur;
                        else
                            max = up.Max;
                        rtn = new Node(up.Path, max, true);
                    }
                }
                else if (up.IsPath)
                {
                    if (cur > up.Max)
                        max = cur;
                    else
                        max = up.Max;
                    rtn = new Node(up.Path, max, true);
                }
                else
                {
                    if (cur > down.Max)
                        max = cur;
                    else
                        max = down.Max;
                    rtn = new Node(down.Path, max, down.IsPath);
                }

                cache[i].Add(cur, rtn);
                return new Node("D"+rtn.Path, rtn.Max, rtn.IsPath);
            }

            Node badNode = new Node("", int.MaxValue, false);
            cache[i].Add(cur, badNode);

            return badNode;
        }
    }
}
