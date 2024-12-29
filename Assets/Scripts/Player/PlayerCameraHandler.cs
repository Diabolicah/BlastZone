using UnityEngine;
using Fusion;
using Unity.Cinemachine;

public class PlayerCameraHandler : NetworkBehaviour
{
    private bool cameraAssigned = false;

    private void Start()
    {
        AssignCamera();
    }

    private void Update()
    {
        // Ensure camera is assigned if delayed due to initialization order
        if (!cameraAssigned)
        {
            AssignCamera();
        }
    }

    private void AssignCamera()
    {
        if (HasInputAuthority) // Ensure this is the local player
        {
            var virtualCamera = FindFirstObjectByType<CinemachineCamera>();
            if (virtualCamera != null)
            {
                virtualCamera.Follow = transform; // Set the camera to follow this player
                cameraAssigned = true; // Prevent redundant assignments
            }
            else
            {
                Debug.LogWarning("CinemachineCamera not found in the scene.");
            }
        }
    }
}
