using CommandLine;
using FluentFlow.Console;
using FluentFlow.Console.Model;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("Hello, FluentFlow!");

Parser.Default.ParseArguments<Options>(args).WithParsed(Migrator.Main);