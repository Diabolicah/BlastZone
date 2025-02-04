using Fusion;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Bullet Weapon Config")]
public class BulletWeaponConfig : ScriptableObject
{
    [SerializeField] private NetworkPrefabRef _bulletPrefab;
    [SerializeField] private float _shootCooldown = 0.5f;
    [SerializeField] private float _bulletSpeed = 10f;
    [SerializeField] private float _bulletLifeTime = 5f;
    [SerializeField] private float _damage = 5f;


    public NetworkPrefabRef BulletPrefab => _bulletPrefab;
    public float ShootCooldown => _shootCooldown;
    public float BulletSpeed => _bulletSpeed;
    public float BulletLifeTime => _bulletLifeTime;
    public float Damage => _damage;
}
