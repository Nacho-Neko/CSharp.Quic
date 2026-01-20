
using System;
using System.IO;
using System.Text;

namespace CSharp.Quic;

/// <summary>
/// Logs TLS secrets to a file for debugging with tools like Wireshark.
/// Enabled via the SSLKEYLOGFILE environment variable.
/// </summary>
internal static class SslKeyLogger
{
    private static readonly string? s_keyLogFile = Environment.GetEnvironmentVariable("SSLKEYLOGFILE");

    private static readonly object s_lock = new object();

    static SslKeyLogger()
    {
        // Check if SSLKEYLOGFILE environment variable is set
        if (!string.IsNullOrEmpty(s_keyLogFile))
        {
            try
            {
                // Ensure the file can be created/opened
                using (File.Open(s_keyLogFile, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                {
                    // File is accessible
                }

                if (NetEventSource.Log.IsEnabled())
                {
                    NetEventSource.Info(null, $"TLS key logging enabled. Writing to: {s_keyLogFile}");
                }
            }
            catch (Exception ex)
            {
                if (NetEventSource.Log.IsEnabled())
                {
                    NetEventSource.Error(null, $"Failed to initialize SSL key log file '{s_keyLogFile}': {ex.Message}");
                }
                s_keyLogFile = null;
            }
        }
    }

    /// <summary>
    /// Gets whether TLS key logging is enabled.
    /// </summary>
    public static bool IsEnabled => !string.IsNullOrEmpty(s_keyLogFile);

    /// <summary>
    /// Writes TLS secrets to the key log file in NSS key log format.
    /// </summary>
    public static void WriteSecrets(
        ReadOnlySpan<byte> clientRandom,
        Span<byte> clientHandshakeTrafficSecret,
        Span<byte> serverHandshakeTrafficSecret,
        Span<byte> clientTrafficSecret0,
        Span<byte> serverTrafficSecret0,
        Span<byte> clientEarlyTrafficSecret)
    {
        if (!IsEnabled || clientRandom.IsEmpty)
        {
            return;
        }

        try
        {
            lock (s_lock)
            {
                using StreamWriter writer = new StreamWriter(s_keyLogFile!, append: true, Encoding.ASCII);

                // NSS Key Log Format: https://firefox-source-docs.mozilla.org/security/nss/legacy/key_log_format/index.html
                // Format: <Label> <ClientRandom> <Secret>

                if (!clientHandshakeTrafficSecret.IsEmpty)
                {
                    WriteSecret(writer, "CLIENT_HANDSHAKE_TRAFFIC_SECRET", clientRandom, clientHandshakeTrafficSecret);
                }

                if (!serverHandshakeTrafficSecret.IsEmpty)
                {
                    WriteSecret(writer, "SERVER_HANDSHAKE_TRAFFIC_SECRET", clientRandom, serverHandshakeTrafficSecret);
                }

                if (!clientTrafficSecret0.IsEmpty)
                {
                    WriteSecret(writer, "CLIENT_TRAFFIC_SECRET_0", clientRandom, clientTrafficSecret0);
                }

                if (!serverTrafficSecret0.IsEmpty)
                {
                    WriteSecret(writer, "SERVER_TRAFFIC_SECRET_0", clientRandom, serverTrafficSecret0);
                }

                if (!clientEarlyTrafficSecret.IsEmpty)
                {
                    WriteSecret(writer, "CLIENT_EARLY_TRAFFIC_SECRET", clientRandom, clientEarlyTrafficSecret);
                }
            }
        }
        catch (Exception ex)
        {
            if (NetEventSource.Log.IsEnabled())
            {
                NetEventSource.Error(null, $"Failed to write SSL key log: {ex.Message}");
            }
        }
    }

    private static void WriteSecret(StreamWriter writer, string label, ReadOnlySpan<byte> clientRandom, Span<byte> secret)
    {
        // Format: <Label> <ClientRandom_hex> <Secret_hex>
        writer.Write(label);
        writer.Write(' ');
        WriteHex(writer, clientRandom);
        writer.Write(' ');
        WriteHex(writer, secret);
        writer.WriteLine();
    }

    private static void WriteHex(StreamWriter writer, ReadOnlySpan<byte> data)
    {
        Span<char> hexChars = stackalloc char[data.Length * 2];

        for (int i = 0; i < data.Length; i++)
        {
            byte b = data[i];
            hexChars[i * 2] = GetHexChar(b >> 4);
            hexChars[i * 2 + 1] = GetHexChar(b & 0x0F);
        }

        writer.Write(hexChars);
    }

    private static char GetHexChar(int value)
    {
        return (char)(value < 10 ? '0' + value : 'a' + (value - 10));
    }
}