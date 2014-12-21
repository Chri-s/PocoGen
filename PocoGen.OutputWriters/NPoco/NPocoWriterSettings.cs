﻿using System;
using System.ComponentModel;
using PocoGen.Common;

namespace PocoGen.OutputWriters.NPoco
{
    public class NPocoWriterSettings : ChangeTrackingBase, ISettings
    {
        public NPocoWriterSettings()
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

        private bool includeSchema;
        [Category("Code Generation")]
        [Description("Include the schema name in the TableNameAttibute.")]
        [DisplayName("Include Schema")]
        public bool IncludeSchema
        {
            get { return this.includeSchema; }
            set { this.ChangeProperty(ref this.includeSchema, value); }
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
            repository.SetOption("IndentationChar", Enum.GetName(typeof(IndentationChar), this.IndentationChar));
            repository.SetOption("IndentationSize", this.IndentationSize);
            repository.SetOption("ClassModifier", Enum.GetName(typeof(ClassModifier), this.ClassModifier));
            repository.SetOption("Language", Enum.GetName(typeof(Language), this.Language));
            repository.SetOption("IncludeSchema", this.IncludeSchema);
            return repository;
        }

        public void Deserialize(SettingsRepository repository)
        {
            string stringValue;
            int intValue;
            bool boolValue;

            this.Namespace = repository.TryGetValue("Namespace", out stringValue) ? stringValue : string.Empty;
            this.IndentationChar = repository.TryGetValue("IndentationChar", out stringValue) ? (IndentationChar)Enum.Parse(typeof(IndentationChar), stringValue) : OutputWriters.IndentationChar.Space;
            this.IndentationSize = repository.TryGetValue("IndentationSize", out intValue) ? intValue : 4;
            this.ClassModifier = repository.TryGetValue("ClassModifier", out stringValue) ? (ClassModifier)Enum.Parse(typeof(ClassModifier), stringValue) : OutputWriters.ClassModifier.Public;
            this.Language = repository.TryGetValue("Language", out stringValue) ? (Language)Enum.Parse(typeof(Language), stringValue) : Language.CSharp;
            this.IncludeSchema = repository.TryGetValue("IncludeSchema", out boolValue) ? boolValue : false;
        }

        public void ResetToDefaults()
        {
            this.Namespace = string.Empty;
            this.IndentationChar = IndentationChar.Space;
            this.IndentationSize = 4;
            this.ClassModifier = OutputWriters.ClassModifier.Public;
            this.Language = OutputWriters.Language.CSharp;
            this.IncludeSchema = false;

            this.AcceptChanges();
        }
    }
}