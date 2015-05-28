/* =================================================================================================
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

        public void Write7_PocoGenDefinition(object o)
        {
            WriteStartDocument();
            if (o == null)
            {
                WriteNullTagLiteral(@"PocoGenDefinition", @"");
                return;
            }
            TopLevelElement();
            Write6_Definition(@"PocoGenDefinition", @"", ((global::PocoGen.Common.FileFormat.Definition)o), true, false);
        }

        void Write6_Definition(string n, string ns, global::PocoGen.Common.FileFormat.Definition o, bool isNullable, bool needType)
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
                global::PocoGen.Common.FileFormat.TableCollection a = (global::PocoGen.Common.FileFormat.TableCollection)((global::PocoGen.Common.FileFormat.TableCollection)o.@Tables);
                if (a != null)
                {
                    WriteStartElement(@"Tables", @"", null, false);
                    for (int ia = 0; ia < ((System.Collections.ICollection)a).Count; ia++)
                    {
                        Write4_Table(@"Table", @"", ((global::PocoGen.Common.FileFormat.Table)a[ia]), true, false);
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
                        Write5_OutputWriterPlugIn(@"OutputWriter", @"", ((global::PocoGen.Common.FileFormat.OutputWriterPlugIn)a[ia]), true, false);
                    }
                    WriteEndElement();
                }
            }
            WriteElementString(@"OutputBasePath", @"", ((global::System.String)o.@OutputBasePath));
            WriteEndElement(o);
        }

        void Write5_OutputWriterPlugIn(string n, string ns, global::PocoGen.Common.FileFormat.OutputWriterPlugIn o, bool isNullable, bool needType)
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

        void Write4_Table(string n, string ns, global::PocoGen.Common.FileFormat.Table o, bool isNullable, bool needType)
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
            WriteElementString(@"Name", @"", ((global::System.String)o.@Name));
            WriteElementStringRaw(@"Ignore", @"", System.Xml.XmlConvert.ToString((global::System.Boolean)((global::System.Boolean)o.@Ignore)));
            WriteElementString(@"PropertyName", @"", ((global::System.String)o.ClassName));
            {
                global::PocoGen.Common.FileFormat.ColumnCollection a = (global::PocoGen.Common.FileFormat.ColumnCollection)((global::PocoGen.Common.FileFormat.ColumnCollection)o.@Columns);
                if (a != null)
                {
                    WriteStartElement(@"Columns", @"", null, false);
                    for (int ia = 0; ia < ((System.Collections.ICollection)a).Count; ia++)
                    {
                        Write3_Column(@"Column", @"", ((global::PocoGen.Common.FileFormat.Column)a[ia]), true, false);
                    }
                    WriteEndElement();
                }
            }
            WriteEndElement(o);
        }

        void Write3_Column(string n, string ns, global::PocoGen.Common.FileFormat.Column o, bool isNullable, bool needType)
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
                    Write5_OutputWriterPlugIn(n, ns, (global::PocoGen.Common.FileFormat.OutputWriterPlugIn)o, isNullable, true);
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

        public object Read7_PocoGenDefinition()
        {
            object o = null;
            Reader.MoveToContent();
            if (Reader.NodeType == System.Xml.XmlNodeType.Element)
            {
                if (((object)Reader.LocalName == (object)id1_PocoGenDefinition && (object)Reader.NamespaceURI == (object)id2_Item))
                {
                    o = Read6_Definition(true, true);
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

        global::PocoGen.Common.FileFormat.Definition Read6_Definition(bool isNullable, bool checkType)
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
            global::PocoGen.Common.FileFormat.TableCollection a_5 = (global::PocoGen.Common.FileFormat.TableCollection)o.@Tables;
            global::PocoGen.Common.FileFormat.OutputWriterPlugInCollection a_6 = (global::PocoGen.Common.FileFormat.OutputWriterPlugInCollection)o.@OutputWriters;
            bool[] paramsRead = new bool[8];
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
                    else if (((object)Reader.LocalName == (object)id11_Tables && (object)Reader.NamespaceURI == (object)id2_Item))
                    {
                        if (!ReadNull())
                        {
                            global::PocoGen.Common.FileFormat.TableCollection a_5_0 = (global::PocoGen.Common.FileFormat.TableCollection)o.@Tables;
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
                                        if (((object)Reader.LocalName == (object)id12_Table && (object)Reader.NamespaceURI == (object)id2_Item))
                                        {
                                            if ((object)(a_5_0) == null) Reader.Skip(); else a_5_0.Add(Read4_Table(true, true));
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
                                    CheckReaderCount(ref whileIterations3, ref readerCount3);
                                }
                                ReadEndElement();
                            }
                        }
                    }
                    else if (((object)Reader.LocalName == (object)id13_OutputWriters && (object)Reader.NamespaceURI == (object)id2_Item))
                    {
                        if (!ReadNull())
                        {
                            global::PocoGen.Common.FileFormat.OutputWriterPlugInCollection a_6_0 = (global::PocoGen.Common.FileFormat.OutputWriterPlugInCollection)o.@OutputWriters;
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
                                        if (((object)Reader.LocalName == (object)id14_OutputWriter && (object)Reader.NamespaceURI == (object)id2_Item))
                                        {
                                            if ((object)(a_6_0) == null) Reader.Skip(); else a_6_0.Add(Read5_OutputWriterPlugIn(true, true));
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
                                    CheckReaderCount(ref whileIterations4, ref readerCount4);
                                }
                                ReadEndElement();
                            }
                        }
                    }
                    else if (!paramsRead[7] && ((object)Reader.LocalName == (object)id15_OutputBasePath && (object)Reader.NamespaceURI == (object)id2_Item))
                    {
                        {
                            o.@OutputBasePath = Reader.ReadElementString();
                        }
                        paramsRead[7] = true;
                    }
                    else
                    {
                        UnknownNode((object)o, @":SchemaReader, :ConnectionString, :UseAnsiQuoting, :TableNameGenerators, :ColumnNameGenerators, :Tables, :OutputWriters, :OutputBasePath");
                    }
                }
                else
                {
                    UnknownNode((object)o, @":SchemaReader, :ConnectionString, :UseAnsiQuoting, :TableNameGenerators, :ColumnNameGenerators, :Tables, :OutputWriters, :OutputBasePath");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations0, ref readerCount0);
            }
            ReadEndElement();
            return o;
        }

        global::PocoGen.Common.FileFormat.OutputWriterPlugIn Read5_OutputWriterPlugIn(bool isNullable, bool checkType)
        {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType)
            {
                if (xsiType == null || ((object)((System.Xml.XmlQualifiedName)xsiType).Name == (object)id16_OutputWriterPlugIn && (object)((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item))
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
            int whileIterations5 = 0;
            int readerCount5 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None)
            {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element)
                {
                    if (!paramsRead[0] && ((object)Reader.LocalName == (object)id17_Guid && (object)Reader.NamespaceURI == (object)id2_Item))
                    {
                        {
                            o.@Guid = Reader.ReadElementString();
                        }
                        paramsRead[0] = true;
                    }
                    else if (!paramsRead[1] && ((object)Reader.LocalName == (object)id18_Name && (object)Reader.NamespaceURI == (object)id2_Item))
                    {
                        {
                            o.@Name = Reader.ReadElementString();
                        }
                        paramsRead[1] = true;
                    }
                    else if (!paramsRead[2] && ((object)Reader.LocalName == (object)id19_Configuration && (object)Reader.NamespaceURI == (object)id2_Item))
                    {
                        o.@Configuration = (global::PocoGen.Common.SettingsRepository)ReadSerializable((System.Xml.Serialization.IXmlSerializable)new global::PocoGen.Common.SettingsRepository());
                        paramsRead[2] = true;
                    }
                    else if (!paramsRead[3] && ((object)Reader.LocalName == (object)id20_FileName && (object)Reader.NamespaceURI == (object)id2_Item))
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
                CheckReaderCount(ref whileIterations5, ref readerCount5);
            }
            ReadEndElement();
            return o;
        }

        global::PocoGen.Common.FileFormat.Table Read4_Table(bool isNullable, bool checkType)
        {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType)
            {
                if (xsiType == null || ((object)((System.Xml.XmlQualifiedName)xsiType).Name == (object)id12_Table && (object)((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item))
                {
                }
                else
                    throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::PocoGen.Common.FileFormat.Table o;
            o = new global::PocoGen.Common.FileFormat.Table();
            global::PocoGen.Common.FileFormat.ColumnCollection a_3 = (global::PocoGen.Common.FileFormat.ColumnCollection)o.@Columns;
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
            int whileIterations6 = 0;
            int readerCount6 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None)
            {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element)
                {
                    if (!paramsRead[0] && ((object)Reader.LocalName == (object)id18_Name && (object)Reader.NamespaceURI == (object)id2_Item))
                    {
                        {
                            o.@Name = Reader.ReadElementString();
                        }
                        paramsRead[0] = true;
                    }
                    else if (!paramsRead[1] && ((object)Reader.LocalName == (object)id21_Ignore && (object)Reader.NamespaceURI == (object)id2_Item))
                    {
                        {
                            o.@Ignore = System.Xml.XmlConvert.ToBoolean(Reader.ReadElementString());
                        }
                        paramsRead[1] = true;
                    }
                    else if (!paramsRead[2] && ((object)Reader.LocalName == (object)id22_PropertyName && (object)Reader.NamespaceURI == (object)id2_Item))
                    {
                        {
                            o.ClassName = Reader.ReadElementString();
                        }
                        paramsRead[2] = true;
                    }
                    else if (((object)Reader.LocalName == (object)id23_Columns && (object)Reader.NamespaceURI == (object)id2_Item))
                    {
                        if (!ReadNull())
                        {
                            global::PocoGen.Common.FileFormat.ColumnCollection a_3_0 = (global::PocoGen.Common.FileFormat.ColumnCollection)o.@Columns;
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
                                        if (((object)Reader.LocalName == (object)id24_Column && (object)Reader.NamespaceURI == (object)id2_Item))
                                        {
                                            if ((object)(a_3_0) == null) Reader.Skip(); else a_3_0.Add(Read3_Column(true, true));
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
                                    CheckReaderCount(ref whileIterations7, ref readerCount7);
                                }
                                ReadEndElement();
                            }
                        }
                    }
                    else
                    {
                        UnknownNode((object)o, @":Name, :Ignore, :PropertyName, :Columns");
                    }
                }
                else
                {
                    UnknownNode((object)o, @":Name, :Ignore, :PropertyName, :Columns");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations6, ref readerCount6);
            }
            ReadEndElement();
            return o;
        }

        global::PocoGen.Common.FileFormat.Column Read3_Column(bool isNullable, bool checkType)
        {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType)
            {
                if (xsiType == null || ((object)((System.Xml.XmlQualifiedName)xsiType).Name == (object)id24_Column && (object)((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item))
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
            int whileIterations8 = 0;
            int readerCount8 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None)
            {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element)
                {
                    if (!paramsRead[0] && ((object)Reader.LocalName == (object)id18_Name && (object)Reader.NamespaceURI == (object)id2_Item))
                    {
                        {
                            o.@Name = Reader.ReadElementString();
                        }
                        paramsRead[0] = true;
                    }
                    else if (!paramsRead[1] && ((object)Reader.LocalName == (object)id21_Ignore && (object)Reader.NamespaceURI == (object)id2_Item))
                    {
                        {
                            o.@Ignore = System.Xml.XmlConvert.ToBoolean(Reader.ReadElementString());
                        }
                        paramsRead[1] = true;
                    }
                    else if (!paramsRead[2] && ((object)Reader.LocalName == (object)id22_PropertyName && (object)Reader.NamespaceURI == (object)id2_Item))
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
                CheckReaderCount(ref whileIterations8, ref readerCount8);
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
                if (xsiType == null || ((object)((System.Xml.XmlQualifiedName)xsiType).Name == (object)id25_PlugIn && (object)((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item))
                {
                }
                else if (((object)((System.Xml.XmlQualifiedName)xsiType).Name == (object)id16_OutputWriterPlugIn && (object)((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item))
                    return Read5_OutputWriterPlugIn(isNullable, false);
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
            int whileIterations9 = 0;
            int readerCount9 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None)
            {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element)
                {
                    if (!paramsRead[0] && ((object)Reader.LocalName == (object)id17_Guid && (object)Reader.NamespaceURI == (object)id2_Item))
                    {
                        {
                            o.@Guid = Reader.ReadElementString();
                        }
                        paramsRead[0] = true;
                    }
                    else if (!paramsRead[1] && ((object)Reader.LocalName == (object)id18_Name && (object)Reader.NamespaceURI == (object)id2_Item))
                    {
                        {
                            o.@Name = Reader.ReadElementString();
                        }
                        paramsRead[1] = true;
                    }
                    else if (!paramsRead[2] && ((object)Reader.LocalName == (object)id19_Configuration && (object)Reader.NamespaceURI == (object)id2_Item))
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
                CheckReaderCount(ref whileIterations9, ref readerCount9);
            }
            ReadEndElement();
            return o;
        }

        protected override void InitCallbacks()
        {
        }

        string id2_Item;
        string id12_Table;
        string id22_PropertyName;
        string id25_PlugIn;
        string id5_ConnectionString;
        string id11_Tables;
        string id14_OutputWriter;
        string id23_Columns;
        string id20_FileName;
        string id10_ColumnNameGenerator;
        string id8_TableNameGenerator;
        string id3_Definition;
        string id9_ColumnNameGenerators;
        string id19_Configuration;
        string id18_Name;
        string id7_TableNameGenerators;
        string id24_Column;
        string id13_OutputWriters;
        string id16_OutputWriterPlugIn;
        string id17_Guid;
        string id4_SchemaReader;
        string id1_PocoGenDefinition;
        string id21_Ignore;
        string id6_UseAnsiQuoting;
        string id15_OutputBasePath;

        protected override void InitIDs()
        {
            id2_Item = Reader.NameTable.Add(@"");
            id12_Table = Reader.NameTable.Add(@"Table");
            id22_PropertyName = Reader.NameTable.Add(@"PropertyName");
            id25_PlugIn = Reader.NameTable.Add(@"PlugIn");
            id5_ConnectionString = Reader.NameTable.Add(@"ConnectionString");
            id11_Tables = Reader.NameTable.Add(@"Tables");
            id14_OutputWriter = Reader.NameTable.Add(@"OutputWriter");
            id23_Columns = Reader.NameTable.Add(@"Columns");
            id20_FileName = Reader.NameTable.Add(@"FileName");
            id10_ColumnNameGenerator = Reader.NameTable.Add(@"ColumnNameGenerator");
            id8_TableNameGenerator = Reader.NameTable.Add(@"TableNameGenerator");
            id3_Definition = Reader.NameTable.Add(@"Definition");
            id9_ColumnNameGenerators = Reader.NameTable.Add(@"ColumnNameGenerators");
            id19_Configuration = Reader.NameTable.Add(@"Configuration");
            id18_Name = Reader.NameTable.Add(@"Name");
            id7_TableNameGenerators = Reader.NameTable.Add(@"TableNameGenerators");
            id24_Column = Reader.NameTable.Add(@"Column");
            id13_OutputWriters = Reader.NameTable.Add(@"OutputWriters");
            id16_OutputWriterPlugIn = Reader.NameTable.Add(@"OutputWriterPlugIn");
            id17_Guid = Reader.NameTable.Add(@"Guid");
            id4_SchemaReader = Reader.NameTable.Add(@"SchemaReader");
            id1_PocoGenDefinition = Reader.NameTable.Add(@"PocoGenDefinition");
            id21_Ignore = Reader.NameTable.Add(@"Ignore");
            id6_UseAnsiQuoting = Reader.NameTable.Add(@"UseAnsiQuoting");
            id15_OutputBasePath = Reader.NameTable.Add(@"OutputBasePath");
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
            ((XmlSerializationWriterDefinition)writer).Write7_PocoGenDefinition(objectToSerialize);
        }

        protected override object Deserialize(System.Xml.Serialization.XmlSerializationReader reader)
        {
            return ((XmlSerializationReaderDefinition)reader).Read7_PocoGenDefinition();
        }
    }
}