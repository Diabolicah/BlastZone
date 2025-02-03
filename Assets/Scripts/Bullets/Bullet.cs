using Fusion;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    [Networked] public Vector3 _direction { get; set; } // Networked bullet direction
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _lifeTime = 5f;
    [SerializeField] private TickTimer _life;


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

    public void Shoot(Vector3 direction)
    {
        _direction = direction;
        _life = TickTimer.CreateFromSeconds(Runner, _lifeTime);
    }
}
