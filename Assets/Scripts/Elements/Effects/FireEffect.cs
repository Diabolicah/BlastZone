using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class FireEffect : IElement
{
    private float _fireDamage;
    private float _fireDamageDuration;
    private CooldownManager _fireDurationManager;
    public void activate(NetworkRunner runner, NetworkObject bulletShooter, Transform position, NetworkObject playersHit)
    {
        throw new System.NotImplementedException();
    }
}
