using Fusion;
using UnityEngine;
using static Unity.Collections.Unicode;

public class Tank_Weapons : NetworkBehaviour
{
    [SerializeField] private Transform _shootPoint; // Assign in inspector
    [SerializeField] private BulletWeaponConfig _weaponConfig; // Assign config asset
    [SerializeField] private TrippleBulletWeaponConfig _trippleWeaponConfig; // Test Weapon need to remove later
    private IWeapon _tankWeapon;
    private IWeaponFactory _weaponFactory;

    public override void Spawned()
    {
        // Initialize factory with dependencies
        //_weaponFactory = new WeaponBulletFactory(_weaponConfig, _shootPoint);
        _weaponFactory = new WeaponTrippleBulletFactory(_trippleWeaponConfig, _shootPoint);
        _tankWeapon = _weaponFactory.CreateWeapon();
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data) && HasInputAuthority && data.isShooting)
        {
            Debug.Log(HasStateAuthority);
            if (HasStateAuthority)
            {
                ServerFire();
            }else
            {
                RequestFireRpc();
            }
        }
    }

    public void setTankWeapon(IWeaponFactory _weaponFactory)
    {
        _tankWeapon = _weaponFactory.CreateWeapon();
    }
    private void ServerFire()
    {
        _tankWeapon.fire(Runner);
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RequestFireRpc()
    {
        ServerFire();
    }
}
