using Minever.Networking.Packets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Minever.Networking.Protocols.Tests;

[TestClass]
public class Protocol0Tests
{
    private static IEnumerable<object> SupportedPacketsTestSetup
    {
        get
        {
            return new[]
            {
                // Handshake
                new object[] { 0x00, new MinecraftPacketKind(MinecraftPacketDirection.ClientToServer, MinecraftConnectionState.Handshake), "Handshake" },

                // Status
                new object[] { 0x00, new MinecraftPacketKind(MinecraftPacketDirection.ClientToServer, MinecraftConnectionState.Status), "Status request" },
                new object[] { 0x01, new MinecraftPacketKind(MinecraftPacketDirection.ClientToServer, MinecraftConnectionState.Status), "Ping request" },

                new object[] { 0x00, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Status), "Status response" },
                new object[] { 0x01, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Status), "Ping response" },

                // Login
                new object[] { 0x00, new MinecraftPacketKind(MinecraftPacketDirection.ClientToServer, MinecraftConnectionState.Login), "Login initialize" },
                new object[] { 0x01, new MinecraftPacketKind(MinecraftPacketDirection.ClientToServer, MinecraftConnectionState.Login), "Encryption request" },

                new object[] { 0x00, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Login), "Disconnect" },
                new object[] { 0x01, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Login), "Encryption response" },
                new object[] { 0x02, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Login), "Login success" },

                // Play
                new object[] { 0x00, new MinecraftPacketKind(MinecraftPacketDirection.ClientToServer, MinecraftConnectionState.Play), "Keep alive" },
                new object[] { 0x01, new MinecraftPacketKind(MinecraftPacketDirection.ClientToServer, MinecraftConnectionState.Play), "Chat message" },
                new object[] { 0x02, new MinecraftPacketKind(MinecraftPacketDirection.ClientToServer, MinecraftConnectionState.Play), "Use entity" },
                new object[] { 0x03, new MinecraftPacketKind(MinecraftPacketDirection.ClientToServer, MinecraftConnectionState.Play), "Player" },
                new object[] { 0x04, new MinecraftPacketKind(MinecraftPacketDirection.ClientToServer, MinecraftConnectionState.Play), "Player position" },
                new object[] { 0x05, new MinecraftPacketKind(MinecraftPacketDirection.ClientToServer, MinecraftConnectionState.Play), "Player look" },
                new object[] { 0x06, new MinecraftPacketKind(MinecraftPacketDirection.ClientToServer, MinecraftConnectionState.Play), "Player position and look" },
                new object[] { 0x07, new MinecraftPacketKind(MinecraftPacketDirection.ClientToServer, MinecraftConnectionState.Play), "Player digging" },
                new object[] { 0x08, new MinecraftPacketKind(MinecraftPacketDirection.ClientToServer, MinecraftConnectionState.Play), "Player block placement" },
                new object[] { 0x09, new MinecraftPacketKind(MinecraftPacketDirection.ClientToServer, MinecraftConnectionState.Play), "Held item change" },
                new object[] { 0x0A, new MinecraftPacketKind(MinecraftPacketDirection.ClientToServer, MinecraftConnectionState.Play), "Animation" },
                new object[] { 0x0B, new MinecraftPacketKind(MinecraftPacketDirection.ClientToServer, MinecraftConnectionState.Play), "Entity action" },
                new object[] { 0x0C, new MinecraftPacketKind(MinecraftPacketDirection.ClientToServer, MinecraftConnectionState.Play), "Steer vehicle" },
                new object[] { 0x0D, new MinecraftPacketKind(MinecraftPacketDirection.ClientToServer, MinecraftConnectionState.Play), "Close window" },
                new object[] { 0x0E, new MinecraftPacketKind(MinecraftPacketDirection.ClientToServer, MinecraftConnectionState.Play), "Click window" },
                new object[] { 0x0F, new MinecraftPacketKind(MinecraftPacketDirection.ClientToServer, MinecraftConnectionState.Play), "Confirm transaction" },
                new object[] { 0x10, new MinecraftPacketKind(MinecraftPacketDirection.ClientToServer, MinecraftConnectionState.Play), "Creative inventory action" },
                new object[] { 0x11, new MinecraftPacketKind(MinecraftPacketDirection.ClientToServer, MinecraftConnectionState.Play), "Enchant item" },
                new object[] { 0x12, new MinecraftPacketKind(MinecraftPacketDirection.ClientToServer, MinecraftConnectionState.Play), "Update sign" },
                new object[] { 0x13, new MinecraftPacketKind(MinecraftPacketDirection.ClientToServer, MinecraftConnectionState.Play), "Player abilities" },
                new object[] { 0x14, new MinecraftPacketKind(MinecraftPacketDirection.ClientToServer, MinecraftConnectionState.Play), "Tab-complete" },
                new object[] { 0x15, new MinecraftPacketKind(MinecraftPacketDirection.ClientToServer, MinecraftConnectionState.Play), "Client settings" },
                new object[] { 0x16, new MinecraftPacketKind(MinecraftPacketDirection.ClientToServer, MinecraftConnectionState.Play), "Client status" },
                new object[] { 0x17, new MinecraftPacketKind(MinecraftPacketDirection.ClientToServer, MinecraftConnectionState.Play), "Plugin message" },

                new object[] { 0x00, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Keep alive" },
                new object[] { 0x01, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Join game" },
                new object[] { 0x02, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Chat message" },
                new object[] { 0x03, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Time update" },
                new object[] { 0x04, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Entity equipment" },
                new object[] { 0x05, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Spawn position" },
                new object[] { 0x06, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Update health" },
                new object[] { 0x07, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Respawn" },
                new object[] { 0x08, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Player position" },
                new object[] { 0x09, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Held item change" },
                new object[] { 0x0A, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Use bed" },
                new object[] { 0x0B, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Animation" },
                new object[] { 0x0C, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Spawn player" },
                new object[] { 0x0D, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Collect item" },
                new object[] { 0x0E, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Spawn object" },
                new object[] { 0x0F, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Spawn mob" },
                new object[] { 0x10, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Spawn painting" },
                new object[] { 0x11, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Spawn experience orb" },
                new object[] { 0x12, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Entity velocity" },
                new object[] { 0x13, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Destroy entities" },
                new object[] { 0x14, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Entity" },
                new object[] { 0x15, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Entity relative move" },
                new object[] { 0x16, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Entity look" },
                new object[] { 0x17, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Entity look and relative move" },
                new object[] { 0x18, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Entity teleport" },
                new object[] { 0x19, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Entity head look" },
                new object[] { 0x1A, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Entity status" },
                new object[] { 0x1B, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Attach entity" },
                new object[] { 0x1C, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Entity metadata" },
                new object[] { 0x1D, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Entity effect" },
                new object[] { 0x1E, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Remove entity effect" },
                new object[] { 0x1F, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Set experience" },
                new object[] { 0x20, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Entity properties" },
                new object[] { 0x21, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Chunk data" },
                new object[] { 0x22, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Multi block change" },
                new object[] { 0x23, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Block change" },
                new object[] { 0x24, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Block action" },
                new object[] { 0x25, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Block break animation" },
                new object[] { 0x26, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Map chunk bulk" },
                new object[] { 0x27, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Explosion" },
                new object[] { 0x28, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Effect" },
                new object[] { 0x29, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Sound effect" },
                new object[] { 0x2A, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Particle" },
                new object[] { 0x2B, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Change game state" },
                new object[] { 0x2C, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Spawn global entity" },
                new object[] { 0x2D, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Open window" },
                new object[] { 0x2E, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Close window" },
                new object[] { 0x2F, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Set slot" },
                new object[] { 0x30, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Window items" },
                new object[] { 0x31, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Window property" },
                new object[] { 0x32, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Confirm transaction" },
                new object[] { 0x33, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Update sign" },
                new object[] { 0x34, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Maps" },
                new object[] { 0x35, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Update block entity" },
                new object[] { 0x36, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Sign editor open" },
                new object[] { 0x37, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Statistics" },
                new object[] { 0x38, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Player list item" },
                new object[] { 0x39, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Player abilities" },
                new object[] { 0x3A, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Tab-complete" },
                new object[] { 0x3B, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Scoreboard objective" },
                new object[] { 0x3C, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Update score" },
                new object[] { 0x3D, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Display scoreboard" },
                new object[] { 0x3E, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Teams" },
                new object[] { 0x3F, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Plugin message" },
                new object[] { 0x40, new MinecraftPacketKind(MinecraftPacketDirection.ServerToClient, MinecraftConnectionState.Play), "Disconnect" },
            };
        }
    }

    [TestMethod]
    public void Protocol0_NoArguments_ThrowsNoException()
    {
        _ = new Protocol0();
    }

    [TestMethod]
    [DynamicData(nameof(SupportedPacketsTestSetup))]
    public void IsPackaetSupported_SupportedPacketInfo_True(int packetId, MinecraftPacketKind packetKind, string packetName)
    {
        var actual = new Protocol0().IsPacketSupported(packetId, packetKind);

        Assert.IsTrue(actual, @$"Packet ""{packetName}"" 0x({packetId:X2}) is not supported.");
    }
}
