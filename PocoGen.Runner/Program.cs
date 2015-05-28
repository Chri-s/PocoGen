using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Reflection;
using PocoGen.Common;
using PocoGen.Common.FileFormat;

namespace PocoGen.Runner
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("USAGE: PocoGen.Runner <POCO File>");
                return;
            }

            string path = args[0];
            if (path.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
            {
                Console.Error.WriteLine("ERROR: Invalid path \"" + path + "\".");
                Environment.Exit((int)ReturnCodes.InvalidPath);
            }

            path = Path.GetFullPath(path);

            try
            {
                if (!File.Exists(path))
                {
                    Console.Error.WriteLine("ERROR: File not found: \"" + path + "\".");
                    Environment.Exit((int)ReturnCodes.FileNotFound);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("ERROR: " + ex.Message);
                Environment.Exit((int)ReturnCodes.FileNotFound);
            }

            Definition definition = null;
            try
            {
                definition = Definition.Load(path);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("ERROR: Could not load file: " + ex.Message);
                Environment.Exit((int)ReturnCodes.InvalidFileFormat);
            }

            using (DirectoryCatalog catalog = new DirectoryCatalog(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)))
            {
                var container = new CompositionContainer(catalog, CompositionOptions.DisableSilentRejection | CompositionOptions.IsThreadSafe);

                Program program = new Program();
                container.ComposeParts(program);

                try
                {
                    program.Run(definition, path);
                    Environment.Exit((int)ReturnCodes.Success);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("ERROR: Could not run POCO generation: " + ex.ToString());
                    Environment.Exit((int)ReturnCodes.CouldNotRun);
                }
            }
        }

        [Import]
        public Engine Engine { get; set; }

        private static void CheckForUnrecognizedPlugIns(List<UnknownPlugIn> unrecognizedPlugIns)
        {
            if (unrecognizedPlugIns.Count > 0)
            {
                Console.Error.WriteLine("ERROR: The POCO definition file contains the following plug ins which were not found:");
                Console.Error.WriteLine();

                foreach (UnknownPlugIn unrecognizedPlugIn in unrecognizedPlugIns)
                {
                    switch (unrecognizedPlugIn.PlugInType)
                    {
                        case PlugInType.SchemaReader:
                            Console.Error.Write("Schema Reader: ");
                            break;

                        case PlugInType.TableNameGenerator:
                            Console.Error.Write("Table Name Generator: ");
                            break;

                        case PlugInType.ColumnNameGenerator:
                            Console.Error.Write("Column Name Generator: ");
                            break;

                        case PlugInType.OutputWriter:
                            Console.Error.Write("Output Writer: ");
                            break;

                        default:
                            Console.Error.Write(Enum.GetName(typeof(PlugInType), unrecognizedPlugIn.PlugInType) + ": ");
                            break;
                    }

                    Console.Error.WriteLine(unrecognizedPlugIn.PlugIn.Name);
                }

                Environment.Exit((int)ReturnCodes.UnrecognizedPlugIns);
            }
        }

        private void Run(Definition definition, string path)
        {
            List<UnknownPlugIn> unrecognizedPlugIns;
            this.Engine.SetFromDefinition(definition, out unrecognizedPlugIns);

            CheckForUnrecognizedPlugIns(unrecognizedPlugIns);

            string basePath = Path.GetDirectoryName(path);

            this.Engine.Generate(basePath).Wait();
        }
    }
}