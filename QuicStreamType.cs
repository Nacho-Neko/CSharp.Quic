using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp.Quic;

public enum QuicStreamType
{
    /// <summary>
    /// Write-only for the peer that opened the stream. Read-only for the peer that accepted the stream.
    /// </summary>
    Unidirectional,

    /// <summary>
    /// For both peers, read and write capable.
    /// </summary>
    Bidirectional
}