using Fusion;
using UnityEngine;

public class WeaponTrippleBulletFactory : IWeaponFactory
{
    private readonly TrippleBulletWeaponConfig _config;
    private readonly Transform _shootPoint;

    public WeaponTrippleBulletFactory(TrippleBulletWeaponConfig config, Transform shootPoint)
    {
        _config = config;
        _shootPoint = shootPoint;
    }

    public IWeapon CreateWeapon()
    {
        return new Weapon_TripleBullet(_config, _shootPoint);
    }
}
