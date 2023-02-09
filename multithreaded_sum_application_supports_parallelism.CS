using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace 510Lab2
{
 class Program
 {
 static ulong max = 500000000;
 static ulong SingleThread()
 {
 ulong sum = 0;
 for (ulong i = 0; i <= max; i++)
 {
 sum += i;
 }
 return sum;
 }
 static void MultiThreadMethod(object args)
 {
 ValuesClass values = args as ValuesClass;
 ulong length = max / values.Count;
 ulong start = values.ID * length +1 ;
 ulong end = start + length;
 values.Result = 0;
 for (ulong i = start; i < end; i++)
 {
 values.Result += i;
 }

 }
 static ulong MultiThread(int count)
 {
 List<ValuesClass> valuesList = new List<ValuesClass>();
 ulong totalSum = 0;
 ulong rem = max % (ulong)count;
 List<Thread> threadsList = new List<Thread>();
 for (int i = 0; i <count; i++)
 {
 threadsList.Add(new Thread(new ParameterizedThreadStart(MultiThreadMethod)));
 valuesList.Add(new ValuesClass()
 {
 Count = (ulong)count,
 ID = (ulong)i,
 Result = 0,
 });
 threadsList[i].Start(valuesList[i]);
 }
 for (int i = 0; i < count; i++)
 {
 threadsList[i].Join();
 totalSum += valuesList[i].Result;
 }
 for (ulong i = 0; i < rem; i++)
 {
 totalSum = totalSum + max - i;
 }
 return totalSum;
 }
 static void BalancedThreadMethod(object args)
 {
 ValuesClass values = args as ValuesClass;
 ulong inc = values.Count;
 ulong start = values.ID +1;
 values.Result = 0;
 for (ulong i = start; i <= max; i = i+inc)
 {
 values.Result += i;
 }
 }
 static ulong BalancedThread(int count)
 {
 List<ValuesClass> valuesList = new List<ValuesClass>();
 ulong totalSum = 0;
 List<Thread> threadsList = new List<Thread>();
 for (int i = 0; i < count; i++)
 {
 threadsList.Add(new Thread(new ParameterizedThreadStart(BalancedThreadMethod)));
 valuesList.Add(new ValuesClass()
 {
 Count = (ulong)count,
 ID = (ulong)i,
 Result = 0,
 });
 threadsList[i].Start(valuesList[i]);
 }
 for (int i = 0; i < count; i++)
 {
 threadsList[i].Join();
 totalSum += valuesList[i].Result;
 }
 return totalSum;
 }
 static void Main(string[] args)
 {
 var watch = System.Diagnostics.Stopwatch.StartNew();
 ulong result = SingleThread();
 Console.WriteLine("Single threaded result:" + result);
 watch.Stop();
 Console.WriteLine("Elapsed Time for Single Threaded program is {0} ", watch.ElapsedMilliseconds + "ms"
+ "\n");
 for (int i = 2; i <= 10; i++)
 {
 watch = System.Diagnostics.Stopwatch.StartNew();
 result = MultiThread(i);
 Console.WriteLine(i + " Multithreaded result:" + result);
 watch.Stop();
 Console.WriteLine("Elapsed Time for " + i + " MultiThreaded program is {0} ",
watch.ElapsedMilliseconds + "ms" + "\n");
 }
 for (int i = 2; i <= 10; i++)
 {
 watch = System.Diagnostics.Stopwatch.StartNew();
 result = BalancedThread(i);
 Console.WriteLine(i + " Balancedthreaded result:" + result);
 watch.Stop();
 Console.WriteLine("Elapsed time for " + i + " Balancedthreaded program is {0} ",
watch.ElapsedMilliseconds + "ms" + "\n");
 }
 }
 }
 class ValuesClass
 {
 public ulong Count { get; set; }
 public ulong ID { get; set; }
 public ulong Result { get; set; }
 }
}