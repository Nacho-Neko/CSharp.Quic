// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Logging;
using System;

namespace CSharp.Quic;

/// <summary>
/// 网络事件日志源（桥接到 ILogger）
/// 替代 .NET 运行时内部的 NetEventSource
/// </summary>
internal static class NetEventSource
{
    private static ILogger? _logger;

    public static class Log
    {
        internal static bool IsEnabled()
        {
            return false;
        }
    }

    /// <summary>
    /// 配置日志提供程序（在应用启动时调用）
    /// </summary>
    public static void ConfigureLogger(ILogger logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// 配置日志提供程序（通过 ILoggerFactory）
    /// </summary>
    public static void ConfigureLogger(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger("System.Net.Quic");
    }

    /// <summary>
    /// 是否启用日志
    /// </summary>
    public static bool IsEnabled() => _logger?.IsEnabled(LogLevel.Information) ?? false;

    /// <summary>
    /// 记录信息日志
    /// </summary>
    public static void Info(object? thisOrContextObject, string message)
    {
        if (_logger != null && _logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation("[QUIC] {Message}", message);
        }
        else
        {
            // Fallback 到控制台
            Console.WriteLine($"[QUIC INFO] {message}");
        }
    }

    /// <summary>
    /// 记录警告日志
    /// </summary>
    public static void Warning(object? thisOrContextObject, string message)
    {
        if (_logger != null && _logger.IsEnabled(LogLevel.Warning))
        {
            _logger.LogWarning("[QUIC] {Message}", message);
        }
        else
        {
            Console.WriteLine($"[QUIC WARNING] {message}");
        }
    }

    /// <summary>
    /// 记录错误日志
    /// </summary>
    public static void Error(object? thisOrContextObject, string message)
    {
        if (_logger != null && _logger.IsEnabled(LogLevel.Error))
        {
            _logger.LogError("[QUIC] {Message}", message);
        }
        else
        {
            Console.WriteLine($"[QUIC ERROR] {message}");
        }
    }

    /// <summary>
    /// 记录异常日志
    /// </summary>
    public static void Exception(object? thisOrContextObject, Exception exception, string message)
    {
        if (_logger != null && _logger.IsEnabled(LogLevel.Error))
        {
            _logger.LogError(exception, "[QUIC] {Message}", message);
        }
        else
        {
            Console.WriteLine($"[QUIC ERROR] {message}: {exception}");
        }
    }
}
