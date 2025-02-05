using Fusion;
using UnityEngine;

public class ApplyExternalForce : NetworkBehaviour
{
    [SerializeField] CharacterController _characterController;
    [Networked] private Vector3 _currentForce { get; set; }
    [Networked] private TickTimer _forceTimer { get; set; }

    private void FixedUpdate()
    {
        if (HasStateAuthority && !_forceTimer.ExpiredOrNotRunning(Runner))
        {
            ApplyForceMovement();
        }
    }

    private void ApplyForceMovement()
    {
        _characterController.Move(_currentForce * Runner.DeltaTime);
        _currentForce = Vector3.Lerp(_currentForce, Vector3.zero, Runner.DeltaTime * 5f);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_ApplyForce(Vector3 force, float duration)
    {
        _currentForce = force;
        _forceTimer = TickTimer.CreateFromSeconds(Runner, duration);
    }
}
