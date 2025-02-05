using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class FireEffect : IElement
{
    private float _fireDamage;
    private float _fireDamageDuration;
    private CooldownManager _fireDurationManager;

    public FireEffect(float fireDamage, float fireDamageDuration)
    {
        _fireDamage = fireDamage;
        _fireDamageDuration = fireDamageDuration;
        _fireDurationManager = new CooldownManager();
    }

    public void activate(NetworkRunner runner, NetworkObject bulletShooter, Vector3 position, NetworkObject playersHit)
    {
        Debug.Log(playersHit.ToString() + " Is On Fire");
    }
}
