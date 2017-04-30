//-----------------------------------------------------------------------
// Google Code Jam solution template
//      by szmate1618
// This is not how I normally code, but anything goes in a competition
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

// The last character of this namespace's name should be the letter code (in capital) of the problem,
// since the code below uses reflection to parse that from here and automatically load the correct input file.
namespace ProblemX
{
    class Program
    {
        #region Boilerplate
        // Parse problem code from namespace name.
        static String problem = typeof(Program).Namespace[typeof(Program).Namespace.Length - 1].ToString();
        // Obviously Windows-specific.
        static String downloadsFolder = String.Format(@"C:\Users\{0}\Downloads", Environment.UserName);

        // Static contructor for initialization.
        static Program()
        {
            // This is mainly for enforcing dot as decimal separator.
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
        }

        static String GetLatestInput(String inputFolder, String problem, String size)
        {
            // This may be a little slow, so ideally the inputFolder should only contain the actual input files.
            DirectoryInfo directory = new DirectoryInfo(inputFolder);
            String file = directory
                .GetFiles()
                .Where(s => s.Name.Contains(problem) && (s.Name.Contains(size)))
                .OrderBy(s => s.LastWriteTime)
                .Last().FullName;
            Console.WriteLine("Using {0} as input.", file);
            return file;
        }

        static void OpenInDefaultEditor(String path)
        {
            System.Diagnostics.Process.Start(path);
        }
        #endregion

        // This is a competition, no reason not to declare things as globals.
        static Int64 T;
        static Int64 N;
        static Int64 M;
        static Int64[] Qi;

        static void Main(string[] args)
        {
            StreamReader sr = new StreamReader("in.txt");
            //StreamReader sr = new StreamReader(getLatestInput(downloadsFolder, problem, "small"));
            //StreamReader sr = new StreamReader(getLatestInput(downloadsFolder, problem, "large"));
            StreamWriter sw = new StreamWriter("out.txt");

            /*Int64*/ T = Convert.ToInt64(sr.ReadLine());
            for (int i = 0; i < T; i++)
            {
                string[] NM = sr.ReadLine().Split();
                /*Int64*/ N = Convert.ToInt64(NM[0]);
                /*Int64*/ M = Convert.ToInt64(NM[1]);

                /*Int64[]*/ Qi = sr.ReadLine().Split().Select(x => Convert.ToInt64(x)).ToArray();

                for (int j = 0; j < N; j++)
                {

                }

                sw.WriteLine("Case #{0}: {1}", i + 1, "Correct solution goes here");
                
            }

            Console.WriteLine("DONE - press any key to escape");
            Console.ReadKey();
            OpenInDefaultEditor(((FileStream)(sw.BaseStream)).Name);
            sr.Close();
            sw.Close();
        }
    }
}
