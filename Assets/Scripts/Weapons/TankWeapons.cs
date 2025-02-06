using Fusion;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Collections.Unicode;

public class TankWeapons : NetworkBehaviour
{
    [SerializeField] public Transform _shootPoint;
    [SerializeField] private BulletWeaponConfig _weaponConfig;
    [SerializeField] private NetworkObject _playerNetworkObj;
    [SerializeField] public Image WeaponIcon;

    PlayerStatsStruct _playerStats = PlayerStatsStruct.Default;

    private IWeapon _tankWeapon;

    public override void Spawned()
    {
        _tankWeapon = new WeaponBullet(_weaponConfig, _shootPoint);
        if (IsProxy)
        {
            WeaponIcon.gameObject.SetActive(false);
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (Input.GetKey(KeyCode.Space) && HasInputAuthority)
        {
            if (HasStateAuthority)
            {
                _playerStats = Runner.FindObject(_playerNetworkObj.Id).GetComponent<StatsManager>().Stats;
                ServerFire(_playerStats, _playerNetworkObj.Id);
            }else
            {
                _playerStats = Runner.FindObject(_playerNetworkObj.Id).GetComponent<StatsManager>().Stats;
                RequestFireRpc(_playerStats, _playerNetworkObj.Id);
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
