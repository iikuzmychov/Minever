using Minever.Networking.Serialization.Attributes;

namespace Minever.Networking.Packets;

public sealed record UpdateHealth
{
    [PacketPropertyOrder(1)]
    public float Health { get; init; }

    [PacketPropertyOrder(2)]
    public float Food { get; init; }

    [PacketPropertyOrder(3)]
    public float FoodSaturation { get; init; }

    public UpdateHealth() { }

    public UpdateHealth(float health, float food, float foodSaturation)
    {
        Health         = health;
        Food           = food;
        FoodSaturation = foodSaturation;
    }
}
