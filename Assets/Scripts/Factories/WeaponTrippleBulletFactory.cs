using Fusion;
using UnityEngine;

public class WeaponTrippleBulletFactory : IWeaponFactory
{
    private readonly BulletWeaponConfig _config;
    private readonly Transform _shootPoint;

    public WeaponTrippleBulletFactory(BulletWeaponConfig config, Transform shootPoint)
    {
        _config = config;
        _shootPoint = shootPoint;
    }

    public IWeapon CreateWeapon()
    {
        return new Weapon_TripleBullet(_config, _shootPoint);
    }
}
