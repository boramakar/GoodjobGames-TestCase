using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "HappyTroll/Data/Level Parameters", fileName = "LevelParameters")]
public class LevelParameters : ScriptableObject
{
    [MinValue(2), MaxValue(10)]
    public int rows;
    [MinValue(2), MaxValue(10)]
    public int columns;
    [MinValue(2), MaxValue(6)]
    public int colors;
    public List<int> iconLimits;
}
