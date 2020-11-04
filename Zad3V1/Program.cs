using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;
using Parser;

namespace Zad3V1
{
    class Program
    {
        static void Main(string[] args)
        {
            //var sw = new Stopwatch();
            //sw.Start();
            using var sr = new StreamReader(args[0]);
            var text = sr.ReadToEnd();
            //Console.WriteLine(text);
            MethodsParser ps = new MethodsParser();
            Dictionary<int, string> verticies;
            int[][] edges;
            (verticies,edges)=ps.ParseText(text);
            //kopiec ...
            //int[][] distance = new int[verticies.Count][];
            //for (int i = 0; i < distance.Length; i++)
            //{
            //    distance[i] = new int[verticies.Count];
            //    for (int j = 0; j < distance.Length; j++)
            //    {
            //        if(i!=j)
            //            distance[i][j] = Int32.MaxValue;
            //    }
            //}

            Stopwatch sw = new Stopwatch();
            Console.WriteLine("Parsed and I'm starting counting time");
            sw.Start();
            ///inicjowanie zmiennych
            int[][] previous = new int[verticies.Count][];
            int[][] valueOfDistFromSourceVertex = new int[verticies.Count][];
            for (int i = 0; i < previous.Length; i++)
            {
                valueOfDistFromSourceVertex[i] = new int[valueOfDistFromSourceVertex.Length];
                previous[i] = new int[previous.Length];
                for (int j = 0; j < previous.Length; j++)
                {
                    previous[i][j] = -1;
                }

            }

            ///Djkstra
            Djkstra(edges, verticies, previous, valueOfDistFromSourceVertex);


            ///znajdywanie najmniejszego
            int[] max = new int[valueOfDistFromSourceVertex.Length];
            for (int i = 0; i < valueOfDistFromSourceVertex.Length; i++)
            {
                max[i] = valueOfDistFromSourceVertex[i].Max();
                
            }
            int smallestIndex=0;
            int smallestValue=Int32.MaxValue;
            for (int i = 0; i < max.Length; i++)
            {
                if (max[i]<smallestValue)
                {
                    smallestValue = max[i];
                    smallestIndex = i;
                }
            }
            sw.Stop();
            Console.WriteLine("Time:{0} \nThe best vertex: {1}",sw.Elapsed, verticies[smallestIndex]);
            //for (int i = 0; i < valueOfDistFromSourceVertex.Length; i++)
            //{
            //    Console.WriteLine(i+":");
            //    for (int j = 0; j < valueOfDistFromSourceVertex[i].Length; j++)
            //    {
            //        Console.Write(j+": "+valueOfDistFromSourceVertex[i][j]+", ");
            //    }
            //    Console.WriteLine();
            //}
        }

        private async static void Djkstra(int[][] edges, Dictionary<int, string> verticies, int[][] previous, int[][] valueOfDistFromSourceVertex)
        {
            List<Task> tasks = new List<Task>();

            
            for (int i = 0; i < verticies.Count; i++)
            {
                tasks.Add(taskToDo(i,edges, verticies, previous, valueOfDistFromSourceVertex));


            }
            await Task.WhenAll(tasks);
        }

        private async static Task taskToDo(int i, int[][] edges, Dictionary<int, string> verticies, int[][] previous, int[][] valueOfDistFromSourceVertex)
        {
            int amount = verticies.Count;
            int[] dist = new int[amount];
            bool[] sptSet = new bool[amount];
            for (int j= 0; j < amount; j++)
            {
                dist[j] = int.MaxValue;
                sptSet[j] = false;
            }
            dist[i] = 0;
            for (int count = 0; count < amount - 1; count++)
            {
               int u = minDistance(dist, sptSet, amount);
                sptSet[u] = true;

             
                for (int v = 0; v < amount; v++)

                    // Update dist[v] only if is not in 
                    // sptSet, there is an edge from u 
                    // to v, and total weight of path 
                    // from src to v through u is smaller 
                    // than current value of dist[v] 
                    if (!sptSet[v] && edges[u][v] != 0 &&
                         dist[u] != int.MaxValue && dist[u] + edges[u][v] < dist[v])
                        dist[v] = dist[u] + edges[u][v];
            }
            for (int j = 0; j < dist.Length; j++)
            {
                valueOfDistFromSourceVertex[i][j] = dist[j];
            }

            ///v2 działa ale wolno
            /********
            int deleted, valueOfDelted;
            var Kopiec = new Kopiec(i, verticies.Count);
            while (Kopiec.IsEmpty())
            {
                (deleted, valueOfDelted) = Kopiec.DeleteMin();
                valueOfDistFromSourceVertex[i][deleted] = valueOfDelted;
                for (int j = 0; j < edges.Length; j++)
                {
                    if (edges[j][0] == deleted)
                    {
                        if (Kopiec.ChangeDistanceIfPossible(valueOfDelted, edges[j][1], edges[j][2]))
                            previous[i][edges[j][1]] = edges[j][0];
                    }
                }
            }
            *********/
        }

        private static int minDistance(int[] dist, bool[] sptSet, int amount)
        {
            // Initialize min value 
            int min = int.MaxValue, min_index = -1;

            for (int v = 0; v < amount; v++)
                if (sptSet[v] == false && dist[v] <= min)
                {
                    min = dist[v];
                    min_index = v;
                }

            return min_index;
        }
    }
}
