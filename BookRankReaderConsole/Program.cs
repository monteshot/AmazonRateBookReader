using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;

namespace BookRankReaderConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine();
            Console.WriteLine("For start reading list of book and collecting data press '1'");
            Console.WriteLine("For start export results to CSV press '2'");

         //   var key = Console.ReadLine();
            readFile();
            createLink();
            //switch (key)
            //{
            //    case "2":
            //        {
            //            Console.WriteLine();
            //            Console.WriteLine("Exporting...");
            //            Console.WriteLine();
            //            //   readFile();

            //            break;
            //        }
            //    case "1":
            //        {
            //            Console.WriteLine();
            //            Console.WriteLine("Reading...");
            //            Console.WriteLine();
            //            readFile();
            //            createLink();
            //            // getBookRate();
            //            printHeader = true;
            //            break;
            //        }
            //    default:
            //        Console.WriteLine("Error");
            //        break;

            //}


        }

        static List<string> linkList = new List<string>();
        static List<string> tempList = new List<string>();


        static void readFile()
        {
            linkList = new List<string>();
            string[] str = { "\n" };
            string[] str1 = { "\n" };
            try
            {
                using (StreamReader rd = new StreamReader(new FileStream("list.txt", FileMode.Open)))
                {
                    str = rd.ReadToEnd().Split(str, StringSplitOptions.RemoveEmptyEntries);
                }
                using (StreamReader rd = new StreamReader(new FileStream("export.csv", FileMode.Open)))
                {
                    str1 = rd.ReadToEnd().Split(str1, StringSplitOptions.RemoveEmptyEntries);
                }
            }
            catch
            {

            }

            for (int i = 0; i < str.Length; i++)
            {
                linkList.Add(str[i]);
                // tempList.Add(str[i]);
                Console.WriteLine(str[i]);

            }
            for (int i = 0; i < str1.Length; i++)
            {
                // linkList.Add(str[i]);
                tempList.Add(str1[i]);
                //  Console.WriteLine(str[i]);

            }
            Console.WriteLine();
            Console.WriteLine("Complete!\nTotal: " + linkList.Count);

            //   Main(start);
            //  return tempList;
        }

        static bool printHeader = true;
        static void writingInCSVMethod(string bookRate, string bookNum)
        {
            FileStream fs1 = new FileStream("export.csv", FileMode.Append);
            StreamWriter writeInCsv = new StreamWriter(fs1, Encoding.Unicode);
            char delimeter = ',';
            if (printHeader)
            {
                writeInCsv.WriteLine("ISBN" + delimeter + DateTime.Now);
                printHeader = false;
            }

            writeInCsv.WriteLine(bookNum + delimeter + bookRate + delimeter);
            writeInCsv.Close();
            fs1.Close();


        }
        static void writingInCSVAdder()
        {
            //FileStream fs1 = new FileStream("export.csv", FileMode.Append);
            //StreamWriter writeInCsv = new StreamWriter(fs1, Encoding.Unicode);

            char delimeter = ',';
            int cnt = 0;
            foreach (var row in tempList)
            {
                int endRowInd = 0;
                try
                {
                    endRowInd = row.IndexOf("\r");
                    row.Remove(row.IndexOf("\r"));
                }
                catch
                {
                    endRowInd = row.IndexOf("\n");
                    row.Remove(row.IndexOf("\n"));
                }

                row.Insert(endRowInd, ",");
                if (cnt == 0)
                {
                    row.Insert(endRowInd + 1, DateTime.Now.ToString());
                }
                else
                {
               //     row.Insert(endRowInd + 1, bookRate);
                }
                cnt++;


                // writeInCsv.WriteLine(row);
            }

            // writeInCsv.WriteLine(bookNum + delimeter + bookRate + delimeter);
            //writeInCsv.Close();
            //fs1.Close();


        }

        static bool lastBook = false;
        static string[] start;

        static string address = "";

        static string createLink()
        {
           FileStream fs1 = new FileStream("export.csv", FileMode.Create);
            StreamWriter writeInCsv = new StreamWriter(fs1, Encoding.Unicode);

            string address = "";
            int cnt = 0;

            foreach (var a in linkList)
            {
               
                cnt++;
                string html = "https://www.amazon.com/dp/";
                html += a;
                address = html.Substring(0, html.Length - 2);
                if (tempList != null)
                {
                    var b = tempList.Single(m => m.Contains(a.Substring(0, a.Length - 2)));
                    b += getBookRate(address, a.Substring(0, a.Length - 2));
                }
                else
                {
                    writingInCSVMethod(getBookRate(address, a.Substring(0, a.Length - 2)), a.Substring(0, a.Length - 2));
                }
                
                if (cnt == linkList.Count)
                {
                    //lastBook = true;
                    Console.WriteLine("Last book reached");
                    Console.WriteLine("Export complete");
                    //writingInCSVAdder();

                 //   Main(start);
                }

            }

            //foreach (var row in tempList)
            //{
            //    writeInCsv.WriteLine(row);
            //}

            writeInCsv.Close();
            fs1.Close();
            return address;
        }

        static async Task<string> GetDocumentAsync(string address)
        {
            var config = Configuration.Default.WithDefaultLoader();
            var document = await BrowsingContext.New(config).OpenAsync(address);
            var cellSelector = "#SalesRank";
            var cells = document.QuerySelector(cellSelector);
            var titles = cells.TextContent;
            return titles;
        }

        static List<string> ALLrate = new List<string>();
        static string getBookRate(string address, string nameBook)
        {
            string t = "";
            try
            {
                t = GetDocumentAsync(address).GetAwaiter().GetResult();
            }
            catch
            {
                Console.WriteLine("Book not found! (" + nameBook + ")");
            }
          return  getNum(t);
            //writingInCSVMethod(getNum(t), nameBook);
           // ALLrate.Add(getNum(t));
            // return t;
            //writingInCSVAdder(getNum(t));
        }

        static string getNum(string input)
        {
            string output = "";
            string[] temp2;
            try
            {

                string[] temp = input.Split(new char[] { '#' });
                temp2 = temp[1].Split(new char[] { ' ' });
                output = temp2[0];
                output = output.Replace(',', '.');
            }
            catch
            {

            }
            return output;
        }

    }


    class BookModel
    {
        public string BookID { get; set; }
        public string Rate { get; set; }


    }
}
