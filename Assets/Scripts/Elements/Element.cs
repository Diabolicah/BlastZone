using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class Element : NetworkBehaviour
{
    private Bullet _bullet;
    private float _aoeRadius = 0f;
    private bool _isElementAoe = false;
    protected List<NetworkId> playersHit = new List<NetworkId>();

    public float AoeRadius { set => _aoeRadius = value; }
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
        {
            GetPlayersInRadius();
            onHitOrExpiredElement();
        }
        base.Despawned(runner, hasState);
    }

    private void OnTargetHit(NetworkObject target)
    {
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

    protected virtual void onHitOrExpiredElement() { }
}
