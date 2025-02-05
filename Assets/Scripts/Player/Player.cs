using Fusion;
using UnityEngine;

public class Player : NetworkBehaviour
{
    private NetworkCharacterController _characterController;
    private MovementSpeed _movementSpeed;
 
    public override void Spawned()
    {
        _characterController = GetComponent<NetworkCharacterController>();
        _movementSpeed = GetComponent<MovementSpeed>();

        if (_movementSpeed != null)
        {
            _movementSpeed.OnStatChanged += HandleStatsChanged;
            //HandleStatsChanged("MaxSpeed", 0, _movementSpeed.GetStat("MaxSpeed"));
            //HandleStatsChanged("Acceleration", 0, _movementSpeed.GetStat("Acceleration"));
        }
    }

    private void OnDisable()
    {
        if (_movementSpeed != null)
            _movementSpeed.OnStatChanged -= HandleStatsChanged;
    }

    private void HandleStatsChanged(string StatName, float oldStat, float newStat)
    {
        if (StatName == "MaxSpeed")
        {
            _characterController.maxSpeed = newStat;
        }
        else if (StatName == "Acceleration")
        {
            _characterController.acceleration = newStat;
        }
    }

    public override void FixedUpdateNetwork()
    {
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        moveDirection.Normalize();
        _characterController.Move(moveDirection * _characterController.maxSpeed * Runner.DeltaTime);

        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("G key was pressed.");
            LevelingManager LM = GetComponent<LevelingManager>();
            LM.AddExp(100f);
        }
    }
}