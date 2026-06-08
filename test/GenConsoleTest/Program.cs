using System;

namespace GenConsoleTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(HumanType.ToDescription(HumanType.Vip));

            foreach(var item in UserTypeExtensions.Source)
            {
                Console.WriteLine($"Value: {item.Value}, Name: {item.Name}, Description: {item.Description}");
            };
        }
    }
}
