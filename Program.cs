using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace Shuffle
{
    class Program
    {
        static IEnumerable<UInt32> Sequence(UInt32 n1, UInt32 n2)
        {
            while (n1 < n2)
            {
                yield return n1++;
            }
        }

        static void Main(string[] args)
        {
            Stopwatch timer;
            UInt32 maxInt;
            List<UInt32> XList = new List<UInt32>();
            Random rnd = new Random();
            bool runValidateIntLoaded = false;
            bool setRandNumIntegerInSequence = false;
            UInt32 randNum = 0;

            Console.WriteLine("Please enter an integer value between 1 and " + UInt32.MaxValue);
            String Result = Console.ReadLine();

            while (!UInt32.TryParse(Result, out maxInt) || maxInt < 1)
            {
                Console.WriteLine("Not a valid number, try again.");

                Result = Console.ReadLine();
            }

            //for (int i = 0; i < args.Length; i++)
            //{
                if (args.Length > 0 && args[0] == "-v")
                {
                    runValidateIntLoaded = true;
                }
                else if (args.Length > 0 && args[0] == "-randomize")
                {
                    setRandNumIntegerInSequence = true;
                    UInt32.TryParse(args[1], out randNum);
                }
            //}

            timer = Stopwatch.StartNew();
            UInt32 minInt = 0;
            UInt32 intCounter = 0;
            while (minInt < maxInt)
            {
                try
                {
                    UInt32 tmaxInt = maxInt;
                    if (setRandNumIntegerInSequence)
                    {
                        maxInt = minInt + randNum;
                        if (maxInt > tmaxInt)
                            maxInt = tmaxInt;
                    }
                    XList.AddRange(Sequence(minInt, maxInt));
                    maxInt = tmaxInt;
                }
                catch (Exception ex)
                {
                }

                XList[0] = XList[XList.Count - 1] + 1;
                minInt += (UInt32)XList.Count;
                int n = XList.Count;
                while (n > 0)
                {
                    int k = (rnd.Next(0, n) % n);
                    n--;
                    UInt32 value = XList[k];
                    XList[k] = XList[n];
                    XList[n] = value;
                    if (!runValidateIntLoaded)
                    {
                        Console.WriteLine(value);
                        intCounter++;
                    }
                }
                XList.Clear();
                if (runValidateIntLoaded)
                {
                    Console.WriteLine("# OF INTEGERS LOADED & RANDOMIZED: " + minInt);
                    intCounter++;
                }
            }
            timer.Stop();
            Console.WriteLine("# OF LINES DISPLAYED: " + intCounter);
            Console.WriteLine("TIME ELAPSED (MS): " + timer.ElapsedMilliseconds);
        }
    }
}
