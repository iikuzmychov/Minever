using Microsoft.CodeAnalysis;
using Minever.Data.Core;
using Minever.Data.Generators.Utils;
using System.Text;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Minever.Data.Generators;

[Generator]
public partial class BlocksGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        //Debugger.Launch();

        var jsonFiles     = context.AdditionalTextsProvider.Where(text => text.Path.EndsWith(".json"));
        var jsonWithPaths = jsonFiles.Select((file, cancellationToken) => (Json: file.GetText(cancellationToken)!.ToString(), file.Path));

        context.RegisterSourceOutput(jsonWithPaths, AddSource);
    }

    private void AddSource(SourceProductionContext context, (string Content, string Path) json)
    {
        var (edition, version) = ParseMincraftVersion(json.Path);

        var sourceBuilder = new StringBuilder();
        sourceBuilder.AppendLine($"using {nameof(Minever)}.{nameof(Data)}.{nameof(Core)};");
        sourceBuilder.AppendLine();
        sourceBuilder.AppendLine($"namespace {nameof(Minever)}.{nameof(Data)}.{edition}.{version}.Blocks;");
        sourceBuilder.AppendLine();

        var blocks = JsonConvert.DeserializeObject<BlockJsonInfo[]>(json.Content)!;

        foreach (var block in blocks)
        {
            sourceBuilder.AppendLine(GenerateBlockSourceCode(block));
            sourceBuilder.AppendLine();
        }

        context.AddSource($"Blocks.{edition}.{version}.g.cs", sourceBuilder.ToString());
    }

    private (string Edition, string Version) ParseMincraftVersion(string path)
    {
        var splitedPath = path.Split(new char[] { '\\', '/' });
        var edition     = splitedPath[splitedPath.Length - 3];
        var version     = splitedPath[splitedPath.Length - 2];

        return (edition.ToMinecraftEdition(), version.ToMinecraftVersion());
    }

    private string GenerateBlockSourceCode(BlockJsonInfo block) =>
        $@"public class {block.Name.ToPascalCase()} : {nameof(IBlock)}
{{
    public int {nameof(IBlock.Id)} {{ get; }} = {block.Id};
    public string {nameof(IBlock.Name)} {{ get; }} = ""{block.Name}"";
}}";
}