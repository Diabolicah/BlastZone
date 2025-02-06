using Fusion;
using UnityEngine;
using static Unity.Collections.Unicode;

public class TankWeapons : NetworkBehaviour
{
    [SerializeField] public Transform _shootPoint; // Assign in inspector
    [SerializeField] private BulletWeaponConfig _weaponConfig; // Assign config asset
    [SerializeField] private NetworkObject _playerNetworkObj; // Test Weapon need to remove later

    PlayerStatsStruct _playerStats = PlayerStatsStruct.Default;

    private IWeapon _tankWeapon;

    public override void Spawned()
    {
        _tankWeapon = new WeaponBullet(_weaponConfig, _shootPoint);
    }

    public override void FixedUpdateNetwork()
    {
        if ((Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space)) && HasInputAuthority)
        {
            if (HasStateAuthority)
            {
                ServerFire(_playerStats, _playerNetworkObj.Id);//change it to the getPlayerStats from the playerStatManager
            }else
            {
                RequestFireRpc(_playerStats, _playerNetworkObj.Id);//change it to the getPlayerStats from the playerStatManager
            }
        }
    }

    public void setTankWeapon(IWeapon weaponConfig)
    {
        _tankWeapon = weaponConfig;
    }
    private void ServerFire(PlayerStatsStruct playerStats, NetworkId bulletShooterId)
    {
        _tankWeapon.fire(Runner, playerStats, bulletShooterId);
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RequestFireRpc(PlayerStatsStruct playerstats, NetworkId bulletShooterId)
    {
        ServerFire(playerstats, bulletShooterId);
    }
}
