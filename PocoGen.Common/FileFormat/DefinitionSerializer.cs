﻿/* =================================================================================================
 * IMPORTANT: You need to regenerate this file if any class in the namespace
 *            --> PocoGen.Common.FileFormat <--
 *            changes!
 *            
 *            To regenerate this file, recompile this project, open the Visual Studio Developer
 *            command prompt, cd to the build/output directory of this project and type:
 *            
 *            SGen.exe /a:PocoGen.Common.dll /t:PocoGen.Common.FileFormat.Definition /keep
 *            
 *            SGen.exe will complain about read only properties, but these warnings can be ignored.
 *            Include the file and remove all lines which generate compiler errors.
 * ================================================================================================= */

namespace PocoGen.Common.FileFormat
{
    internal class XmlSerializationWriterDefinition : System.Xml.Serialization.XmlSerializationWriter
    {

        public void Write10_PocoGenDefinition(object o)
        {
            WriteStartDocument();
            if (o == null)
            {
                WriteNullTagLiteral(@"PocoGenDefinition", @"");
                return;
            }
            TopLevelElement();
            Write9_Definition(@"PocoGenDefinition", @"", ((global::PocoGen.Common.FileFormat.Definition)o), true, false);
        }

        void Write9_Definition(string n, string ns, global::PocoGen.Common.FileFormat.Definition o, bool isNullable, bool needType)
        {
            if ((object)o == null)
            {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType)
            {
                System.Type t = o.GetType();
                if (t == typeof(global::PocoGen.Common.FileFormat.Definition))
                {
                }
                else
                {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"Definition", @"");
            Write2_PlugIn(@"SchemaReader", @"", ((global::PocoGen.Common.FileFormat.PlugIn)o.@SchemaReader), false, false);
            WriteElementString(@"ConnectionString", @"", ((global::System.String)o.@ConnectionString));
            WriteElementStringRaw(@"UseAnsiQuoting", @"", System.Xml.XmlConvert.ToString((global::System.Boolean)((global::System.Boolean)o.@UseAnsiQuoting)));
            {
                global::PocoGen.Common.FileFormat.PlugInCollection a = (global::PocoGen.Common.FileFormat.PlugInCollection)((global::PocoGen.Common.FileFormat.PlugInCollection)o.@TableNameGenerators);
                if (a != null)
                {
                    WriteStartElement(@"TableNameGenerators", @"", null, false);
                    for (int ia = 0; ia < ((System.Collections.ICollection)a).Count; ia++)
                    {
                        Write2_PlugIn(@"TableNameGenerator", @"", ((global::PocoGen.Common.FileFormat.PlugIn)a[ia]), true, false);
                    }
                    WriteEndElement();
                }
            }
            {
                global::PocoGen.Common.FileFormat.PlugInCollection a = (global::PocoGen.Common.FileFormat.PlugInCollection)((global::PocoGen.Common.FileFormat.PlugInCollection)o.@ColumnNameGenerators);
                if (a != null)
                {
                    WriteStartElement(@"ColumnNameGenerators", @"", null, false);
                    for (int ia = 0; ia < ((System.Collections.ICollection)a).Count; ia++)
                    {
                        Write2_PlugIn(@"ColumnNameGenerator", @"", ((global::PocoGen.Common.FileFormat.PlugIn)a[ia]), true, false);
                    }
                    WriteEndElement();
                }
            }
            {
                global::PocoGen.Common.FileFormat.PlugInCollection a = (global::PocoGen.Common.FileFormat.PlugInCollection)((global::PocoGen.Common.FileFormat.PlugInCollection)o.@ForeignKeyParentPropertyNameGenerators);
                if (a != null)
                {
                    WriteStartElement(@"ForeignKeyParentPropertyNameGenerators", @"", null, false);
                    for (int ia = 0; ia < ((System.Collections.ICollection)a).Count; ia++)
                    {
                        Write2_PlugIn(@"ForeignKeyPropertyNameGenerator", @"", ((global::PocoGen.Common.FileFormat.PlugIn)a[ia]), true, false);
                    }
                    WriteEndElement();
                }
            }
            {
                global::PocoGen.Common.FileFormat.PlugInCollection a = (global::PocoGen.Common.FileFormat.PlugInCollection)((global::PocoGen.Common.FileFormat.PlugInCollection)o.@ForeignKeyChildPropertyNameGenerators);
                if (a != null)
                {
                    WriteStartElement(@"ForeignKeyChildPropertyNameGenerators", @"", null, false);
                    for (int ia = 0; ia < ((System.Collections.ICollection)a).Count; ia++)
                    {
                        Write2_PlugIn(@"ForeignKeyPropertyNameGenerator", @"", ((global::PocoGen.Common.FileFormat.PlugIn)a[ia]), true, false);
                    }
                    WriteEndElement();
                }
            }
            {
                global::PocoGen.Common.FileFormat.TableCollection a = (global::PocoGen.Common.FileFormat.TableCollection)((global::PocoGen.Common.FileFormat.TableCollection)o.@Tables);
                if (a != null)
                {
                    WriteStartElement(@"Tables", @"", null, false);
                    for (int ia = 0; ia < ((System.Collections.ICollection)a).Count; ia++)
                    {
                        Write5_Table(@"Table", @"", ((global::PocoGen.Common.FileFormat.Table)a[ia]), true, false);
                    }
                    WriteEndElement();
                }
            }
            {
                global::PocoGen.Common.FileFormat.ForeignKeyCollection a = (global::PocoGen.Common.FileFormat.ForeignKeyCollection)((global::PocoGen.Common.FileFormat.ForeignKeyCollection)o.@ForeignKeys);
                if (a != null)
                {
                    WriteStartElement(@"ForeignKeys", @"", null, false);
                    for (int ia = 0; ia < ((System.Collections.ICollection)a).Count; ia++)
                    {
                        Write7_ForeignKey(@"ForeignKey", @"", ((global::PocoGen.Common.FileFormat.ForeignKey)a[ia]), true, false);
                    }
                    WriteEndElement();
                }
            }
            {
                global::PocoGen.Common.FileFormat.OutputWriterPlugInCollection a = (global::PocoGen.Common.FileFormat.OutputWriterPlugInCollection)((global::PocoGen.Common.FileFormat.OutputWriterPlugInCollection)o.@OutputWriters);
                if (a != null)
                {
                    WriteStartElement(@"OutputWriters", @"", null, false);
                    for (int ia = 0; ia < ((System.Collections.ICollection)a).Count; ia++)
                    {
                        Write8_OutputWriterPlugIn(@"OutputWriter", @"", ((global::PocoGen.Common.FileFormat.OutputWriterPlugIn)a[ia]), true, false);
                    }
                    WriteEndElement();
                }
            }
            WriteElementString(@"OutputBasePath", @"", ((global::System.String)o.@OutputBasePath));
            WriteEndElement(o);
        }

        void Write8_OutputWriterPlugIn(string n, string ns, global::PocoGen.Common.FileFormat.OutputWriterPlugIn o, bool isNullable, bool needType)
        {
            if ((object)o == null)
            {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType)
            {
                System.Type t = o.GetType();
                if (t == typeof(global::PocoGen.Common.FileFormat.OutputWriterPlugIn))
                {
                }
                else
                {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"OutputWriterPlugIn", @"");
            WriteElementString(@"Guid", @"", ((global::System.String)o.@Guid));
            WriteElementString(@"Name", @"", ((global::System.String)o.@Name));
            WriteSerializable((System.Xml.Serialization.IXmlSerializable)((global::PocoGen.Common.SettingsRepository)o.@Configuration), @"Configuration", @"", false, true);
            WriteElementString(@"FileName", @"", ((global::System.String)o.@FileName));
            WriteEndElement(o);
        }

        void Write7_ForeignKey(string n, string ns, global::PocoGen.Common.FileFormat.ForeignKey o, bool isNullable, bool needType)
        {
            if ((object)o == null)
            {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType)
            {
                System.Type t = o.GetType();
                if (t == typeof(global::PocoGen.Common.FileFormat.ForeignKey))
                {
                }
                else
                {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"ForeignKey", @"");
            WriteElementString(@"ParentTableSchema", @"", ((global::System.String)o.@ParentTableSchema));
            WriteElementString(@"ParentTable", @"", ((global::System.String)o.@ParentTable));
            WriteElementString(@"ChildTableSchema", @"", ((global::System.String)o.@ChildTableSchema));
            WriteElementString(@"ChildTable", @"", ((global::System.String)o.@ChildTable));
            WriteElementString(@"Schema", @"", ((global::System.String)o.@Schema));
            WriteElementString(@"Name", @"", ((global::System.String)o.@Name));
            {
                global::PocoGen.Common.FileFormat.ForeignKeyColumnCollection a = (global::PocoGen.Common.FileFormat.ForeignKeyColumnCollection)((global::PocoGen.Common.FileFormat.ForeignKeyColumnCollection)o.@Columns);
                if (a != null)
                {
                    WriteStartElement(@"Columns", @"", null, false);
                    for (int ia = 0; ia < ((System.Collections.ICollection)a).Count; ia++)
                    {
                        Write6_ForeignKeyColumn(@"Column", @"", ((global::PocoGen.Common.FileFormat.ForeignKeyColumn)a[ia]), true, false);
                    }
                    WriteEndElement();
                }
            }
            WriteElementStringRaw(@"IgnoreChildProperty", @"", System.Xml.XmlConvert.ToString((global::System.Boolean)((global::System.Boolean)o.@IgnoreChildProperty)));
            WriteElementStringRaw(@"IgnoreParentProperty", @"", System.Xml.XmlConvert.ToString((global::System.Boolean)((global::System.Boolean)o.@IgnoreParentProperty)));
            WriteElementString(@"ChildPropertyName", @"", ((global::System.String)o.@ChildPropertyName));
            WriteElementString(@"ParentPropertyName", @"", ((global::System.String)o.@ParentPropertyName));
            WriteEndElement(o);
        }

        void Write6_ForeignKeyColumn(string n, string ns, global::PocoGen.Common.FileFormat.ForeignKeyColumn o, bool isNullable, bool needType)
        {
            if ((object)o == null)
            {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType)
            {
                System.Type t = o.GetType();
                if (t == typeof(global::PocoGen.Common.FileFormat.ForeignKeyColumn))
                {
                }
                else
                {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"ForeignKeyColumn", @"");
            WriteElementString(@"ParentTablesColumnName", @"", ((global::System.String)o.@ParentTablesColumnName));
            WriteElementString(@"ChildTablesColumnName", @"", ((global::System.String)o.@ChildTablesColumnName));
            WriteEndElement(o);
        }

        void Write5_Table(string n, string ns, global::PocoGen.Common.FileFormat.Table o, bool isNullable, bool needType)
        {
            if ((object)o == null)
            {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType)
            {
                System.Type t = o.GetType();
                if (t == typeof(global::PocoGen.Common.FileFormat.Table))
                {
                }
                else
                {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"Table", @"");
            WriteElementString(@"Schema", @"", ((global::System.String)o.@Schema));
            WriteElementString(@"Name", @"", ((global::System.String)o.@Name));
            WriteElementStringRaw(@"Ignore", @"", System.Xml.XmlConvert.ToString((global::System.Boolean)((global::System.Boolean)o.@Ignore)));
            WriteElementString(@"PropertyName", @"", ((global::System.String)o.@ClassName));
            {
                global::PocoGen.Common.FileFormat.ColumnCollection a = (global::PocoGen.Common.FileFormat.ColumnCollection)((global::PocoGen.Common.FileFormat.ColumnCollection)o.@Columns);
                if (a != null)
                {
                    WriteStartElement(@"Columns", @"", null, false);
                    for (int ia = 0; ia < ((System.Collections.ICollection)a).Count; ia++)
                    {
                        Write4_Column(@"Column", @"", ((global::PocoGen.Common.FileFormat.Column)a[ia]), true, false);
                    }
                    WriteEndElement();
                }
            }
            WriteEndElement(o);
        }

        void Write4_Column(string n, string ns, global::PocoGen.Common.FileFormat.Column o, bool isNullable, bool needType)
        {
            if ((object)o == null)
            {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType)
            {
                System.Type t = o.GetType();
                if (t == typeof(global::PocoGen.Common.FileFormat.Column))
                {
                }
                else
                {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"Column", @"");
            WriteElementString(@"Name", @"", ((global::System.String)o.@Name));
            WriteElementStringRaw(@"Ignore", @"", System.Xml.XmlConvert.ToString((global::System.Boolean)((global::System.Boolean)o.@Ignore)));
            WriteElementString(@"PropertyName", @"", ((global::System.String)o.@PropertyName));
            WriteEndElement(o);
        }

        void Write2_PlugIn(string n, string ns, global::PocoGen.Common.FileFormat.PlugIn o, bool isNullable, bool needType)
        {
            if ((object)o == null)
            {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType)
            {
                System.Type t = o.GetType();
                if (t == typeof(global::PocoGen.Common.FileFormat.PlugIn))
                {
                }
                else if (t == typeof(global::PocoGen.Common.FileFormat.OutputWriterPlugIn))
                {
                    Write8_OutputWriterPlugIn(n, ns, (global::PocoGen.Common.FileFormat.OutputWriterPlugIn)o, isNullable, true);
                    return;
                }
                else
                {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"PlugIn", @"");
            WriteElementString(@"Guid", @"", ((global::System.String)o.@Guid));
            WriteElementString(@"Name", @"", ((global::System.String)o.@Name));
            WriteSerializable((System.Xml.Serialization.IXmlSerializable)((global::PocoGen.Common.SettingsRepository)o.@Configuration), @"Configuration", @"", false, true);
            WriteEndElement(o);
        }

        protected override void InitCallbacks()
        {
        }
    }

    internal class XmlSerializationReaderDefinition : System.Xml.Serialization.XmlSerializationReader
    {

        public object Read10_PocoGenDefinition()
        {
            object o = null;
            Reader.MoveToContent();
            if (Reader.NodeType == System.Xml.XmlNodeType.Element)
            {
                if (((object)Reader.LocalName == (object)id1_PocoGenDefinition && (object)Reader.NamespaceURI == (object)id2_Item))
                {
                    o = Read9_Definition(true, true);
                }
                else
                {
                    throw CreateUnknownNodeException();
                }
            }
            else
            {
                UnknownNode(null, @":PocoGenDefinition");
            }
            return (object)o;
        }

        global::PocoGen.Common.FileFormat.Definition Read9_Definition(bool isNullable, bool checkType)
        {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType)
            {
                if (xsiType == null || ((object)((System.Xml.XmlQualifiedName)xsiType).Name == (object)id3_Definition && (object)((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item))
                {
                }
                else
                    throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::PocoGen.Common.FileFormat.Definition o;
            o = new global::PocoGen.Common.FileFormat.Definition();
            global::PocoGen.Common.FileFormat.PlugInCollection a_3 = (global::PocoGen.Common.FileFormat.PlugInCollection)o.@TableNameGenerators;
            global::PocoGen.Common.FileFormat.PlugInCollection a_4 = (global::PocoGen.Common.FileFormat.PlugInCollection)o.@ColumnNameGenerators;
            global::PocoGen.Common.FileFormat.PlugInCollection a_5 = (global::PocoGen.Common.FileFormat.PlugInCollection)o.@ForeignKeyParentPropertyNameGenerators;
            global::PocoGen.Common.FileFormat.PlugInCollection a_6 = (global::PocoGen.Common.FileFormat.PlugInCollection)o.@ForeignKeyChildPropertyNameGenerators;
            global::PocoGen.Common.FileFormat.TableCollection a_7 = (global::PocoGen.Common.FileFormat.TableCollection)o.@Tables;
            global::PocoGen.Common.FileFormat.ForeignKeyCollection a_8 = (global::PocoGen.Common.FileFormat.ForeignKeyCollection)o.@ForeignKeys;
            global::PocoGen.Common.FileFormat.OutputWriterPlugInCollection a_9 = (global::PocoGen.Common.FileFormat.OutputWriterPlugInCollection)o.@OutputWriters;
            bool[] paramsRead = new bool[11];
            while (Reader.MoveToNextAttribute())
            {
                if (!IsXmlnsAttribute(Reader.Name))
                {
                    UnknownNode((object)o);
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement)
            {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations0 = 0;
            int readerCount0 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None)
            {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element)
                {
                    if (!paramsRead[0] && ((object)Reader.LocalName == (object)id4_SchemaReader && (object)Reader.NamespaceURI == (object)id2_Item))
                    {
                        o.@SchemaReader = Read2_PlugIn(false, true);
                        paramsRead[0] = true;
                    }
                    else if (!paramsRead[1] && ((object)Reader.LocalName == (object)id5_ConnectionString && (object)Reader.NamespaceURI == (object)id2_Item))
                    {
                        {
                            o.@ConnectionString = Reader.ReadElementString();
                        }
                        paramsRead[1] = true;
                    }
                    else if (!paramsRead[2] && ((object)Reader.LocalName == (object)id6_UseAnsiQuoting && (object)Reader.NamespaceURI == (object)id2_Item))
                    {
                        {
                            o.@UseAnsiQuoting = System.Xml.XmlConvert.ToBoolean(Reader.ReadElementString());
                        }
                        paramsRead[2] = true;
                    }
                    else if (((object)Reader.LocalName == (object)id7_TableNameGenerators && (object)Reader.NamespaceURI == (object)id2_Item))
                    {
                        if (!ReadNull())
                        {
                            global::PocoGen.Common.FileFormat.PlugInCollection a_3_0 = (global::PocoGen.Common.FileFormat.PlugInCollection)o.@TableNameGenerators;
                            if ((Reader.IsEmptyElement))
                            {
                                Reader.Skip();
                            }
                            else
                            {
                                Reader.ReadStartElement();
                                Reader.MoveToContent();
                                int whileIterations1 = 0;
                                int readerCount1 = ReaderCount;
                                while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None)
                                {
                                    if (Reader.NodeType == System.Xml.XmlNodeType.Element)
                                    {
                                        if (((object)Reader.LocalName == (object)id8_TableNameGenerator && (object)Reader.NamespaceURI == (object)id2_Item))
                                        {
                                            if ((object)(a_3_0) == null) Reader.Skip(); else a_3_0.Add(Read2_PlugIn(true, true));
                                        }
                                        else
                                        {
                                            UnknownNode(null, @":TableNameGenerator");
                                        }
                                    }
                                    else
                                    {
                                        UnknownNode(null, @":TableNameGenerator");
                                    }
                                    Reader.MoveToContent();
                                    CheckReaderCount(ref whileIterations1, ref readerCount1);
                                }
                                ReadEndElement();
                            }
                        }
                    }
                    else if (((object)Reader.LocalName == (object)id9_ColumnNameGenerators && (object)Reader.NamespaceURI == (object)id2_Item))
                    {
                        if (!ReadNull())
                        {
                            global::PocoGen.Common.FileFormat.PlugInCollection a_4_0 = (global::PocoGen.Common.FileFormat.PlugInCollection)o.@ColumnNameGenerators;
                            if ((Reader.IsEmptyElement))
                            {
                                Reader.Skip();
                            }
                            else
                            {
                                Reader.ReadStartElement();
                                Reader.MoveToContent();
                                int whileIterations2 = 0;
                                int readerCount2 = ReaderCount;
                                while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None)
                                {
                                    if (Reader.NodeType == System.Xml.XmlNodeType.Element)
                                    {
                                        if (((object)Reader.LocalName == (object)id10_ColumnNameGenerator && (object)Reader.NamespaceURI == (object)id2_Item))
                                        {
                                            if ((object)(a_4_0) == null) Reader.Skip(); else a_4_0.Add(Read2_PlugIn(true, true));
                                        }
                                        else
                                        {
                                            UnknownNode(null, @":ColumnNameGenerator");
                                        }
                                    }
                                    else
                                    {
                                        UnknownNode(null, @":ColumnNameGenerator");
                                    }
                                    Reader.MoveToContent();
                                    CheckReaderCount(ref whileIterations2, ref readerCount2);
                                }
                                ReadEndElement();
                            }
                        }
                    }
                    else if (((object)Reader.LocalName == (object)id11_Item && (object)Reader.NamespaceURI == (object)id2_Item))
                    {
                        if (!ReadNull())
                        {
                            global::PocoGen.Common.FileFormat.PlugInCollection a_5_0 = (global::PocoGen.Common.FileFormat.PlugInCollection)o.@ForeignKeyParentPropertyNameGenerators;
                            if ((Reader.IsEmptyElement))
                            {
                                Reader.Skip();
                            }
                            else
                            {
                                Reader.ReadStartElement();
                                Reader.MoveToContent();
                                int whileIterations3 = 0;
                                int readerCount3 = ReaderCount;
                                while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None)
                                {
                                    if (Reader.NodeType == System.Xml.XmlNodeType.Element)
                                    {
                                        if (((object)Reader.LocalName == (object)id12_Item && (object)Reader.NamespaceURI == (object)id2_Item))
                                        {
                                            if ((object)(a_5_0) == null) Reader.Skip(); else a_5_0.Add(Read2_PlugIn(true, true));
                                        }
                                        else
                                        {
                                            UnknownNode(null, @":ForeignKeyPropertyNameGenerator");
                                        }
                                    }
                                    else
                                    {
                                        UnknownNode(null, @":ForeignKeyPropertyNameGenerator");
                                    }
                                    Reader.MoveToContent();
                                    CheckReaderCount(ref whileIterations3, ref readerCount3);
                                }
                                ReadEndElement();
                            }
                        }
                    }
                    else if (((object)Reader.LocalName == (object)id13_Item && (object)Reader.NamespaceURI == (object)id2_Item))
                    {
                        if (!ReadNull())
                        {
                            global::PocoGen.Common.FileFormat.PlugInCollection a_6_0 = (global::PocoGen.Common.FileFormat.PlugInCollection)o.@ForeignKeyChildPropertyNameGenerators;
                            if ((Reader.IsEmptyElement))
                            {
                                Reader.Skip();
                            }
                            else
                            {
                                Reader.ReadStartElement();
                                Reader.MoveToContent();
                                int whileIterations4 = 0;
                                int readerCount4 = ReaderCount;
                                while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None)
                                {
                                    if (Reader.NodeType == System.Xml.XmlNodeType.Element)
                                    {
                                        if (((object)Reader.LocalName == (object)id12_Item && (object)Reader.NamespaceURI == (object)id2_Item))
                                        {
                                            if ((object)(a_6_0) == null) Reader.Skip(); else a_6_0.Add(Read2_PlugIn(true, true));
                                        }
                                        else
                                        {
                                            UnknownNode(null, @":ForeignKeyPropertyNameGenerator");
                                        }
                                    }
                                    else
                                    {
                                        UnknownNode(null, @":ForeignKeyPropertyNameGenerator");
                                    }
                                    Reader.MoveToContent();
                                    CheckReaderCount(ref whileIterations4, ref readerCount4);
                                }
                                ReadEndElement();
                            }
                        }
                    }
                    else if (((object)Reader.LocalName == (object)id14_Tables && (object)Reader.NamespaceURI == (object)id2_Item))
                    {
                        if (!ReadNull())
                        {
                            global::PocoGen.Common.FileFormat.TableCollection a_7_0 = (global::PocoGen.Common.FileFormat.TableCollection)o.@Tables;
                            if ((Reader.IsEmptyElement))
                            {
                                Reader.Skip();
                            }
                            else
                            {
                                Reader.ReadStartElement();
                                Reader.MoveToContent();
                                int whileIterations5 = 0;
                                int readerCount5 = ReaderCount;
                                while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None)
                                {
                                    if (Reader.NodeType == System.Xml.XmlNodeType.Element)
                                    {
                                        if (((object)Reader.LocalName == (object)id15_Table && (object)Reader.NamespaceURI == (object)id2_Item))
                                        {
                                            if ((object)(a_7_0) == null) Reader.Skip(); else a_7_0.Add(Read5_Table(true, true));
                                        }
                                        else
                                        {
                                            UnknownNode(null, @":Table");
                                        }
                                    }
                                    else
                                    {
                                        UnknownNode(null, @":Table");
                                    }
                                    Reader.MoveToContent();
                                    CheckReaderCount(ref whileIterations5, ref readerCount5);
                                }
                                ReadEndElement();
                            }
                        }
                    }
                    else if (((object)Reader.LocalName == (object)id16_ForeignKeys && (object)Reader.NamespaceURI == (object)id2_Item))
                    {
                        if (!ReadNull())
                        {
                            global::PocoGen.Common.FileFormat.ForeignKeyCollection a_8_0 = (global::PocoGen.Common.FileFormat.ForeignKeyCollection)o.@ForeignKeys;
                            if ((Reader.IsEmptyElement))
                            {
                                Reader.Skip();
                            }
                            else
                            {
                                Reader.ReadStartElement();
                                Reader.MoveToContent();
                                int whileIterations6 = 0;
                                int readerCount6 = ReaderCount;
                                while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None)
                                {
                                    if (Reader.NodeType == System.Xml.XmlNodeType.Element)
                                    {
                                        if (((object)Reader.LocalName == (object)id17_ForeignKey && (object)Reader.NamespaceURI == (object)id2_Item))
                                        {
                                            if ((object)(a_8_0) == null) Reader.Skip(); else a_8_0.Add(Read7_ForeignKey(true, true));
                                        }
                                        else
                                        {
                                            UnknownNode(null, @":ForeignKey");
                                        }
                                    }
                                    else
                                    {
                                        UnknownNode(null, @":ForeignKey");
                                    }
                                    Reader.MoveToContent();
                                    CheckReaderCount(ref whileIterations6, ref readerCount6);
                                }
                                ReadEndElement();
                            }
                        }
                    }
                    else if (((object)Reader.LocalName == (object)id18_OutputWriters && (object)Reader.NamespaceURI == (object)id2_Item))
                    {
                        if (!ReadNull())
                        {
                            global::PocoGen.Common.FileFormat.OutputWriterPlugInCollection a_9_0 = (global::PocoGen.Common.FileFormat.OutputWriterPlugInCollection)o.@OutputWriters;
                            if ((Reader.IsEmptyElement))
                            {
                                Reader.Skip();
                            }
                            else
                            {
                                Reader.ReadStartElement();
                                Reader.MoveToContent();
                                int whileIterations7 = 0;
                                int readerCount7 = ReaderCount;
                                while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None)
                                {
                                    if (Reader.NodeType == System.Xml.XmlNodeType.Element)
                                    {
                                        if (((object)Reader.LocalName == (object)id19_OutputWriter && (object)Reader.NamespaceURI == (object)id2_Item))
                                        {
                                            if ((object)(a_9_0) == null) Reader.Skip(); else a_9_0.Add(Read8_OutputWriterPlugIn(true, true));
                                        }
                                        else
                                        {
                                            UnknownNode(null, @":OutputWriter");
                                        }
                                    }
                                    else
                                    {
                                        UnknownNode(null, @":OutputWriter");
                                    }
                                    Reader.MoveToContent();
                                    CheckReaderCount(ref whileIterations7, ref readerCount7);
                                }
                                ReadEndElement();
                            }
                        }
                    }
                    else if (!paramsRead[10] && ((object)Reader.LocalName == (object)id20_OutputBasePath && (object)Reader.NamespaceURI == (object)id2_Item))
                    {
                        {
                            o.@OutputBasePath = Reader.ReadElementString();
                        }
                        paramsRead[10] = true;
                    }
                    else
                    {
                        UnknownNode((object)o, @":SchemaReader, :ConnectionString, :UseAnsiQuoting, :TableNameGenerators, :ColumnNameGenerators, :ForeignKeyParentPropertyNameGenerators, :ForeignKeyChildPropertyNameGenerators, :Tables, :ForeignKeys, :OutputWriters, :OutputBasePath");
                    }
                }
                else
                {
                    UnknownNode((object)o, @":SchemaReader, :ConnectionString, :UseAnsiQuoting, :TableNameGenerators, :ColumnNameGenerators, :ForeignKeyParentPropertyNameGenerators, :ForeignKeyChildPropertyNameGenerators, :Tables, :ForeignKeys, :OutputWriters, :OutputBasePath");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations0, ref readerCount0);
            }
            ReadEndElement();
            return o;
        }

        global::PocoGen.Common.FileFormat.OutputWriterPlugIn Read8_OutputWriterPlugIn(bool isNullable, bool checkType)
        {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType)
            {
                if (xsiType == null || ((object)((System.Xml.XmlQualifiedName)xsiType).Name == (object)id21_OutputWriterPlugIn && (object)((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item))
                {
                }
                else
                    throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::PocoGen.Common.FileFormat.OutputWriterPlugIn o;
            o = new global::PocoGen.Common.FileFormat.OutputWriterPlugIn();
            bool[] paramsRead = new bool[4];
            while (Reader.MoveToNextAttribute())
            {
                if (!IsXmlnsAttribute(Reader.Name))
                {
                    UnknownNode((object)o);
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement)
            {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations8 = 0;
            int readerCount8 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None)
            {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element)
                {
                    if (!paramsRead[0] && ((object)Reader.LocalName == (object)id22_Guid && (object)Reader.NamespaceURI == (object)id2_Item))
                    {
                        {
                            o.@Guid = Reader.ReadElementString();
                        }
                        paramsRead[0] = true;
                    }
                    else if (!paramsRead[1] && ((object)Reader.LocalName == (object)id23_Name && (object)Reader.NamespaceURI == (object)id2_Item))
                    {
                        {
                            o.@Name = Reader.ReadElementString();
                        }
                        paramsRead[1] = true;
                    }
                    else if (!paramsRead[2] && ((object)Reader.LocalName == (object)id24_Configuration && (object)Reader.NamespaceURI == (object)id2_Item))
                    {
                        o.@Configuration = (global::PocoGen.Common.SettingsRepository)ReadSerializable((System.Xml.Serialization.IXmlSerializable)new global::PocoGen.Common.SettingsRepository());
                        paramsRead[2] = true;
                    }
                    else if (!paramsRead[3] && ((object)Reader.LocalName == (object)id25_FileName && (object)Reader.NamespaceURI == (object)id2_Item))
                    {
                        {
                            o.@FileName = Reader.ReadElementString();
                        }
                        paramsRead[3] = true;
                    }
                    else
                    {
                        UnknownNode((object)o, @":Guid, :Name, :Configuration, :FileName");
                    }
                }
                else
                {
                    UnknownNode((object)o, @":Guid, :Name, :Configuration, :FileName");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations8, ref readerCount8);
            }
            ReadEndElement();
            return o;
        }

        global::PocoGen.Common.FileFormat.ForeignKey Read7_ForeignKey(bool isNullable, bool checkType)
        {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType)
            {
                if (xsiType == null || ((object)((System.Xml.XmlQualifiedName)xsiType).Name == (object)id17_ForeignKey && (object)((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item))
                {
                }
                else
                    throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::PocoGen.Common.FileFormat.ForeignKey o;
            o = new global::PocoGen.Common.FileFormat.ForeignKey();
            global::PocoGen.Common.FileFormat.ForeignKeyColumnCollection a_6 = (global::PocoGen.Common.FileFormat.ForeignKeyColumnCollection)o.@Columns;
            bool[] paramsRead = new bool[11];
            while (Reader.MoveToNextAttribute())
            {
                if (!IsXmlnsAttribute(Reader.Name))
                {
                    UnknownNode((object)o);
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement)
            {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations9 = 0;
            int readerCount9 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None)
            {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element)
                {
                    if (!paramsRead[0] && ((object)Reader.LocalName == (object)id26_ParentTableSchema && (object)Reader.NamespaceURI == (object)id2_Item))
                    {
                        {
                            o.@ParentTableSchema = Reader.ReadElementString();
                        }
                        paramsRead[0] = true;
                    }
                    else if (!paramsRead[1] && ((object)Reader.LocalName == (object)id27_ParentTable && (object)Reader.NamespaceURI == (object)id2_Item))
                    {
                        {
                            o.@ParentTable = Reader.ReadElementString();
                        }
                        paramsRead[1] = true;
                    }
                    else if (!paramsRead[2] && ((object)Reader.LocalName == (object)id28_ChildTableSchema && (object)Reader.NamespaceURI == (object)id2_Item))
                    {
                        {
                            o.@ChildTableSchema = Reader.ReadElementString();
                        }
                        paramsRead[2] = true;
                    }
                    else if (!paramsRead[3] && ((object)Reader.LocalName == (object)id29_ChildTable && (object)Reader.NamespaceURI == (object)id2_Item))
                    {
                        {
                            o.@ChildTable = Reader.ReadElementString();
                        }
                        paramsRead[3] = true;
                    }
                    else if (!paramsRead[4] && ((object)Reader.LocalName == (object)id30_Schema && (object)Reader.NamespaceURI == (object)id2_Item))
                    {
                        {
                            o.@Schema = Reader.ReadElementString();
                        }
                        paramsRead[4] = true;
                    }
                    else if (!paramsRead[5] && ((object)Reader.LocalName == (object)id23_Name && (object)Reader.NamespaceURI == (object)id2_Item))
                    {
                        {
                            o.@Name = Reader.ReadElementString();
                        }
                        paramsRead[5] = true;
                    }
                    else if (((object)Reader.LocalName == (object)id31_Columns && (object)Reader.NamespaceURI == (object)id2_Item))
                    {
                        if (!ReadNull())
                        {
                            global::PocoGen.Common.FileFormat.ForeignKeyColumnCollection a_6_0 = (global::PocoGen.Common.FileFormat.ForeignKeyColumnCollection)o.@Columns;
                            if ((Reader.IsEmptyElement))
                            {
                                Reader.Skip();
                            }
                            else
                            {
                                Reader.ReadStartElement();
                                Reader.MoveToContent();
                                int whileIterations10 = 0;
                                int readerCount10 = ReaderCount;
                                while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None)
                                {
                                    if (Reader.NodeType == System.Xml.XmlNodeType.Element)
                                    {
                                        if (((object)Reader.LocalName == (object)id32_Column && (object)Reader.NamespaceURI == (object)id2_Item))
                                        {
                                            if ((object)(a_6_0) == null) Reader.Skip(); else a_6_0.Add(Read6_ForeignKeyColumn(true, true));
                                        }
                                        else
                                        {
                                            UnknownNode(null, @":Column");
                                        }
                                    }
                                    else
                                    {
                                        UnknownNode(null, @":Column");
                                    }
                                    Reader.MoveToContent();
                                    CheckReaderCount(ref whileIterations10, ref readerCount10);
                                }
                                ReadEndElement();
                            }
                        }
                    }
                    else if (!paramsRead[7] && ((object)Reader.LocalName == (object)id33_IgnoreChildProperty && (object)Reader.NamespaceURI == (object)id2_Item))
                    {
                        {
                            o.@IgnoreChildProperty = System.Xml.XmlConvert.ToBoolean(Reader.ReadElementString());
                        }
                        paramsRead[7] = true;
                    }
                    else if (!paramsRead[8] && ((object)Reader.LocalName == (object)id34_IgnoreParentProperty && (object)Reader.NamespaceURI == (object)id2_Item))
                    {
                        {
                            o.@IgnoreParentProperty = System.Xml.XmlConvert.ToBoolean(Reader.ReadElementString());
                        }
                        paramsRead[8] = true;
                    }
                    else if (!paramsRead[9] && ((object)Reader.LocalName == (object)id35_ChildPropertyName && (object)Reader.NamespaceURI == (object)id2_Item))
                    {
                        {
                            o.@ChildPropertyName = Reader.ReadElementString();
                        }
                        paramsRead[9] = true;
                    }
                    else if (!paramsRead[10] && ((object)Reader.LocalName == (object)id36_ParentPropertyName && (object)Reader.NamespaceURI == (object)id2_Item))
                    {
                        {
                            o.@ParentPropertyName = Reader.ReadElementString();
                        }
                        paramsRead[10] = true;
                    }
                    else
                    {
                        UnknownNode((object)o, @":ParentTableSchema, :ParentTable, :ChildTableSchema, :ChildTable, :Schema, :Name, :Columns, :IgnoreChildProperty, :IgnoreParentProperty, :ChildPropertyName, :ParentPropertyName");
                    }
                }
                else
                {
                    UnknownNode((object)o, @":ParentTableSchema, :ParentTable, :ChildTableSchema, :ChildTable, :Schema, :Name, :Columns, :IgnoreChildProperty, :IgnoreParentProperty, :ChildPropertyName, :ParentPropertyName");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations9, ref readerCount9);
            }
            ReadEndElement();
            return o;
        }

        global::PocoGen.Common.FileFormat.ForeignKeyColumn Read6_ForeignKeyColumn(bool isNullable, bool checkType)
        {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType)
            {
                if (xsiType == null || ((object)((System.Xml.XmlQualifiedName)xsiType).Name == (object)id37_ForeignKeyColumn && (object)((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item))
                {
                }
                else
                    throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::PocoGen.Common.FileFormat.ForeignKeyColumn o;
            o = new global::PocoGen.Common.FileFormat.ForeignKeyColumn();
            bool[] paramsRead = new bool[2];
            while (Reader.MoveToNextAttribute())
            {
                if (!IsXmlnsAttribute(Reader.Name))
                {
                    UnknownNode((object)o);
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement)
            {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations11 = 0;
            int readerCount11 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None)
            {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element)
                {
                    if (!paramsRead[0] && ((object)Reader.LocalName == (object)id38_ParentTablesColumnName && (object)Reader.NamespaceURI == (object)id2_Item))
                    {
                        {
                            o.@ParentTablesColumnName = Reader.ReadElementString();
                        }
                        paramsRead[0] = true;
                    }
                    else if (!paramsRead[1] && ((object)Reader.LocalName == (object)id39_ChildTablesColumnName && (object)Reader.NamespaceURI == (object)id2_Item))
                    {
                        {
                            o.@ChildTablesColumnName = Reader.ReadElementString();
                        }
                        paramsRead[1] = true;
                    }
                    else
                    {
                        UnknownNode((object)o, @":ParentTablesColumnName, :ChildTablesColumnName");
                    }
                }
                else
                {
                    UnknownNode((object)o, @":ParentTablesColumnName, :ChildTablesColumnName");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations11, ref readerCount11);
            }
            ReadEndElement();
            return o;
        }

        global::PocoGen.Common.FileFormat.Table Read5_Table(bool isNullable, bool checkType)
        {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType)
            {
                if (xsiType == null || ((object)((System.Xml.XmlQualifiedName)xsiType).Name == (object)id15_Table && (object)((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item))
                {
                }
                else
                    throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::PocoGen.Common.FileFormat.Table o;
            o = new global::PocoGen.Common.FileFormat.Table();
            global::PocoGen.Common.FileFormat.ColumnCollection a_4 = (global::PocoGen.Common.FileFormat.ColumnCollection)o.@Columns;
            bool[] paramsRead = new bool[5];
            while (Reader.MoveToNextAttribute())
            {
                if (!IsXmlnsAttribute(Reader.Name))
                {
                    UnknownNode((object)o);
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement)
            {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations12 = 0;
            int readerCount12 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None)
            {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element)
                {
                    if (!paramsRead[0] && ((object)Reader.LocalName == (object)id30_Schema && (object)Reader.NamespaceURI == (object)id2_Item))
                    {
                        {
                            o.@Schema = Reader.ReadElementString();
                        }
                        paramsRead[0] = true;
                    }
                    else if (!paramsRead[1] && ((object)Reader.LocalName == (object)id23_Name && (object)Reader.NamespaceURI == (object)id2_Item))
                    {
                        {
                            o.@Name = Reader.ReadElementString();
                        }
                        paramsRead[1] = true;
                    }
                    else if (!paramsRead[2] && ((object)Reader.LocalName == (object)id40_Ignore && (object)Reader.NamespaceURI == (object)id2_Item))
                    {
                        {
                            o.@Ignore = System.Xml.XmlConvert.ToBoolean(Reader.ReadElementString());
                        }
                        paramsRead[2] = true;
                    }
                    else if (!paramsRead[3] && ((object)Reader.LocalName == (object)id41_PropertyName && (object)Reader.NamespaceURI == (object)id2_Item))
                    {
                        {
                            o.@ClassName = Reader.ReadElementString();
                        }
                        paramsRead[3] = true;
                    }
                    else if (((object)Reader.LocalName == (object)id31_Columns && (object)Reader.NamespaceURI == (object)id2_Item))
                    {
                        if (!ReadNull())
                        {
                            global::PocoGen.Common.FileFormat.ColumnCollection a_4_0 = (global::PocoGen.Common.FileFormat.ColumnCollection)o.@Columns;
                            if ((Reader.IsEmptyElement))
                            {
                                Reader.Skip();
                            }
                            else
                            {
                                Reader.ReadStartElement();
                                Reader.MoveToContent();
                                int whileIterations13 = 0;
                                int readerCount13 = ReaderCount;
                                while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None)
                                {
                                    if (Reader.NodeType == System.Xml.XmlNodeType.Element)
                                    {
                                        if (((object)Reader.LocalName == (object)id32_Column && (object)Reader.NamespaceURI == (object)id2_Item))
                                        {
                                            if ((object)(a_4_0) == null) Reader.Skip(); else a_4_0.Add(Read4_Column(true, true));
                                        }
                                        else
                                        {
                                            UnknownNode(null, @":Column");
                                        }
                                    }
                                    else
                                    {
                                        UnknownNode(null, @":Column");
                                    }
                                    Reader.MoveToContent();
                                    CheckReaderCount(ref whileIterations13, ref readerCount13);
                                }
                                ReadEndElement();
                            }
                        }
                    }
                    else
                    {
                        UnknownNode((object)o, @":Schema, :Name, :Ignore, :PropertyName, :Columns");
                    }
                }
                else
                {
                    UnknownNode((object)o, @":Schema, :Name, :Ignore, :PropertyName, :Columns");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations12, ref readerCount12);
            }
            ReadEndElement();
            return o;
        }

        global::PocoGen.Common.FileFormat.Column Read4_Column(bool isNullable, bool checkType)
        {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType)
            {
                if (xsiType == null || ((object)((System.Xml.XmlQualifiedName)xsiType).Name == (object)id32_Column && (object)((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item))
                {
                }
                else
                    throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::PocoGen.Common.FileFormat.Column o;
            o = new global::PocoGen.Common.FileFormat.Column();
            bool[] paramsRead = new bool[3];
            while (Reader.MoveToNextAttribute())
            {
                if (!IsXmlnsAttribute(Reader.Name))
                {
                    UnknownNode((object)o);
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement)
            {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations14 = 0;
            int readerCount14 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None)
            {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element)
                {
                    if (!paramsRead[0] && ((object)Reader.LocalName == (object)id23_Name && (object)Reader.NamespaceURI == (object)id2_Item))
                    {
                        {
                            o.@Name = Reader.ReadElementString();
                        }
                        paramsRead[0] = true;
                    }
                    else if (!paramsRead[1] && ((object)Reader.LocalName == (object)id40_Ignore && (object)Reader.NamespaceURI == (object)id2_Item))
                    {
                        {
                            o.@Ignore = System.Xml.XmlConvert.ToBoolean(Reader.ReadElementString());
                        }
                        paramsRead[1] = true;
                    }
                    else if (!paramsRead[2] && ((object)Reader.LocalName == (object)id41_PropertyName && (object)Reader.NamespaceURI == (object)id2_Item))
                    {
                        {
                            o.@PropertyName = Reader.ReadElementString();
                        }
                        paramsRead[2] = true;
                    }
                    else
                    {
                        UnknownNode((object)o, @":Name, :Ignore, :PropertyName");
                    }
                }
                else
                {
                    UnknownNode((object)o, @":Name, :Ignore, :PropertyName");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations14, ref readerCount14);
            }
            ReadEndElement();
            return o;
        }

        global::PocoGen.Common.FileFormat.PlugIn Read2_PlugIn(bool isNullable, bool checkType)
        {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType)
            {
                if (xsiType == null || ((object)((System.Xml.XmlQualifiedName)xsiType).Name == (object)id42_PlugIn && (object)((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item))
                {
                }
                else if (((object)((System.Xml.XmlQualifiedName)xsiType).Name == (object)id21_OutputWriterPlugIn && (object)((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item))
                    return Read8_OutputWriterPlugIn(isNullable, false);
                else
                    throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::PocoGen.Common.FileFormat.PlugIn o;
            o = new global::PocoGen.Common.FileFormat.PlugIn();
            bool[] paramsRead = new bool[3];
            while (Reader.MoveToNextAttribute())
            {
                if (!IsXmlnsAttribute(Reader.Name))
                {
                    UnknownNode((object)o);
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement)
            {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations15 = 0;
            int readerCount15 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None)
            {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element)
                {
                    if (!paramsRead[0] && ((object)Reader.LocalName == (object)id22_Guid && (object)Reader.NamespaceURI == (object)id2_Item))
                    {
                        {
                            o.@Guid = Reader.ReadElementString();
                        }
                        paramsRead[0] = true;
                    }
                    else if (!paramsRead[1] && ((object)Reader.LocalName == (object)id23_Name && (object)Reader.NamespaceURI == (object)id2_Item))
                    {
                        {
                            o.@Name = Reader.ReadElementString();
                        }
                        paramsRead[1] = true;
                    }
                    else if (!paramsRead[2] && ((object)Reader.LocalName == (object)id24_Configuration && (object)Reader.NamespaceURI == (object)id2_Item))
                    {
                        o.@Configuration = (global::PocoGen.Common.SettingsRepository)ReadSerializable((System.Xml.Serialization.IXmlSerializable)new global::PocoGen.Common.SettingsRepository());
                        paramsRead[2] = true;
                    }
                    else
                    {
                        UnknownNode((object)o, @":Guid, :Name, :Configuration");
                    }
                }
                else
                {
                    UnknownNode((object)o, @":Guid, :Name, :Configuration");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations15, ref readerCount15);
            }
            ReadEndElement();
            return o;
        }

        protected override void InitCallbacks()
        {
        }

        string id3_Definition;
        string id30_Schema;
        string id13_Item;
        string id36_ParentPropertyName;
        string id11_Item;
        string id14_Tables;
        string id25_FileName;
        string id19_OutputWriter;
        string id33_IgnoreChildProperty;
        string id15_Table;
        string id31_Columns;
        string id34_IgnoreParentProperty;
        string id40_Ignore;
        string id17_ForeignKey;
        string id21_OutputWriterPlugIn;
        string id32_Column;
        string id28_ChildTableSchema;
        string id16_ForeignKeys;
        string id37_ForeignKeyColumn;
        string id7_TableNameGenerators;
        string id4_SchemaReader;
        string id27_ParentTable;
        string id2_Item;
        string id20_OutputBasePath;
        string id1_PocoGenDefinition;
        string id10_ColumnNameGenerator;
        string id24_Configuration;
        string id23_Name;
        string id6_UseAnsiQuoting;
        string id42_PlugIn;
        string id22_Guid;
        string id41_PropertyName;
        string id12_Item;
        string id18_OutputWriters;
        string id26_ParentTableSchema;
        string id39_ChildTablesColumnName;
        string id38_ParentTablesColumnName;
        string id29_ChildTable;
        string id35_ChildPropertyName;
        string id9_ColumnNameGenerators;
        string id5_ConnectionString;
        string id8_TableNameGenerator;

        protected override void InitIDs()
        {
            id3_Definition = Reader.NameTable.Add(@"Definition");
            id30_Schema = Reader.NameTable.Add(@"Schema");
            id13_Item = Reader.NameTable.Add(@"ForeignKeyChildPropertyNameGenerators");
            id36_ParentPropertyName = Reader.NameTable.Add(@"ParentPropertyName");
            id11_Item = Reader.NameTable.Add(@"ForeignKeyParentPropertyNameGenerators");
            id14_Tables = Reader.NameTable.Add(@"Tables");
            id25_FileName = Reader.NameTable.Add(@"FileName");
            id19_OutputWriter = Reader.NameTable.Add(@"OutputWriter");
            id33_IgnoreChildProperty = Reader.NameTable.Add(@"IgnoreChildProperty");
            id15_Table = Reader.NameTable.Add(@"Table");
            id31_Columns = Reader.NameTable.Add(@"Columns");
            id34_IgnoreParentProperty = Reader.NameTable.Add(@"IgnoreParentProperty");
            id40_Ignore = Reader.NameTable.Add(@"Ignore");
            id17_ForeignKey = Reader.NameTable.Add(@"ForeignKey");
            id21_OutputWriterPlugIn = Reader.NameTable.Add(@"OutputWriterPlugIn");
            id32_Column = Reader.NameTable.Add(@"Column");
            id28_ChildTableSchema = Reader.NameTable.Add(@"ChildTableSchema");
            id16_ForeignKeys = Reader.NameTable.Add(@"ForeignKeys");
            id37_ForeignKeyColumn = Reader.NameTable.Add(@"ForeignKeyColumn");
            id7_TableNameGenerators = Reader.NameTable.Add(@"TableNameGenerators");
            id4_SchemaReader = Reader.NameTable.Add(@"SchemaReader");
            id27_ParentTable = Reader.NameTable.Add(@"ParentTable");
            id2_Item = Reader.NameTable.Add(@"");
            id20_OutputBasePath = Reader.NameTable.Add(@"OutputBasePath");
            id1_PocoGenDefinition = Reader.NameTable.Add(@"PocoGenDefinition");
            id10_ColumnNameGenerator = Reader.NameTable.Add(@"ColumnNameGenerator");
            id24_Configuration = Reader.NameTable.Add(@"Configuration");
            id23_Name = Reader.NameTable.Add(@"Name");
            id6_UseAnsiQuoting = Reader.NameTable.Add(@"UseAnsiQuoting");
            id42_PlugIn = Reader.NameTable.Add(@"PlugIn");
            id22_Guid = Reader.NameTable.Add(@"Guid");
            id41_PropertyName = Reader.NameTable.Add(@"PropertyName");
            id12_Item = Reader.NameTable.Add(@"ForeignKeyPropertyNameGenerator");
            id18_OutputWriters = Reader.NameTable.Add(@"OutputWriters");
            id26_ParentTableSchema = Reader.NameTable.Add(@"ParentTableSchema");
            id39_ChildTablesColumnName = Reader.NameTable.Add(@"ChildTablesColumnName");
            id38_ParentTablesColumnName = Reader.NameTable.Add(@"ParentTablesColumnName");
            id29_ChildTable = Reader.NameTable.Add(@"ChildTable");
            id35_ChildPropertyName = Reader.NameTable.Add(@"ChildPropertyName");
            id9_ColumnNameGenerators = Reader.NameTable.Add(@"ColumnNameGenerators");
            id5_ConnectionString = Reader.NameTable.Add(@"ConnectionString");
            id8_TableNameGenerator = Reader.NameTable.Add(@"TableNameGenerator");
        }
    }

    internal abstract class XmlSerializer1 : System.Xml.Serialization.XmlSerializer
    {
        protected override System.Xml.Serialization.XmlSerializationReader CreateReader()
        {
            return new XmlSerializationReaderDefinition();
        }
        protected override System.Xml.Serialization.XmlSerializationWriter CreateWriter()
        {
            return new XmlSerializationWriterDefinition();
        }
    }

    internal sealed class DefinitionSerializer : XmlSerializer1
    {

        public override System.Boolean CanDeserialize(System.Xml.XmlReader xmlReader)
        {
            return xmlReader.IsStartElement(@"PocoGenDefinition", @"");
        }

        protected override void Serialize(object objectToSerialize, System.Xml.Serialization.XmlSerializationWriter writer)
        {
            ((XmlSerializationWriterDefinition)writer).Write10_PocoGenDefinition(objectToSerialize);
        }

        protected override object Deserialize(System.Xml.Serialization.XmlSerializationReader reader)
        {
            return ((XmlSerializationReaderDefinition)reader).Read10_PocoGenDefinition();
        }
    }
}