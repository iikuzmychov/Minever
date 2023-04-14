[![License](https://img.shields.io/github/license/iiKuzmychov/Minever)](https://github.com/iiKuzmychov/Minever/blob/master/LICENSE.md)

# Minever

... is an **open-source .NET library** designed to support various versions of **Minecraft Java & Bedrock** editions and provides tools for building bots and client applications.

## State

In draft. Many things need to be considered.

## Supported Minecraft versions
### Java Edition

| Version | Protocol version | State |
| --- | --- | --- |
| 1.7.10 | 5 | *In development* |

### Bedrok Edition

*Currently no version supported :(*

## Projects

| Project name | Meaning |
| --- | --- |
| Minever.Core | Minecraft client core |
| Minever.X.Core<br>&nbsp;&nbsp;&nbsp;&nbsp;*Minever.Java.Core*<br>&nbsp;&nbsp;&nbsp;&nbsp;*Minever.Bedrock.Core* | Minecraft "X Edition" client core<br>&nbsp;&nbsp;&nbsp;&nbsp;*Minecraft "Java Edition" client core*<br>&nbsp;&nbsp;&nbsp;&nbsp;*Minecraft "Bedrock Edition" client core*<br> |
| Minever.X.Protocols.Vy<br>&nbsp;&nbsp;&nbsp;&nbsp;*Minever.Java.Protocols.V5* | Minecraft "X Edition" client for protocol version `y`<br>&nbsp;&nbsp;&nbsp;&nbsp;*Minecraft "Java Edition" client for version protocol `5`* |
| Minever.X.Universal<br>&nbsp;&nbsp;&nbsp;&nbsp;*Minever.Java.Universal*<br>&nbsp;&nbsp;&nbsp;&nbsp;*Minever.Bedrock.Universal* | Minecraft "X Edition" client for any version<br>&nbsp;&nbsp;&nbsp;&nbsp;*Minecraft "Java Edition" client for any version*<br>&nbsp;&nbsp;&nbsp;&nbsp;*Minecraft "Bedrock Edition" client for any version*<br> |
| Minever.Universal | Minecraft client for both Java and Bedrock editions |
| Minever.Console | Minecraft console client *(will be moved to separate repository)* |
<!--| Minever.X.Va.Vb.Vc<br>&nbsp;&nbsp;&nbsp;&nbsp;*Minever.Java.V1.V7.V10* | Minecraft "X Edition" client for version `a.b.c`<br>&nbsp;&nbsp;&nbsp;&nbsp;*Minecraft "Java Edition" client for version `1.7.10`* |-->

## License

Minever is licensed under the [MIT](https://github.com/iiKuzmychov/Minever/blob/master/LICENSE.md) license.
