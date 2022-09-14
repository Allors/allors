namespace Allors.Repository.Domain
{
    using Allors.Text;
    using System.Collections.Generic;
    using System;
    using Inflector;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public class Field
    {

        private readonly Inflector inflector;

        public Field(Inflector inflector, SemanticModel semanticModel, Record record, PropertyDeclarationSyntax propertyDeclaration)
        {
            this.inflector = inflector;

            this.AttributeByName = new Dictionary<string, Attribute>();
            this.AttributesByName = new Dictionary<string, Attribute[]>();

            this.Record = record;

            var propertySymbol = (IPropertySymbol)semanticModel.GetDeclaredSymbol(propertyDeclaration);
            this.Name = propertySymbol.Name;

            var xmlDocString = propertySymbol.GetDocumentationCommentXml(null, true);
            this.XmlDoc = !string.IsNullOrWhiteSpace(xmlDocString) ? new XmlDoc(xmlDocString) : null;

            record.FieldByName.Add(this.Name, this);
        }

        public string Name { get; }

        public Record Record { get; }

        public XmlDoc XmlDoc { get; set; }

        public BehavioralType Type { get; set; }

        public Dictionary<string, Attribute> AttributeByName { get; }

        public Dictionary<string, Attribute[]> AttributesByName { get; }

        public bool IsOne => !this.IsMany;

        public bool IsMany { get; set; }

        public override string ToString() => $"{this.Record.Name}.{this.Name}";
    }
}
