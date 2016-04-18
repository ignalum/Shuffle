using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Shuffle
{
    class Program
    {
        static bool runValidateIntLoaded = false;
        static bool setRandNumIntegerInSequence = false;
        static UInt32 randNum = 0;
        static UInt32 numOfThreads = 1;
        static UInt32 intCounter = 0;
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
            //List<UInt32> XList = new List<UInt32>();
            //Random rnd = new Random();
            //bool runValidateIntLoaded = false;
            //bool setRandNumIntegerInSequence = false;
            //UInt32 randNum = 0;

            Console.WriteLine("Please enter an integer value between 1 and " + UInt32.MaxValue);
            String Result = Console.ReadLine();

            while (!UInt32.TryParse(Result, out maxInt) || maxInt < 1)
            {
                Console.WriteLine("Not a valid number, try again.");

                Result = Console.ReadLine();
            }

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-v")
                {
                    runValidateIntLoaded = true;
                }
                else if (args[i] == "-randomize")
                {
                    setRandNumIntegerInSequence = true;
                    UInt32.TryParse(args[i + 1], out randNum);
                }
                else if (args[i] == "-threads")
                {
                    UInt32.TryParse(args[i + 1], out numOfThreads);
                }
            }

            timer = Stopwatch.StartNew();

            UInt32 maxSize = maxInt / numOfThreads;
            //UInt32 numOfThreads = (maxInt / maxSize) + 1;
            WaitHandle[] waitHandles = new WaitHandle[numOfThreads];
            for (UInt32 i = 0; i < numOfThreads; i++)
            {
                UInt32 j = i;
                EventWaitHandle handle = new EventWaitHandle(false, EventResetMode.ManualReset);
                ThreadStart myThreadDelegate = delegate
                {
                    if (maxInt > maxSize * (j + 1) && numOfThreads > j + 1)
                        ShuffleProc(maxSize * j, maxSize * (j + 1), out intCounter);
                    else
                        ShuffleProc(maxSize * j, maxInt, out intCounter);
                    handle.Set();
                };
                Thread myThread = new Thread(myThreadDelegate);
                waitHandles[i] = handle;
                myThread.Start();
            }
            WaitHandle.WaitAll(waitHandles);
            //intCounter += intCounter;
            //UInt32 minInt = 0;
            //UInt32 intCounter = 0;
            //while (minInt < maxInt)
            //{
            //    try
            //    {
            //        UInt32 tmaxInt = maxInt;
            //        if (setRandNumIntegerInSequence)
            //        {
            //            maxInt = minInt + randNum;
            //            if (maxInt > tmaxInt)
            //                maxInt = tmaxInt;
            //        }
            //        XList.AddRange(Sequence(minInt, maxInt));
            //        maxInt = tmaxInt;
            //    }
            //    catch (Exception ex)
            //    {
            //    }

            //    XList[0] = XList[XList.Count - 1] + 1;
            //    minInt += (UInt32)XList.Count;
            //    int n = XList.Count;
            //    while (n > 0)
            //    {
            //        int k = (rnd.Next(0, n) % n);
            //        n--;
            //        UInt32 value = XList[k];
            //        XList[k] = XList[n];
            //        XList[n] = value;
            //        if (!runValidateIntLoaded)
            //        {
            //            Console.WriteLine(value);
            //            intCounter++;
            //        }
            //    }
            //    XList.Clear();
            //    if (runValidateIntLoaded)
            //    {
            //        Console.WriteLine("# OF INTEGERS LOADED & RANDOMIZED: " + minInt);
            //        intCounter++;
            //    }
            //}
            timer.Stop();
            Console.WriteLine("# OF LINES DISPLAYED: " + intCounter);
            Console.WriteLine("TIME ELAPSED (MS): " + timer.ElapsedMilliseconds);
        }

        static void ShuffleProc(UInt32 minInt, UInt32 maxInt, out UInt32 intCounter)
        {
            List<UInt32> XList = new List<UInt32>();
            Random rnd = new Random();
            //UInt32 minInt = 0;
            intCounter = 0;
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
        }
    }
}
