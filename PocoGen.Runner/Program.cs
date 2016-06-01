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

            using (DirectoryCatalog catalog = new DirectoryCatalog(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)))
            {
                var container = new CompositionContainer(catalog, CompositionOptions.DisableSilentRejection | CompositionOptions.IsThreadSafe);

                Program program = new Program();
                container.ComposeParts(program);

                try
                {
                    program.Run(path);
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

                        case PlugInType.ForeignKeyPropertyNameGenerator:
                            Console.Error.Write("Foreign Key Property Name Generator: ");
                            break;

                        default:
                            Console.Error.Write(Enum.GetName(typeof(PlugInType), unrecognizedPlugIn.PlugInType) + ": ");
                            break;
                    }

                    Console.Error.WriteLine(unrecognizedPlugIn.Name);
                }

                Environment.Exit((int)ReturnCodes.UnrecognizedPlugIns);
            }
        }

        private void Run(string path)
        {
            List<UnknownPlugIn> unrecognizedPlugIns;
            try
            {
                this.Engine.Load(path, out unrecognizedPlugIns);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("ERROR: Could not load file: " + ex.Message);
                Environment.Exit((int)ReturnCodes.InvalidFileFormat);

                // This return will never be reached, but the compiler throws the warning "Use of unassigned local variable 'unrecognizedPlugIns'" if we don't include it.
                return;
            }

            CheckForUnrecognizedPlugIns(unrecognizedPlugIns);

            string basePath = Path.GetDirectoryName(path);

            this.Engine.Generate(basePath).Wait();
        }
    }
}