using UnityEngine;

[System.Serializable]
public class Situation 
{
    [Range(1, 3), SerializeField] public int deitiesRequired;

    [TextArea(1, 3), SerializeField] public string textEng;
    [TextArea(1, 3), SerializeField] public string textSpa;

    public int DeitiesRequired() { return deitiesRequired; }
    public string TextEng() { return textEng; }
    public string TextSpa() { return textSpa; }
}
