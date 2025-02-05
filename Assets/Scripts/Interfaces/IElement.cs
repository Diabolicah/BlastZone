using System;
using System.Collections.Generic;
using Fusion;
using NUnit.Framework;
using UnityEngine;

public interface IElement
{
    void activate(NetworkRunner runner, NetworkObject bulletShooter, Vector3 position, NetworkObject playersHit);
}
