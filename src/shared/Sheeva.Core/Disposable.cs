namespace Sheeva.Core;

public abstract class Disposable : IDisposable
{
    private bool disposedValue;

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void DisposeResources()
    {
    }

    private void Dispose(bool disposing)
    {
        if (this.disposedValue)
        {
            return;
        }

        if (disposing)
        {
            this.DisposeResources();
        }

        this.disposedValue = true;
    }
}
