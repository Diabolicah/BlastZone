using System;
using UnityEngine;

public class PlayerStats
{
    private float _health;
    private float _healthRegen;
    private float _damage;
    private float _attackSpeed;
    private float _bulletSpeed;
    private float _movementSpeed;

    public PlayerStats()
    {
        _health = 1;
        _healthRegen = 1;
        _damage = 1;
        _attackSpeed = 1;
        _bulletSpeed = 1;
        _movementSpeed = 1;
    }
    public float Health { get => _health; set => _health = value; }
    public float HealthRegen { get => _healthRegen; set => _healthRegen = value; }
    public float Damage { get => _damage; set => _damage = value; }
    public float AttackSpeed { get => _attackSpeed; set => _attackSpeed = value; }
    public float BulletSpeed { get => _bulletSpeed; set => _bulletSpeed = value; }
    public float MovementSpeed { get => _movementSpeed; set => _movementSpeed = value; }
    public string Encode()
    {
        return string.Join(",", new string[]
        {
            _health.ToString(),
            _healthRegen.ToString(),
            _damage.ToString(),
            _attackSpeed.ToString(),
            _bulletSpeed.ToString(),
            _movementSpeed.ToString()
        });
    }

    public static PlayerStats Decode(string data)
    {
        if (string.IsNullOrEmpty(data))
            throw new ArgumentException("Input data cannot be null or empty.", nameof(data));

        var parts = data.Split(',');

        if (parts.Length != 6)
            throw new FormatException("Invalid string format for PlayerStats. Expected 6 comma-separated values.");

        var stats = new PlayerStats();
        stats.Health = float.Parse(parts[0]);
        stats.HealthRegen = float.Parse(parts[1]);
        stats.Damage = float.Parse(parts[2]);
        stats.AttackSpeed = float.Parse(parts[3]);
        stats.BulletSpeed = float.Parse(parts[4]);
        stats.MovementSpeed = float.Parse(parts[5]);

        return stats;
    }
}
