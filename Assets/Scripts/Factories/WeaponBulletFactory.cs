using Fusion;
using UnityEngine;

public class WeaponBulletFactory : IWeaponFactory
{
    private readonly BulletWeaponConfig _config;
    private readonly Transform _shootPoint;

    public WeaponBulletFactory(BulletWeaponConfig config, Transform shootPoint)
    {
        _config = config;
        _shootPoint = shootPoint;
    }

    public IWeapon CreateWeapon()
    {
        return new Weapon_Bullet(_config, _shootPoint);
    }
}
