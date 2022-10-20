using System;

public class EventManager
{
    public static event Action GameStartEvent;
    public static void GameStart()
    {
        GameStartEvent?.Invoke();
    }

    public static event Action GamePauseEvent;
    public static void GamePause()
    {
        GamePauseEvent?.Invoke();
    }
}