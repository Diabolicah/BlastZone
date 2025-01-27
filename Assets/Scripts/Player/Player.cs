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
        if (ShootCooldownTimer.IsRunning)
        {
            Debug.Log($"Cooldown active. Remaining time: {ShootCooldownTimer.RemainingTime(Runner)}");
        }
        if (GetInput(out NetworkInputData data))
        {
           
            if (HasInputAuthority && data.isShooting)
            {
                tryShoot();
            }
                data.direction.Normalize();
                _cc.Move(5 * data.direction * Runner.DeltaTime);
        }
    }

    private void tryShoot()
    {
        if (Runner.IsServer)
        {
            if (ShootCooldownTimer.ExpiredOrNotRunning(Runner))
            {
                ShootCooldownTimer = TickTimer.CreateFromSeconds(Runner, _shootCooldown); // Reset cooldown timer
                SpawnBullet(_shootPoint.position, _shootPoint.forward);
                Debug.Log($"Bullet spawned by server for player {Runner.LocalPlayer}");
            }
            else
            {
                Debug.Log($"Cooldown still active on server for player {Runner.LocalPlayer}");
            }
        }
        else
        {
            // Clients request the server to shoot
            Debug.Log($"Client requesting to shoot from server: {Runner.LocalPlayer}");
            ShootBulletRPC(_shootPoint.position, _shootPoint.forward);
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
        if (ShootCooldownTimer.ExpiredOrNotRunning(Runner))
        {
            ShootCooldownTimer = TickTimer.CreateFromSeconds(Runner, _shootCooldown); // Reset cooldown timer
            SpawnBullet(position, direction);
            Debug.Log($"RPC: Bullet spawned by server for player {Runner.LocalPlayer}");
        }
        else
        {
            Debug.Log("RPC: Cooldown still active, no bullet spawned");
        }
    }


}