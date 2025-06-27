using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using PetVideoApp.ViewModels;

namespace PetVideoApp.Pages;

public partial class DrawingPage : ContentPage
{
    public DrawingPage(DrawingViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
  
}