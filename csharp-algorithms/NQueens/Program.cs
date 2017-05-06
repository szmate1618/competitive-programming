using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace NQueens
{
    class NQueensProblem
    {
        long size;
        long[] pdiags, mdiags;
        long[] nexts, prevs;

        long queensPlaced;

        public NQueensProblem(long N)
        {
            size = N;

            pdiags = new long[2 * size - 1];
            mdiags = new long[2 * size - 1];

            nexts = new long[size + 2];
            prevs = new long[size + 2];
            for (int i = 0; i < size + 2; i++)
            {
                nexts[i] = i + 1;
                prevs[i] = i - 1;
            }

            queensPlaced = 0;
        }

        public void PlaceQueenAt(long r, long c)
        {
            pdiags[r + c]++;
            mdiags[r - c + size - 1]++;

            nexts[prevs[c + 1]] = nexts[c + 1];
            prevs[nexts[c + 1]] = prevs[c + 1];

            queensPlaced++;
        }

        public void RemoveQueenFrom(long r, long c)
        {
            pdiags[r + c]--;
            mdiags[r - c + size - 1]--;

            nexts[prevs[c + 1]] = c + 1;
            prevs[nexts[c + 1]] = c + 1;

            queensPlaced--;
        }

        public long Solve()
        {
            if (queensPlaced == size) return 1;
            else if (nexts[0] - 1 >= size) return 0;

            long solutionsFound = 0;
            long r = queensPlaced;
            long c = nexts[0] - 1;
            while(c < size)
            {
                if (pdiags[r + c] == 0 && mdiags[r - c + size - 1] == 0)
                {
                    PlaceQueenAt(r, c);
                    solutionsFound += Solve();
                    RemoveQueenFrom(r, c);
                }

                c = nexts[c + 1] - 1;
            }
            return solutionsFound;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {

            for (int i = 1; i < 30; i++)
			{
                Stopwatch sw = new Stopwatch();
                sw.Start();

                NQueensProblem p = new NQueensProblem(i);
                Console.WriteLine(p.Solve());

                sw.Stop();
                Console.WriteLine(sw.Elapsed);
                Console.WriteLine();
			}

            Console.WriteLine("Done.");
            Console.ReadLine();
        }
    }
}
