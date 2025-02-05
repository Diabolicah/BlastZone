using Fusion;
using UnityEngine;

public class IceElement : Element
{
    
    [SerializeField] private float _slowDuration = 2f;
    [Range(-5f, -0.1f)]
    [SerializeField] private float _slowPercentage = 2f;

    public override void ElementEffect(NetworkObject Shooter, NetworkObject target)
    {
        if (Shooter.TryGetComponent<PlayerElement>(out PlayerElement playerElement))
        {
            playerElement.useElement(new IceEffect(_slowPercentage, _slowDuration), _slowDuration, Shooter, transform.position, target);
        }
    }
}
