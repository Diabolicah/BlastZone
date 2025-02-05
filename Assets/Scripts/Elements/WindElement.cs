using Fusion;
using UnityEngine;

public class WindElement : Element
{
    [SerializeField] private float _windEffectDuration = 5f;
    [Range(1f, 5f)]
    [SerializeField] private float _windPushBackForce = 2f;

    public override void ElementEffect(NetworkObject Shooter, NetworkObject target)
    {
        if (Shooter.TryGetComponent<PlayerElement>(out PlayerElement playerElement))
        {
            playerElement.useElement(new FireEffect(fireDamage, _fireDamageDuration), _windEffectDuration, Shooter, transform.position, target);
        }
    }
}
