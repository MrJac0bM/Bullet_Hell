using UnityEngine;

[CreateAssetMenu(fileName = "New Shot Pattern", menuName = "Shot Patterns/Shot Pattern")]
public class ShotPattern : ScriptableObject
{
    [Header("Pattern Configuration")]
    public string patternName = "New Pattern";
    public int Repetitions = 1;
    public ShotSetting[] PatternSettings;
    
    [Header("Visual")]
    public Color patternColor = Color.white;
}