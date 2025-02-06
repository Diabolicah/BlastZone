using System;
using Fusion;
using UnityEngine;
using UnityEngine.UIElements;

public class Bullet : NetworkBehaviour
{
    public event Action<NetworkObject, NetworkObject> OnTargetHit;
    public event Action<NetworkObject> OnBulletDespawn;

    private TickTimer _life;
    private float _lifeTime;
    private float _damage;
    private bool _isBulletAlive;
    private NetworkObject _bulletShooter;

    private int _fireTick;
    private Vector3 _firePosition;
    private Vector3 _fireVelocity;

    public NetworkObject BulletShooter { get => _bulletShooter;}
    public float BulletDamage { get => _damage; }

    public void Init(Vector3 position, Vector3 direction, float bulletSpeed, float lifeTime, float damage, NetworkObject BulletShooter)
    {
        _lifeTime = lifeTime;
        _damage = damage;
        _bulletShooter = BulletShooter;
        _life = TickTimer.CreateFromSeconds(Runner, _lifeTime);
        _isBulletAlive = true;
        _fireTick = Runner.Tick;
        _firePosition = position;
        _fireVelocity = direction * bulletSpeed;
    }

    private Vector3 GetMovePosition(float currentTick)
    {
        float time = (currentTick - _fireTick) * Runner.DeltaTime;

        if (time <= 0f)
            return _firePosition;

        return _firePosition + _fireVelocity * time;
    }

    public override void FixedUpdateNetwork()
    {
        if (IsProxy == true)
            return;

        var previousPosition = GetMovePosition(Runner.Tick - 1);
        var nextPosition = GetMovePosition(Runner.Tick);

        transform.position = nextPosition;

        var direction = nextPosition - previousPosition;
        var _hitMask = LayerMask.GetMask("Default");

        if (_isBulletAlive && Physics.SphereCast(previousPosition, 0.8f, direction.normalized, out RaycastHit hitInfo, direction.magnitude, _hitMask))
        {
            if (hitInfo.collider.TryGetComponent<NetworkObject>(out NetworkObject networkObj))
            {
                if (networkObj.Id == _bulletShooter.Id) return;
                Health playerHealth = hitInfo.collider.GetComponentInParent<Health>();
                if (playerHealth != null)
                {
                    (bool success, bool isDead) = playerHealth.ApplyDamage(_damage);
                    if (success)
                    {
                        if (isDead) {
                            LevelingManager LM = _bulletShooter.GetComponent<LevelingManager>();
                            DeathXpValue dxv = hitInfo.collider.GetComponent<DeathXpValue>();
                            LM.AddExp(dxv.XpValue);
                        }
                        else
                        {
                            OnTargetHit?.Invoke(_bulletShooter, networkObj);
                        }
                        _isBulletAlive = false;
                        Runner.Despawn(Object);
                    }
                }
            }
        }

        if (_isBulletAlive && _life.ExpiredOrNotRunning(Runner))
        {
            OnBulletDespawn?.Invoke(_bulletShooter);
            Runner.Despawn(Object);
        }
    }
}
