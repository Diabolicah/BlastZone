using Fusion;
using UnityEngine;
using UnityEngine.UIElements;

public class Bullet : NetworkBehaviour
{
    private TickTimer _life;
    private float _lifeTime;
    private float _damage;
    private bool _isAlive;
    private bool _bulletHit;
    private NetworkObject _bulletShooter;

    private int _fireTick;
    private Vector3 _firePosition;
    private Vector3 _fireVelocity;

    public NetworkObject BulletShooter { get => _bulletShooter;}

    public void Init(Vector3 position, Vector3 direction, float bulletSpeed, float lifeTime, float damage, NetworkObject BulletShooter)
    {
        _lifeTime = lifeTime;
        _damage = damage;
        _bulletShooter = BulletShooter;
        _life = TickTimer.CreateFromSeconds(Runner, _lifeTime);
        _isAlive = true;
        _bulletHit = false;
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

        if (_bulletHit && Physics.Raycast(previousPosition, direction.normalized, out RaycastHit hitInfo, direction.magnitude, _hitMask))
        {
            if (hitInfo.collider.TryGetComponent<NetworkObject>(out NetworkObject obj))
            {
                if (obj.Id == _bulletShooter.Id) return;
                Health playerHealth = hitInfo.collider.GetComponentInParent<Health>();
                if (playerHealth != null)
                {
                    Debug.Log("Damage Applied");
                    (bool success, bool isDead) = playerHealth.ApplyDamage(_damage);
                    _bulletHit = true;
                    if (success)
                    {
                        if (isDead) Debug.Log(_bulletShooter.ToString() + " Killed a player");
                        Runner.Despawn(Object);
                        _isAlive = false;
                    }
                }
            }
        }

        if (_isAlive && _life.ExpiredOrNotRunning(Runner))
        {
            Runner.Despawn(Object);
        }
    }
}
