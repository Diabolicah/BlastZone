using Fusion;
using UnityEngine;

public class Player : NetworkBehaviour
{
    private NetworkCharacterController _characterController;
    private StatsManager _statsManager;

    [SerializeField] private float baseMaxSpeed = 4f;
    [SerializeField] private float baseAcceleration = 20f;

    public override void Spawned()
    {
        _characterController = GetComponent<NetworkCharacterController>();
        _statsManager = GetComponent<StatsManager>();

        if (_statsManager != null)
        {
            _statsManager.OnStatsChanged += HandleStatsChanged;
            HandleStatsChanged(_statsManager.Stats, _statsManager.Stats);
        }
    }

    private void OnDisable()
    {
        if (_statsManager != null)
            _statsManager.OnStatsChanged -= HandleStatsChanged;
    }

    private void HandleStatsChanged(PlayerStatsStruct oldStats, PlayerStatsStruct newStats)
    {
        float multiplier = newStats.MovementSpeed;
        _characterController.maxSpeed = baseMaxSpeed * multiplier;
        _characterController.acceleration = baseAcceleration * multiplier;
    }

    public override void FixedUpdateNetwork()
    {
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        moveDirection.Normalize();
        _characterController.Move(moveDirection * _characterController.maxSpeed * Runner.DeltaTime);
    }
}