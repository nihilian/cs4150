using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kattis.IO;
using Priority_Queue;

namespace GetShorty
{
    class Program
    {

        private class IntersectionHeap
        {
            private IComparer<Intersection> comparer;
            private Intersection[] heap;
            private int[] indexLookup;
            public int Size { private set; get; }
            public IntersectionHeap(IComparer<Intersection> comp, int capacity)
            {
                comparer = comp;
                heap = new Intersection[capacity];
                indexLookup = new int[capacity];
                for (int i = 0; i < capacity; i++)
                {
                    heap[i] = null;
                    indexLookup[i] = -1;
                }

                Size = 0;
            }

            public void InsertOrChange(Intersection inter)
            {
                int index = indexLookup[inter.Number];
                if (Size == 0)
                {
                    heap[Size] = inter;
                    indexLookup[inter.Number] = Size;
                    Size++;
                }

                else if (index > -1)
                {
                    if (comparer.Compare(inter, heap[index]) > 0)
                    {
                        heap[index] = inter;
                        BubbleUp(index);
                    }
                }

                else
                {
                    heap[Size] = inter;
                    indexLookup[inter.Number] = Size;
                    BubbleUp(Size);
                    Size++;
                }
            }

            private void BubbleUp(int child)
            {
                int parent = (child - 1) / 2;
                while (parent >= 0)
                {
                    if (comparer.Compare(heap[child], heap[parent]) > 0)
                    {
                        Swap(parent, child);
                        child = parent;
                        parent = (child - 1) / 2;
                    }
                    else
                        break;
                }
            }

            private void BubbleDown(int parent)
            {
                int child1 = parent * 2 + 1;
                int child2 = parent * 2 + 2;
                while (!(child1 > Size - 1))
                {
                    if (comparer.Compare(heap[child1], heap[parent]) > 0)
                    {
                        if (!(child2 > Size-1) && comparer.Compare(heap[child2], heap[child1]) > 0)
                        {
                            Swap(parent, child2);
                            parent = child2;
                            child1 = parent * 2 + 1;
                            child2 = parent * 2 + 2;
                        }
                        else
                        {
                            Swap(parent, child1);
                            parent = child1;
                            child1 = parent * 2 + 1;
                            child2 = parent * 2 + 2;
                        }

                    }
                    else if (!(child2 > Size - 1) && comparer.Compare(heap[child2], heap[parent]) > 0)
                    {
                        Swap(parent, child2);
                        parent = child2;
                        child1 = parent * 2 + 1;
                        child2 = parent * 2 + 2;
                    }
                    else
                        break;
                }
            }

            private void Swap(int x, int y)
            {
                indexLookup[heap[x].Number] = y;
                indexLookup[heap[y].Number] = x;
                Intersection temp = heap[x];
                heap[x] = heap[y];
                heap[y] = temp;
            }

            public Intersection DeleteMax()
            {
                Intersection max = heap[0];
                if (Size == 1)
                {
                    heap[0] = null;
                    indexLookup[max.Number] = -1;
                    Size--;
                    return max;
                }
                indexLookup[heap[0].Number] = 0;
                indexLookup[heap[Size - 1].Number] = -1;
                heap[0] = heap[Size - 1];
                heap[Size - 1] = null;
                Size--;
                BubbleDown(0);
                return max;
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
            IntersectionHeap priorityQueue;
            string output = "";
            IntersectionComp comparer = new IntersectionComp();
            Scanner input = new Scanner();
            int n = input.NextInt();
            int m = input.NextInt();
            while (n * m != 0)
            {
                intersections = new Dictionary<int, Intersection>();
                priorityQueue = new IntersectionHeap(comparer, n);
                for (int i = 0; i < n; i++)
                {
                    Intersection intersection = new Intersection(i);
                    intersections.Add(i, intersection);
                }
                for (int i = 0; i < m; i++)
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
                start.bestFactor = 1f;
                priorityQueue.InsertOrChange(start);
                Dijkstra(priorityQueue);
                Intersection end;
                intersections.TryGetValue(n - 1, out end);
                output = output + end.bestFactor.ToString("0.0000") + "\n";
                n = input.NextInt();
                m = input.NextInt();
            }
            Console.Write(output);
            Console.Read();
        }

        private static void Dijkstra(IntersectionHeap priorityQueue)
        {
            Intersection current;
            while (priorityQueue.Size != 0)
            {
                current = priorityQueue.DeleteMax();
                foreach (Corridor corridor in current.GetCorridors())
                {
                    if (corridor.Factor * current.bestFactor > corridor.NextIntersection.bestFactor)
                    {
                        corridor.NextIntersection.bestFactor = corridor.Factor * current.bestFactor;
                        priorityQueue.InsertOrChange(corridor.NextIntersection);
                    }
                }
            }
        }
    }
}
