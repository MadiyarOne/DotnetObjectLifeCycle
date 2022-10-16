using System.Net.Sockets;

namespace DotnetObjectLifeCycle;



class DisposeExample : IDisposable
{
    private bool disposed = false;


    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposed)
        {
            return;
        }

        if (disposing)
        {
            //release managed resources
        }
        //release unmanaged resources
    }

    ~DisposeExample()
    {
        Dispose(false);
    }
}

class CascadeDisposable : IDisposable
{
    private readonly Stream _stream;

    
    public CascadeDisposable(Stream stream)
    {
        _stream = stream;
    }


    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _stream.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}

class CascadeDisposableWithUnmanagedResources : IDisposable
{
    private Stream _stream;

    private void ReleaseUnmanagedResources()
    {
        // TODO release unmanaged resources here
    }

    protected virtual void Dispose(bool disposing)
    {
        ReleaseUnmanagedResources();
        if (disposing)
        {
            _stream.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~CascadeDisposableWithUnmanagedResources()
    {
        Dispose(false);
    }
}
