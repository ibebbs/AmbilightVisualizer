using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Media;
using Caliburn.Micro;
using Caliburn.Micro.Reactive.Extensions;
using System.Diagnostics;

namespace Ambilight 
{
    public class ShellViewModel : Screen, IShell
    {
        private static readonly Brush DefaultBrush = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));

        private readonly ObservableProperty<Brush> _topBrush;
        private readonly ObservableProperty<Brush> _leftBrush;
        private readonly ObservableProperty<Brush> _rightBrush;
        private readonly ObservableProperty<Brush> _bottomBrush;
        private readonly ObservableProperty<int> _fps;
        private readonly IConnectableObservable<Processed> _source;

        private IDisposable _brushSubscription;
        private IDisposable _providerSubscription;

        public ShellViewModel(IProvider provider)
        {
            _topBrush = new ObservableProperty<Brush>(DefaultBrush, this, () => TopBrush);
            _leftBrush = new ObservableProperty<Brush>(DefaultBrush, this, () => LeftBrush);
            _rightBrush = new ObservableProperty<Brush>(DefaultBrush, this, () => RightBrush);
            _bottomBrush = new ObservableProperty<Brush>(DefaultBrush, this, () => BottomBrush);
            _fps = new ObservableProperty<int>(0, this, () => Fps);

            _source = provider.Processed.ObserveOnDispatcher().Publish();

            _brushSubscription = new CompositeDisposable(
                _source.Buffer(TimeSpan.FromSeconds(1)).Select(buffer => buffer.Count).Subscribe(_fps),
                _source.Select(processed => processed.Top).Select(colors => AsGradientBrush(colors, Side.Top)).Subscribe(_topBrush),
                _source.Select(processed => processed.Left).Select(colors => AsGradientBrush(colors, Side.Left)).Subscribe(_leftBrush),
                _source.Select(processed => processed.Right).Select(colors => AsGradientBrush(colors, Side.Right)).Subscribe(_rightBrush),
                _source.Select(processed => processed.Bottom).Select(colors => AsGradientBrush(colors, Side.Bottom)).Subscribe(_bottomBrush)
            );
        }

        private Brush AsGradientBrush(Color[] arg, Side side)
        {
            if (arg != null && arg.Length > 0)
            {
                double offsetIncrement = 1.0 / arg.Length;
                double angle = (side == Side.Left || side == Side.Right) ? 90.0 : 0.0;

                return new LinearGradientBrush(new GradientStopCollection(arg.Select((color, index) => new GradientStop(color, index * offsetIncrement))), angle);
            }
            else
            {
                return DefaultBrush;
            }
        }

        protected override void OnActivate()
        {
            base.OnActivate();

            _providerSubscription = _source.Connect();
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);

            if (close)
            {
                if (_providerSubscription != null)
                {
                    _providerSubscription.Dispose();
                    _providerSubscription = null;
                }

                if (_brushSubscription != null)
                {
                    _brushSubscription.Dispose();
                    _brushSubscription = null;
                }
            }
        }

        public Brush TopBrush 
        {
            get { return _topBrush.Get(); }
        }

        public Brush LeftBrush 
        {
            get { return _leftBrush.Get(); }
        }

        public Brush RightBrush 
        {
            get { return _rightBrush.Get(); }
        }

        public Brush BottomBrush 
        {
            get { return _bottomBrush.Get(); }
        }

        public int Fps
        {
            get { return _fps.Get(); }
        }
    }
}