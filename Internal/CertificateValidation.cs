// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace CSharp.Quic;

internal static class CertificateValidation
{
    /// <summary>
    /// Builds the certificate chain and verifies its properties.
    /// </summary>
    /// <param name="chain">The X509Chain to build and verify.</param>
    /// <param name="certificate">The certificate to validate.</param>
    /// <param name="checkCertName">Whether to check the certificate name against the target host.</param>
    /// <param name="isServer">True if validating a server certificate (client mode), false for client certificate (server mode).</param>
    /// <param name="targetHost">The target host name to validate against (for server certificates).</param>
    /// <param name="certData">The raw certificate data (optional, used for additional checks).</param>
    /// <returns>SslPolicyErrors indicating any validation failures.</returns>
    internal static SslPolicyErrors BuildChainAndVerifyProperties(
        X509Chain chain,
        X509Certificate2 certificate,
        bool checkCertName,
        bool isServer,
        string? targetHost,
        ReadOnlySpan<byte> certData = default)
    {
        SslPolicyErrors errors = SslPolicyErrors.None;

        // Build the certificate chain
        bool chainBuilt = chain.Build(certificate);

        // Check chain build status
        if (!chainBuilt)
        {
            errors |= SslPolicyErrors.RemoteCertificateChainErrors;
        }

        // Check for specific chain errors
        foreach (X509ChainStatus status in chain.ChainStatus)
        {
            // Ignore NoError and UntrustedRoot (if it's self-signed and we're validating it)
            if (status.Status == X509ChainStatusFlags.NoError)
            {
                continue;
            }

            // For self-signed certificates in testing, you might want to ignore UntrustedRoot
            // Uncomment the following if you need to support self-signed certs:
            // if (status.Status == X509ChainStatusFlags.UntrustedRoot)
            // {
            //     continue;
            // }

            errors |= SslPolicyErrors.RemoteCertificateChainErrors;
            break;
        }

        // Check certificate name (for server certificates only)
        if (checkCertName && isServer && !string.IsNullOrEmpty(targetHost))
        {
            if (!VerifyCertificateName(certificate, targetHost))
            {
                errors |= SslPolicyErrors.RemoteCertificateNameMismatch;
            }
        }

        return errors;
    }

    /// <summary>
    /// Verifies that the certificate name matches the target host.
    /// </summary>
    private static bool VerifyCertificateName(X509Certificate2 certificate, string targetHost)
    {
        // Check Subject Alternative Names (SAN)
        foreach (X509Extension extension in certificate.Extensions)
        {
            if (extension is X509SubjectAlternativeNameExtension sanExtension)
            {
                foreach (string dnsName in sanExtension.EnumerateDnsNames())
                {
                    if (MatchHostName(targetHost, dnsName))
                    {
                        return true;
                    }
                }

                foreach (System.Net.IPAddress ipAddress in sanExtension.EnumerateIPAddresses())
                {
                    if (System.Net.IPAddress.TryParse(targetHost, out var targetIp) && 
                        ipAddress.Equals(targetIp))
                    {
                        return true;
                    }
                }
            }
        }

        // Fallback to Common Name (CN) in Subject
        string? commonName = certificate.GetNameInfo(X509NameType.SimpleName, false);
        if (!string.IsNullOrEmpty(commonName) && MatchHostName(targetHost, commonName))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Matches a hostname against a certificate name pattern (supports wildcards).
    /// </summary>
    private static bool MatchHostName(string hostname, string pattern)
    {
        hostname = hostname.ToLowerInvariant();
        pattern = pattern.ToLowerInvariant();

        // Exact match
        if (hostname == pattern)
        {
            return true;
        }

        // Wildcard match (e.g., *.example.com)
        if (pattern.StartsWith("*."))
        {
            string patternDomain = pattern.Substring(2);
            int dotIndex = hostname.IndexOf('.');
            if (dotIndex >= 0)
            {
                string hostDomain = hostname.Substring(dotIndex + 1);
                return hostDomain == patternDomain;
            }
        }

        return false;
    }
}