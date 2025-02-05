using Fusion;
using UnityEngine;

public class FireElement : Element
{
    [SerializeField] private float _fireDamageDuration = 5f;
    [SerializeField] private float _fireDamagePercentageFromBulletDamage = 0.2f;

    public override void ElementEffect(NetworkObject Shooter, NetworkObject target)
    {
        Debug.Log("ElementEffect");
        if (Shooter.TryGetComponent<PlayerElement>(out PlayerElement playerElement))
        {
            float fireDamage = BulletDamage * _fireDamagePercentageFromBulletDamage;
            playerElement.useElement(new FireEffect(BulletDamage, _fireDamageDuration), Shooter, transform.position, target);
        }
    }
}
    