using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class Element : NetworkBehaviour
{
    [SerializeField] private float _aoeRadius = 5f;
    [SerializeField] private bool _isElementAoe;
    private NetworkObject _bulletShooter;
    private List<NetworkObject> _playersHit = new List<NetworkObject>();
    private float _bulletDamage;

    public float AoeRadius { get => _aoeRadius; set => _aoeRadius = value; }
    public float BulletDamage { get => _bulletDamage; }
    public bool IsElementAoe { get => _isElementAoe; set => _isElementAoe = value; }

    public override void Spawned()
    {
        if(TryGetComponent<Bullet>(out Bullet bullet))
        {
            bullet.OnTargetHit += OnTargetHit;
            bullet.OnBulletDespawn += OnBulletDespawn;
        }
        base.Spawned();
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        if (_isElementAoe)
        {
            GetPlayersInRadius();
        }
        
        if (_playersHit.Count > 0)
        {
            OnHitOrExpiredElement();
            _playersHit.Clear();
        }

        base.Despawned(runner, hasState);
    }

    private void OnTargetHit(NetworkObject shooter, NetworkObject target)
    {
        _bulletShooter = shooter;
        _playersHit.Add(target);
    }
    private void OnBulletDespawn(NetworkObject shooter)
    {
        _bulletShooter = shooter;
    }

    private void GetPlayersInRadius()
    {
        _playersHit.Clear();
        if (IsProxy == true) return;
        if (Runner == null || !Runner.IsRunning) return;

        float radiusSqr = _aoeRadius * _aoeRadius;

        foreach (PlayerRef playerRef in Runner.ActivePlayers)
        {
            if (Runner.TryGetPlayerObject(playerRef, out NetworkObject playerObject))
            {
                if (playerObject.Id == _bulletShooter.Id) continue;
                if ((playerObject.transform.position - transform.position).sqrMagnitude <= radiusSqr)
                {
                    if (!_playersHit.Contains(playerObject))
                    {
                        _playersHit.Add(playerObject);
                    }
                }
            }
        }
    }

    protected virtual void OnHitOrExpiredElement() {
        if (IsProxy == true) return;
        if (Runner == null || !Runner.IsRunning) return;
        if (TryGetComponent<Bullet>(out Bullet bullet))
            _bulletDamage = bullet.BulletDamage;

        foreach (NetworkId playerObject in _playersHit)
        {
            Debug.Log(playerObject.ToString() + " Got Hit!!");
            ElementEffect(_bulletShooter, Runner.FindObject(playerObject));
        }
    }

    public virtual void ElementEffect(NetworkObject Shooter, NetworkObject target) { }
}
