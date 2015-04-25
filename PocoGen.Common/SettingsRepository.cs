using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using System.Xml.Serialization;

namespace PocoGen.Common
{
    /// <summary>
    /// Stores settings for modules in a dictionary-like way. These settings can be persisted and loaded from a XmlSerializer.
    /// </summary>
    public sealed class SettingsRepository : IXmlSerializable
    {
        private readonly Dictionary<string, SettingsType> optionTypes = new Dictionary<string, SettingsType>();
        private readonly Dictionary<string, string> stringOptions = new Dictionary<string, string>();
        private readonly Dictionary<string, bool> boolOptions = new Dictionary<string, bool>();
        private readonly Dictionary<string, int> intOptions = new Dictionary<string, int>();
        private readonly Dictionary<string, decimal> decimalOptions = new Dictionary<string, decimal>();
        private readonly Dictionary<string, DateTime> dateTimeOptions = new Dictionary<string, DateTime>();

        /// <summary>
        /// Sets a setting with a String value.
        /// </summary>
        /// <param name="name">The name of the setting.</param>
        /// <param name="value">The value of the setting.</param>
        public void SetOption(string name, string value)
        {
            this.SetOption(name, SettingsType.String, this.stringOptions, value);
        }

        /// <summary>
        /// Sets a setting with a Boolean value.
        /// </summary>
        /// <param name="name">The name of the setting.</param>
        /// <param name="value">The value of the setting.</param>
        public void SetOption(string name, bool value)
        {
            this.SetOption(name, SettingsType.Boolean, this.boolOptions, value);
        }

        /// <summary>
        /// Sets a setting with a Int32 value.
        /// </summary>
        /// <param name="name">The name of the setting.</param>
        /// <param name="value">The value of the setting.</param>
        public void SetOption(string name, int value)
        {
            this.SetOption(name, SettingsType.Int32, this.intOptions, value);
        }

        /// <summary>
        /// Sets a setting with a Decimal value.
        /// </summary>
        /// <param name="name">The name of the setting.</param>
        /// <param name="value">The value of the setting.</param>
        public void SetOption(string name, decimal value)
        {
            this.SetOption(name, SettingsType.Decimal, this.decimalOptions, value);
        }

        /// <summary>
        /// Sets a setting with a DateTime value.
        /// </summary>
        /// <param name="name">The name of the setting.</param>
        /// <param name="value">The value of the setting.</param>
        public void SetOption(string name, DateTime value)
        {
            this.SetOption(name, SettingsType.DateTime, this.dateTimeOptions, value);
        }

        /// <summary>
        /// Returns whether a specified setting is included in this repository.
        /// </summary>
        /// <param name="name">The name of the setting.</param>
        /// <returns>true if this repository contains the setting, otherwise false.</returns>
        public bool ContainsOption(string name)
        {
            return this.optionTypes.ContainsKey(name);
        }

        /// <summary>
        /// Tries to retrieve a Boolean setting from this repository.
        /// </summary>
        /// <param name="name">The name of the setting.</param>
        /// <param name="value">The value of the setting if it is included in this repository.</param>
        /// <returns>true if this setting is included in this repository, otherwise false.</returns>
        public bool TryGetValue(string name, out bool value)
        {
            return this.boolOptions.TryGetValue(name, out value);
        }

        /// <summary>
        /// Tries to retrieve a DateTime setting from this repository.
        /// </summary>
        /// <param name="name">The name of the setting.</param>
        /// <param name="value">The value of the setting if it is included in this repository.</param>
        /// <returns>true if this setting is included in this repository, otherwise false.</returns>
        public bool TryGetValue(string name, out DateTime value)
        {
            return this.dateTimeOptions.TryGetValue(name, out value);
        }

        /// <summary>
        /// Tries to retrieve a Decimal setting from this repository.
        /// </summary>
        /// <param name="name">The name of the setting.</param>
        /// <param name="value">The value of the setting if it is included in this repository.</param>
        /// <returns>true if this setting is included in this repository, otherwise false.</returns>
        public bool TryGetValue(string name, out decimal value)
        {
            return this.decimalOptions.TryGetValue(name, out value);
        }

        /// <summary>
        /// Tries to retrieve a Int32 setting from this repository.
        /// </summary>
        /// <param name="name">The name of the setting.</param>
        /// <param name="value">The value of the setting if it is included in this repository.</param>
        /// <returns>true if this setting is included in this repository, otherwise false.</returns>
        public bool TryGetValue(string name, out int value)
        {
            return this.intOptions.TryGetValue(name, out value);
        }

        /// <summary>
        /// Tries to retrieve a String setting from this repository.
        /// </summary>
        /// <param name="name">The name of the setting.</param>
        /// <param name="value">The value of the setting if it is included in this repository.</param>
        /// <returns>true if this setting is included in this repository, otherwise false.</returns>
        public bool TryGetValue(string name, out string value)
        {
            return this.stringOptions.TryGetValue(name, out value);
        }

        /// <summary>
        /// Removes a specified setting from this repository.
        /// </summary>
        /// <param name="name">The name of the setting.</param>
        public void RemoveOption(string name)
        {
            if (!this.optionTypes.ContainsKey(name))
            {
                return;
            }

            switch (this.optionTypes[name])
            {
                case SettingsType.String:
                    this.stringOptions.Remove(name);
                    break;
                case SettingsType.Boolean:
                    this.boolOptions.Remove(name);
                    break;
                case SettingsType.Int32:
                    this.intOptions.Remove(name);
                    break;
                case SettingsType.Decimal:
                    this.decimalOptions.Remove(name);
                    break;
                case SettingsType.DateTime:
                    this.dateTimeOptions.Remove(name);
                    break;
                default:
                    throw new InvalidOperationException("Invalid option type.");
            }

            this.optionTypes.Remove(name);
        }

        private static void WriteOption(XmlWriter writer, string elementName, string propertyName, string value)
        {
            writer.WriteStartElement(elementName);
            writer.WriteAttributeString("name", propertyName);
            writer.WriteValue(value);
            writer.WriteEndElement();
        }

        private void SetOption<T>(string name, SettingsType type, Dictionary<string, T> dictionary, T value)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("name is null or empty.", "name");
            }

            this.RemoveOption(name);

            this.optionTypes.Add(name, type);
            dictionary.Add(name, value);
        }

        private void ImportValue(string type, string propertyName, string value)
        {
            switch (type)
            {
                case "Boolean":
                    this.SetOption(propertyName, bool.Parse(value));
                    break;

                case "DateTime":
                    this.SetOption(propertyName, DateTime.ParseExact(value, "r", CultureInfo.InvariantCulture));
                    break;

                case "Decimal":
                    this.SetOption(propertyName, decimal.Parse(value, CultureInfo.InvariantCulture));
                    break;

                case "Int32":
                    this.SetOption(propertyName, int.Parse(value, CultureInfo.InvariantCulture));
                    break;

                case "String":
                    this.SetOption(propertyName, value);
                    break;

                default:
                    throw new ArgumentException("Invalid property type: " + type);
            }
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            foreach (var item in this.boolOptions)
            {
                SettingsRepository.WriteOption(writer, "Boolean", item.Key, item.Value.ToString());
            }

            foreach (var item in this.dateTimeOptions)
            {
                SettingsRepository.WriteOption(writer, "DateTime", item.Key, item.Value.ToString("r", CultureInfo.InvariantCulture));
            }

            foreach (var item in this.decimalOptions)
            {
                SettingsRepository.WriteOption(writer, "Decimal", item.Key, item.Value.ToString(CultureInfo.InvariantCulture));
            }

            foreach (var item in this.intOptions)
            {
                SettingsRepository.WriteOption(writer, "Int32", item.Key, item.Value.ToString(CultureInfo.InvariantCulture));
            }

            foreach (var item in this.stringOptions)
            {
                SettingsRepository.WriteOption(writer, "String", item.Key, item.Value);
            }
        }

        System.Xml.Schema.XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            reader.MoveToElement();
            if (reader.IsEmptyElement)
            {
                reader.Skip();
                return;
            }

            reader.ReadStartElement();

            reader.MoveToContent();
            while (reader.NodeType != XmlNodeType.EndElement && reader.NodeType != XmlNodeType.None)
            {
                this.ReadOption(reader);
                reader.MoveToContent();
            }

            reader.ReadEndElement();
        }

        private void ReadOption(XmlReader reader)
        {
            string type = reader.LocalName;
            string propertyName = reader.GetAttribute("name");
            string value = reader.ReadElementString();

            this.ImportValue(type, propertyName, value);
        }
    }
}