using System.Collections.Generic;
using Fusion;
using NUnit.Framework;
using UnityEngine;

public class PlayerElement : NetworkBehaviour
{
  public void useElement(IElement element, NetworkId elementShooter, Transform usedElementPosition,List<NetworkId> playerHits)
   {
        element.activate(Runner, elementShooter, usedElementPosition, playerHits);
   }
}
