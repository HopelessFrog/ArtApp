using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PetVideoApp.Extensions;
using PetVideoApp.Models;
using PetVideoApp.Popups.PopupViewModels;
using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core.Views;
using PetVideoApp.Helpers;

namespace PetVideoApp.ViewModels
{
    public partial class DrawingViewModel : ObservableObject
    {
        private readonly DrawingHistoryManager _historyManager;
        private readonly IPopupService _popupService;

        [ObservableProperty] private Color _selectedColor = Colors.Black;
        [ObservableProperty] private float _lineWidth = 5.0f;
        [ObservableProperty] private bool _isColorListVisible;
        [ObservableProperty] private bool _isStrokeSliderVisible;

        [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(UndoCommand))]
        private bool _canUndo;

        [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(RedoCommand))]
        private bool _canRedo;

        public ObservableCollection<IDrawingLine> DrawedLines { get; set; } = new();

        public ObservableCollection<Color> AvailableColors { get; set; } = new()
        {
            Colors.Black,
            Colors.Red,
            Colors.Blue,
            Colors.Green,
            Colors.Yellow,
            Colors.Orange,
            Colors.Purple,
            Colors.Pink
        };

        public DrawingViewModel(IPopupService popupService)
        {
            _popupService = popupService;
            _historyManager = new DrawingHistoryManager(DrawedLines);
            _historyManager.HistoryChanged += (s, e) =>
            {
                CanUndo = _historyManager.CanUndo;
                CanRedo = _historyManager.CanRedo;
            };
        }

        [RelayCommand]
        private async Task OnToggleColorList()
        {
            IsColorListVisible = !IsColorListVisible;
        }

        [RelayCommand]
        private async Task OnToggleStrokeSlider()
        {
            IsStrokeSliderVisible = !IsStrokeSliderVisible;
        }

        [RelayCommand]
        private async Task DrawingLine(IDrawingLine line)
        {
            _historyManager.Record(new LineAction() { Line = line, Type = ActionType.Add });
        }

        [RelayCommand]
        private void SelectColor(Color color)
        {
            SelectedColor = color;
        }

        [RelayCommand(CanExecute = nameof(CanUndo))]
        private void Undo()
        {
            _historyManager.Undo();
        }

        [RelayCommand(CanExecute = nameof(CanRedo))]
        private void Redo()
        {
            _historyManager.Redo();
        }

        [RelayCommand]
        private void Clear()
        {
            if (DrawedLines.Count == 0) return;

            _historyManager.Clear();

            DrawedLines.Clear();
        }

        [RelayCommand]
        private async Task ChangeColor()
        {
            var query = new Dictionary<string, object>()
            {
                [nameof(ColorPopupViewModel.Red)] = SelectedColor.Red,
                [nameof(ColorPopupViewModel.Green)] = SelectedColor.Green,
                [nameof(ColorPopupViewModel.Blue)] = SelectedColor.Blue,
                [nameof(ColorPopupViewModel.Alpha)] = SelectedColor.Alpha
            };
           var color =  await _popupService.ShowPopupAsync<ColorPopupViewModel, Color>(Shell.Current,
                shellParameters: query);

           if (color.Result is not null)
               SelectedColor = color.Result;
        }
    }
}