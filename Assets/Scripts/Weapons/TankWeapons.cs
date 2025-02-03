using Fusion;
using UnityEngine;
using static Unity.Collections.Unicode;

public class TankWeapons : NetworkBehaviour
{
    [SerializeField] private Transform _shootPoint; // Assign in inspector
    [SerializeField] private BulletWeaponConfig _weaponConfig; // Assign config asset
    [SerializeField] private TripleBulletWeaponConfig _trippleWeaponConfig; // Test Weapon need to remove later

    PlayerStats _playerStats;

    private IWeapon _tankWeapon;
    private IWeaponFactory _weaponFactory;

    public override void Spawned()
    {
        // Initialize factory with dependencies
        //_weaponFactory = new WeaponBulletFactory(_weaponConfig, _shootPoint);
        _weaponFactory = new WeaponTripleBulletFactory(_trippleWeaponConfig, _shootPoint);
        _tankWeapon = _weaponFactory.CreateWeapon();
        //add the player stats manager
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data) && HasInputAuthority && data.isShooting)
        {
            if (HasStateAuthority)
            {
                ServerFire(_playerStats);//change it to the getPlayerStats from the playerStatManager
            }else
            {
                RequestFireRpc(_playerStats.Encode());//change it to the getPlayerStats from the playerStatManager
            }
        }
    }

    public void setTankWeapon(IWeaponFactory weaponFactory)
    {
        _tankWeapon = weaponFactory.CreateWeapon();
    }
    private void ServerFire(PlayerStats playerStats)
    {

        _tankWeapon.fire(Runner, playerStats);
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RequestFireRpc(string playerstats)
    {
        ServerFire(PlayerStats.Decode(playerstats));
    }
}
