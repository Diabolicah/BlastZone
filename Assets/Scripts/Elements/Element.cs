using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class Element : NetworkBehaviour
{
    [SerializeField] private float _aoeRadius = 5f;
    [SerializeField] private bool _isElementAoe = true;
    private NetworkObject _bulletShooter;
    protected List<NetworkId> playersHit = new List<NetworkId>();

    public float AoeRadius { get => _aoeRadius; set => _aoeRadius = value; }
    public bool IsElementAoe { get => _isElementAoe; set => _isElementAoe = value; }

    public override void Spawned()
    {
        TryGetComponent<Bullet>(out Bullet bullet);
        bullet.OnTargetHit += OnTargetHit;
        bullet.OnBulletDespawn += OnBulletDespawn;
        base.Spawned();
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        if (_isElementAoe)
            GetPlayersInRadius();
        
        if (playersHit.Count > 0)
        {
            onHitOrExpiredElement();
            playersHit.Clear();
        }

        base.Despawned(runner, hasState);
    }

    private void OnTargetHit(NetworkObject shooter, NetworkObject target)
    {
        _bulletShooter = shooter;
        playersHit.Add(target.Id);
    }
    private void OnBulletDespawn(NetworkObject shooter)
    {
        _bulletShooter = shooter;
    }

    private void GetPlayersInRadius()
    {
        playersHit.Clear();
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
                    if (!playersHit.Contains(playerObject.Id))
                    {
                        playersHit.Add(playerObject.Id);
                    }
                }
            }
        }
    }

    protected virtual void onHitOrExpiredElement() {
        Debug.Log(playersHit.ToString());
        foreach (NetworkId playerObjec in playersHit)
        {
            Debug.Log(playerObjec.ToString() + " Got Hit!!");
        }
    }
}
