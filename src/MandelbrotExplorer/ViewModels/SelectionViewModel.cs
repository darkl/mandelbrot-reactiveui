using System.Windows;
using ReactiveUI;

namespace MandelbrotExplorer.ViewModels
{
    public class SelectionViewModel : ReactiveObject
    {
        private double _Left;
        private double _Top;
        private double _Width;
        private double _Height;
        private Visibility _Visible;

        public SelectionViewModel(Point edge, Point otherEdge)
        {
            Top = System.Math.Min(edge.Y, otherEdge.Y);
            Left = System.Math.Min(edge.X, otherEdge.X);
            Width = System.Math.Abs(edge.X - otherEdge.X);
            Height = System.Math.Abs(edge.Y - otherEdge.Y);
            Visible = Visibility.Visible;
        }

        public Visibility Visible
        {
            get
            {
                return _Visible;
            }
            set
            {
                this.RaiseAndSetIfChanged(x => x.Visible,
                                          value);
            }
        }


        public double Left
        {
            get
            {
                return _Left;
            }
            set
            {
                this.RaiseAndSetIfChanged(x => x.Left,
                                          value);
            }
        }

        public double Top
        {
            get
            {
                return _Top;
            }
            set
            {
                this.RaiseAndSetIfChanged(x => x.Top,
                                          value);
            }
        }

        public double Width
        {
            get
            {
                return _Width;
            }
            set
            {
                this.RaiseAndSetIfChanged(x => x.Width,
                                          value);
            }
        }

        public double Height
        {
            get
            {
                return _Height;
            }
            set
            {
                this.RaiseAndSetIfChanged(x => x.Height,
                                          value);
            }
        }

        protected bool Equals(SelectionViewModel other)
        {
            return _Left.Equals(other._Left) && _Top.Equals(other._Top) && _Width.Equals(other._Width) && _Height.Equals(other._Height) && _Visible.Equals(other._Visible);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SelectionViewModel) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = _Left.GetHashCode();
                hashCode = (hashCode*397) ^ _Top.GetHashCode();
                hashCode = (hashCode*397) ^ _Width.GetHashCode();
                hashCode = (hashCode*397) ^ _Height.GetHashCode();
                hashCode = (hashCode*397) ^ _Visible.GetHashCode();
                return hashCode;
            }
        }
    }
}