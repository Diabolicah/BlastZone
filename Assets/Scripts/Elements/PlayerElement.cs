using System.Collections.Generic;
using Fusion;
using NUnit.Framework;
using UnityEngine;

public class PlayerElement : NetworkBehaviour
{
  public void useElement(IElement element, NetworkObject elementShooter, Transform usedElementPosition, NetworkObject playerHits)
   {
        element.activate(Runner, elementShooter, usedElementPosition, playerHits);
   }
}
