using System;
using UnityEngine;
using UnityEngine.UI;

public class CustomizeTankColor : MonoBehaviour
{
    [SerializeField]
    private GameObject tankHead;
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
    
    
    private Color _newColor;
    

    public void Start()
    {
        tankHeadRenderer = tankHead.GetComponent<Renderer>();
        tankCanonRenderer = tankCanon.GetComponent<Renderer>();
        tankBodyRenderer = tankBody.GetComponent<Renderer>();
        tankWheelsCoverRenderer = tankWheelsCover.GetComponent<Renderer>();
        tankChainsRenderer = tankChains.GetComponent<Renderer>();
    }
    
    // Methods for changing the color of each tank part.
    public void ChangeTankHeadColor(string color)
    {
        ChangeColor(tankHeadRenderer, color, "Head");
    }
    
    public void ChangeTankCanonColor(string color)
    {
        ChangeColor(tankCanonRenderer, color, "Canon");
    }
    
    public void ChangeTankBodyColor(string color)
    {
        ChangeColor(tankBodyRenderer, color, "Body");
    }
    
    public void ChangeTankWheelsCoverColor(string color)
    {
        ChangeColor(tankWheelsCoverRenderer, color,"WheelsCover");
    }

    public void ChangeTankChainsColor(string color)
    {
        ChangeColor(tankChainsRenderer, color, "Chains");
    }
    // Shared method for applying the chosen color using a switch statement.
    private void ChangeColor(Renderer renderer, string color, string part)
    {
        Debug.Log("cus color:" + color);
        switch (color.ToLower())
        {
            case "red":
                _newColor = new Color(1, 0, 0);
                break;
            case "yellow":
                _newColor = new Color(1, 1, 0);
                break;
            case "green":
                _newColor = new Color(0, 1, 0);
                break;
            case "blue":
                _newColor = new Color(0.37f, 0.5f, 1);
                break;
            case "pink":
                _newColor = new Color(1, 0.5f, 0.9f);
                break;
            default:
                _newColor = new Color(1, 1, 1);
                break;
        }
        renderer.material.color = _newColor;
        PlayerPrefs.SetString(part, $"#{ColorUtility.ToHtmlStringRGBA(_newColor)}");
    }
}
