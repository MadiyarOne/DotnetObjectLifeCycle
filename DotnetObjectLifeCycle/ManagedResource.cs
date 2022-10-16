using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;

namespace DotnetObjectLifeCycle;


class Demo
{
    static void Main(string[] args)
    {
        // using var logger = new Logger("logs.txt");
        // var managedResource = new ManagedResource("Dummy.txt", logger);
        // managedResource.Process();
        // Console.WriteLine("Some logs");
        //

    }
}



public class ManagedResource : IDisposable
{
    //Managed resource
    private FileStream? _fileStreamResource;
    //Unmanaged resource
    private IntPtr _unmanagedContext;
    private Logger _logger;
    
    private bool _disposed = false;

    public ManagedResource(string filename, Logger logger)
    {
        try
        {
            _logger = logger;
            _logger.Log("Creating unmanaged and managed resources");
            AllocateUnmanagedResource();
            _fileStreamResource = File.OpenRead(filename);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Dispose();
            throw;
        }
    }

    private void AllocateUnmanagedResource()
    {
        _unmanagedContext = Marshal.AllocHGlobal(1000);
        if (_unmanagedContext == IntPtr.Zero)
        {
            throw new Exception("Failed to initialize unmanaged resource");
        }
    }

    public void Process()
    {
        if (_fileStreamResource == null) throw new NullReferenceException(nameof(_fileStreamResource));
        var buffer = new byte[_fileStreamResource.Length];
         _fileStreamResource.Read(buffer, 0, buffer.Length);
        Marshal.Copy(buffer, 0, _unmanagedContext, buffer.Length);
        buffer = new byte[1000];
        
        using var fs = File.OpenWrite("ProcessedDummy.txt");
        fs.Write(buffer);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;
        if (disposing)
        {
            //dispose managed resources
            _fileStreamResource?.Close();
            _fileStreamResource = null;
            _logger.Log("Managed resource freed");
            _logger.Dispose();
            _logger = null;
        }
        //free unmanaged resources
        if (_unmanagedContext != new IntPtr( /* 0 */))
        {
            //Dispose unmanaged context
            Marshal.FreeHGlobal(_unmanagedContext);
            _unmanagedContext = IntPtr.Zero;
            Console.WriteLine("Unmanaged resource freed");
        }
        
        
        _disposed = true;
    }

    ~ManagedResource()
    {
        Dispose(false);
    }
}

class InheritedManagedResource : ManagedResource
{
    private Stream _networkStream;
    public InheritedManagedResource(string filename, Logger logger, Stream networkStream) : base(filename, logger)
    {
        _networkStream = networkStream;
    }

    protected override void Dispose(bool disposing)
    {
        _networkStream?.Close();
        _networkStream = null;
        base.Dispose(disposing);
    }
}



public class Logger : IDisposable
{
    private readonly FileStream _fileStream;
    private readonly StreamWriter _writer;

    public Logger(string filename)
    {
        _fileStream = File.OpenWrite(filename);
        _writer = new StreamWriter(_fileStream);
    }


    protected virtual void Dispose(bool disposing)
    {
        if (!disposing) return;
        _writer.Close();
    }

    public void Log(string message)
    {
        _writer.WriteLine(message);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}