using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace MandelbrotExplorer.ViewModels
{
    public class DimensionsViewModel : ReactiveObject
    {
        private double _Left;
        private double _Right;
        private double _Top;
        private double _Bottom;
        private int _Iterations;
        
        private int _Width;
        private int _Height;

        public DimensionsViewModel()
        {
            Width = 800;
            Height = 600;
            Bottom = -1.5;
            Top = 1.5;
            Iterations = 200;
            Right = 2;
            Left = -2;
        }

        public int Width
        {
            get
            {
                return this._Width;
            }
            set
            {
                this.RaiseAndSetIfChanged(x => x.Width, value);
            }
        }

        public int Height
        {
            get
            {
                return this._Height;
            }
            set
            {
                this.RaiseAndSetIfChanged(x => x.Height, value);
            }
        }

        public double Left
        {
            get
            {
                return this._Left;
            }
            set
            {
                this.RaiseAndSetIfChanged(x => x.Left, value);
            }
        }

        public double Right
        {
            get
            {
                return this._Right;
            }
            set
            {
                this.RaiseAndSetIfChanged(x => x.Right, value);
            }
        }

        public double Top
        {
            get
            {
                return this._Top;
            }
            set
            {
                this.RaiseAndSetIfChanged(x => x.Top, value);
            }
        }

        public double Bottom
        {
            get
            {
                return this._Bottom;
            }
            set
            {
                this.RaiseAndSetIfChanged(x => x.Bottom, value);
            }
        }

        public int Iterations
        {
            get
            {
                return this._Iterations;
            }
            set
            {
                this.RaiseAndSetIfChanged(x => x.Iterations, value);
            }
        }

        public DimensionsViewModel GetSelectionDimensions(SelectionViewModel selection)
        {
            return new DimensionsViewModel()
                {
                    Height = 120,
                    Width = 160,
                    Iterations = Iterations,
                    Left = this.Left + selection.Left*(this.Right - this.Left)/(this.Width),
                    Right = this.Left + (selection.Left + selection.Width)*(this.Right - this.Left)/(this.Width),
                    Top = this.Top - selection.Top*(this.Top - this.Bottom)/(this.Height),
                    Bottom = this.Top - (selection.Top + selection.Height)*(this.Top - this.Bottom)/(this.Height)                    
                };
        }

        protected bool Equals(DimensionsViewModel other)
        {
            return _Left.Equals(other._Left) && _Right.Equals(other._Right) && _Top.Equals(other._Top) && _Bottom.Equals(other._Bottom) && _Iterations == other._Iterations && _Width == other._Width && _Height == other._Height;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DimensionsViewModel) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = _Left.GetHashCode();
                hashCode = (hashCode*397) ^ _Right.GetHashCode();
                hashCode = (hashCode*397) ^ _Top.GetHashCode();
                hashCode = (hashCode*397) ^ _Bottom.GetHashCode();
                hashCode = (hashCode*397) ^ _Iterations;
                hashCode = (hashCode*397) ^ _Width;
                hashCode = (hashCode*397) ^ _Height;
                return hashCode;
            }
        }
    }
}