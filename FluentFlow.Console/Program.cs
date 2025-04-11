using CommandLine;
using FluentFlow.Console;
using FluentFlow.Console.Model;

Parser.Default.ParseArguments<Options>(args).WithParsed(Migrator.Main);