using System;
using Fusion;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using ColorUtility = UnityEngine.ColorUtility;

public class Player : NetworkBehaviour, IAfterSpawned
{
    private NetworkCharacterController _characterController;
    private MovementSpeed _movementSpeed;
    [SerializeField] private TextMeshProUGUI _Playerusername;
    
    [Networked] public string Username { get; set; }
    [Networked] public int Team { get; set; }
    [Networked] public string TankHeadColor { get; set; }
    [Networked] public string tankCanonColor { get; set; }
    [Networked] public string tankBodyColor { get; set; }
    [Networked] public string tankWheelsCoverColor { get; set; }
    [Networked] public string tankChainsColor { get; set; }
    
    
 
    [SerializeField]
    private GameObject TankHead;
    [SerializeField]
    private GameObject tankCanon;
    [SerializeField]
    private GameObject tankBody;
    [SerializeField]
    private GameObject tankWheelsCover;
    [SerializeField]
    private GameObject tankChains;
    
    private Renderer tankHeadRenderer;
    private Renderer tankCanonRenderer;
    private Renderer tankBodyRenderer;
    private Renderer tankWheelsCoverRenderer;
    private Renderer tankChainsRenderer;
    

    public override void Spawned()
    {
        tankHeadRenderer = TankHead.GetComponent<Renderer>();
        tankCanonRenderer = tankCanon.GetComponent<Renderer>();
        tankBodyRenderer = tankBody.GetComponent<Renderer>();
        tankWheelsCoverRenderer = tankWheelsCover.GetComponent<Renderer>();
        tankChainsRenderer = tankChains.GetComponent<Renderer>();
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
        
        if (HasStateAuthority)
        {
            if (PlayerPrefs.HasKey("Head"))
            {
                TankHeadColor = PlayerPrefs.GetString("Head");
            }
            if (PlayerPrefs.HasKey("Canon"))
            {
                tankCanonColor = PlayerPrefs.GetString("Canon");
            }
            if (PlayerPrefs.HasKey("Body"))
            {
                tankBodyColor = PlayerPrefs.GetString("Body");
            }

            if (PlayerPrefs.HasKey("WheelsCover"))
            {
                tankWheelsCoverColor = PlayerPrefs.GetString("WheelsCover");
            }
            if (PlayerPrefs.HasKey("Chains"))
            {
                tankChainsColor = PlayerPrefs.GetString("Chains");
            }
        }
    }

    public void AfterSpawned()
    {
        if (TankHeadColor != "")
        {
            tankHeadRenderer.material.color = ChangeColor(TankHeadColor);
        }
        if (tankCanonColor != "")
        {
            tankCanonRenderer.material.color = ChangeColor(tankCanonColor);
        }
        if (tankBodyColor != "")
        {
            tankBodyRenderer.material.color = ChangeColor(tankBodyColor);
        }
        if (tankWheelsCoverColor != "")
        {
            tankWheelsCoverRenderer.material.color = ChangeColor(tankWheelsCoverColor);
        }
        if (tankChainsColor != "")
        {
            tankChainsRenderer.material.color = ChangeColor(tankChainsColor);
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
    
    private Color ChangeColor(string color)
    {
        Debug.Log("color"+color);
        Color _newColor;
        ColorUtility.TryParseHtmlString(color, out _newColor);
        Debug.Log("_newColor"+_newColor);
        return _newColor;
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