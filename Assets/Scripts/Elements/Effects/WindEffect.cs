using Fusion;
using UnityEngine;

public class WindEffect : IElement
{
    private float _pushBackForce;
    private float _pushBackDuration;
    public WindEffect(float pushBackForce, float pushBackDuration) { 
        _pushBackForce = pushBackForce;
        _pushBackDuration = pushBackDuration;
    }
    public void activate(NetworkRunner runner, NetworkObject bulletShooter, Vector3 position, NetworkObject playersHit)
    {
        throw new System.NotImplementedException();
    }
}
