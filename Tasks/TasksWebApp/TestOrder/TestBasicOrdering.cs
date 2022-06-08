using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace TestOrder
{
    public class TestBasicOrdering
    {
        private readonly ITestOutputHelper output;

        public TestBasicOrdering(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void TestOrderNumbers()
        {
            int[] N = { 1, 5, 34, 2, 5, 6, 8, 9, 33, 7};
            for (int i = 0; i < N.Length; i++)
            {
                for (int j = i+1; j < N.Length; j++)
                {
                    if ( N[i] > N[j])
                    {
                        //int t = N[i];
                        //N[i] = N[j];    // swap j with i
                        //N[j] = t;       // swap i with j         
                        // use tuple to swap 
                        (N[j], N[i]) = (N[i], N[j]);
                    }
                    int k = 0;
                    output.WriteLine($"Loop[{i}] {N[k++]} {N[k++]} {N[k++]} {N[k++]} {N[k++]} {N[k++]} {N[k++]} {N[k++]} {N[k++]} {N[k++]} ");
                }
            }

        }

        public class Anything
        {
            public string Name { get; set; } = String.Empty;
            public int Order { get; set; }

        }

        [Fact]
        public void TestOrderAnything()
        {
            var stuff = new List<Anything> {
                new Anything{ Name = "meh", Order = 3 },
                new Anything{ Name = "more meh", Order = 4 },
                new Anything{ Name = "high priority", Order = 10 },
                new Anything { Name = "low priority", Order = 1 },
                new Anything{ Name = "medium", Order = 5 },
                new Anything{ Name = "kinda important", Order = 6 },
                };

            output.WriteLine("Not ordered... Yet!");
            foreach (var s in stuff)
            {
                output.WriteLine($"{s.Name} == {s.Order}");
            }

            Order(stuff);

            output.WriteLine("\nOrdered Correctly!");
            foreach (var s in stuff)
            {
                output.WriteLine($"{s.Name} == {s.Order}");
            }
        }

        private void Order(List<Anything> stuff)
        {
            for (int i = 0; i < stuff.Count; i++)
            {
                for (int j = i + 1; j < stuff.Count; j++)
                {
                    if (stuff[i].Order > stuff[j].Order)
                    {
                        Swap(stuff, i, j);
                    }

                }
            }


        }
        private static void Swap<T>(List<T> list, int left, int right)
        {
            //T temp;
            //temp = list[left];
            (list[left], list[right]) = (list[right], list[left]);
            //list[left] = list[right];
            //list[right] = temp;
        }


    }
}