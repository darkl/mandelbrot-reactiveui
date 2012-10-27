using System;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using MandelbrotExplorer.Extensions;
using MandelbrotExplorer.ViewModels;

namespace MandelbrotExplorer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            mSelection = GetSelection(FractalDisplay);
            this.DataContext = new MapViewModel(mSelection);
        }

        private IObservable<SelectionViewModel> GetSelection(UIElement relativeTo)
        {
            IObservable<SelectionViewModel> mouseMoveObservable =
                from mouseDown in relativeTo.GetMouseDown()
                let mouseDownPosition = mouseDown.EventArgs.GetPosition(relativeTo)
                from mouseMove in relativeTo.GetMouseMove().TakeUntil(relativeTo.GetMouseUp())
                let mouseMovePosition = mouseMove.EventArgs.GetPosition(relativeTo)
                select new SelectionViewModel(mouseDownPosition, mouseMovePosition);

            return mouseMoveObservable;
        }

        private readonly IObservable<SelectionViewModel> mSelection;
    }
}
