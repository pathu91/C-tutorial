using System;

namespace ConsoleApp1
{
    public class Program
    {
        static void Main(string[] args)
        {
            bool canIVote = true;

            Console.WriteLine("Biggest Integer : {0}", int.MaxValue);
            Console.WriteLine("Smallest Integer : {0}", int.MinValue);

            Console.WriteLine("Biggest Long : {0}", long.MaxValue);
            Console.WriteLine("Biggest Long : {0}", long.MinValue);

            decimal decPiVal = 3.1415926535897932384626433832M;
            decimal decBigNum = 3.0000000000000000000000000011M;
            Console.WriteLine("DEC : PI + bigNum= {0}", decPiVal + decBigNum);
        }
    }
}