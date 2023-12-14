namespace Allors.Meta.Generation;

using System.IO;

public static class Generate
{
    public static Log Execute(Model.Model meta, string template, string output, string workspaceName = null)
    {
        var log = new GenerateLog();

        var templateFileInfo = new FileInfo(template);
        var stringTemplate = new StringTemplate(templateFileInfo);
        var outputDirectoryInfo = new DirectoryInfo(output);

        stringTemplate.Generate(meta, workspaceName, outputDirectoryInfo, log);

        return log;
    }
}
