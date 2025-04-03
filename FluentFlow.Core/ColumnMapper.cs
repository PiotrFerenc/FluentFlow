namespace FluentFlow.Core;

public static class ColumnMapper
{
    public static string Map(CSharpType type)
        => CSharpTypeMapper.Mappings.TryGetValue(type, out var map) ? map : string.Empty;
}