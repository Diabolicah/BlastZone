using System;
using Fusion;

[Serializable]
public struct PlayerStatsStruct : INetworkStruct
{
    public float Health;
    public float HealthRegen;
    public float Damage;
    public float AttackSpeed;
    public float BulletSpeed;
    public float MovementSpeed;

    public PlayerStatsStruct(
        float health, float healthRegen, float damage,
        float attackSpeed, float bulletSpeed, float movementSpeed)
    {
        Health = health;
        HealthRegen = healthRegen;
        Damage = damage;
        AttackSpeed = attackSpeed;
        BulletSpeed = bulletSpeed;
        MovementSpeed = movementSpeed;
    }
}
