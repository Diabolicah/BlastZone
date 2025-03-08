using Fusion;
using UnityEngine;

public class WindElement : Element
{
    [Range(0.1f, 2f)]
    [SerializeField] private float _windEffectDuration = 0.5f;
    [Range(0.1f, 0.5f)]
    [SerializeField] private float _speedBoostPercentage = 0.3f;

    public override void ElementEffect(NetworkObject Shooter, NetworkObject target)
    {
        if (Shooter.TryGetComponent<PlayerElement>(out PlayerElement playerElement))
        {
            playerElement.UseElement(new WindEffect(_speedBoostPercentage, _windEffectDuration), _windEffectDuration, Shooter, transform.position, target);
        }
    }
}
