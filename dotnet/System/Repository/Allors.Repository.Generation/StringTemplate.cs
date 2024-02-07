// <copyright file="StringTemplate.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Defines the Default type.
// </summary>

namespace Allors.Repository.Generation;

using System;
using System.IO;
using System.Linq;
using System.Xml;
using Antlr4.StringTemplate;
using Antlr4.StringTemplate.Misc;
using Allors.Repository.Model;
using NLog;

public class StringTemplate
{
    private const string TemplateId = "TemplateId";
    private const string TemplateName = "TemplateName";
    private const string TemplateVersion = "TemplateVersion";
    private const string TemplateConfiguration = "TemplateConfiguration";

    private const string TemplateKey = "template";
    private const string RepositoryKey = "repository";
    private const string InputKey = "input";
    private const string OutputKey = "output";
    private const string GenerationKey = "generation";
    private const string CompositeKey = "composite";
    private const string PropertyKey = "property";
    private const string MethodTypeKey = "method";

    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    private readonly FileInfo fileInfo;

    public StringTemplate(FileInfo fileInfo)
    {
        this.fileInfo = fileInfo;

        this.fileInfo.Refresh();
        if (!this.fileInfo.Exists)
        {
            throw new Exception("Template file not found: " + fileInfo.FullName);
        }

        TemplateGroup templateGroup = new TemplateGroupFile(this.fileInfo.FullName, '$', '$');

        this.Id = Render(templateGroup, TemplateId) != null ? new Guid(Render(templateGroup, TemplateId)) : Guid.Empty;
        this.Name = Render(templateGroup, TemplateName);
        this.Version = Render(templateGroup, TemplateVersion);

        if (this.Id == Guid.Empty)
        {
            throw new Exception("Template has no id");
        }
    }

    public Guid Id { get; }

    public string Name { get; }

    public string Version { get; }

    public override string ToString() => this.Name;

    public void Generate(RepositoryModel repository, DirectoryInfo outputDirectory)
    {
        TemplateGroup templateGroup = new TemplateGroupFile(this.fileInfo.FullName, '$', '$');

        templateGroup.ErrorManager = new ErrorManager(new LogAdapter());

        var configurationTemplate = templateGroup.GetInstanceOf(TemplateConfiguration);
        configurationTemplate.Add(RepositoryKey, repository);

        var configurationXml = new XmlDocument();
        configurationXml.LoadXml(configurationTemplate.Render());

        var location = new Location(outputDirectory);
        foreach (XmlElement generation in configurationXml.DocumentElement.SelectNodes(GenerationKey))
        {
            var templateName = generation.GetAttribute(TemplateKey);
            var template = templateGroup.GetInstanceOf(templateName);
            var output = generation.GetAttribute(OutputKey);

            template.Add(RepositoryKey, repository);

            if (generation.HasAttribute(InputKey))
            {
                var input = new Guid(generation.GetAttribute(InputKey));
                var compositeModel = repository.Composites.FirstOrDefault(v => v.IdAsGuid == input);
                if (compositeModel != null)
                {
                    template.Add(CompositeKey, compositeModel);
                }
                else
                {
                    var propertyModel = repository.Properties.FirstOrDefault(v => v.IdAsGuid == input);
                    if (propertyModel != null)
                    {
                        template.Add(PropertyKey, propertyModel);
                    }
                    else
                    {
                        var methodModel = repository.Methods.FirstOrDefault(v => v.IdAsGuid == input);
                        if (methodModel != null)
                        {
                            template.Add(MethodTypeKey, methodModel);
                        }
                        else
                        {
                            throw new ArgumentException(input + " was not found");
                        }
                    }
                }
            }

            var result = template.Render();
            location.Save(output, result);
        }
    }

    private static string Render(TemplateGroup templateGroup, string templateName)
    {
        var template = templateGroup.GetInstanceOf(templateName);
        if (template != null)
        {
            return template.Render();
        }

        return null;
    }

    private class LogAdapter : ITemplateErrorListener
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public void CompiletimeError(TemplateMessage msg) => Logger.Error(msg.ToString());

        public void RuntimeError(TemplateMessage msg) => Logger.Error(msg.ToString());

        public void IOError(TemplateMessage msg) => Logger.Error(msg.ToString());

        public void InternalError(TemplateMessage msg) => Logger.Error(msg.ToString());
    }
}
