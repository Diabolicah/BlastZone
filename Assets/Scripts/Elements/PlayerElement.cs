using System.Collections.Generic;
using Fusion;
using NUnit.Framework;
using UnityEngine;
using static UnityEditor.Rendering.FilterWindow;

public class PlayerElement : NetworkBehaviour
{
    private bool _isElementUsed = false;
    private IElement _element;
    private NetworkObject _elementShooter;
    private Vector3 _usedElementPosition;
    private NetworkObject _playerHit;
    private CooldownManager _elementDurationManager;

    public override void Spawned()
    {
        _elementDurationManager = new CooldownManager();
    }
    public override void FixedUpdateNetwork()
    {
        if (_elementDurationManager.IsCooldownExpiredOrNotRunning(Runner))
        {
            _isElementUsed = false;
        }

        if (_isElementUsed)
        {
            _element.activate(Runner, _elementShooter, _usedElementPosition, _playerHit);
        }
    }


    public void useElement(IElement element, float elementDuration,NetworkObject elementShooter, Vector3 usedElementPosition, NetworkObject playerHit)
    {
        _element = element;
        _elementShooter = elementShooter;
        _usedElementPosition = usedElementPosition;
        _playerHit = playerHit;
        _isElementUsed = true;
        _elementDurationManager.ResetCooldown(Runner, elementDuration);
    }
}
