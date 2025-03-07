using System;
using Fusion;
using TMPro;
using UnityEngine;

public class Player : NetworkBehaviour, IAfterSpawned
{
    private NetworkCharacterController _characterController;
    private MovementSpeed _movementSpeed;
    [SerializeField] private TextMeshProUGUI _Playerusername;
    [Networked] public string Username { get; set; }
    [Networked] public int Team { get; set; }

    public override void Spawned()
    {
        _characterController = GetComponent<NetworkCharacterController>();
        _movementSpeed = GetComponent<MovementSpeed>();

        if (_movementSpeed != null)
        {
            _movementSpeed.OnStatChanged += HandleStatsChanged;
        }

        if (NetworkManager.selectedGameMode == "TeamDeathmatch")
        {
            Team = SC_TeamManager.Instance.GetTeam();
        }
        else
        {
            Team = 0;
        }

        if (HasStateAuthority)
        {
            Username = PlayerPrefs.HasKey("PlayerName") ? PlayerPrefs.GetString("PlayerName") : SC_LoginLogic.PlayerName;
            LevelingManager levelingManager = GetComponent<LevelingManager>();
            if (levelingManager != null)
            {
                levelingManager.Rank = PlayerPrefs.HasKey("Rank") ? PlayerPrefs.GetInt("Rank") : SC_LoginLogic.PlayerRank != "" ? int.Parse(SC_LoginLogic.PlayerRank) : 1;
            }
        }
    }

    public void AfterSpawned()
    {
        _Playerusername.text = Username;
        _Playerusername.color = Color.white;
        if (Team == 1)
        {
            _Playerusername.color = Color.blue;
        }
        else if (Team == 2)
        {
            _Playerusername.color = Color.red;
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
            LevelingManager LM = GetComponent<LevelingManager>();
            LM.AddExp(100f);
        }
    }

}