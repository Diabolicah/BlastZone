using System;
using Fusion;
using UI;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Collections.Unicode;

public class ObjectHealth : Health
{
    private CooldownManager _repsawnMangaer;
    [SerializeField] private float _repsawnTime;
    private Collider _collider;
    private MeshRenderer _meshRenderer;
    private NetworkBool _isDestroyed;

    public override void Spawned()
    {
        _collider = GetComponent<Collider>();
        _meshRenderer = GetComponent<MeshRenderer>();

        if (Object.HasStateAuthority)
        {
            ResetObject();
        }
    }

    public override void FixedUpdateNetwork()
    {
        if(_isDestroyed && _repsawnMangaer.IsCooldownExpired(Runner))
        {
            ResetObject();
        }
    }

    protected override void OnDeath()
    {
        _isDestroyed = true;
        _repsawnMangaer.ResetCooldown(Runner, _repsawnTime);
        TogglePhysicalState(false);
    }
    
    private void ResetObject()
    {
        SetStat(CURRENT_HEALTH, GetStat(MAX_HEALTH));
        _isDestroyed = false;
        TogglePhysicalState(true);
    }

    private void TogglePhysicalState(bool active)
    {
        _collider.enabled = active;
        _meshRenderer.enabled = active;
    }
}
