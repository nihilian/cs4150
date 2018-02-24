using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kattis.IO;

namespace GetShorty
{
    class Program
    {

        private class IntersectionHeap
        {
            private IComparer<Intersection> comparer;
            private Intersection[] heap;
            public int Size { private set; get; }
            public IntersectionHeap(IComparer<Intersection> comp, int capacity)
            {
                comparer = comp;
                heap = new Intersection[capacity];
                Size = 0;
            }

            public void InsertOrChange(Intersection inter)
            {
                heap[Size] = inter;
                if (Size != 0)
                {
                    int child = Size;
                    int parent = (child - 1) / 2;
                    while (parent >= 0)
                    {
                        if (comparer.Compare(heap[parent], heap[child]) < 0)
                        {
                            Swap(parent, child);
                            child = parent;
                            parent = (child - 1) / 2;
                        }
                        else
                            break;
                    }
                }
                Size++;
            }

            private void Swap(int x, int y)
            {
                Intersection temp = heap[x];
                heap[x] = heap[y];
                heap[y] = temp;
            }

            public Intersection DeleteMax()
            {

            }
        }

        private class Intersection
        {
            public int Number { private set; get; }
            public float bestFactor = -1;
            private List<Corridor> corridors;

            public Intersection(int number)
            {
                Number = number;
                corridors = new List<Corridor>();
            }

            public void AddCorridor(Intersection intersection, float factor)
            {
                Corridor corridor = new Corridor(intersection, factor);
                corridors.Add(corridor);
            }

            public IEnumerable<Corridor> GetCorridors()
            {
                foreach (Corridor corridors in corridors)
                    yield return corridors;
            }

            public override int GetHashCode()
            {
                return Number;
            }
        }

        private class Corridor
        {
            public Intersection NextIntersection { private set; get; }
            public float Factor { private set; get; }

            public Corridor(Intersection intersection, float factor)
            {
                NextIntersection = intersection;
                Factor = factor;
            }
        }

        private class IntersectionComp : IComparer<Intersection>
        {
            int IComparer<Intersection>.Compare(Intersection x, Intersection y)
            {
                if (x.bestFactor < y.bestFactor)
                    return -1;
                else if (x.bestFactor > y.bestFactor)
                    return 1;
                else
                    return 0;
            }
        }

        static void Main(string[] args)
        {
            Dictionary<int, Intersection> intersections;
            SortedSet<Intersection> priorityQueue;
            string output = "";
            IntersectionComp comparer = new IntersectionComp();
            Scanner input = new Scanner();
            int n = input.NextInt();
            int m = input.NextInt();
            while(n != 0 && m != 0)
            {
                intersections = new Dictionary<int, Intersection>();
                priorityQueue = new SortedSet<Intersection>(comparer);
                for (int i = 0; i < n; i++)
                {
                    Intersection intersection = new Intersection(i);
                    intersections.Add(i, intersection);
                }
                for(int i = 0; i < m; i++)
                {
                    int interKey1 = input.NextInt();
                    int interKey2 = input.NextInt();
                    float factor = input.NextFloat();
                    Intersection inter1, inter2;
                    intersections.TryGetValue(interKey1, out inter1);
                    intersections.TryGetValue(interKey2, out inter2);
                    inter1.AddCorridor(inter2, factor);
                    inter2.AddCorridor(inter1, factor);
                }
                Intersection start;
                intersections.TryGetValue(0, out start);
                start.bestFactor = 1;
                priorityQueue.Add(start);
                Dijkstra(priorityQueue);
                Intersection end;
                intersections.TryGetValue(n - 1, out end);
                output = output + end.bestFactor.ToString("0.0000") + "\n";
                n = input.NextInt();
                m = input.NextInt();
            }
            Console.Write(output);
        }

        private static void Dijkstra(SortedSet<Intersection> priorityQueue)
        {
            Intersection current;
            while (priorityQueue.Count != 0)
            {
                current = priorityQueue.Max;
                priorityQueue.Remove(priorityQueue.Max);
                foreach(Corridor corridor in current.GetCorridors())
                {
                    if (corridor.NextIntersection.bestFactor < corridor.Factor * current.bestFactor)
                    {
                        corridor.NextIntersection.bestFactor = corridor.Factor * current.bestFactor;
                        if (priorityQueue.Contains(corridor.NextIntersection))
                            priorityQueue.Remove(corridor.NextIntersection);
                        priorityQueue.Add(corridor.NextIntersection);
                    }
                }
            }
        }
    }
}
