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
    
    public void Start()
    {

    }
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
    }

    public void AfterSpawned()
    {
        
        if (HasStateAuthority)
        {
            if (PlayerPrefs.HasKey("Head"))
            {
                tankHeadRenderer.material.color = ChangeColor(PlayerPrefs.GetString("Head"));
            }
            if (PlayerPrefs.HasKey("Canon"))
            {
                tankCanonRenderer.material.color = ChangeColor(PlayerPrefs.GetString("Canon"));
            }
            if (PlayerPrefs.HasKey("Body"))
            {
                tankBodyRenderer.material.color = ChangeColor(PlayerPrefs.GetString("Body"));
            }
            if (PlayerPrefs.HasKey("WheelsCover"))
            {
                tankWheelsCoverRenderer.material.color = ChangeColor(PlayerPrefs.GetString("WheelsCover"));
            }
            if (PlayerPrefs.HasKey("Chains"))
            {
                tankChainsRenderer.material.color = ChangeColor(PlayerPrefs.GetString("Chains"));
            }
        }
    }

    private void FixedUpdate()
    {
        _Playerusername.text = Username;
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