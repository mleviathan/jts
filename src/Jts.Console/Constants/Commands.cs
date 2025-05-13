using System.Data;

namespace Jts.Constants;

public static class Commands
{
    public static readonly string[] AvailableCommands = [Exit, Help, Copy, Issues];
    public const string Exit = "exit";
    public const string Help = "help";
    public const string Copy = "copy";
    public const string Issues = "issues";
}