using Talkie.Signals;

namespace Talkie.Interceptors;

internal sealed class TakeSignalInterceptor(int count) : ISignalInterceptor
{
    private readonly object _locker = new();

    private int _iterations;

    public InterceptionResult Intercept(Signal signal, CancellationToken cancellationToken)
    {
        lock (_locker)
        {
            if (_iterations == count)
            {
                return InterceptionResult.Break();
            }

            ++_iterations;

            return InterceptionResult.Continue();
        }
    }
}
