﻿using System;
using System.ComponentModel;
using PocoGen.Common;

namespace PocoGen.OutputWriters.DapperExtensions
{
    public class DapperExtensionsWriterSettings : ISettings
    {
        private const string ClassNameFormatDefault = "{0}Mapping";

        public DapperExtensionsWriterSettings()
        {
            this.Namespace = string.Empty;
            this.PocoNamespace = string.Empty;
            this.IndentationChar = IndentationChar.Space;
            this.IndentationSize = 4;
            this.ClassModifier = OutputWriters.ClassModifier.Public;
            this.ClassNameFormat = DapperExtensionsWriterSettings.ClassNameFormatDefault;
            this.Language = OutputWriters.Language.CSharp;
        }

        [Category("Code Generation")]
        [Description("The namespace for the generated classes.")]
        public string Namespace { get; set; }

        [Category("Code Generation")]
        [Description("The namespace of the POCOs.")]
        [DisplayName("POCO Namespace")]
        public string PocoNamespace { get; set; }

        [Category("Formatting")]
        [Description("The character used to indented the code.")]
        [DisplayName("Indentation character")]
        public IndentationChar IndentationChar { get; set; }

        [Category("Formatting")]
        [Description("The number of characters used for one indentation.")]
        [DisplayName("Indentation size")]
        public int IndentationSize { get; set; }

        [Category("Code Generation")]
        [DisplayName("Class modifier")]
        public ClassModifier ClassModifier { get; set; }

        [Category("Code Generation")]
        [Description("The format of the mapping class name")]
        [DisplayName("Class Name Format")]
        public string ClassNameFormat { get; set; }

        [Category("Code Generation")]
        [Description("Sets the language of the generated code.")]
        public Language Language { get; set; }

        public SettingsRepository Serialize()
        {
            SettingsRepository repository = new SettingsRepository();
            repository.SetOption("Namespace", this.Namespace);
            repository.SetOption("PocoNamespace", this.PocoNamespace);
            repository.SetOption("IndentationChar", Enum.GetName(typeof(IndentationChar), this.IndentationChar));
            repository.SetOption("IndentationSize", this.IndentationSize);
            repository.SetOption("ClassModifier", Enum.GetName(typeof(ClassModifier), this.ClassModifier));
            repository.SetOption("ClassNameFormat", this.ClassNameFormat);
            repository.SetOption("Language", Enum.GetName(typeof(Language), this.Language));
            return repository;
        }

        public void Deserialize(SettingsRepository repository)
        {
            string stringValue;
            int intValue;

            this.Namespace = repository.TryGetValue("Namespace", out stringValue) ? stringValue : string.Empty;
            this.PocoNamespace = repository.TryGetValue("PocoNamespace", out stringValue) ? stringValue : string.Empty;
            this.IndentationChar = repository.TryGetValue("IndentationChar", out stringValue) ? (IndentationChar)Enum.Parse(typeof(IndentationChar), stringValue) : OutputWriters.IndentationChar.Space;
            this.IndentationSize = repository.TryGetValue("IndentationSize", out intValue) ? intValue : 4;
            this.ClassModifier = repository.TryGetValue("ClassModifier", out stringValue) ? (ClassModifier)Enum.Parse(typeof(ClassModifier), stringValue) : OutputWriters.ClassModifier.Public;
            this.ClassNameFormat = repository.TryGetValue("ClassNameFormat", out stringValue) ? stringValue : DapperExtensionsWriterSettings.ClassNameFormatDefault;
            this.Language = repository.TryGetValue("Language", out stringValue) ? (Language)Enum.Parse(typeof(Language), stringValue) : Language.CSharp;
        }
    }
}