using System;
using System.Reactive.Linq;
using System.Windows.Media;
using MandelbrotExplorer.Math;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace MandelbrotExplorer.ViewModels
{
    public class MapViewModel : ReactiveObject
    {
        private SelectionViewModel _Selection;
        private DimensionsViewModel _Dimensions;
        private ObservableAsPropertyHelper<ImageSource> _Output;
        private ObservableAsPropertyHelper<MandelbrotFractal> _Fractal;
        private ObservableAsPropertyHelper<ImageSource> _Preview;
        private ObservableAsPropertyHelper<DimensionsViewModel> _PreviewDimensions;

        public MapViewModel(IObservable<SelectionViewModel> selection)
        {
            this.Dimensions = new DimensionsViewModel();

            selection.Subscribe(x => this.Selection = x);
            // Doesn't work..
            //selection.BindTo(this, x => x.Selection);

            this.WhenAny(x => x.Dimensions.Iterations,
                         x => x.Selection,
                         (iterations, selectedArea) => selectedArea.Value)
                .Where(x => x != null)
                .Select(x => Dimensions.GetSelectionDimensions(x))
                .ToProperty(this, x => x.PreviewDimensions);

            this.WhenAny(x => x.PreviewDimensions,
                         x => x.Value)
                .Throttle(TimeSpan.FromSeconds(1))
                .ObserveOnDispatcher()
                .Where(x => x != null)
                .Select(x =>
                        MandelbrotFractal.GetImageFromDimensions(x))
                .ToProperty(this, x => x.Preview);

            this.WhenAny(x => x.Dimensions,
                         x => x.Value)
                .Select(x => new MandelbrotFractal(_Dimensions.Width, _Dimensions.Height,
                                                   _Dimensions.Iterations, _Dimensions.Left,
                                                   _Dimensions.Right, _Dimensions.Top,
                                                   _Dimensions.Bottom))
                .ToProperty(this,
                            x => x.Fractal);

            this.WhenAny(x => x.Dimensions.Iterations,
                         x => x.Fractal,
                         (x, dimensions) => x.Value).
                Throttle(TimeSpan.FromSeconds(1)).
                ObserveOnDispatcher().
                Select(x => Fractal.GetImage(x))
                .ToProperty(this, x => x.Output);

            this.ZoomCommand =
                new ReactiveCommand(this.WhenAny(x => x.Selection,
                                                 x => x.Value).Select(x => x != null));

            this.ZoomCommand.Subscribe(x =>
            {
                Dimensions = new DimensionsViewModel()
                    {
                        Height = Dimensions.Height,
                        Width = Dimensions.Width,
                        Iterations = Dimensions.Iterations,
                        Bottom = PreviewDimensions.Bottom,
                        Top = PreviewDimensions.Top,
                        Left = PreviewDimensions.Left,
                        Right = PreviewDimensions.Right
                    };

                Selection = null;
            });
        }

        public MandelbrotFractal Fractal
        {
            get
            {
                return _Fractal.Value;
            }
        }


        public ReactiveCommand ZoomCommand
        {
            get;
            private set;
        }

        public DimensionsViewModel PreviewDimensions
        {
            get
            {
                return _PreviewDimensions.Value;
            }
        }

        public ImageSource Preview
        {
            get
            {
                return _Preview.Value;
            }
        }

        public SelectionViewModel Selection
        {
            get
            {
                return this._Selection;
            }
            set
            {
                this.RaiseAndSetIfChanged(x => x.Selection, value);
            }
        }

        public DimensionsViewModel Dimensions
        {
            get
            {
                return this._Dimensions;
            }
            set
            {
                this.RaiseAndSetIfChanged(x => x.Dimensions, value);                
            }
        }

        public ImageSource Output
        {
            get
            {
                return this._Output.Value;
            }
        }
    }
}