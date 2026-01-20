using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp.Quic;

[Flags]
public enum QuicAbortDirection
{
    /// <summary>
    /// Abort the read side of the stream.
    /// </summary>
    Read = 1,
    /// <summary>
    /// Abort the write side of the stream.
    /// </summary>
    Write = 2,
    /// <summary>
    /// Abort both sides of the stream, i.e.: <see cref="Read"/> and <see cref="Write"/>) at the same time.
    /// </summary>
    Both = Read | Write
}
