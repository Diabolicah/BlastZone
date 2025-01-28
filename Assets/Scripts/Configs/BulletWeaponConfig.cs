using Fusion;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Bullet Weapon Config")]
public class BulletWeaponConfig : ScriptableObject
{
    [SerializeField] private NetworkPrefabRef _bulletPrefab;
    [SerializeField] private float _shootCooldown = 0.5f;

    public NetworkPrefabRef BulletPrefab => _bulletPrefab;
    public float ShootCooldown => _shootCooldown;
}
