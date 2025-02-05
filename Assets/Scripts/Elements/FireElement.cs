using Fusion;
using UnityEngine;

public class FireElement : Element
{
    [SerializeField] private float _fireDamageDuration;
    [SerializeField] private int _fireDamagePercentageFromBulletDamage;

    public override void ElementEffect(NetworkObject Shooter, NetworkObject target)
    {
        if (Shooter.TryGetComponent<PlayerElement>(out PlayerElement playerElement))
        {
            float fireDamage = BulletDamage * _fireDamagePercentageFromBulletDamage;
            playerElement.useElement(new FireEffect(BulletDamage, _fireDamageDuration), Shooter, transform.position, target);
        }
    }
}
    