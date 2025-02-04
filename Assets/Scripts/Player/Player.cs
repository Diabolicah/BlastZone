using Fusion;
using UnityEngine;

public class Player : NetworkBehaviour
{
    private NetworkCharacterController _characterController;
    
    private StatsManager _statsManager;
    public override void Spawned()
    {
        _characterController = GetComponent<NetworkCharacterController>();
        _statsManager = GetComponent<StatsManager>();
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            data.direction.Normalize();
            _characterController.Move(5 * data.direction * Runner.DeltaTime);
        }
    }
}