using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Scriptable Objects/Level")]
public class LevelSO : ScriptableObject
{
    public GameObject levelPrefab;
    public string topCaption;
    public string boldText;
    public string hint;
    public bool hideTopAndBottom;
}
