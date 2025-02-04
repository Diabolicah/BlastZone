using Fusion;
using UnityEngine;

public interface IWeapon
{
    void fire(NetworkRunner runner, PlayerStatsStruct playerStats);
}
