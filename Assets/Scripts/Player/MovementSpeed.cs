using Fusion;
using WebSocketSharp;

public class MovementSpeed : BaseStats
{
    private string MAX_SPEED = "MaxSpeed";
    private string ACCELERATION = "Acceleration";

    protected override void OnStatManagerChange(PlayerStatsStruct oldStats, PlayerStatsStruct newStats)
    {
        if (oldStats.MovementSpeed != newStats.MovementSpeed)
        {
            ApplyDefaultMultiplierAndReset(MAX_SPEED, newStats.MovementSpeed);
            ApplyDefaultMultiplierAndReset(ACCELERATION, newStats.MovementSpeed);
        }
    }


    public void ApplyTemporaryModifier(float amount, float duration) => RPC_ApplyTemporaryModifier(amount, duration);

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void RPC_ApplyTemporaryModifier(float amount, float duration, RpcInfo info = default)
    {
        ApplyTemporaryMultiplier(MAX_SPEED, amount, duration);
        ApplyTemporaryMultiplier(ACCELERATION, amount, duration);
    }
}

