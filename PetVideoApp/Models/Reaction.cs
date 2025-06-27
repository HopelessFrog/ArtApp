using CommunityToolkit.Mvvm.ComponentModel;

namespace PetVideoApp.Models;

public partial class Reaction : ObservableObject
{
    [ObservableProperty] private string _source;

    [ObservableProperty] private int _count;
    
}