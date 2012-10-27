using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Input;

namespace MandelbrotExplorer.Extensions
{
    public static class ControlExtensions
    {
        public static IObservable<EventPattern<MouseButtonEventArgs>> GetMouseUp(this UIElement element)
        {
            return
                Observable.FromEventPattern<MouseButtonEventArgs>(
                    x => element.MouseUp += new MouseButtonEventHandler(x),
                    x => element.MouseUp -= new MouseButtonEventHandler(x));
        }

        public static IObservable<EventPattern<MouseButtonEventArgs>> GetMouseDown(this UIElement element)
        {
            return
                Observable.FromEventPattern<MouseButtonEventArgs>(
                    x => element.MouseDown += new MouseButtonEventHandler(x),
                    x => element.MouseDown -= new MouseButtonEventHandler(x));
        }

        public static IObservable<EventPattern<MouseEventArgs>> GetMouseMove(this UIElement element)
        {
            return
                Observable.FromEventPattern<MouseEventArgs>(
                    x => element.MouseMove += new MouseEventHandler(x),
                    x => element.MouseMove -= new MouseEventHandler(x));
        }
    }
}