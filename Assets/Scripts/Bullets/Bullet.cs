using Fusion;
using UnityEngine;
using UnityEngine.UIElements;

public class Bullet : NetworkBehaviour
{
    private TickTimer _life;
    private float _lifeTime;
    private float _damage;
    private NetworkObject _bulletShooter;

    private int _fireTick;
    private Vector3 _firePosition;
    private Vector3 _fireVelocity;

    public void Init(Vector3 position, Vector3 direction, float bulletSpeed, float lifeTime, float damage, NetworkObject BulletShooter)
    {
        _lifeTime = lifeTime;
        _damage = damage;
        _bulletShooter = BulletShooter;
        _life = TickTimer.CreateFromSeconds(Runner, _lifeTime);

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

        transform.position += nextPosition;

        var direction = nextPosition - previousPosition;
        var _hitMask = LayerMask.GetMask("Default");

        if (Runner.LagCompensation.Raycast(previousPosition, direction, direction.magnitude, Object.InputAuthority,
         out LagCompensatedHit hit, _hitMask, HitOptions.IncludePhysX | HitOptions.IgnoreInputAuthority))
        {
            if (hit.Hitbox)
            {
                Health playerHealth = hit.Hitbox.Root.GetComponent<Health>();
                if (playerHealth != null)
                {
                    (bool success, bool isDead) = playerHealth.ApplyDamage(_damage);
                    if (!success) return;
                    if (isDead) Debug.Log(_bulletShooter.ToString() + " Killed a player");
                    Runner.Despawn(Object);
                }
            }
        }

        if (_life.ExpiredOrNotRunning(Runner))
        {
            Runner.Despawn(Object);
        }
    }
   
    //private void OnTriggerEnter(Collider other)
    //{
        
    //    if (!HasStateAuthority) return;
    //    if (other.TryGetComponent<NetworkObject>(out NetworkObject obj))
    //    {
    //        if (obj.Id == _bulletShooter.Id) return;
    //        if (other.TryGetComponent<Hitbox>(out Hitbox hitbox))
    //        {
    //            Health playerHealth = hitbox.Root.GetComponent<Health>();
    //            if (playerHealth != null)
    //            {
    //                (bool success, bool isDead) = playerHealth.ApplyDamage(_damage);
    //                if (!success) return;
    //                if (isDead) Debug.Log(_bulletShooter.ToString() + " Killed a player");
    //                Runner.Despawn(Object);
    //            }
    //        }
    //    }
    //}

}
