using Fusion;
using UnityEngine;

public class RespawnObject : NetworkBehaviour
{
    [SerializeField] private GameObject _objectCanvaHealth;
    [SerializeField] private float _repsawnTime;

    private Collider _collider;
    private MeshRenderer _meshRenderer;
    private ObjectHealth _objectHealthScript;

    private NetworkBool _isDestroyed;
    private CooldownManager _repsawnMangaer;

    public override void Spawned()
    {
        base.Spawned();
        _repsawnMangaer = new CooldownManager();
        _isDestroyed = false;
        _collider = GetComponent<Collider>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _objectHealthScript = GetComponent<ObjectHealth>();
    }

    public override void FixedUpdateNetwork()
    {
        if (_isDestroyed && _repsawnMangaer.IsCooldownExpired(Runner))
        {
            ResetObject();
        }
    }

    public void Activate()
    {
        _isDestroyed = true;
        _repsawnMangaer.ResetCooldown(Runner, _repsawnTime);
        TogglePhysicalState(false);
    }

    private void ResetObject()
    {
        _isDestroyed = false;
        TogglePhysicalState(true);
        _objectHealthScript.ResetHealth();
    }


    private void TogglePhysicalState(bool active)
    {
        RpcTogglePhysicalState(active);
    }


    [Rpc(RpcSources.All, RpcTargets.All)]
    private void RpcTogglePhysicalState(bool active)
    {
        _collider.enabled = active;
        _meshRenderer.enabled = active;
        _objectHealthScript.enabled = active;
        _objectCanvaHealth.SetActive(active);
    }
}