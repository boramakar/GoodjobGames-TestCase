using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "HappyTroll/Data/Game Parameters", fileName = "GameParameters")]
public class GameParameters : ScriptableObject
{
    public float fadeDuration = 0.5f;
    public List<GameObject> gamePieces;
}
