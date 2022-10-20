using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGamePiece
{
    public void Enable();
    public void Disable();
    public void SetIcon(int iconIndex);
}
