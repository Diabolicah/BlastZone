using Fusion;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    [Networked] public Vector3 _direction { get; set; } // Networked bullet direction
    [SerializeField] private TickTimer _life;
    private float _speed;
    private float _lifeTime;
    private float _damage;


    // FixedUpdateNetwork is called once per server tick
    public override void FixedUpdateNetwork()
    {
        if (Runner.IsForward)
        {
            transform.position += _direction * _speed * Time.deltaTime;
            if (_life.ExpiredOrNotRunning(Runner))
            {
                Runner.Despawn(Object); // Despawn the bullet after its lifetime expires
            }
        }
    }

    public void Shoot(Vector3 direction, float speed, float lifeTime, float damage)
    {
        _direction = direction;
        _speed = speed;
        _lifeTime = lifeTime;
        _damage = damage;
        _life = TickTimer.CreateFromSeconds(Runner, _lifeTime);
    }
}
