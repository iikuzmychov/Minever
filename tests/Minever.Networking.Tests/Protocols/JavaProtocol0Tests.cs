using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minever.Networking.Packets;
using System;
using System.Collections.Generic;

namespace Minever.Networking.Protocols.Tests;

[TestClass]
public class JavaProtocol0Tests
{
    private static IEnumerable<object> SupportedPacketsTestSetup
    {
        get
        {
            return new[]
            {
                // Handshake
                new object[] { 0x00, new PacketContext(PacketDirection.ClientToServer, ConnectionState.Handshake), "Handshake" },

                // Status
                new object[] { 0x00, new PacketContext(PacketDirection.ClientToServer, ConnectionState.Status), "Status request" },
                new object[] { 0x01, new PacketContext(PacketDirection.ClientToServer, ConnectionState.Status), "Ping request" },

                new object[] { 0x00, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Status), "Status response" },
                new object[] { 0x01, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Status), "Ping response" },

                // Login
                new object[] { 0x00, new PacketContext(PacketDirection.ClientToServer, ConnectionState.Login), "Login initialize" },
                new object[] { 0x01, new PacketContext(PacketDirection.ClientToServer, ConnectionState.Login), "Encryption request" },

                new object[] { 0x00, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Login), "Disconnect" },
                new object[] { 0x01, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Login), "Encryption response" },
                new object[] { 0x02, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Login), "Login success" },

                // Play
                new object[] { 0x00, new PacketContext(PacketDirection.ClientToServer, ConnectionState.Play), "Keep alive" },
                new object[] { 0x01, new PacketContext(PacketDirection.ClientToServer, ConnectionState.Play), "Chat message" },
                new object[] { 0x02, new PacketContext(PacketDirection.ClientToServer, ConnectionState.Play), "Use entity" },
                new object[] { 0x03, new PacketContext(PacketDirection.ClientToServer, ConnectionState.Play), "Player" },
                new object[] { 0x04, new PacketContext(PacketDirection.ClientToServer, ConnectionState.Play), "Player position" },
                new object[] { 0x05, new PacketContext(PacketDirection.ClientToServer, ConnectionState.Play), "Player look" },
                new object[] { 0x06, new PacketContext(PacketDirection.ClientToServer, ConnectionState.Play), "Player position and look" },
                new object[] { 0x07, new PacketContext(PacketDirection.ClientToServer, ConnectionState.Play), "Player digging" },
                new object[] { 0x08, new PacketContext(PacketDirection.ClientToServer, ConnectionState.Play), "Player block placement" },
                new object[] { 0x09, new PacketContext(PacketDirection.ClientToServer, ConnectionState.Play), "Held item change" },
                new object[] { 0x0A, new PacketContext(PacketDirection.ClientToServer, ConnectionState.Play), "Animation" },
                new object[] { 0x0B, new PacketContext(PacketDirection.ClientToServer, ConnectionState.Play), "Entity action" },
                new object[] { 0x0C, new PacketContext(PacketDirection.ClientToServer, ConnectionState.Play), "Steer vehicle" },
                new object[] { 0x0D, new PacketContext(PacketDirection.ClientToServer, ConnectionState.Play), "Close window" },
                new object[] { 0x0E, new PacketContext(PacketDirection.ClientToServer, ConnectionState.Play), "Click window" },
                new object[] { 0x0F, new PacketContext(PacketDirection.ClientToServer, ConnectionState.Play), "Confirm transaction" },
                new object[] { 0x10, new PacketContext(PacketDirection.ClientToServer, ConnectionState.Play), "Creative inventory action" },
                new object[] { 0x11, new PacketContext(PacketDirection.ClientToServer, ConnectionState.Play), "Enchant item" },
                new object[] { 0x12, new PacketContext(PacketDirection.ClientToServer, ConnectionState.Play), "Update sign" },
                new object[] { 0x13, new PacketContext(PacketDirection.ClientToServer, ConnectionState.Play), "Player abilities" },
                new object[] { 0x14, new PacketContext(PacketDirection.ClientToServer, ConnectionState.Play), "Tab-complete" },
                new object[] { 0x15, new PacketContext(PacketDirection.ClientToServer, ConnectionState.Play), "Client settings" },
                new object[] { 0x16, new PacketContext(PacketDirection.ClientToServer, ConnectionState.Play), "Client status" },
                new object[] { 0x17, new PacketContext(PacketDirection.ClientToServer, ConnectionState.Play), "Plugin message" },

                new object[] { 0x00, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Keep alive" },
                new object[] { 0x01, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Join game" },
                new object[] { 0x02, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Chat message" },
                new object[] { 0x03, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Time update" },
                new object[] { 0x04, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Entity equipment" },
                new object[] { 0x05, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Spawn position" },
                new object[] { 0x06, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Update health" },
                new object[] { 0x07, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Respawn" },
                new object[] { 0x08, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Player position" },
                new object[] { 0x09, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Held item change" },
                new object[] { 0x0A, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Use bed" },
                new object[] { 0x0B, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Animation" },
                new object[] { 0x0C, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Spawn player" },
                new object[] { 0x0D, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Collect item" },
                new object[] { 0x0E, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Spawn object" },
                new object[] { 0x0F, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Spawn mob" },
                new object[] { 0x10, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Spawn painting" },
                new object[] { 0x11, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Spawn experience orb" },
                new object[] { 0x12, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Entity velocity" },
                new object[] { 0x13, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Destroy entities" },
                new object[] { 0x14, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Entity" },
                new object[] { 0x15, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Entity relative move" },
                new object[] { 0x16, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Entity look" },
                new object[] { 0x17, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Entity look and relative move" },
                new object[] { 0x18, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Entity teleport" },
                new object[] { 0x19, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Entity head look" },
                new object[] { 0x1A, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Entity status" },
                new object[] { 0x1B, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Attach entity" },
                new object[] { 0x1C, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Entity metadata" },
                new object[] { 0x1D, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Entity effect" },
                new object[] { 0x1E, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Remove entity effect" },
                new object[] { 0x1F, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Set experience" },
                new object[] { 0x20, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Entity properties" },
                new object[] { 0x21, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Chunk data" },
                new object[] { 0x22, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Multi block change" },
                new object[] { 0x23, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Block change" },
                new object[] { 0x24, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Block action" },
                new object[] { 0x25, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Block break animation" },
                new object[] { 0x26, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Map chunk bulk" },
                new object[] { 0x27, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Explosion" },
                new object[] { 0x28, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Effect" },
                new object[] { 0x29, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Sound effect" },
                new object[] { 0x2A, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Particle" },
                new object[] { 0x2B, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Change game state" },
                new object[] { 0x2C, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Spawn global entity" },
                new object[] { 0x2D, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Open window" },
                new object[] { 0x2E, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Close window" },
                new object[] { 0x2F, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Set slot" },
                new object[] { 0x30, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Window items" },
                new object[] { 0x31, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Window property" },
                new object[] { 0x32, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Confirm transaction" },
                new object[] { 0x33, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Update sign" },
                new object[] { 0x34, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Maps" },
                new object[] { 0x35, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Update block entity" },
                new object[] { 0x36, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Sign editor open" },
                new object[] { 0x37, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Statistics" },
                new object[] { 0x38, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Player list item" },
                new object[] { 0x39, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Player abilities" },
                new object[] { 0x3A, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Tab-complete" },
                new object[] { 0x3B, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Scoreboard objective" },
                new object[] { 0x3C, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Update score" },
                new object[] { 0x3D, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Display scoreboard" },
                new object[] { 0x3E, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Teams" },
                new object[] { 0x3F, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Plugin message" },
                new object[] { 0x40, new PacketContext(PacketDirection.ServerToClient, ConnectionState.Play), "Disconnect" },
            };
        }
    }

    [TestMethod]
    public void JavaProtocol0_NoArguments_ThrowsNoException()
    {
        _ = new JavaProtocol0();
    }

    [TestMethod]
    [DynamicData(nameof(SupportedPacketsTestSetup))]
    public void IsPackaetSupported_SupportedPacketInfo_True(int packetId, PacketContext context, string packetName)
    {
        var isPacketSupported = new JavaProtocol0().IsPacketSupported(packetId, context);

        Console.Write(@$"Packet ""{packetName}"" (0x{packetId:X2}, {context.ConnectionState} state, {context.Direction}).");
        Assert.IsTrue(isPacketSupported, "The packet is not supported.");
    }
}
