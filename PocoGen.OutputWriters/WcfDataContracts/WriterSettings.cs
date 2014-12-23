using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PocoGen.Common;

namespace PocoGen.OutputWriters.WcfDataContracts
{
    public class WriterSettings : ChangeTrackingBase, ISettings
    {
        public WriterSettings()
        {
            this.ResetToDefaults();
        }

        private string @namespace;
        [Category("Code Generation")]
        [Description("The namespace for the generated classes.")]
        public string Namespace
        {
            get { return this.@namespace; }
            set { this.ChangeProperty(ref this.@namespace, value); }
        }

        private string xmlNamespace;
        [Category("Code Generation")]
        [DisplayName("Xml namespace")]
        [Description("The optional namespace for the DataContract attributes")]
        public string XmlNamespace
        {
            get { return this.xmlNamespace; }
            set { this.ChangeProperty(ref this.xmlNamespace, value); }
        }

        private bool writeName;
        [Category("Code Generation")]
        [DisplayName("Write name")]
        [Description("Whether to write the Name property for the DataContract and DataMember attributes")]
        public bool WriteName
        {
            get { return this.writeName; }
            set { this.ChangeProperty(ref this.writeName, value); }
        }

        private IndentationChar indentationChar;
        [Category("Formatting")]
        [Description("The character used to indented the code.")]
        [DisplayName("Indentation character")]
        public IndentationChar IndentationChar
        {
            get { return this.indentationChar; }
            set { this.ChangeProperty(ref this.indentationChar, value); }
        }

        private int indentationSize;
        [Category("Formatting")]
        [Description("The number of characters used for one indentation.")]
        [DisplayName("Indentation size")]
        public int IndentationSize
        {
            get { return this.indentationSize; }
            set { this.ChangeProperty(ref this.indentationSize, value); }
        }

        private ClassModifier classModifier;
        [Category("Code Generation")]
        [DisplayName("Class modifier")]
        public ClassModifier ClassModifier
        {
            get { return this.classModifier; }
            set { this.ChangeProperty(ref this.classModifier, value); }
        }

        private Language language;
        [Category("Code Generation")]
        [Description("Sets the language of the generated code.")]
        public Language Language
        {
            get { return this.language; }
            set { this.ChangeProperty(ref this.language, value); }
        }

        public SettingsRepository Serialize()
        {
            SettingsRepository repository = new SettingsRepository();
            repository.SetOption("Namespace", this.Namespace);
            repository.SetOption("XmlNamespace", this.XmlNamespace);
            repository.SetOption("WriteName", this.WriteName);
            repository.SetOption("IndentationChar", Enum.GetName(typeof(IndentationChar), this.IndentationChar));
            repository.SetOption("IndentationSize", this.IndentationSize);
            repository.SetOption("ClassModifier", Enum.GetName(typeof(ClassModifier), this.ClassModifier));
            repository.SetOption("Language", Enum.GetName(typeof(Language), this.Language));
            return repository;
        }

        public void Deserialize(SettingsRepository repository)
        {
            string stringValue;
            int intValue;
            bool boolValue;

            this.Namespace = repository.TryGetValue("Namespace", out stringValue) ? stringValue : string.Empty;
            this.XmlNamespace = repository.TryGetValue("XmlNamespace", out stringValue) ? stringValue : string.Empty;
            this.WriteName = repository.TryGetValue("WriteName", out boolValue) ? boolValue : true;
            this.IndentationChar = repository.TryGetValue("IndentationChar", out stringValue) ? (IndentationChar)Enum.Parse(typeof(IndentationChar), stringValue) : OutputWriters.IndentationChar.Space;
            this.IndentationSize = repository.TryGetValue("IndentationSize", out intValue) ? intValue : 4;
            this.ClassModifier = repository.TryGetValue("ClassModifier", out stringValue) ? (ClassModifier)Enum.Parse(typeof(ClassModifier), stringValue) : OutputWriters.ClassModifier.Public;
            this.Language = repository.TryGetValue("Language", out stringValue) ? (Language)Enum.Parse(typeof(Language), stringValue) : Language.CSharp;
        }

        public void ResetToDefaults()
        {
            this.Namespace = string.Empty;
            this.XmlNamespace = string.Empty;
            this.WriteName = true;
            this.IndentationChar = IndentationChar.Space;
            this.IndentationSize = 4;
            this.ClassModifier = OutputWriters.ClassModifier.Public;
            this.Language = OutputWriters.Language.CSharp;

            this.AcceptChanges();
        }
    }
}