using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;

namespace NextTurn.UE.Markup
{
    internal static class XamlNamespaceParser
    {
        /// <exception cref="ArgumentException" />
        internal static string ParseUri(string uri)
        {
            int colonIndex = uri.IndexOf(':');
            if (colonIndex < 0)
            {
                throw new ArgumentException();
            }

            ReadOnlySpan<char> usingKeyword = uri.AsSpan()[..colonIndex];
            if (!usingKeyword.Equals("using", StringComparison.Ordinal))
            {
                throw new ArgumentException();
            }

            return uri[(colonIndex + 1)..];
        }
    }

    internal sealed class XamlReader
    {
        private readonly XmlReader xmlReader;

        internal XamlReader(string fileName)
        {
            this.xmlReader = XmlReader.Create(fileName);
            Type? parentType = null;
            object? parent = null;
            while (this.xmlReader.Read())
            {
                if (this.xmlReader.NodeType == XmlNodeType.Element)
                {
                    string localName = this.xmlReader.LocalName;
                    string namespaceUri = this.xmlReader.NamespaceURI;
                    if (!namespaceUri.StartsWith("using:"))
                    {
                        throw new XamlException(this.xmlReader);
                    }

                    string @namespace = namespaceUri["using:".Length..];

                    if (localName.Contains('.'))
                    {
                        if (parentType is null)
                        {
                            throw new XamlException(this.xmlReader);
                        }

                        if (@namespace != parentType.Namespace)
                        {
                            throw new XamlException(this.xmlReader);
                        }

                        string[] names = localName.Split('.');
                        if (names.Length != 2)
                        {
                            throw new XamlException(this.xmlReader);
                        }

                        string typeName = names[0];
                        string propertyName = names[1];

                        if (typeName != parentType.Name)
                        {
                            throw new XamlException(this.xmlReader);
                        }

                        SetPropertyValue(propertyName, this.xmlReader.ReadElementContentAsString());

                        if (this.xmlReader.AttributeCount != 0)
                        {
                            throw new XamlException(this.xmlReader);
                        }

                        continue;
                    }

                    Type? type = System.Type.GetType(@namespace + '.' + localName);
                    if (type is null)
                    {
                        throw new XamlException(this.xmlReader);
                    }

                    parentType = type;
                    parent = Activator.CreateInstance(type);

                    for (int i = 0; i < this.xmlReader.AttributeCount; i++)
                    {
                        this.xmlReader.MoveToAttribute(i);
                        
                        string attributeName = this.xmlReader.Name;
                        if (attributeName.StartsWith("xmlns"))
                        {
                            continue;
                        }

                        SetPropertyValue(attributeName, this.xmlReader.Value);
                    }
                }
            }

            this.xmlReader.Dispose();

            void SetPropertyValue(string propertyName, string propertyValue)
            {
                PropertyInfo? property = parentType!.GetProperty(propertyName);
                if (property is null)
                {
                    throw new XamlException(this.xmlReader);
                }

                property.SetValue(parent, Convert.ChangeType(propertyValue, property.PropertyType));
            }
        }

        internal XamlMember? Member => throw new NotImplementedException();

        internal XamlNodeType NodeType => throw new NotImplementedException();

        internal XamlType? Type => throw new NotImplementedException();

        internal bool Read()
        {
            return this.xmlReader.Read();
        }
    }

    internal enum XamlNodeType
    {
        None,
        StartObject,
        EndObject,
        StartMember,
        EndMember,
        Value,
    }

    internal sealed class XamlException : Exception
    {
        internal XamlException(XmlReader reader)
            : base(reader is IXmlLineInfo lineInfo ? $"({lineInfo.LineNumber}, {lineInfo.LinePosition})" : null)
        {
            _ = System.Diagnostics.Debugger.Launch();
        }
    }

    internal sealed class XamlMember
    {

    }

    internal sealed class XamlNamespace
    {
        private readonly XamlSchemaContext schemaContext;

        private readonly string codeNamespace;

        private readonly Dictionary<string, XamlType> types = new Dictionary<string, XamlType>();

        internal XamlNamespace(XamlSchemaContext schemaContext, string codeNamespace)
        {
            this.schemaContext = schemaContext;
            this.codeNamespace = codeNamespace;
        }

        private Type? GetType(string name)
        {
            foreach (Assembly assembly in new Assembly[] { })
            {
                Type? type = assembly.GetType(this.codeNamespace + '.' + name);
                if (type is not null)
                {
                    return type;
                }
            }

            return null;
        }

        internal XamlType GetXamlType(string name)
        {
            if (this.types.TryGetValue(name, out XamlType? xamlType))
            {
                return xamlType;
            }

            Type type = this.GetType(name);
            xamlType = this.schemaContext.GetXamlType(type);
            this.types.Add(name, xamlType);
            return xamlType;
        }
    }

    internal sealed class XamlSchemaContext
    {
        private readonly List<Assembly> referenceAssemblies;

        private readonly Dictionary<string, XamlNamespace> namespaces = new Dictionary<string, XamlNamespace>();
        private readonly Dictionary<Type, XamlType> types = new Dictionary<Type, XamlType>();

        internal XamlSchemaContext(List<Assembly> referenceAssemblies)
        {
            this.referenceAssemblies = referenceAssemblies;
        }

        private XamlNamespace GetXamlNamespace(string xmlNamespace)
        {
            if (this.namespaces.TryGetValue(xmlNamespace, out XamlNamespace? xamlNamespace))
            {
                return xamlNamespace;
            }

            xamlNamespace = new XamlNamespace(this, XamlNamespaceParser.ParseUri(xmlNamespace));
            this.namespaces.Add(xmlNamespace, xamlNamespace);
            return xamlNamespace;
        }

        private XamlType GetXamlType(string @namespace, string name)
        {
            XamlNamespace xamlNamespace = this.GetXamlNamespace(@namespace);
            return xamlNamespace.GetXamlType(name);
        }

        internal XamlType GetXamlType(Type type)
        {
            if (this.types.TryGetValue(type, out XamlType? xamlType))
            {
                return xamlType;
            }

            xamlType = new XamlType(type);
            this.types.Add(type, xamlType);
            return xamlType;
        }
    }

    internal sealed class XamlType
    {
        private readonly Type underlyingType;

        internal XamlType(Type underlyingType)
        {
            this.underlyingType = underlyingType;
        }
    }
}
