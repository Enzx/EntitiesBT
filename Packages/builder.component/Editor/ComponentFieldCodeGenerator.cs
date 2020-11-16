using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using EntitiesBT.Variant;
using Unity.Entities;
using UnityEditor;

namespace EntitiesBT.Editor
{
    public class BlobArrayFieldCodeGenerator : INodeDataFieldCodeGenerator
    {
        public bool ShouldGenerate(FieldInfo fi)
        {
            return fi.FieldType.IsGenericType && fi.FieldType.GetGenericTypeDefinition() == typeof(BlobArray<>);
        }

        public string GenerateField(FieldInfo fi)
        {
            return $"public {fi.FieldType.GenericTypeArguments[0].FullName}[] {fi.Name};";
        }

        public string GenerateBuild(FieldInfo fi)
        {
            return $"builder.AllocateArray(ref data.{fi.Name}, {fi.Name});";
        }
    }

    public class BlobStringFieldCodeGenerator : INodeDataFieldCodeGenerator
    {
        public bool ShouldGenerate(FieldInfo fi)
        {
            return fi.FieldType.IsGenericType && fi.FieldType.GetGenericTypeDefinition() == typeof(BlobString);
        }

        public string GenerateField(FieldInfo fi)
        {
            return $"public string {fi.Name};";
        }

        public string GenerateBuild(FieldInfo fi)
        {
            return $"builder.AllocateString(ref data.{fi.Name}, {fi.Name});";
        }
    }

    public abstract class BlobVariantFieldCodeGenerator : INodeDataFieldCodeGenerator
    {
        private const string _HEAD_LINE = "// Automatically generated by `BlobVariantFieldCodeGenerator`";

        public bool ShouldGenerateVariantInterface = true;
        public string VariantInterfaceDirectory = "Variant";
        public string VariantInterfaceNamespace = "EntitiesBT.Variant";
        public string VariantPropertyNameSuffix = "";
        public NodeCodeGenerator Generator;
        protected abstract string _variantType { get; }
        private string _suffix => $"{_variantType}{VariantPropertyNameSuffix}";

        public bool ShouldGenerate(FieldInfo fi)
        {
            return fi.FieldType.IsGenericType && fi.FieldType.GetGenericTypeDefinition() == typeof(BlobVariantReader<>);
        }

        public string GenerateField(FieldInfo fi)
        {
            var valueType = fi.FieldType.GetGenericArguments()[0];
            if (ShouldGenerateVariantInterface) GenerateVariantInterface();
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("[UnityEngine.SerializeReference, SerializeReferenceButton]");
            stringBuilder.Append(" ");
            stringBuilder.AppendLine($"public {VariantInterfaceNamespace}.{valueType.Name}{_suffix} {fi.Name};");
            return stringBuilder.ToString();

            void GenerateVariantInterface()
            {
                var directory = Path.GetDirectoryName(AssetDatabase.GetAssetPath(Generator)) + "/" + VariantInterfaceDirectory;
                if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

                var filepath = $"{directory}/{valueType.Name}{_suffix}.cs";
                if (!File.Exists(filepath) || File.ReadLines(filepath).FirstOrDefault() == _HEAD_LINE)
                {
                    using (var writer = new StreamWriter(filepath))
                    {
                        writer.WriteLine(_HEAD_LINE);
                        writer.WriteLine(VariantGenerator.NamespaceBegin(VariantInterfaceNamespace));

                        writer.CreateReaderVariants(valueType, null, _suffix);
                        var writerSuffix = $"Writer{VariantPropertyNameSuffix}";

                        writer.WriteLine(VariantGenerator.NamespaceEnd());
                    }
                }
            }
        }

        public string GenerateBuild(FieldInfo fi)
        {
            return $"{fi.Name}.Allocate(ref builder, ref data.{fi.Name}, Self, tree);";
        }
    }

    [Serializable]
    public class BlobVariantFieldCodeGeneratorForOdin : INodeDataFieldCodeGenerator
    {
        public bool ShouldGenerate(FieldInfo fi)
        {
            return fi.FieldType.IsGenericType && fi.FieldType.GetGenericTypeDefinition() == typeof(BlobVariantReader<>);
        }

        public string GenerateField(FieldInfo fi)
        {
            var variantType = fi.FieldType.GetGenericArguments()[0];
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("[OdinSerialize, NonSerialized]");
            stringBuilder.Append("        ");
            stringBuilder.AppendLine($"public EntitiesBT.Variant.VariantProperty<{variantType.FullName}> {fi.Name};");
            return stringBuilder.ToString();
        }

        public string GenerateBuild(FieldInfo fi)
        {
            return $"{fi.Name}.Allocate(ref builder, ref data.{fi.Name}, Self, tree);";
        }
    }
}
