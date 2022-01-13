using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Esta clase será solo para contener/guardar todos los eventos en un solo lugar y que sea de facil acceso
public static class ArkanoidEvent
{
    // Delegate: referencia a un metodo

    public delegate void GameStartAction();
    public static GameStartAction OnGameStartEvent;

    public delegate void GameOverAction();
    public static GameOverAction OnGameOverEvent;

    public delegate void BallDeadZoneAction(Ball ball);
    public static BallDeadZoneAction OnBallReachDeadZoneEvent;

    public delegate void BlockDestroyedAction(int blockID);
    public static BlockDestroyedAction OnBlockDestroyedEvent;

    public delegate void ScoreUpdatedAction(int score, int totalScore);
    public static ScoreUpdatedAction OnScoreUpdatedEvent;

    public delegate void LevelUpdateAction(int level);
    public static LevelUpdateAction OnLevelUpdatedEvent;

    public delegate void GameWinAction();
    public static GameWinAction OnGameWinEvent;
}

