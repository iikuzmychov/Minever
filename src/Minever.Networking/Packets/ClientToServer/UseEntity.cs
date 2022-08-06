﻿using Minever.Networking.Serialization.Attributes;
using Minever.Networking.Serialization.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minever.Networking.Packets;

/// <summary>
/// Maybe "EntityInteractionType" would be a better name?
/// </summary>
[PacketConverter(typeof(PacketEnumConverter<UseEntityAction, byte>))]
public enum UseEntityAction : byte
{ 
    LeftClick  = 0,
    RigthClick = 1,
}

/// <summary>
/// Maybe "EntityInteraction" would be a better name?
/// </summary>
public sealed record UseEntity
{
    [PacketPropertyOrder(1)]
    public int TargetEntityId { get; init; }

    [PacketPropertyOrder(2)]
    public UseEntityAction Action { get; init; }

    public UseEntity() { }

    public UseEntity(int targetEntityId, UseEntityAction action)
    {
        TargetEntityId = targetEntityId;
        Action         = action;
    }
}
