using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PocoGen.Common.FileFormat;

namespace PocoGen.Common
{
    [Export]
    public class Engine : ChangeTrackingBase
    {
        public event EventHandler<TableEventArgs> TableRenamed;

        public Engine()
        {
            this.UseAnsiQuoting = false;
            this.ConnectionString = string.Empty;
            this.TableNameGenerators = new TableNameGeneratorPlugInCollection();
            this.OutputWriters = new OutputWriterPlugInCollection();
            this.TableChanges = new TableChangeCollection();
            this.Tables = new TableCollection();
            this.ColumnNameGenerators = new ColumnNameGeneratorPlugInCollection();
            this.UnknownPlugIns = new UnknownPlugInCollection(new List<UnknownPlugIn>());

            this.AcceptChanges();
        }

        [ImportMany(typeof(ISchemaReader))]
        public IEnumerable<Lazy<ISchemaReader, ISchemaReaderMetadata>> AvailableSchemaReaders { get; set; }

        [ImportMany(typeof(ITableNameGenerator))]
        public IEnumerable<Lazy<ITableNameGenerator, ITableNameGeneratorMetadata>> AvailableTableNameGenerators { get; set; }

        [ImportMany(typeof(IColumnNameGenerator))]
        public IEnumerable<Lazy<IColumnNameGenerator, IColumnNameGeneratorMetadata>> AvailableColumnNameGenerators { get; set; }

        [ImportMany(typeof(IOutputWriter))]
        public IEnumerable<Lazy<IOutputWriter, IOutputWriterMetadata>> AvailableOutputWriters { get; set; }

        private SchemaReaderPlugIn schemaReader;
        public SchemaReaderPlugIn SchemaReader
        {
            get { return this.schemaReader; }
            set { this.ChangeProperty(ref this.schemaReader, value); }
        }

        private string connectionString;
        public string ConnectionString
        {
            get { return this.connectionString; }
            set { this.ChangeProperty(ref this.connectionString, value); }
        }

        private bool useAnsiQuoting;
        public bool UseAnsiQuoting
        {
            get { return this.useAnsiQuoting; }
            set { this.ChangeProperty(ref this.useAnsiQuoting, value); }
        }

        public TableNameGeneratorPlugInCollection TableNameGenerators { get; private set; }

        public ColumnNameGeneratorPlugInCollection ColumnNameGenerators { get; private set; }

        public TableCollection Tables { get; private set; }

        public OutputWriterPlugInCollection OutputWriters { get; private set; }

        public UnknownPlugInCollection UnknownPlugIns { get; private set; }

        public override bool IsChanged
        {
            get
            {
                bool isSchemaReaderChanged = (this.SchemaReader == null) ? false : this.SchemaReader.IsChanged;

                return base.IsChanged ||
                    this.ColumnNameGenerators.IsChanged ||
                    this.OutputWriters.IsChanged ||
                    this.TableChanges.IsChanged ||
                    this.TableNameGenerators.IsChanged ||
                    isSchemaReaderChanged;
            }
        }

        public override void AcceptChanges()
        {
            base.AcceptChanges();

            this.ColumnNameGenerators.AcceptChanges();
            this.OutputWriters.AcceptChanges();
            this.TableChanges.AcceptChanges();
            this.TableNameGenerators.AcceptChanges();

            if (this.schemaReader != null)
            {
                this.SchemaReader.AcceptChanges();
            }
        }

        public async Task<string> TestConnection()
        {
            if (this.SchemaReader == null)
            {
                throw new InvalidOperationException("SchemaReader is null");
            }

            if (string.IsNullOrEmpty(this.ConnectionString))
            {
                throw new InvalidOperationException("ConnectionString is null or empty.");
            }

            try
            {
                await Task.Run(() => this.SchemaReader.TestConnectionString(this.ConnectionString));

                return string.Empty;
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder();

                Exception currentException = ex;

                while (currentException != null)
                {
                    if (message.Length > 0)
                    {
                        message.Append(Environment.NewLine + "--> ");
                    }

                    message.Append(currentException.Message);

                    currentException = currentException.InnerException;
                }

                return message.ToString();
            }
        }

        public async Task LoadTables()
        {
            if (this.SchemaReader == null)
            {
                throw new InvalidOperationException("SchemaReader is null.");
            }

            if (this.ConnectionString == null)
            {
                throw new InvalidOperationException("ConnectionString is not set.");
            }

            this.Tables = await Task.Run(() => this.SchemaReader.ReadSchema(this.ConnectionString));

            this.ApplyNamingGenerators();
        }

        public void ApplyNamingGenerators(Table table)
        {
            table.GeneratedClassName = table.Name;
            foreach (TableNameGeneratorPlugIn tableNameGenerator in this.TableNameGenerators)
            {
                table.GeneratedClassName = tableNameGenerator.GetClassName(table);
            }

            this.ApplyColumnNamingGenerators(table);

            this.OnTableRenamed(table);
        }

        public void ApplyNamingGenerators()
        {
            Parallel.ForEach(this.Tables, this.ApplyNamingGenerators);
        }

        public async Task GenerateCode(TextWriter stream, OutputWriterPlugIn outputWriter, OutputInformation outputInformation)
        {
            if (this.OutputWriters == null)
            {
                throw new InvalidOperationException("OutputWriter is null.");
            }

            IDBEscaper dbEscaper = this.UseAnsiQuoting ? new AnsiDbEscaper() : this.SchemaReader.DBEscaper;
            await Task.Run(() => outputWriter.Write(stream, this.GetTablesWithChanges(), dbEscaper, outputInformation));
        }

        public void Reset()
        {
            this.UseAnsiQuoting = false;
            this.ConnectionString = string.Empty;
            this.SchemaReader = null;
            this.TableNameGenerators.Clear();
            this.ColumnNameGenerators.Clear();
            this.Tables.Clear();
            this.TableChanges.Clear();
            this.OutputWriters.Clear();

            this.AcceptChanges();
        }

        public async Task Generate(string basePath)
        {
            if (string.IsNullOrEmpty(basePath))
            {
                throw new ArgumentException("basePath is null or empty.", "basePath");
            }

            if (!Directory.Exists(basePath))
            {
                throw new ArgumentException(string.Format("\"{0}\" not found.", basePath), "basePath");
            }

            this.CheckConfiguration();

            await this.LoadTables();

            this.ApplyNamingGenerators();

            TableCollection tablesWithChanges = this.GetTablesWithChanges();

            IDBEscaper dbEscaper = this.UseAnsiQuoting ? new AnsiDbEscaper() : this.SchemaReader.DBEscaper;
            foreach (OutputWriterPlugIn outputWriter in this.OutputWriters)
            {
                string path = Path.Combine(basePath, outputWriter.FileName);
                using (FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read))
                using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
                {
                    outputWriter.Write(writer, tablesWithChanges, dbEscaper, null);
                }
            }
        }

        public Definition GetDefinition()
        {
            Definition definition = new Definition();
            definition.ConnectionString = this.ConnectionString;
            definition.UseAnsiQuoting = this.UseAnsiQuoting;

            if (this.SchemaReader != null)
            {
                SettingsRepository settings = (this.SchemaReader.Settings != null) ? this.SchemaReader.Settings.Serialize() : null;
                definition.SchemaReader = new PlugIn(this.SchemaReader.Metadata.Guid, this.SchemaReader.Metadata.Name, settings);
            }

            foreach (TableNameGeneratorPlugIn tableNameGenerator in this.TableNameGenerators)
            {
                SettingsRepository settings = (tableNameGenerator.Settings != null) ? tableNameGenerator.Settings.Serialize() : null;
                definition.TableNameGenerators.Add(new PlugIn(tableNameGenerator.Guid, tableNameGenerator.Name, settings));
            }

            foreach (ColumnNameGeneratorPlugIn columnNameGenerator in this.ColumnNameGenerators)
            {
                SettingsRepository settings = (columnNameGenerator.Settings != null) ? columnNameGenerator.Settings.Serialize() : null;
                definition.ColumnNameGenerators.Add(new PlugIn(columnNameGenerator.Guid, columnNameGenerator.Name, settings));
            }

            definition.Tables.AddRange(this.TableChanges.Select(t => t.ToTable()));

            foreach (OutputWriterPlugIn outputWriter in this.OutputWriters)
            {
                SettingsRepository settings = (outputWriter.Settings != null) ? outputWriter.Settings.Serialize() : null;
                definition.OutputWriters.Add(new FileFormat.OutputWriterPlugIn(outputWriter.Guid, outputWriter.Name, outputWriter.FileName, settings));
            }

            return definition;
        }

        public void SetFromDefinition(Definition definition, out List<UnknownPlugIn> unrecognizedPlugIns)
        {
            if (definition == null)
            {
                throw new ArgumentNullException("definition", "definition is null.");
            }

            unrecognizedPlugIns = new List<UnknownPlugIn>();

            this.ConnectionString = definition.ConnectionString;
            this.UseAnsiQuoting = definition.UseAnsiQuoting;

            this.LoadSchemaReader(definition, unrecognizedPlugIns);
            this.LoadTableNameGenerators(definition, unrecognizedPlugIns);
            this.LoadColumnNameGenerators(definition, unrecognizedPlugIns);

            this.Tables.Clear();
            this.TableChanges = new TableChangeCollection(definition.Tables.Select(t => t.ToTableChange()));

            this.LoadOutputWriters(definition, unrecognizedPlugIns);

            this.AcceptChanges();
        }

        private void LoadSchemaReader(Definition definition, List<UnknownPlugIn> unrecognizedPlugIns)
        {
            this.SchemaReader = null;

            if (definition.SchemaReader != null)
            {
                SchemaReaderPlugIn schemaReader = (from reader in this.AvailableSchemaReaders
                                                   where reader.Metadata.Guid == definition.SchemaReader.Guid
                                                   select reader.GetPlugIn()).FirstOrDefault();

                if (schemaReader == null)
                {
                    unrecognizedPlugIns.Add(new UnknownPlugIn(PlugInType.SchemaReader, definition.SchemaReader));
                }

                if (schemaReader != null && schemaReader.Settings != null && definition.SchemaReader.Configuration != null)
                {
                    schemaReader.Settings.Deserialize(definition.SchemaReader.Configuration);
                }

                this.SchemaReader = schemaReader;
            }
        }

        private void LoadTableNameGenerators(Definition definition, List<UnknownPlugIn> unrecognizedPlugIns)
        {
            this.TableNameGenerators.Clear();
            foreach (var definitionTableNameGenerator in definition.TableNameGenerators)
            {
                TableNameGeneratorPlugIn tableNameGenerator = (from generator in this.AvailableTableNameGenerators
                                                               where generator.Metadata.Guid == definitionTableNameGenerator.Guid
                                                               select generator.GetPlugIn()).FirstOrDefault();

                if (tableNameGenerator == null)
                {
                    unrecognizedPlugIns.Add(new UnknownPlugIn(PlugInType.TableNameGenerator, definitionTableNameGenerator));
                }

                if (tableNameGenerator != null && tableNameGenerator.Settings != null && definitionTableNameGenerator.Configuration != null)
                {
                    tableNameGenerator.Settings.Deserialize(definitionTableNameGenerator.Configuration);
                }

                this.TableNameGenerators.Add(tableNameGenerator);
            }
        }

        private void LoadColumnNameGenerators(Definition definition, List<UnknownPlugIn> unrecognizedPlugIns)
        {
            this.ColumnNameGenerators.Clear();
            foreach (var definitionColumnNameGenerator in definition.ColumnNameGenerators)
            {
                ColumnNameGeneratorPlugIn columnNameGenerator = (from generator in this.AvailableColumnNameGenerators
                                                                 where generator.Metadata.Guid == definitionColumnNameGenerator.Guid
                                                                 select generator.GetPlugIn()).FirstOrDefault();

                if (columnNameGenerator == null)
                {
                    unrecognizedPlugIns.Add(new UnknownPlugIn(PlugInType.ColumnNameGenerator, definitionColumnNameGenerator));
                }

                if (columnNameGenerator != null && columnNameGenerator.Settings != null && definitionColumnNameGenerator.Configuration != null)
                {
                    columnNameGenerator.Settings.Deserialize(definitionColumnNameGenerator.Configuration);
                }

                this.ColumnNameGenerators.Add(columnNameGenerator);
            }
        }

        private void LoadOutputWriters(Definition definition, List<UnknownPlugIn> unrecognizedPlugIns)
        {
            this.OutputWriters.Clear();

            foreach (var definitionOutputWriter in definition.OutputWriters)
            {
                OutputWriterPlugIn outputWriter = (from writer in this.AvailableOutputWriters
                                                   where writer.Metadata.Guid == definitionOutputWriter.Guid
                                                   select writer.GetPlugIn()).FirstOrDefault();

                if (outputWriter == null)
                {
                    unrecognizedPlugIns.Add(new UnknownPlugIn(PlugInType.OutputWriter, definitionOutputWriter));
                }
                else
                {
                    outputWriter.FileName = definitionOutputWriter.FileName;

                    if (outputWriter.Settings != null && definitionOutputWriter.Configuration != null)
                    {
                        outputWriter.Settings.Deserialize(definitionOutputWriter.Configuration);
                    }

                    this.OutputWriters.Add(outputWriter);
                }
            }
        }

        private void CheckConfiguration()
        {
            if (this.SchemaReader == null)
            {
                throw new InvalidOperationException("No schema reader selected.");
            }

            if (this.OutputWriters.Count == 0)
            {
                throw new InvalidOperationException("No output writer added.");
            }

            foreach (OutputWriterPlugIn outputWriter in this.OutputWriters)
            {
                if (string.IsNullOrWhiteSpace(outputWriter.FileName) || !Utilities.CheckFileNameNotAbsolute(outputWriter.FileName))
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "\"{0}\" is not a valid output filename.", outputWriter.FileName));
                }
            }
        }

        private TableCollection GetTablesWithChanges()
        {
            TableCollection clonedTables = this.Tables.Clone();
            this.TableChanges.ApplyChanges(clonedTables);

            return clonedTables;
        }

        private void ApplyColumnNamingGenerators(Table table)
        {
            foreach (Column column in table.Columns)
            {
                column.GeneratedPropertyName = column.Name;

                foreach (ColumnNameGeneratorPlugIn columnNameGenerator in this.ColumnNameGenerators)
                {
                    column.GeneratedPropertyName = columnNameGenerator.GetPropertyName(table, column);
                }
            }
        }

        private void OnTableRenamed(Table table)
        {
            EventHandler<TableEventArgs> handler = this.TableRenamed;
            if (handler != null)
            {
                handler(this, new TableEventArgs(table));
            }
        }
    }
}