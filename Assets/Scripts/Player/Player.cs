using Fusion;
using UnityEngine;

public class Player : NetworkBehaviour
{
    private NetworkCharacterController _characterController;
    private StatsManager _statsManager;

    [SerializeField] private float baseMaxSpeed = 2f;
    [SerializeField] private float baseAcceleration = 10f;

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
        if (GetInput(out NetworkInputData data))
        {
            data.direction.Normalize();
            _characterController.Move(data.direction * _characterController.maxSpeed * Runner.DeltaTime);
        }
    }
}