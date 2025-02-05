using Fusion;
using UnityEngine;

public class WindElement : Element
{
    [Range(0.1f, 1f)]
    [SerializeField] private float _windEffectDuration = 0.5f;
    [Range(1f, 25f)]
    [SerializeField] private float _windPushBackForce = 2f;

    public override void ElementEffect(NetworkObject Shooter, NetworkObject target)
    {
        if (Shooter.TryGetComponent<PlayerElement>(out PlayerElement playerElement))
        {
            playerElement.useElement(new WindEffect(_windPushBackForce, _windEffectDuration), _windEffectDuration, Shooter, transform.position, target);
        }
    }
}
