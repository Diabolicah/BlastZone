using System;
using UnityEngine;
using  UnityEngine.UI;

public class CustomizeTankColor : MonoBehaviour
{
    [SerializeField]
    private GameObject tankBody;
    
    private Renderer tankBodyRenderer;
    private Color _newtankBodyColor;
    
    public void Start()
    {
        tankBodyRenderer = tankBody.GetComponent<Renderer>();
    }
    
    public void ChangeBodyColor(string color)
    {
        switch (color)
        {
            case "red":
                _newtankBodyColor = new Color(1, 0, 0);
                break;
            case "green":
                _newtankBodyColor = new Color(0, 1, 0);
                break;
            case "blue":
                _newtankBodyColor = new Color(0, 0, 1);
                break;
            case "yellow":
                _newtankBodyColor = new Color(1, 1, 0);
                break;
            default:
                _newtankBodyColor = new Color(1, 1, 1);
                break;
        }
        
        tankBodyRenderer.material.color = _newtankBodyColor;
    }
}


// _newtankBodyColor = new Color(r, g, b);
// tankBodyRenderer.material.color = _newtankBodyColor;
