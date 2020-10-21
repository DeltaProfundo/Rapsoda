using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Fade : MonoBehaviour
{
    public Image background;
    public TextMeshProUGUI label;
    
    public void Setup(string text)
    {
        label.SetText(text);
    }
}
