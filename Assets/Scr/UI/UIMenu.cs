using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMenu : MonoBehaviour
{
    private CanvasGroup _canvasGroup;

    void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 1; //Asegurar que siempre empiece visible cuando el juego inicie
        ArkanoidEvent.OnGameStartEvent += OnGameStart;
        ArkanoidEvent.OnGameOverEvent += OnGameOver;
    }

    private void OnDestroy()
    {
        ArkanoidEvent.OnGameStartEvent -= OnGameStart;
        ArkanoidEvent.OnGameOverEvent -= OnGameOver;
    }

    private void OnGameStart()
    {
        _canvasGroup.alpha = 0; //Queremos que el menu principal se oculte para que el usuario juegue
    }
   
    private void OnGameOver()
    {
        _canvasGroup.alpha = 1; //Queremos que el menu vuelva a ser visible cuando el jugador pierda
    }

}
