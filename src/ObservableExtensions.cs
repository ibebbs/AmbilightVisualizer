using System;
using System.Reactive.Linq;

namespace Ambilight
{
    public static class ObservableExtensions
    {
        public static IObservable<T> OnSubscription<T>(this IObservable<T> source,  Action action)
        {
            return Observable.Create<T>(
                observer =>
                {
                    IDisposable subscription = source.Subscribe(observer);

                    action();

                    return subscription;
                }
            );
        }
    }
}
