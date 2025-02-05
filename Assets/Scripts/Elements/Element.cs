using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class Element : NetworkBehaviour
{
    [SerializeField] private float _aoeRadius = 2f;
    [SerializeField] private bool _isElementAoe = true;
    private Bullet _bullet;
    protected List<NetworkId> playersHit = new List<NetworkId>();

    public float AoeRadius { get => _aoeRadius; set => _aoeRadius = value; }
    public bool IsElementAoe { get => _isElementAoe; set => _isElementAoe = value; }

    public override void Spawned()
    {
        TryGetComponent<Bullet>(out Bullet _bullet);
        _bullet.OnTargetHit += OnTargetHit;
        base.Spawned();
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        Debug.Log("Activated Element");
        if (_isElementAoe)
            GetPlayersInRadius();
        
        if (playersHit.Count > 0)
            onHitOrExpiredElement();
        
        base.Despawned(runner, hasState);
    }

    private void OnTargetHit(NetworkObject target)
    {
        playersHit.Clear();
        playersHit.Add(target.Id);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (TryGetComponent<Bullet>(out Bullet bullet))
    //    {
    //        _bulletShooter = bullet.BulletShooter;
    //        if (other.GetComponent<NetworkObject>().Id == _bulletShooter.Id)
    //            { return; }
    //        Debug.Log(other.gameObject);
    //    }
    //}

    private void GetPlayersInRadius()
    {
        playersHit.Clear();

        if (Runner == null || !Runner.IsRunning) return;

        float radiusSqr = _aoeRadius * _aoeRadius;

        foreach (PlayerRef playerRef in Runner.ActivePlayers)
        {
            if (Runner.TryGetPlayerObject(playerRef, out NetworkObject playerObject))
            {
                if (playerObject.Id == _bullet.BulletShooter.Id) continue;
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
        foreach (NetworkId playerObjec in playersHit)
        {
            Debug.Log(playerObjec.ToString() + " Got Hit!!");
        }
    }
}
