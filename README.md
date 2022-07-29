# Minever

Minever is an open-source .NET library primarily aimed at supporting various versions of Minecraft (13w41b+) and providing them with a general user interface for creating client applications.

# Current development state

Initial state. Many things need to be done.

# Solution structure

## Class libraries

| Project | Description |
| ------- | ----------- |
| Minever.Networking | The core project. It contains an implementation of packages, protocols, serializer, writers, readers etc. |
| Minever.Client | It contains Minecraft clients implementation (low-level MinecrafPacketClient and high-level MinecraftClient). |

## Tests

| Project | Description |
| ------- | ----------- |
| Minever.Tests.Networking | Tests for Minever.Networking. |

## Console applications

| Project | Description |
| ------- | ----------- |
| Minever.ConsoleApplication | The console project for testing Minever.Client. |

# License

MIT license.
