using System;
using System.IO;
using System.Linq;
using System.Text;

namespace FixLicenseHeader
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] header = new[]
            {
                "//-----------------------------------------------------------------------------",
                "// Copyright (c) Microsoft Corporation.  All rights reserved.",
                "//-----------------------------------------------------------------------------",
                "",
            };

            string[] startingLines = new[]
            {
                "using ",
                "namespace ",
                "[assembly: "
            };

            string[] files = Directory.GetFiles(args[0], "*.cs", SearchOption.AllDirectories);

            foreach (string file in files)
            {
                if (file.EndsWith(".designer.cs", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"File processing skipped: {file}");

                    continue;
                }

                try
                {
                    string[] contents = File.ReadAllLines(file);

                    int index = 0;
                    for (; index < contents.Length; index++)
                    {
                        string line = contents[index].Trim();

                        if (startingLines.Any(p => line.StartsWith(p, StringComparison.Ordinal)))
                        {
                            break;
                        }
                    }

                    File.WriteAllLines(file, header.Concat(contents.Skip(index)), Encoding.UTF8);

                    Console.WriteLine($"File processing succeeded: {file}");
                }
                catch
                {
                    Console.WriteLine($"File processing failed: {file}");
                }

            }

            Console.WriteLine("Completed.");
        }
    }
}
