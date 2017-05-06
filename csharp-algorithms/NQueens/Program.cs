using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace NQueens
{
    class EliminationList
    {
        public static void Remove(long[] prevs, long[] nexts, long i)
        {
            nexts[prevs[i]] = nexts[i];
            prevs[nexts[i]] = prevs[i];
        }

        public static void Insert(long[] prevs, long[] nexts, long i)
        {
            nexts[prevs[i]] = i;
            prevs[nexts[i]] = i;
        }
    }

    class NQueensProblem
    {
        long size;
        long[] cols;
        long[] pdiags, mdiags;
        long[][] nexts, prevs;

        long queensPlaced;

        public NQueensProblem(long N)
        {
            size = N;

            cols = new long[size];
            pdiags = new long[2 * size - 1];
            mdiags = new long[2 * size - 1];

            nexts = new long[size][];
            for (int i = 0; i < size; i++)
            {
                nexts[i] = new long[size + 2];
                for (int j = 0; j < size + 2; j++)
                {
                    nexts[i][j] = j + 1;
                }
            }
            prevs = new long[size][];
            for (int i = 0; i < size; i++)
            {
                prevs[i] = new long[size + 2];
                for (int j = 0; j < size + 2; j++)
                {
                    prevs[i][j] = j - 1;
                }
            }

            queensPlaced = 0;
        }

        public void PlaceQueenAt(long r, long c)
        {
            for (long i = r + 1; i < size; i++)
            {
                if (pdiags[i + c] == 0 && mdiags[i - c + size - 1] == 0) EliminationList.Remove(prevs[i], nexts[i], c + 1);
                if (c + 1 + (i - r) < size + 2 - 1 && cols[c + (i - r)] == 0 && pdiags[i + (c + (i - r))] == 0) EliminationList.Remove(prevs[i], nexts[i], c + 1 + (i - r));
                if (c + 1 - (i - r) >= 2 - 1 && cols[c - (i - r)] == 0 && mdiags[i - (c - (i - r)) + size - 1] == 0) EliminationList.Remove(prevs[i], nexts[i], c + 1 - (i - r));   
            }

            cols[c]++;
            pdiags[r + c]++;
            mdiags[r - c + size - 1]++;

            queensPlaced++;
        }

        public void RemoveQueenFrom(long r, long c)
        {
            cols[c]--;
            pdiags[r + c]--;
            mdiags[r - c + size - 1]--;

            for (long i = r + 1; i < size; i++)
            {
                if (c + 1 - (i - r) >= 2 - 1 && cols[c - (i - r)] == 0 && mdiags[i - (c - (i - r)) + size - 1] == 0) EliminationList.Insert(prevs[i], nexts[i], c + 1 - (i - r));
                if (c + 1 + (i - r) < size + 2 - 1 && cols[c + (i - r)] == 0 && pdiags[i + (c + (i - r))] == 0) EliminationList.Insert(prevs[i], nexts[i], c + 1 + (i - r));
                if (pdiags[i + c] == 0 && mdiags[i - c + size - 1] == 0) EliminationList.Insert(prevs[i], nexts[i], c + 1);
            }

            queensPlaced--;
        }

        public long Solve()
        {
            if (queensPlaced == size) return 1;
            else if (nexts[queensPlaced][0] - 1 >= size) return 0;

            long solutionsFound = 0;
            long r = queensPlaced;
            long c = nexts[queensPlaced][0] - 1;
            while (c < size)
            {
                PlaceQueenAt(r, c);
                solutionsFound += Solve();
                RemoveQueenFrom(r, c);

                c = nexts[queensPlaced][c + 1] - 1;
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
