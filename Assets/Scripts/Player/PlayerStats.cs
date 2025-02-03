using UnityEngine;

public class PlayerStats
{
    private float _health;
    private float _healthRegen;
    private float _damage;
    private float _attackSpeed;
    private float _bulletSpeed;
    private float _movementSpeed;

    PlayerStats()
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
}
