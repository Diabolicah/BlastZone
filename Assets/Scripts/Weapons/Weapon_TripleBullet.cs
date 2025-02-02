using Fusion;
using UnityEngine;
using static Unity.Collections.Unicode;
using UnityEngine.UIElements;

public class Weapon_TripleBullet : IWeapon
{
    private readonly BulletWeaponConfig _config;
    private readonly Transform _shootPoint;
    private CooldownManager _cooldownManager;

    public Weapon_TripleBullet(BulletWeaponConfig config, Transform shootPoint)
    {
        _config = config;
        _shootPoint = shootPoint;
        _cooldownManager = new CooldownManager();
    }

    public void fire(NetworkRunner runner)
    {
        throw new System.NotImplementedException();
    }
}
