using CommandLine;
using FluentFlow.Console;
using FluentFlow.Console.Model;
using Microsoft.Extensions.DependencyInjection;

Parser.Default.ParseArguments<Options>(args).WithParsed(Migrator.Main);