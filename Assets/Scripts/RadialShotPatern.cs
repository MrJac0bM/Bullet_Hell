using UnityEngine;

[CreateAssetMenu(fileName = "New Radial Shot Pattern", menuName = "Shot Patterns/Radial Shot Pattern")]
public class RadialShotPatern : ScriptableObject
{
    public int Repetitions;
    public RadialShotSetting[] PatternSettings; 
}