using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;

namespace BookRankReaderConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine();
            Console.WriteLine("For start reading list of book and collecting data press '1'");
            Console.WriteLine("For start export results to CSV press '2'");

            var key = Console.ReadLine();
            switch (key)
            {
                case "2":
                    {
                        Console.WriteLine();
                        Console.WriteLine("Exporting...");
                        Console.WriteLine();
                        //   readFile();

                        break;
                    }
                case "1":
                    {
                        Console.WriteLine();
                        Console.WriteLine("Reading...");
                        Console.WriteLine();
                        readFile();

                        getBookRate();
                        break;
                    }
                default:
                    Console.WriteLine("Error");
                    break;

            }


        }

        static List<string> linkList = new List<string>();
        string createLink()
        {
            string html;// = "https://www.amazon.com/dp/";
            // html += textBox1.Text;
            foreach (var a in linkList)
            {
                html = "https://www.amazon.com/dp/";
                html += a;
            }


            return null;
        }

        static void readFile()
        {
            linkList = new List<string>();
            string[] str = { "\n" };
            using (StreamReader rd = new StreamReader(new FileStream("list.txt", FileMode.Open)))
            {
                str = rd.ReadToEnd().Split(str, StringSplitOptions.RemoveEmptyEntries);
            }
            for (int i = 0; i < str.Length; i++)
            {
                linkList.Add(str[i]);

                Console.WriteLine(str[i]);

            }
            Console.WriteLine();
            Console.WriteLine("Complete!\nTotal: " + linkList.Count);
            //   Console.ReadKey();
            Main(start);
            //  return linkList;
        }
        static void writingInCSVMethod(string bookRate, string bookNum)
        {
            FileStream fs1 = new FileStream("log.csv", FileMode.Append);
            StreamWriter writeInCsv = new StreamWriter(fs1, Encoding.Unicode);
            char delimeter = '\t';
            writeInCsv.WriteLine(bookNum + delimeter + bookRate);

            writeInCsv.Close();
            fs1.Close();
        }

        static string[] start;
      //  static string[] address;
        static async void getBookRate()
        {
            foreach (var a in linkList)
            {


                string html = "https://www.amazon.com/dp/";
                html += a;
                var config = Configuration.Default.WithDefaultLoader();
                // Load the names of all The Big Bang Theory episodes from Wikipedia
                string address=html.Substring(0,html.Length-2);
                //try
                //{

                //    address = html.Split(new char[] { '\n' });
                //    if (address.Length <= 1)
                //    {
                //        address = html.Split(new char[] { '\r' });
                //    }
                //}
                //catch { }




                // Asynchronously get the document in a new context using the configuration
                try
                {
                    var document = await BrowsingContext.New(config).OpenAsync(address);

             
              

                // This CSS selector gets the desired content
                var cellSelector = "tr.vevent td:nth-child(3)";
                cellSelector = "#SalesRank";
                // Perform the query to get all cells with the content
                // var cells = document.QuerySelectorAll(cellSelector);
                var cells = document.QuerySelector(cellSelector);
                // We are only interested in the text - select it with LINQ
                // var titles = cells.Select(m => m.TextContent);
                var titles = cells.TextContent;

                writingInCSVMethod(getNum(titles), a);
                }
                catch (Exception e)
                {

                    Console.WriteLine(e);
                    Console.ReadKey();
                    throw;
                }
               

                // textBox2.Text += getNum(titles);


            }

        }
        static string getNum(string input)
        {
            string output;
            string[] temp2;
            string[] temp = input.Split(new char[] { '#' });
            temp2 = temp[1].Split(new char[] { ' ' });
            output = temp2[0];
            return output;
        }

    }


    class BookModel
    {
        public string BookID { get; set; }
        public string Rate { get; set; }


    }
}
