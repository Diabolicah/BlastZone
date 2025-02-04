using Fusion;
using UnityEngine;

public class Element : NetworkBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject);
    }
}
