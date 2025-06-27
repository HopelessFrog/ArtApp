using CommunityToolkit.Maui.Core;

namespace PetVideoApp.Models;

public enum ActionType
{
    Add,
    Remove
}

public class LineAction
{
    public required ActionType Type { get; set; }
    public required IDrawingLine Line { get; set; }
}
