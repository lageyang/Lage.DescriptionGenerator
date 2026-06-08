using System;

namespace GenConsoleTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(HumanType.ToDescription(HumanType.Vip));

            foreach(var item in HumanType.GeneratedSource)
            {
                Console.WriteLine($"Value: {item.Value}, Name: {item.Name}, Description: {item.Description}");
            };
        }
    }
}
