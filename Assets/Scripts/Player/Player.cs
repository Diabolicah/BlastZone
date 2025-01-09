using Fusion;
using UnityEngine;

public class Player : NetworkBehaviour
{
    private NetworkCharacterController _cc;
    [SerializeField] private NetworkPrefabRef _bulletPrefab;
    [SerializeField] private Transform _shootPoint; // Point from where bullets are spawned
    [SerializeField] private float _shootCooldown = 0.5f; // Cooldown in seconds
    [Networked] private TickTimer ShootCooldownTimer { get; set; } // Networked timer for cooldown


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
        if (ShootCooldownTimer.ExpiredOrNotRunning(Runner))
        {
            ShootCooldownTimer = TickTimer.CreateFromSeconds(Runner, _shootCooldown); // Reset cooldown timer
            if (Runner.IsServer)
            {
                SpawnBullet(_shootPoint.position, _shootPoint.forward);
            }
            else
            {
                // Call the RPC to request the server to spawn the bullet
                ShootBulletRPC(_shootPoint.position, _shootPoint.forward);
            }
        }

    }
    private void SpawnBullet(Vector3 position, Vector3 direction)
    {
        Quaternion bulletRotation = Quaternion.LookRotation(direction, Vector3.up);
        // Spawn the bullet on the server
        var bullet = Runner.Spawn(_bulletPrefab, position, bulletRotation);
        var bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.Initialize(direction);
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void ShootBulletRPC(Vector3 position, Vector3 direction)
    {
        // Ensure the bullet is spawned only on the server
        if (Runner.IsServer)
        {
            SpawnBullet(position, direction);
        }
    }


}