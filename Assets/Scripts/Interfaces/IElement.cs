using System;
using System.Collections.Generic;
using Fusion;
using NUnit.Framework;
using UnityEngine;

public interface IElement
{
    void activate(NetworkRunner runner, NetworkId bulletShooter, Transform position, List<NetworkId> playersHit);
}
