# QUIC Library - Extended MsQuic Control

A fork of the QUIC implementation from Microsoft's .NET System library, providing enhanced control over MsQuic features including XDP (eXpress Data Path) and BBR (Bottleneck Bandwidth and RTT) congestion control.

## Overview

This library extends the standard .NET QUIC implementation with additional configuration options for fine-tuning MsQuic behavior, enabling advanced networking scenarios and performance optimization.

## Features

- **Extended MsQuic Configuration** - Access to advanced MsQuic settings beyond the standard .NET API
- **XDP Support** - Enable kernel-bypass networking for ultra-low latency
- **BBR Congestion Control** - Configure modern congestion control algorithms
- **Full .NET Compatibility** - Drop-in replacement for System.Net.Quic
