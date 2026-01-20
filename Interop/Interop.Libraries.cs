
namespace CSharp.Quic;

internal static partial class Interop
{
    internal static partial class Libraries
    {
#if WINDOWS
        internal const string MsQuic = "msquic.dll";
#elif LINUX
        internal const string MsQuic = "libmsquic.so";
#elif OSX
        internal const string MsQuic = "libmsquic.dylib";
#else
        internal const string MsQuic = "msquic";
#endif
    }
}
