[![License](https://img.shields.io/github/license/iiKuzmychov/Minever)](https://github.com/iiKuzmychov/Minever/blob/master/LICENSE.md)

# Minever

Miniver is an open-source .NET library primarily aimed at supporting various versions of Minecraft Java Edition & Bedrock Edition and providing tools for creating bots and client applications.

## Current state

In development. Many things need to be done. Library architecture, aslo, still in development.

## Supported versions
### Java Edition

| Minecraft versions | Protocol version | State            |
| ------------------ | ---------------- | ---------------- |
| 13w41b             | 0                | *In development* |

### Bedrok Edition

No one version are supported now.

## Solution structure

### Class libraries

| Project | Description |
| ------- | ----------- |
| Minever.Networking | The core project. It contains an implementation of packages, protocols, packet serializer, writer, reader etc. |
| Minever.Client | It contains Minecraft clients implementation (low-level packet clients and high-level MinecraftClient). |

### Tests

| Project | Description |
| ------- | ----------- |
| Minever.Tests.Networking | Tests for Minever.Networking. |

### Console applications

| Project | Description |
| ------- | ----------- |
| Minever.ConsoleApplication | The console project for debuging Minever.Client. |

## License

Minever is licensed under the [MIT](https://github.com/iiKuzmychov/Minever/blob/master/LICENSE.md) license.
