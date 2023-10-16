// See https://aka.ms/new-console-template for more information
// Console.WriteLine("Hello, World!");

using System;
using ThermoFisher.CommonCore.Data.Business;
using ThermoFisher.CommonCore.Data.Interfaces;
using ThermoFisher.CommonCore.RawFileReader;

namespace SldReader
{
    class MainClass
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please provide the path to the SLD file.");
                return;
            }

            string sldFilePath = args[0];
            ReadAndPrintSamples(sldFilePath);
        }

        private static void ReadAndPrintSamples(string sldFilePath)
        {
            // Initialize the SLD file reader
            var sldFile = SequenceFileReaderFactory.ReadFile(sldFilePath);

            if (sldFile.IsError)
            {
                Console.WriteLine($"Error opening the SLD file: {sldFile.FileError.ErrorMessage}");
                return;
            }

            // if (!(sldFile is ISequenceFileAccess))
            if (!(sldFile is ISequenceFileAccess))
            {
                Console.WriteLine("This file does not support sequence file access.");
                return;
            }

            // Console.WriteLine("List of Samples:");
            foreach (var sample in sldFile.Samples)
            {
                string parentPath = sample.Path;
                // string sampleName = sample.R;
                string rawFileName = sample.RawFileName;
                // Console.WriteLine($"- {parentPath}\\{sampleName}");
                Console.WriteLine($"{parentPath}\\{rawFileName}.raw");
            }

            // Always close the file when done
            // sldFile.Dispose()
        }
    }
}

