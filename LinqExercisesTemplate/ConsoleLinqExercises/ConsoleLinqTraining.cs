using Logic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleLinqExercises
{
    internal class ConsoleLinqTraining
    {
        private static Controller _controller;

        /// <summary>
        ///     Methode zur Ausgabe einer Überschrift
        /// </summary>
        /// <param name="caption"></param>
        private static void PrintCaption(string caption)
        {
            ConsoleColor initialColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n" + caption);
            Console.ForegroundColor = initialColor;
        }

        /// <summary>
        ///     Methode zur Ausgabe einer Überschrift, gefolgt von einer generischen Collection (ToString()
        ///     Methode ToString() der Listenelemente muss sinnvoll überschrieben sein!)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="caption"></param>
        /// <param name="res"></param>
        private static void PrintResult<T>(string caption, IEnumerable<T> res)
        {
            PrintCaption(caption);
            foreach (T item in res)
            {
                Console.WriteLine(item.ToString());
            }
            Console.WriteLine();
        }

        /// <summary>
        ///     Demonstration der Verwendung von optionalen Parametern
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        private static void UseOptionalParameters(int a, int b = 3, string c = "Hallo")
        {
            Console.WriteLine("{0} {1} {2}", a, b, c);
        }

        private static void Main()
        {
            //UseOptionalParameters(99, c: "xx");

            //Hinweis: Zum Linq-Training eignet sich auch das kostenlose Tool LinqPad hervorragend! (z.B. als Kombination mit SQL-Server-Express Tabellen)
            _controller = new Controller();

            var allCds = _controller.Cds.ToList();
            PrintResult("Alle CD´s", allCds);

            //Selektiere nur jene CDs die als Country UK eingetragen haben! (Groß/Kleinschreibung soll ignoriert werden)
            //Sorierung nach Company, bei gleicher Company soll nach CD-Titel sortiert werden.

            IEnumerable<Cd> result = allCds.Where(loc => loc.Country.ToUpper() == "UK")
                                    .OrderBy(orderByCompany => orderByCompany.Company)
                                    .ThenBy(orderByTitle => orderByTitle.Title);
                

            PrintResult("Nur UK-CD´s", result);


            //Gebe die CD´s gruppiert nach Company aus. D.h. zuerst Company als Überschrift, gefolgt von allen Cd´s zur Company.
            //Sorierung nach Company Name und dann nach CD-Titel
            //IEnumerable<IGrouping<string, Cd>> result2Query = from cd in _controller.Cds
            //    orderby cd.Title
            //    group cd by cd.Company into companyCds
            //    select companyCds;

            IOrderedEnumerable<IGrouping<string, Cd>> groupedResult = allCds.GroupBy(gB => gB.Company).OrderBy(g => g.Key);

            //GroupBy liefert eine Liste (von IGrouping Elementen) wobei jedes Element den gemeinsamen Key und wiederum eine Liste mit den Elementen beinhaltet

            PrintCaption("CD´s gruppiert nach Company");
            foreach (var companyCds in groupedResult)
            {
                Console.WriteLine($"Company:{companyCds.Key}");
                foreach (var cd in companyCds)
                {
                    Console.WriteLine(cd.Title);
                }
                Console.WriteLine();
            }

            // Aggregatsmethoden

            PrintCaption("Kosten aller Polydor Titel!");

            double cost = allCds.Where(w => w.Company == "Polydor").Sum(s => s.Price);
            Console.WriteLine("Kosten von Polydor Titel: {0:f2}\n", cost);

            //Beispiel: Selektiere die Anzahl der Titel und deren Durchschnittspreis je Land (Country) in ein anonymes Objekt
            //Ergebnisliste soll Elemente mit den Feldern Country, TitleCount und AvgPrice beinhalten. 
            //Sortiert nach Country
            PrintCaption("Titelanzahl und Durchschnittspreis je Land:");

            var result3Query = from cd in _controller.Cds
                               group cd by cd.Country
                into countryCds
                               orderby countryCds.First().Country
                               select new
                               {
                                   Country = countryCds.Key,
                                   TitleCount = countryCds.Count(),
                                   AvgPrice = countryCds.Average(cd => cd.Price)
                               };

            var resultAverageCountAndPricePerCountry = result3Query;

            foreach (var item in resultAverageCountAndPricePerCountry)
            {
                Console.WriteLine($"Land: {item.Country,-10} - Anzahl der Titel: {item.TitleCount,4} - Durch.Preis: {item.AvgPrice:f2}");
            }

            //Überladene Where Methode mit zwei Parameter (Element und Index des Elements in der Collection)
            int[] numbers = { 1, 22, 34, 45, 22, 67, 78, 89, 90, 101 };

            var eachSecondNumber = numbers.Where((x,y) => x%2 == 1);
            PrintResult("Jede zweite Zahl...", eachSecondNumber);

            //Es soll eine Liste mit: CD-Title, Company-Name, Company-Director ausgegeben werden
            var joinExample = default(IEnumerable<Object>);

            PrintCaption("Title - Company-Name - Director");
            foreach (var item in joinExample)
            {
                // Console.WriteLine("{0,-25} {1,-20} {2,-20}", item.Title, item.Company, item.Director);
            }

            //Je Company-Director soll der Durchschnittspreis aller zu seiner Company gehörenden CD´s ausgegeben werden.
            var joinAvgEx = default(IEnumerable<Object>);

            PrintCaption("Avg-Price / CompanyDirector");
            foreach (var item in joinAvgEx)
            {
                //Console.WriteLine("{0,-20} {1,6:f2}", item.Director, item.AvgPrice);
            }

        }

        /// <summary>
        ///     Ermittelt die Position des Elements im Array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="element"></param>
        /// <param name="array"></param>
        /// <returns></returns>
        private static int GetIndexOf<T>(T element, T[] array)
        {
            return Array.IndexOf(array, element);
        }


    }
}