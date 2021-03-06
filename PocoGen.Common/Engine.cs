﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using PocoGen.Common.FileFormat;

namespace PocoGen.Common
{
    [Export]
    public class Engine : ChangeTrackingBase
    {
        private FileFormat.TableCollection savedTables;
        private FileFormat.ForeignKeyCollection savedForeignKeys;

        public Engine()
        {
            this.UseAnsiQuoting = false;
            this.ConnectionString = string.Empty;
            this.TableNameGenerators = new TableNameGeneratorPlugInCollection();
            this.OutputWriters = new OutputWriterPlugInCollection();
            this.savedTables = new FileFormat.TableCollection();
            this.savedForeignKeys = new FileFormat.ForeignKeyCollection();
            this.Tables = new TableCollection();
            this.ForeignKeys = new ForeignKeyCollection();
            this.ColumnNameGenerators = new ColumnNameGeneratorPlugInCollection();
            this.ForeignKeyParentPropertyNameGenerators = new ForeignKeyPropertyNameGeneratorPlugInCollection();
            this.ForeignKeyChildPropertyNameGenerators = new ForeignKeyPropertyNameGeneratorPlugInCollection();
            this.UnknownPlugIns = new UnknownPlugInCollection(new List<UnknownPlugIn>());

            this.AcceptChanges();
        }

        [ImportMany(typeof(ISchemaReader))]
        public IEnumerable<Lazy<ISchemaReader, ISchemaReaderMetadata>> AvailableSchemaReaders { get; set; }

        [ImportMany(typeof(ITableNameGenerator))]
        public IEnumerable<Lazy<ITableNameGenerator, ITableNameGeneratorMetadata>> AvailableTableNameGenerators { get; set; }

        [ImportMany(typeof(IColumnNameGenerator))]
        public IEnumerable<Lazy<IColumnNameGenerator, IColumnNameGeneratorMetadata>> AvailableColumnNameGenerators { get; set; }

        [ImportMany(typeof(IForeignKeyPropertyNameGenerator))]
        public IEnumerable<Lazy<IForeignKeyPropertyNameGenerator, IForeignKeyPropertyNameGeneratorMetadata>> AvailableForeignKeyPropertyNameGenerators { get; set; }

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

        public ForeignKeyPropertyNameGeneratorPlugInCollection ForeignKeyParentPropertyNameGenerators { get; private set; }

        public ForeignKeyPropertyNameGeneratorPlugInCollection ForeignKeyChildPropertyNameGenerators { get; private set; }

        public TableCollection Tables { get; private set; }

        public ForeignKeyCollection ForeignKeys { get; private set; }

        public OutputWriterPlugInCollection OutputWriters { get; private set; }

        public UnknownPlugInCollection UnknownPlugIns { get; private set; }

        public override bool IsChanged
        {
            get
            {
                bool isSchemaReaderChanged = (this.SchemaReader == null) ? false : this.SchemaReader.IsChanged;

                return base.IsChanged ||
                    this.ColumnNameGenerators.IsChanged ||
                    this.ForeignKeyParentPropertyNameGenerators.IsChanged ||
                    this.ForeignKeyChildPropertyNameGenerators.IsChanged ||
                    this.OutputWriters.IsChanged ||
                    this.savedTables.IsChanged ||
                    this.TableNameGenerators.IsChanged ||
                    isSchemaReaderChanged;
            }
        }

        public override void AcceptChanges()
        {
            base.AcceptChanges();

            this.ColumnNameGenerators.AcceptChanges();
            this.ForeignKeyParentPropertyNameGenerators.AcceptChanges();
            this.ForeignKeyChildPropertyNameGenerators.AcceptChanges();
            this.OutputWriters.AcceptChanges();
            this.savedTables.AcceptChanges();
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

            this.Tables = await Task.Run(() => this.SchemaReader.ReadTables(this.ConnectionString));
            this.ForeignKeys = await Task.Run(() => this.SchemaReader.ReadForeignKeys(this.ConnectionString));

            Parallel.ForEach(
                this.savedTables,
                savedTable =>
                {
                    Table table = this.Tables[savedTable.Schema, savedTable.Name];
                    if (table == null)
                    {
                        return;
                    }

                    if (savedTable.ClassName != null)
                    {
                        table.EffectiveClassName = savedTable.ClassName;
                    }

                    table.Ignore = savedTable.Ignore;

                    foreach (FileFormat.Column savedColumn in savedTable.Columns)
                    {
                        Column column = table.Columns[savedColumn.Name];
                        if (column == null)
                        {
                            continue;
                        }

                        if (savedColumn.PropertyName != null)
                        {
                            column.EffectivePropertyName = savedColumn.PropertyName;
                        }

                        column.Ignore = savedColumn.Ignore;
                    }
                });

            Parallel.ForEach(
                this.savedForeignKeys,
                savedForeignKey =>
                {
                    ForeignKey foreignKey = this.ForeignKeys[savedForeignKey];
                    if (foreignKey == null)
                    {
                        return;
                    }

                    if (savedForeignKey.ParentPropertyName != null)
                    {
                        foreignKey.EffectiveParentPropertyName = savedForeignKey.ParentPropertyName;
                    }

                    if (savedForeignKey.ChildPropertyName != null)
                    {
                        foreignKey.EffectiveChildPropertyName = savedForeignKey.ChildPropertyName;
                    }

                    foreignKey.IgnoreParentProperty = savedForeignKey.IgnoreParentProperty;
                    foreignKey.IgnoreChildProperty = savedForeignKey.IgnoreChildProperty;
                });

            this.BindTablesAndForeignKeys();

            this.SubscribePropertyChangedEvents();

            this.ApplyNamingGenerators();
        }

        private void BindTablesAndForeignKeys()
        {
            foreach (ForeignKey foreignKey in this.ForeignKeys)
            {
                Table parentTable = this.Tables[foreignKey.ParentSchema, foreignKey.ParentTableName];
                Table childTable = this.Tables[foreignKey.ChildSchema, foreignKey.ChildTableName];

                foreignKey.ParentTable = parentTable;
                foreignKey.ChildTable = childTable;

                parentTable.ChildForeignKeys.Add(foreignKey);
                childTable.ParentForeignKeys.Add(foreignKey);

                ColumnCollection childPrimaryKey = childTable.GetPrimaryKeyColumns();
                if (childPrimaryKey.Select(cpk => cpk.Name).SequenceEqual(foreignKey.Columns.Select(c => c.ChildTablesColumnName)))
                {
                    foreignKey.RelationshipType = RelationshipType.OneToZeroOrOne;
                }
                else
                {
                    foreignKey.RelationshipType = RelationshipType.OneToMany;
                }
            }
        }

        private void SubscribePropertyChangedEvents()
        {
            foreach (Table table in this.Tables)
            {
                table.PropertyChanged += this.TablePropertyChanged;

                foreach (Column column in table.Columns)
                {
                    column.PropertyChanged += this.ColumnPropertyChanged;
                }
            }

            foreach (ForeignKey foreignKey in this.ForeignKeys)
            {
                foreignKey.PropertyChanged += this.ForeignKeyPropertyChanged;
            }
        }

        public void ApplyNamingGenerators(Table table)
        {
            table.GeneratedClassName = table.Name;
            foreach (TableNameGeneratorPlugIn tableNameGenerator in this.TableNameGenerators)
            {
                table.GeneratedClassName = tableNameGenerator.GetClassName(table);
            }

            this.ApplyColumnNamingGenerators(table);
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
            await Task.Run(() => outputWriter.Write(stream, this.Tables, dbEscaper, outputInformation));
        }

        public void Reset()
        {
            this.UseAnsiQuoting = false;
            this.ConnectionString = string.Empty;
            this.SchemaReader = null;
            this.TableNameGenerators.Clear();
            this.ColumnNameGenerators.Clear();
            this.ForeignKeyParentPropertyNameGenerators.Clear();
            this.ForeignKeyChildPropertyNameGenerators.Clear();
            this.Tables.Clear();
            this.ForeignKeys.Clear();
            this.savedTables.Clear();
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

            IDBEscaper dbEscaper = this.UseAnsiQuoting ? new AnsiDbEscaper() : this.SchemaReader.DBEscaper;
            foreach (OutputWriterPlugIn outputWriter in this.OutputWriters)
            {
                string path = Path.Combine(basePath, outputWriter.FileName);
                using (FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read))
                using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
                {
                    outputWriter.Write(writer, this.Tables, dbEscaper, null);
                }
            }
        }

        public void Save(string path)
        {
            Definition document = new Definition();
            this.SaveTo(document);
            document.Save(path);

            this.AcceptChanges();
        }

        public void Save(Stream stream)
        {
            Definition document = new Definition();
            this.SaveTo(document);
            document.Save(stream);

            this.AcceptChanges();
        }

        public void Save(XmlWriter writer)
        {
            Definition document = new Definition();
            this.SaveTo(document);
            document.Save(writer);

            this.AcceptChanges();
        }

        public void Load(string path, out List<UnknownPlugIn> unknownPlugIns)
        {
            Definition definition = Definition.Load(path);

            this.Load(definition, out unknownPlugIns);
        }

        public void Load(Stream stream, out List<UnknownPlugIn> unknownPlugIns)
        {
            Definition definition = Definition.Load(stream);

            this.Load(definition, out unknownPlugIns);
        }

        public void Load(XmlReader reader, out List<UnknownPlugIn> unknownPlugIns)
        {
            Definition definition = Definition.Load(reader);

            this.Load(definition, out unknownPlugIns);
        }

        private void SaveTo(Definition document)
        {
            document.ConnectionString = this.ConnectionString;
            document.UseAnsiQuoting = this.UseAnsiQuoting;

            if (this.SchemaReader != null)
            {
                SettingsRepository settings = (this.SchemaReader.Settings != null) ? this.SchemaReader.Settings.Serialize() : null;
                document.SchemaReader = new PlugIn(this.SchemaReader.Metadata.Guid, this.SchemaReader.Metadata.Name, settings);
            }

            foreach (TableNameGeneratorPlugIn tableNameGenerator in this.TableNameGenerators)
            {
                SettingsRepository settings = (tableNameGenerator.Settings != null) ? tableNameGenerator.Settings.Serialize() : null;
                document.TableNameGenerators.Add(new PlugIn(tableNameGenerator.Guid, tableNameGenerator.Name, settings));
            }

            foreach (ColumnNameGeneratorPlugIn columnNameGenerator in this.ColumnNameGenerators)
            {
                SettingsRepository settings = (columnNameGenerator.Settings != null) ? columnNameGenerator.Settings.Serialize() : null;
                document.ColumnNameGenerators.Add(new PlugIn(columnNameGenerator.Guid, columnNameGenerator.Name, settings));
            }

            foreach (ForeignKeyPropertyNameGeneratorPlugIn foreignKeyParentPropertyNameGenerator in this.ForeignKeyParentPropertyNameGenerators)
            {
                SettingsRepository settings = (foreignKeyParentPropertyNameGenerator.Settings != null) ? foreignKeyParentPropertyNameGenerator.Settings.Serialize() : null;
                document.ForeignKeyParentPropertyNameGenerators.Add(new PlugIn(foreignKeyParentPropertyNameGenerator.Guid, foreignKeyParentPropertyNameGenerator.Name, settings));
            }

            foreach (ForeignKeyPropertyNameGeneratorPlugIn foreignKeyChildPropertyNameGenerator in this.ForeignKeyChildPropertyNameGenerators)
            {
                SettingsRepository settings = (foreignKeyChildPropertyNameGenerator.Settings != null) ? foreignKeyChildPropertyNameGenerator.Settings.Serialize() : null;
                document.ForeignKeyChildPropertyNameGenerators.Add(new PlugIn(foreignKeyChildPropertyNameGenerator.Guid, foreignKeyChildPropertyNameGenerator.Name, settings));
            }

            document.Tables.AddRange(this.savedTables);

            foreach (OutputWriterPlugIn outputWriter in this.OutputWriters)
            {
                SettingsRepository settings = (outputWriter.Settings != null) ? outputWriter.Settings.Serialize() : null;
                document.OutputWriters.Add(new FileFormat.OutputWriterPlugIn(outputWriter.Guid, outputWriter.Name, outputWriter.FileName, settings));
            }
        }

        private void Load(Definition definition, out List<UnknownPlugIn> unknownPlugIns)
        {
            if (definition == null)
            {
                throw new ArgumentNullException("definition", "definition is null.");
            }

            unknownPlugIns = new List<UnknownPlugIn>();

            this.ConnectionString = definition.ConnectionString;
            this.UseAnsiQuoting = definition.UseAnsiQuoting;

            this.LoadSchemaReader(definition, unknownPlugIns);
            this.LoadTableNameGenerators(definition, unknownPlugIns);
            this.LoadColumnNameGenerators(definition, unknownPlugIns);
            this.LoadForeignKeyParentPropertyNameGenerators(definition, unknownPlugIns);
            this.LoadForeignKeyChildPropertyNameGenerators(definition, unknownPlugIns);

            this.Tables.Clear();
            this.savedTables = definition.Tables;

            this.LoadOutputWriters(definition, unknownPlugIns);

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

        private void LoadForeignKeyParentPropertyNameGenerators(Definition definition, List<UnknownPlugIn> unrecognizedPlugIns)
        {
            this.ForeignKeyParentPropertyNameGenerators.Clear();
            foreach (var definitionForeignKeyParentPropertyNameGenerator in definition.ForeignKeyParentPropertyNameGenerators)
            {
                ForeignKeyPropertyNameGeneratorPlugIn foreignKeyParentPropertyNameGenerator = (from generator in this.AvailableForeignKeyPropertyNameGenerators
                                                                                               where generator.Metadata.Guid == definitionForeignKeyParentPropertyNameGenerator.Guid
                                                                                               select generator.GetPlugIn()).FirstOrDefault();

                if (foreignKeyParentPropertyNameGenerator == null)
                {
                    unrecognizedPlugIns.Add(new UnknownPlugIn(PlugInType.ForeignKeyPropertyNameGenerator, definitionForeignKeyParentPropertyNameGenerator));
                }

                if (foreignKeyParentPropertyNameGenerator != null && foreignKeyParentPropertyNameGenerator.Settings != null && definitionForeignKeyParentPropertyNameGenerator.Configuration != null)
                {
                    foreignKeyParentPropertyNameGenerator.Settings.Deserialize(definitionForeignKeyParentPropertyNameGenerator.Configuration);
                }

                this.ForeignKeyParentPropertyNameGenerators.Add(foreignKeyParentPropertyNameGenerator);
            }
        }

        private void LoadForeignKeyChildPropertyNameGenerators(Definition definition, List<UnknownPlugIn> unrecognizedPlugIns)
        {
            this.ForeignKeyChildPropertyNameGenerators.Clear();
            foreach (var definitionForeignKeyChildPropertyNameGenerator in definition.ForeignKeyChildPropertyNameGenerators)
            {
                var foreignKeyChildPropertyNameGenerator = (from generator in this.AvailableForeignKeyPropertyNameGenerators
                                                            where generator.Metadata.Guid == definitionForeignKeyChildPropertyNameGenerator.Guid
                                                            select generator.GetPlugIn()).FirstOrDefault();

                if (foreignKeyChildPropertyNameGenerator == null)
                {
                    unrecognizedPlugIns.Add(new UnknownPlugIn(PlugInType.ForeignKeyPropertyNameGenerator, definitionForeignKeyChildPropertyNameGenerator));
                }

                if (foreignKeyChildPropertyNameGenerator != null && foreignKeyChildPropertyNameGenerator.Settings != null && definitionForeignKeyChildPropertyNameGenerator.Configuration != null)
                {
                    foreignKeyChildPropertyNameGenerator.Settings.Deserialize(definitionForeignKeyChildPropertyNameGenerator.Configuration);
                }

                this.ForeignKeyChildPropertyNameGenerators.Add(foreignKeyChildPropertyNameGenerator);
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

        private void ColumnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(Column.EffectivePropertyName) && e.PropertyName != nameof(Column.Ignore))
            {
                return;
            }

            Column column = (Column)sender;
            Table table = column.Table;
            if (FileFormat.Column.AreDefaultValues(column.Ignore, column.UserChangedPropertyName))
            {
                // Remove the saved column
                FileFormat.Table savedTable = this.savedTables[table.Schema, table.Name];
                if (savedTable == null)
                {
                    // There is no table containing this column, return
                    return;
                }

                FileFormat.Column savedColumn = savedTable.Columns[column.Name];
                if (savedColumn != null)
                {
                    savedTable.Columns.Remove(savedColumn);

                    // Remove the savedTable if it has no changes
                    if (savedTable.HasDefaultValues)
                    {
                        this.savedTables.Remove(savedTable);
                    }
                }
            }
            else
            {
                // Create or update the saved column
                FileFormat.Table savedTable = this.savedTables[table.Schema, table.Name];
                if (savedTable == null)
                {
                    // Saved column does not exist, create it
                    savedTable = new FileFormat.Table(table.Schema, table.Name, table.UserChangedClassName, table.Ignore);
                    this.savedTables.Add(savedTable);
                }

                FileFormat.Column savedColumn = savedTable.Columns[column.Name];
                if (savedColumn == null)
                {
                    // Create and add the saved column
                    savedColumn = new FileFormat.Column(column.Name, column.UserChangedPropertyName, column.Ignore);
                    savedTable.Columns.Add(savedColumn);
                }
                else
                {
                    // Update the saved columnNameGenerators
                    savedColumn.Ignore = column.Ignore;
                    savedColumn.PropertyName = column.UserChangedPropertyName;
                }
            }
        }

        private void TablePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(Table.EffectiveClassName) && e.PropertyName != nameof(Table.Ignore))
            {
                return;
            }

            Table table = (Table)sender;
            if (FileFormat.Table.AreDefaultValues(table.Ignore, table.UserChangedClassName))
            {
                FileFormat.Table savedTable = this.savedTables[table.Schema, table.Name];
                if (savedTable != null)
                {
                    this.savedTables.Remove(savedTable);
                }
            }
            else
            {
                FileFormat.Table savedTable = this.savedTables[table.Schema, table.Name];
                if (savedTable == null)
                {
                    savedTable = new FileFormat.Table(table.Schema, table.Name, table.UserChangedClassName, table.Ignore);
                    this.savedTables.Add(savedTable);
                }
                else
                {
                    savedTable.ClassName = table.UserChangedClassName;
                    savedTable.Ignore = table.Ignore;
                }
            }
        }

        private void ForeignKeyPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(ForeignKey.EffectiveParentPropertyName) &&
                e.PropertyName != nameof(ForeignKey.IgnoreParentProperty) &&
                e.PropertyName != nameof(ForeignKey.EffectiveChildPropertyName) &&
                e.PropertyName != nameof(ForeignKey.IgnoreChildProperty))
            {
                return;
            }

            ForeignKey foreignKey = (ForeignKey)sender;
            if (FileFormat.ForeignKey.AreDefaultValues(foreignKey.IgnoreParentProperty, foreignKey.UserChangedParentPropertyName, foreignKey.IgnoreChildProperty, foreignKey.UserChangedChildPropertyName))
            {
                FileFormat.ForeignKey savedForeignKey = this.savedForeignKeys[foreignKey];
                if (savedForeignKey != null)
                {
                    this.savedForeignKeys.Remove(savedForeignKey);
                }
            }
            else
            {
                FileFormat.ForeignKey savedForeignKey = this.savedForeignKeys[foreignKey];
                if (savedForeignKey == null)
                {
                    savedForeignKey = new FileFormat.ForeignKey(foreignKey.Schema, foreignKey.Name, foreignKey.ParentSchema, foreignKey.ParentTableName, foreignKey.ChildSchema, foreignKey.ChildTableName);
                    this.savedForeignKeys.Add(savedForeignKey);
                }
                else
                {
                    savedForeignKey.ParentPropertyName = foreignKey.UserChangedParentPropertyName;
                    savedForeignKey.IgnoreParentProperty = foreignKey.IgnoreParentProperty;
                    savedForeignKey.ChildPropertyName = foreignKey.UserChangedChildPropertyName;
                    savedForeignKey.IgnoreChildProperty = foreignKey.IgnoreChildProperty;
                }
            }
        }
    }
}