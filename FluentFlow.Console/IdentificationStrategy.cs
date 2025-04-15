using FluentFlow.Core.Code;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FluentFlow.Console;

public static class IdentificationStrategy
{
    public static AttributeArgumentSyntax DateTimeStamp() => AttributeArgument.Build((long.Parse(DateTime.Now.ToString("yyyyMMddHHmm"))));
}