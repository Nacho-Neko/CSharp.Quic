// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CSharp.Quic;

/// <summary>
/// TLS Alert messages as defined in RFC 5246 and RFC 8446
/// TLS 警报消息，定义在 RFC 5246 和 RFC 8446
/// </summary>
internal enum TlsAlertMessage : byte
{
    /// <summary>
    /// Close notification (0)
    /// </summary>
    CloseNotify = 0,

    /// <summary>
    /// Unexpected message (10)
    /// </summary>
    UnexpectedMessage = 10,

    /// <summary>
    /// Bad record MAC (20)
    /// </summary>
    BadRecordMac = 20,

    /// <summary>
    /// Decryption failed (21) - TLS 1.0 only
    /// </summary>
    DecryptionFailed = 21,

    /// <summary>
    /// Record overflow (22)
    /// </summary>
    RecordOverflow = 22,

    /// <summary>
    /// Decompression failure (30) - deprecated
    /// </summary>
    DecompressionFailure = 30,

    /// <summary>
    /// Handshake failure (40)
    /// </summary>
    HandshakeFailure = 40,

    /// <summary>
    /// No certificate (41) - SSL 3.0 only
    /// </summary>
    NoCertificate = 41,

    /// <summary>
    /// Bad certificate (42)
    /// </summary>
    BadCertificate = 42,

    /// <summary>
    /// Unsupported certificate (43)
    /// </summary>
    UnsupportedCertificate = 43,

    /// <summary>
    /// Certificate revoked (44)
    /// </summary>
    CertificateRevoked = 44,

    /// <summary>
    /// Certificate expired (45)
    /// </summary>
    CertificateExpired = 45,

    /// <summary>
    /// Certificate unknown (46)
    /// </summary>
    CertificateUnknown = 46,

    /// <summary>
    /// Illegal parameter (47)
    /// </summary>
    IllegalParameter = 47,

    /// <summary>
    /// Unknown CA (48)
    /// </summary>
    UnknownCA = 48,

    /// <summary>
    /// Access denied (49)
    /// </summary>
    AccessDenied = 49,

    /// <summary>
    /// Decode error (50)
    /// </summary>
    DecodeError = 50,

    /// <summary>
    /// Decrypt error (51)
    /// </summary>
    DecryptError = 51,

    /// <summary>
    /// Export restriction (60) - deprecated
    /// </summary>
    ExportRestriction = 60,

    /// <summary>
    /// Protocol version (70)
    /// </summary>
    ProtocolVersion = 70,

    /// <summary>
    /// Insufficient security (71)
    /// </summary>
    InsufficientSecurity = 71,

    /// <summary>
    /// Internal error (80)
    /// </summary>
    InternalError = 80,

    /// <summary>
    /// Inappropriate fallback (86) - RFC 7507
    /// </summary>
    InappropriateFallback = 86,

    /// <summary>
    /// User canceled (90)
    /// </summary>
    UserCanceled = 90,

    /// <summary>
    /// No renegotiation (100) - TLS 1.2 and earlier
    /// </summary>
    NoRenegotiation = 100,

    /// <summary>
    /// Missing extension (109) - TLS 1.3
    /// </summary>
    MissingExtension = 109,

    /// <summary>
    /// Unsupported extension (110)
    /// </summary>
    UnsupportedExtension = 110,

    /// <summary>
    /// Certificate unobtainable (111)
    /// </summary>
    CertificateUnobtainable = 111,

    /// <summary>
    /// Unrecognized name (112)
    /// </summary>
    UnrecognizedName = 112,

    /// <summary>
    /// Bad certificate status response (113)
    /// </summary>
    BadCertificateStatusResponse = 113,

    /// <summary>
    /// Bad certificate hash value (114)
    /// </summary>
    BadCertificateHashValue = 114,

    /// <summary>
    /// Unknown PSK identity (115) - TLS 1.3
    /// </summary>
    UnknownPskIdentity = 115,

    /// <summary>
    /// Certificate required (116) - TLS 1.3
    /// </summary>
    CertificateRequired = 116,

    /// <summary>
    /// No application protocol (120) - RFC 7301
    /// </summary>
    NoApplicationProtocol = 120
}
