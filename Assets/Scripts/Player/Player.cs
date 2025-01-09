using Fusion;
using UnityEngine;

public class Player : NetworkBehaviour
{
    private NetworkCharacterController _cc;
    [SerializeField] private NetworkPrefabRef _bulletPrefab;
    [SerializeField] private Transform _shootPoint; // Point from where bullets are spawned
    [SerializeField] private float _shootCooldown = 0.5f; // Cooldown in seconds
    [Networked] private TickTimer _shootCooldownTimer { get; set; } // Networked timer for cooldown


    private void Awake()
    {
        _cc = GetComponent<NetworkCharacterController>();
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
           
            if (HasInputAuthority && data.isShooting)
            {
                tryShoot();
            }
            if (!data.isShooting)
            {
                data.direction.Normalize();
                _cc.Move(5 * data.direction * Runner.DeltaTime);
            }
        }
    }

    private void tryShoot()
    {
        if (_shootCooldownTimer.ExpiredOrNotRunning(Runner))
        {
            _shootCooldownTimer = TickTimer.CreateFromSeconds(Runner, _shootCooldown); // Reset cooldown timer
            if (Runner.IsServer)
            {
                SpawnBullet();
            }
            else
            {
                // Call the RPC to request the server to spawn the bullet
                ShootBulletRPC(_shootPoint.position, _shootPoint.forward);
            }
        }

    }
    private void SpawnBullet()
    {
        // Spawn the bullet on the server
        var bullet = Runner.Spawn(_bulletPrefab, _shootPoint.position, Quaternion.identity);
        var bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.Initialize(_shootPoint.forward);
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void ShootBulletRPC(Vector3 position, Vector3 direction)
    {
        // Ensure the bullet is spawned only on the server
        if (Runner.IsServer)
        {
            SpawnBullet();
        }
    }


}