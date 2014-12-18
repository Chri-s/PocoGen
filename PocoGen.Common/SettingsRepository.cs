using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using System.Xml.Serialization;

namespace PocoGen.Common
{
    public sealed class SettingsRepository : IXmlSerializable
    {
        private readonly Dictionary<string, SettingsType> optionTypes = new Dictionary<string, SettingsType>();
        private readonly Dictionary<string, string> stringOptions = new Dictionary<string, string>();
        private readonly Dictionary<string, bool> boolOptions = new Dictionary<string, bool>();
        private readonly Dictionary<string, int> intOptions = new Dictionary<string, int>();
        private readonly Dictionary<string, decimal> decimalOptions = new Dictionary<string, decimal>();
        private readonly Dictionary<string, DateTime> dateTimeOptions = new Dictionary<string, DateTime>();

        public void SetOption(string name, string value)
        {
            this.SetOption(name, SettingsType.String, this.stringOptions, value);
        }

        public void SetOption(string name, bool value)
        {
            this.SetOption(name, SettingsType.Boolean, this.boolOptions, value);
        }

        public void SetOption(string name, int value)
        {
            this.SetOption(name, SettingsType.Int32, this.intOptions, value);
        }

        public void SetOption(string name, decimal value)
        {
            this.SetOption(name, SettingsType.Decimal, this.decimalOptions, value);
        }

        public void SetOption(string name, DateTime value)
        {
            this.SetOption(name, SettingsType.DateTime, this.dateTimeOptions, value);
        }

        public bool ContainsOption(string name)
        {
            return this.optionTypes.ContainsKey(name);
        }

        public bool TryGetValue(string name, out bool value)
        {
            return this.boolOptions.TryGetValue(name, out value);
        }

        public bool TryGetValue(string name, out DateTime value)
        {
            return this.dateTimeOptions.TryGetValue(name, out value);
        }

        public bool TryGetValue(string name, out decimal value)
        {
            return this.decimalOptions.TryGetValue(name, out value);
        }

        public bool TryGetValue(string name, out int value)
        {
            return this.intOptions.TryGetValue(name, out value);
        }

        public bool TryGetValue(string name, out string value)
        {
            return this.stringOptions.TryGetValue(name, out value);
        }

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