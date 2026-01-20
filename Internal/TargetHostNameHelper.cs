// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Globalization;

namespace CSharp.Quic;

internal static class TargetHostNameHelper
{
    /// <summary>
    /// Normalizes the host name for certificate validation.
    /// </summary>
    internal static string NormalizeHostName(string hostName)
    {
        if (string.IsNullOrEmpty(hostName))
        {
            return string.Empty;
        }

        // Remove leading/trailing whitespace
        hostName = hostName.Trim();

        // Convert to lowercase for case-insensitive comparison
        // Use IdnMapping to handle internationalized domain names (IDN)
        try
        {
            IdnMapping idn = new IdnMapping();
            return idn.GetAscii(hostName);
        }
        catch
        {
            // If IDN conversion fails, just return lowercase
            return hostName.ToLowerInvariant();
        }
    }
}