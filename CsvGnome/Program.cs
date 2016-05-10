﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome
{
    class Program
    {
        public const string FileExt = ".csv";

        private static int n = 10; 

        /// <summary>
        /// Number of data lines to generate.
        /// </summary>
        public static int N
        {
            get { return n; }
        }

        /// <summary>
        /// Path to output file.
        /// </summary>
        public static string FilePath = @"C:\Users\revol\Desktop\csvGnomeOut\";

        /// <summary>
        /// Name of output file.
        /// </summary>
        public static string FileName = "CsvGnome";

        public static string File => $"{FilePath}{FileName}{FileExt}";

        static readonly FieldBrain FieldBrain = new FieldBrain();
        static readonly Interpreter Interpreter = new Interpreter();
        static readonly Reporter Reporter = new Reporter();
        static readonly Writer Writer = new Writer();

        static void Main(string[] args)
        {
            Action nextAction = Action.Continue;
            while(nextAction == Action.Continue)
            {
                Reporter.Report(FieldBrain.Fields, N, File);

                nextAction = Interpreter.Interpret(Console.ReadLine(), FieldBrain);

                // Write to file
                if (nextAction == Action.Run || nextAction == Action.Write) Writer.WriteToFile(FieldBrain.Fields, File, N);

                // "Write" continues execution
                if (nextAction == Action.Write) nextAction = Action.Continue;
            }
        }

        public static void SetN(int n)
        {

        }
    }
}