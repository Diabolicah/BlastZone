using Fusion;
using UnityEngine;

public class WeaponTripleBulletFactory : IWeaponFactory
{
    private readonly TripleBulletWeaponConfig _config;
    private readonly Transform _shootPoint;

    public WeaponTripleBulletFactory(TripleBulletWeaponConfig config, Transform shootPoint)
    {
        _config = config;
        _shootPoint = shootPoint;
    }

    public IWeapon CreateWeapon()
    {
        return new Weapon_TripleBullet(_config, _shootPoint);
    }
}
