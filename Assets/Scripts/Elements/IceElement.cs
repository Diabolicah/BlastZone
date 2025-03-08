using Fusion;
using UnityEngine;

public class IceElement : Element
{
    
    [SerializeField] private float _slowDuration;
    [Range(-5f, -0.1f)]
    [SerializeField] private float _slowPercentage;

    public override void ElementEffect(NetworkObject Shooter, NetworkObject target)
    {
        if (Shooter.TryGetComponent<PlayerElement>(out PlayerElement playerElement))
        {
            playerElement.UseElement(new IceEffect(_slowPercentage, _slowDuration), _slowDuration, Shooter, transform.position, target);
        }
    }
}
