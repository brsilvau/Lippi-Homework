using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class AsyncEnumerableMock<T> : IAsyncEnumerable<T>, IAsyncEnumerator<T>
{
    private readonly IEnumerator<T> _enumerator;

    public AsyncEnumerableMock(IEnumerable<T> enumerable)
    {
        _enumerator = enumerable.GetEnumerator();
    }

    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        return this;
    }

    public ValueTask DisposeAsync()
    {
        _enumerator.Dispose();
        return ValueTask.CompletedTask;
    }

    public ValueTask<bool> MoveNextAsync()
    {
        return new ValueTask<bool>(_enumerator.MoveNext());
    }

    public T Current => _enumerator.Current;
}
