using System.Numerics;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MandelbrotExplorer.ViewModels;

namespace MandelbrotExplorer.Math
{
    public class MandelbrotFractal
    {
        private int[,] _IterationMap;
        private int _Width;
        private int _Height;
        private int _MaxIterations;
        private double _Left;
        private double _Right;
        private double _Top;
        private double _Bottom;

        public MandelbrotFractal(int width = 640, int height = 480, int maxIterations = 100,
                                 double left = -2, double right = 2, double top = 1.5, double bottom = -1.5)
        {
            _Width = width;
            _Height = height;
            _MaxIterations = maxIterations;
            _Left = left;
            _Right = right;
            _Top = top;
            _Bottom = bottom;
            _IterationMap = InitIterations();
        }

        private int[,] InitIterations()
        {
            int[,] iterationResult = new int[_Width, _Height];

            Parallel.For(0, _Width,
                         i => Parallel.For(0, _Height,
                                           j =>
                                           iterationResult[i, j] =
                                           IterateForPixel(i, j, _MaxIterations)));

            return iterationResult;
        }

        private int IterateForPixel(int i, int j, int iterationNumber)
        {
            double x = _Left +
                       i*(_Right - _Left)/
                       (_Width - 1);
            
            double y = _Top -
                       j*(_Top - _Bottom)/
                       (_Height - 1);
            
            return Iteration(x, y, iterationNumber);
        }

        private int Iteration(double x, double y, int iterationNumber)
        {
            int iteration = 0;

            Complex first = new Complex(x, y);
            Complex current = first;

            while (current.Magnitude < 2 &&
                   iteration < iterationNumber)
            {
                current = current * current + first;
                iteration++;
            }

            return iteration;
        }

        public ImageSource GetImage(int iterationNumber)
        {
            int[,] iterations = GetIterationMap(iterationNumber);

            BitmapSource bitmap;
            PixelFormat pf = PixelFormats.Rgb24;
            int width, height, rawStride;
            byte[] pixelData;
            width = _Width;
            height = _Height;
            rawStride = (width * pf.BitsPerPixel + 7) / 8;
            pixelData = new byte[rawStride * height];

            for (int i = 0; i < _Width; i++)
            {
                for (int j = 0; j < _Height; j++)
                {
                    SetPixel(i, j, iterations[i, j], pixelData, rawStride, iterationNumber);
                }
            }

            bitmap = BitmapSource.Create(width, height,
                                         96, 96, pf, null, pixelData, rawStride);
            return bitmap;
        }

        private int[,] GetIterationMap(int iterationNumber)
        {
            if (iterationNumber >= _MaxIterations)
            {
                RecalculateIterationMap(iterationNumber);
                return _IterationMap;
            }
            else
            {
                int[,] result = new int[_Width, _Height];

                for (int i = 0; i < _Width; i++)
                {
                    for (int j = 0; j < _Height; j++)
                    {
                        result[i, j] = System.Math.Min(_IterationMap[i, j], iterationNumber);
                    }
                }

                return result;
            }
        }

        private void RecalculateIterationMap(int iterationNumber)
        {
            for (int i = 0; i < _Width; i++)
            {
                for (int j = 0; j < _Height; j++)
                {
                    if (_IterationMap[i, j] == _MaxIterations)
                    {
                        _IterationMap[i, j] = IterateForPixel(i, j, iterationNumber);
                    }
                }
            }

            _MaxIterations = iterationNumber;
        }

        private static void SetPixel(int x, int y, int c, byte[] buffer, int rawStride, int iterations)
        {
            int xIndex = x * 3;
            int yIndex = y * rawStride;
            c = iterations - c;
            byte color = (byte)((c * 255) / iterations);
            buffer[xIndex + yIndex] = color;
            buffer[xIndex + yIndex + 1] = color;
            buffer[xIndex + yIndex + 2] = color;
        }

        public static ImageSource GetImageFromDimensions(DimensionsViewModel dimensions)
        {
            MandelbrotFractal fractal = new MandelbrotFractal
                (dimensions.Width,
                 dimensions.Height,
                 dimensions.Iterations,
                 dimensions.Left,
                 dimensions.Right,
                 dimensions.Top,
                 dimensions.Bottom);

            return fractal.GetImage(fractal._MaxIterations);
        }
    }
}