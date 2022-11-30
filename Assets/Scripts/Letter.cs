using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Letter : MonoBehaviour
{
    [SerializeField] private Image backColor;
    [SerializeField] private TextMeshProUGUI text;
     
    [HideInInspector] public bool filled;

    public char Character => text.text[0];

    public void Fill(string letter)
    {
        text.text = letter;
        filled = true;
    }
    
    public void Empty()
    {
        text.text = "";
        filled = false;
    }

    public void SetColor(Color color)
    {
        backColor.color = color;
    }
}