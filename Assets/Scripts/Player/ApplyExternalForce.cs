using Fusion;
using UnityEngine;

public class ApplyExternalForce : NetworkBehaviour
{
    [SerializeField] CharacterController _characterController;
    [Networked] private Vector3 _currentForce { get; set; }
    [Networked] private TickTimer _forceTimer { get; set; }

    public override void FixedUpdateNetwork()
    {
        if (HasStateAuthority)
        {
            ApplyForceMovement();
        }
    }

    private void ApplyForceMovement()
    {
        if (!_forceTimer.ExpiredOrNotRunning(Runner))
        {
            // Apply force while timer is active
            Debug.Log("Apply Force");
            _characterController.Move(_currentForce * Runner.DeltaTime);
            _currentForce = Vector3.Lerp(_currentForce, Vector3.zero, Runner.DeltaTime * 5f);
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_ApplyForce(Vector3 force, float duration)
    {
        Debug.Log("RPC Apply Force");
        _currentForce = force;
        _forceTimer = TickTimer.CreateFromSeconds(Runner, duration);
    }
}
