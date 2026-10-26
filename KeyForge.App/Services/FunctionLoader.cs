using System.IO;
using System.Reflection;
using KeyForge.SDK;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace KeyForge.App.Services;

public class FunctionLoader
{
    public IKeyForgeAction? Load(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException(
                "Function file was not found.",
                filePath
            );
        }

        string code = File.ReadAllText(filePath);

        var syntaxTree = CSharpSyntaxTree.ParseText(code);

        var trustedAssemblies = ((string?)AppContext.GetData(
            "TRUSTED_PLATFORM_ASSEMBLIES"
        ))?.Split(Path.PathSeparator);

        if (trustedAssemblies == null)
        {
            throw new Exception(
                "Could not get .NET runtime assemblies."
            );
        }

        var references = trustedAssemblies
            .Select(path => MetadataReference.CreateFromFile(path))
            .Cast<MetadataReference>()
            .ToList();

        references.Add(
            MetadataReference.CreateFromFile(
                typeof(IKeyForgeAction).Assembly.Location
            )
        );

        var compilation = CSharpCompilation.Create(
            assemblyName: Path.GetRandomFileName(),

            syntaxTrees: new[]
            {
                syntaxTree
            },

            references: references,

            options: new CSharpCompilationOptions(
                OutputKind.DynamicallyLinkedLibrary
            )
        );

        using var stream = new MemoryStream();

        var result = compilation.Emit(stream);

        if (!result.Success)
        {
            var errors = result.Diagnostics
                .Where(d => d.Severity == DiagnosticSeverity.Error)
                .Select(d => d.ToString());

            throw new Exception(
                "Function compilation failed:\n\n" +
                string.Join("\n", errors)
            );
        }

        stream.Seek(0, SeekOrigin.Begin);

        var assembly = Assembly.Load(
            stream.ToArray()
        );

        var actionType = assembly
            .GetTypes()
            .FirstOrDefault(type =>
                typeof(IKeyForgeAction).IsAssignableFrom(type) &&
                !type.IsInterface &&
                !type.IsAbstract
            );

        if (actionType == null)
        {
            throw new Exception(
                "No class implementing IKeyForgeAction was found."
            );
        }

        return Activator.CreateInstance(actionType)
            as IKeyForgeAction;
    }
}