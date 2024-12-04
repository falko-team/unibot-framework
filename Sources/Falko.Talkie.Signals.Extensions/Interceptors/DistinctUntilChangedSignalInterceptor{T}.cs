using Talkie.Signals;

namespace Talkie.Interceptors;

internal sealed class DistinctUntilChangedSignalInterceptor<TSignal, TValue>(Func<TSignal, CancellationToken, TValue> select)
    : SignalInterceptor<TSignal> where TSignal : Signal
{
    private readonly Lock _locker = new();

    private bool _firstTime = true;

    private TValue _lastValue = default!;

    public override InterceptionResult Intercept(TSignal signal, CancellationToken cancellationToken)
    {
        for (;;)
        {
            var currentValue = select(signal, cancellationToken);
            var lastValue = _lastValue;

            if (_firstTime is false && Equals(currentValue, lastValue))
            {
                return InterceptionResult.Break();
            }

            lock (_locker)
            {
                if (_firstTime is false && Equals(_lastValue, lastValue) is false) continue;

                _firstTime = false;
                _lastValue = currentValue;

                return InterceptionResult.Continue();
            }
        }
    }
}
