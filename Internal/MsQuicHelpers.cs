using Microsoft.Quic;
using System;
using System.Buffers.Binary;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using static Microsoft.Quic.MsQuic;

namespace CSharp.Quic;

internal static class MsQuicHelpers
{
    private const int IPv4AddressSize = 16;
    private const int IPv6AddressSize = 28;
    internal static bool TryParse(this EndPoint endPoint, out string? host, out IPAddress? address, out int port)
    {
        if (endPoint is DnsEndPoint dnsEndPoint)
        {
            host = IPAddress.TryParse(dnsEndPoint.Host, out address) ? null : dnsEndPoint.Host;
            port = dnsEndPoint.Port;
            return true;
        }

        if (endPoint is IPEndPoint ipEndPoint)
        {
            host = null;
            address = ipEndPoint.Address;
            port = ipEndPoint.Port;
            return true;
        }

        host = default;
        address = default;
        port = default;
        return false;
    }

    internal static unsafe IPEndPoint QuicAddrToIPEndPoint(QuicAddr* quicAddress, AddressFamily? addressFamilyOverride = null)
    {
        // MsQuic always uses storage size as if IPv6 was used
        Span<byte> addressBytes = new Span<byte>(quicAddress, IPv6AddressSize);

        // 读取地址族以确定实际大小
        AddressFamily family = (AddressFamily)BinaryPrimitives.ReadUInt16LittleEndian(addressBytes);
        int size = family == AddressFamily.InterNetwork ? IPv4AddressSize : IPv6AddressSize;

        // 使用 SocketAddress 公共 API
        SocketAddress socketAddress = new SocketAddress(family, size);

        // 使用 MemoryMarshal 批量复制
        addressBytes.Slice(0, size).CopyTo(socketAddress.Buffer.Span);

        // 通过 EndPoint.Create 转换为 IPEndPoint
        IPEndPoint tempEndPoint = family == AddressFamily.InterNetwork
            ? new IPEndPoint(IPAddress.Any, 0)
            : new IPEndPoint(IPAddress.IPv6Any, 0);

        return (IPEndPoint)tempEndPoint.Create(socketAddress);
        /*
        if (addressFamilyOverride != null)
        {
            SocketAddressPal.SetAddressFamily(addressBytes, (AddressFamily)addressFamilyOverride!);
        }
        return IPEndPointExtensions.CreateIPEndPoint(addressBytes);
        */
    }

    internal static QuicAddr ToQuicAddr(this IPEndPoint ipEndPoint)
    {
        QuicAddr result = default;
        Span<byte> rawAddress = MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref result, 1));

        // 正确用法：Serialize() 返回 SocketAddress
        SocketAddress socketAddress = ipEndPoint.Serialize();

        // 将 SocketAddress 的字节复制到 rawAddress
        int size = Math.Min(socketAddress.Size, rawAddress.Length);

        socketAddress.Buffer.Slice(0, size).Span.CopyTo(rawAddress);

        // ipEndPoint.Serialize(rawAddress);
        return result;
    }

    internal static unsafe T GetMsQuicParameter<T>(MsQuicSafeHandle handle, uint parameter)
        where T : unmanaged
    {
        T value;
        GetMsQuicParameter(handle, parameter, (uint)sizeof(T), (byte*)&value);
        return value;
    }
    internal static unsafe void GetMsQuicParameter(MsQuicSafeHandle handle, uint parameter, uint length, byte* value)
    {
        int status = MsQuicApi.Api.GetParam(
            handle,
            parameter,
            &length,
            value);

        if (StatusFailed(status))
        {
            ThrowHelper.ThrowMsQuicException(status, $"GetParam({handle}, {parameter}) failed");
        }
    }

    internal static unsafe void SetMsQuicParameter<T>(MsQuicSafeHandle handle, uint parameter, T value)
        where T : unmanaged
    {
        SetMsQuicParameter(handle, parameter, (uint)sizeof(T), (byte*)&value);
    }
    internal static unsafe void SetMsQuicParameter(MsQuicSafeHandle handle, uint parameter, uint length, byte* value)
    {
        int status = MsQuicApi.Api.SetParam(
            handle,
            parameter,
            length,
            value);

        if (StatusFailed(status))
        {
            ThrowHelper.ThrowMsQuicException(status, $"SetParam({handle}, {parameter}) failed");
        }
    }
}
