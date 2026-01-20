// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CSharp.Quic;

/// <summary>
/// Congestion control algorithms supported by MsQuic.
/// </summary>
public enum QuicCongestionControlAlgorithm
{
    /// <summary>
    /// Cubic congestion control algorithm (default).
    /// </summary>
    Cubic = 0,

    /// <summary>
    /// BBR (Bottleneck Bandwidth and RTT) congestion control algorithm.
    /// Provides better throughput in high-latency networks.
    /// </summary>
    BBR = 1,

    /// <summary>
    /// Use the platform default algorithm.
    /// </summary>
    Default = Cubic
}