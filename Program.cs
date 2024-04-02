using System.Collections.Generic;
using System.Text;

namespace Lab3
{
    class Program
    {
        public static void Main()
        {
            while (true)
            {
                Console.WriteLine("Введите выражение ");
                string Line = Console.ReadLine();
                try
                { 
                    Composite TreeLine = Parser.Parse(Line);
                    LogicalFunctionsMinimizator l = new(TreeLine);
                    if (l.IsPerfect)
                    {
                        Console.WriteLine("Склеивание");
                        Console.WriteLine(l.Skleivanie);
                        Console.WriteLine("Расчетный метод");
                        Console.WriteLine(l.CalculationMethodRez);
                        Console.WriteLine("Расчетная табличный");
                        Console.WriteLine(l.ImplicantTable); ;
                        Console.WriteLine("Расчетно табличный метод");
                        Console.WriteLine(l.CalculationTableMethodRez);
                        if (l.UniqueLetters.Count <= 3)
                        {
                            Console.WriteLine("Карта Карно");
                            Console.WriteLine(l.KarnoTable);
                            Console.WriteLine("Результат табличного метода:");
                            Console.WriteLine(l.KarnoMapMethodRez);
                        }
                    }
                    else
                        Console.WriteLine("Не совершенная форма");
                }
                catch 
                {
                    Console.WriteLine("Не верное выеражение");
                }
            }
        }
    }



}
