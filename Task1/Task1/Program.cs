using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Task1
{
    class Program
    {
        static void Main(string[] args)
        {

            bc = new BlockingCollection<char>(4);
            Task Prod = new Task(Producer);
            Task Con = new Task(Consumer);
            Con.Start();
            Prod.Start();
            try
            {
                Task.WaitAll(Con, Prod);
            }
            catch (AggregateException exc)
            {
                Console.WriteLine(exc);
            }
            finally
            {
                Con.Dispose();
                Prod.Dispose();
                bc.Dispose();
            }
            Console.ReadLine();
        }
        static BlockingCollection<char> bc;
        static void Producer()
        {
            for (char ch = 'А'; ch <= 'Я'; ch++)
            {
                bc.Add(ch);
                Console.WriteLine("++Производится символ " + ch);
            }
            bc.CompleteAdding();
        }
        static void Consumer()
        {
            char ch;

            while (!bc.IsCompleted)
            {
                if (bc.TryTake(out ch))
                    Console.WriteLine("---Потребляется символ " + bc.Take());
            }

        }
    }
}
