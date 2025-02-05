using Fusion;
using UnityEngine;

public class FireElement : Element
{
    [SerializeField] private float _fireDamageDuration;
    [SerializeField] private int _fireDamagePercentageFromBulletDamage;

    public override void ElementEffect(NetworkObject Shooter, NetworkObject target)
    {

    }
}
    