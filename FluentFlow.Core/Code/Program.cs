// // See https://aka.ms/new-console-template for more information
//
// using Microsoft.CodeAnalysis;
// using Microsoft.CodeAnalysis.CSharp;
// using Microsoft.CodeAnalysis.CSharp.Syntax;
// using Rocketdot.Framework;
//
// var fb = new FluentBuilder("Fluent");
//
// for (int i = 0; i < 5; i++)
// {
//     fb.AddStep("Method" + i);
// }
//
//
// var myClass = ClassBuilder.Build(definition =>
// {
//     definition.Name = "Person";
//     definition.Properties = new List<MemberDeclarationSyntax>(2)
//     {
//         PropertyBuilder.Build(p =>
//         {
//             p.Name = "Name";
//             p.Type = SyntaxKind.StringKeyword;
//         }),
//         PropertyBuilder.Build(p =>
//         {
//             p.Name = "Age";
//             p.Type = SyntaxKind.IntKeyword;
//         })
//     };
//     definition.Constructor = ConstructorBuilder.Build(c => c.Name = "Person");
// });
//
// Console.WriteLine(myClass.NormalizeWhitespace());
//
// var fluent = new FluentBuilder(" Create.Table").AddStep("WithColumn", 
//
//     SyntaxFactory.Argument(SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal("argument1")))
// ).AddStep("AsInt64").AddStep("PrimaryKey", SyntaxFactory.Argument(NameOfBuilder.Build(x => x.Type = "Person.Name"))
//     
//     ).AddStep("Identity").Build();
// Console.WriteLine(fluent.NormalizeWhitespace());