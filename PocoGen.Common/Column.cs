﻿using System;
using System.ComponentModel;

namespace PocoGen.Common
{
    /// <summary>
    /// Represents a column in a table.
    /// </summary>
    public class Column : IChangeTracking
    {
        private readonly FileFormat.Column fileFormatColumn = new FileFormat.Column();

        /// <summary>
        /// Creates a new column.
        /// </summary>
        /// <param name="table">The column's table.</param>
        /// <param name="name">The column's name.</param>
        public Column(Table table, string name)
        {
            if (table == null)
                throw new ArgumentNullException("table", "table is null.");
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("name is null or empty.", "name");

            this.Name = name;
            this.fileFormatColumn.Name = name;
            this.Table = table;
            this.GeneratedPropertyName = string.Empty;
            this.PropertyType = null;
            this.IsPK = false;
            this.IsNullable = false;
            this.IsAutoIncrement = false;
            this.Ignore = false;
        }

        /// <summary>
        /// Creates a new column.
        /// </summary>
        /// <param name="table">The column's table.</param>
        /// <param name="name">The column's name.</param>
        /// <param name="propertyType">The column's property type.</param>
        /// <param name="isNullable">Whether the column can be null.</param>
        /// <param name="isAutoIncrement">Whether the column is an identity column.</param>
        public Column(Table table, string name, ColumnBaseType propertyType, bool isNullable, bool isAutoIncrement)
            : this(table, name)
        {
            if (propertyType == null)
                throw new ArgumentNullException("propertyType", "propertyType is null.");

            this.PropertyType = propertyType;
            this.IsNullable = isNullable;
            this.IsAutoIncrement = isAutoIncrement;
        }

        /// <summary>
        /// Gets the column's table.
        /// </summary>
        public Table Table { get; private set; }

        /// <summary>
        /// Gets or sets the column name in the database.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets the property name which was generated by the column name generators.
        /// </summary>
        public string GeneratedPropertyName { get; set; }

        /// <summary>
        /// Gets or sets the effective property name. This is the <see cref="GeneratedPropertyName"/> if the user didn't change it, otherwise it is the user-changed property name.
        /// </summary>
        public string EffectivePropertyName
        {
            get
            {
                // If the user changed the property name, it was saved to the fileFormatColumn. Otherwise it is the GeneratedPropertyName.
                return this.fileFormatColumn.PropertyName ?? this.GeneratedPropertyName;
            }
            set
            {
                // If the user changes the property name, save the override in the fileFormatColumn.
                if (this.GeneratedPropertyName != value)
                {
                    this.fileFormatColumn.PropertyName = value;
                }
                else
                {
                    this.fileFormatColumn.PropertyName = null;
                }
            }
        }

        /// <summary>
        /// Gets or sets the property's type.
        /// </summary>
        public ColumnBaseType PropertyType { get; set; }

        /// <summary>
        /// Gets or sets whether this column is part of a primary key.
        /// </summary>
        public bool IsPK { get; set; }

        /// <summary>
        /// Gets or sets whether this class is nullable.
        /// </summary>
        public bool IsNullable { get; set; }

        /// <summary>
        /// Gets or sets whether this column is an identity column.
        /// </summary>
        public bool IsAutoIncrement { get; set; }

        /// <summary>
        /// Gets or sets whether this column should be included in the POCO.
        /// </summary>
        public bool Ignore
        {
            get
            {
                return this.fileFormatColumn.Ignore;
            }
            set
            {
                this.fileFormatColumn.Ignore = value;
            }
        }

        internal void ApplyExistingFileFormatColumn(FileFormat.Column column)
        {
            if (column == null)
                throw new ArgumentNullException("column", "column is null.");
            if (this.fileFormatColumn.Name != column.Name)
                throw new ArgumentException("Wrong column for this instance.", "column");

            this.fileFormatColumn.Ignore = column.Ignore;
            this.fileFormatColumn.PropertyName = column.PropertyName;

            if (!column.IsChanged)
                column.AcceptChanges();
        }

        /// <summary>
        /// Gets the <see cref="FileFormat.Column"/> for this column if the user changed some default value; otherwise null.
        /// </summary>
        /// <returns>The <see cref="FileFormat.Column"/> for this column or null if it is not needed.</returns>
        internal FileFormat.Column GetFileFormatColumn()
        {
            if (this.fileFormatColumn.Ignore || this.fileFormatColumn.PropertyName != null)
                return this.fileFormatColumn;

            return null;
        }

        public void AcceptChanges()
        {
            this.fileFormatColumn.AcceptChanges();
        }

        public bool IsChanged
        {
            get { return this.fileFormatColumn.IsChanged; }
        }
    }
}