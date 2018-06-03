using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SenderOfResume.Parsing;

namespace SenderOfResume
{
    class Program
    {

        static void Main(string[] args)
        {
            Parser parser = new Parser();
            parser.Worker();
            Console.ReadKey();
        }
    }
}
