using Fusion;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    [Networked] public Vector3 _direction { get; set; } // Networked bullet direction
    [SerializeField] private TickTimer _life;
    private float _bulletSpeed;
    private float _lifeTime;
    private float _damage;
    private NetworkObject _bulletShooter;

    public void Init(Vector3 direction, float bulletSpeed, float lifeTime, float damage, NetworkObject BulletShooter)
    {
        _direction = direction;
        _bulletSpeed = bulletSpeed;
        _lifeTime = lifeTime;
        _damage = damage;
        _bulletShooter = BulletShooter;
        _life = TickTimer.CreateFromSeconds(Runner, _lifeTime);
    }
    // FixedUpdateNetwork is called once per server tick
    public override void FixedUpdateNetwork()
    {
        if (Runner.IsForward)
        {
            transform.position += _direction * _bulletSpeed * Time.deltaTime;
            if (_life.ExpiredOrNotRunning(Runner))
            {
                Runner.Despawn(Object); // Despawn the bullet after its lifetime expires
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!HasStateAuthority) return;
        if (other.TryGetComponent<NetworkObject>(out NetworkObject obj))
        {
            if (obj.Id == _bulletShooter.Id) return;
            if (other.TryGetComponent<Hitbox>(out Hitbox hitbox))
            {
                Health playerHealth = hitbox.Root.GetComponent<Health>();
                if (playerHealth != null)
                {
                    (bool success, bool isDead) = playerHealth.ApplyDamage(_damage);
                    if (!success) return;
                    if (isDead) Debug.Log(_bulletShooter.ToString() + " Killed a player");
                    Runner.Despawn(Object);
                }
            }
        }
    }

}
