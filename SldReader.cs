// See https://aka.ms/new-console-template for more information
// Console.WriteLine("Hello, World!");

using System;
using System.Collections.Generic;
using ThermoFisher.CommonCore.Data.Business;
using ThermoFisher.CommonCore.Data.Interfaces;
using ThermoFisher.CommonCore.RawFileReader;

using StringOrderedDictionary = System.Collections.Specialized.StringDictionary;


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

        // returns an empty dictionary on errors
        private static StringOrderedDictionary SLDReadSamples(string sldFilePath)
        {
            StringOrderedDictionary rawFilesAcquired = new StringOrderedDictionary();
            // Initialize the SLD file reader
            var sldFile = SequenceFileReaderFactory.ReadFile(sldFilePath);
            if (sldFile.IsError)
            {
                Console.WriteLine($"Error opening the SLD file: {sldFilePath}, {sldFile.FileError.ErrorMessage}");
                return rawFilesAcquired;
            }
            if (!(sldFile is ISequenceFileAccess))
            {
                Console.WriteLine($"This file {sldFilePath} does not support sequence file access.");
                return rawFilesAcquired;
            }
            foreach (var sample in sldFile.Samples)
            {
                // I saw some .raw files in a FreeStyle SLD file and will skip these.
                if (string.IsNullOrEmpty(sample.RawFileName))
                {
                    continue;
                }
                // Sometimes a path sep and sometimes not
                string rawFileName = sample.Path.TrimEnd('\\') + Path.DirectorySeparatorChar + sample.RawFileName + ".raw";
                rawFilesAcquired[rawFileName] = "No";
            }
            // According to docs, the file is not kept open.
            return rawFilesAcquired;
        }  // SLDReadSamples()

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

