using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArkanoidController : MonoBehaviour
{
     private const string BALL_PREFAB_PATH = "Prefabs/Ball";
     private readonly Vector2 BALL_INIT_POSITION = new Vector2(0, -0.86f); //equivalente al const para objetos
    [SerializeField]
     private GridController _gridController;
 
     [Space(20)]
     [SerializeField]
     private List<LevelData> _levels = new List<LevelData>();
 
     private int _currentLevel = 0;
     private int _totalScore = 0;

     private Ball _ballPrefab = null;
     private List<Ball> _balls = new List<Ball>(); // Guarda multiples pelotas
     PowerUp powerUpPrefab;
     
     [SerializeField]
     private Transform powerUpsParent; 

 
    private void Start()
    {
        //Nos suscribimos a los eventos
        ArkanoidEvent.OnBallReachDeadZoneEvent += OnBallReachDeadZone;
        ArkanoidEvent.OnBlockDestroyedEvent += OnBlockDestroyed;
        powerUpPrefab = Resources.Load<PowerUp>("Prefabs/PowerUp");
    }
    private void OnDestroy()
    {
        //Nos desuscribimos para evitar NullReference Exception
        ArkanoidEvent.OnBallReachDeadZoneEvent -= OnBallReachDeadZone;
        ArkanoidEvent.OnBlockDestroyedEvent -= OnBlockDestroyed;
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            InitGame();
        }
    }

    // Invocamos la nueva funcionalidad del GridController para que limpie y actualice el nivel actual
    private void InitGame()
    {
        _currentLevel = 0;
        _totalScore = 0;
        _gridController.BuildGrid(_levels[0]);
        ArkanoidEvent.OnGameStartEvent?.Invoke();
        ArkanoidEvent.OnLevelUpdatedEvent?.Invoke(_currentLevel);
        ArkanoidEvent.OnScoreUpdatedEvent?.Invoke(0,_totalScore);
        SetInitialBall();
    }

    // Obtiene una instancia del objeto Ball, invoca el metodo para inicializar y lo agrega a la lista de pelotas.
     private void SetInitialBall()
     {
         ClearBalls();
         Ball ball = CreateBallAt(BALL_INIT_POSITION);
         ball.Init();
         _balls.Add(ball);
     }

     public void MultiBall()
     {
        while (_balls.Count <3)
        {  
            Ball ball = CreateBallAt(BALL_INIT_POSITION);
            ball.Init();
            _balls.Add(ball);
        }
     }

    // Carga el prefab usando Resources e Instancia un nuevo objeto en la posición deseada.
     private Ball CreateBallAt(Vector2 position)
     {
         if (_ballPrefab == null)
         {
             _ballPrefab = Resources.Load<Ball>(BALL_PREFAB_PATH);
         }
         return Instantiate(_ballPrefab, position, Quaternion.identity);
     }

    // Destruye todas las pelotas actuales y limpia la lista
      private void ClearBalls()
    {
        for (int i = _balls.Count - 1; i >= 0; i--)
        {
            _balls[i].gameObject.SetActive(false);
            Destroy(_balls[i]);
        }
    
        _balls.Clear();
    }

// Se ejecutará cuando OnBallReachDeadZoneEvent se invoque (cuando la pelota toca la BottomWall)
// Ocultamos la pelota, la removemos de la lista y la destruimos
    private void OnBallReachDeadZone(Ball ball)
    {
        ball.Hide();
        _balls.Remove(ball);
        Destroy(ball.gameObject);

        //Verificamos si es GameOver despues de eliminar la ball
        CheckGameOver();
    }

//  Verificar si es Game Over, verificando la cantidad de pelotas restantes.
    private void CheckGameOver()
    {
        if (_balls.Count == 0)
        {
            ClearBalls();
            Debug.Log("Game Over: LOSE!!!");
            ArkanoidEvent.OnGameOverEvent?.Invoke();
        }
    }

    // Se ejecutará cuando OnBlockDestroyedEvent se invoque (se destruyó un bloque)
    private void OnBlockDestroyed(int blockId)
    {
        // Obtenemos el bloque que acabamos de destruir, y en caso de que exista, sumamos su puntaje.
        BlockTile blockDestroyed = _gridController.GetBlockBy(blockId);
        if (blockDestroyed != null)
        {
            _totalScore += blockDestroyed.Score;
            //Invoca el evento cuando se actualiza el puntaje
            ArkanoidEvent.OnScoreUpdatedEvent?.Invoke(blockDestroyed.Score, _totalScore);

            //POWERUPS
            float probability = Random.value;
            if (probability < 0.25f) 
            {
                int powerupnum = Random.Range(0,5); //convertir esto en el powerup del enum
                SpawnPowerUp(blockDestroyed.transform.position, powerupnum);
            }
        }
        
        //Si ya no quedan bloques activos
        if (_gridController.GetBlocksActive() == 0)
        {
            // y no hay más niveles, el juego termina
            _currentLevel++;
            if (_currentLevel >= _levels.Count)
            {
                ClearBalls();
                Debug.LogError("Game Over: WIN!!!!");
            }
            // y hay más niveles, se carga el siguiente nivel
            else
            {
                SetInitialBall();
                _gridController.BuildGrid(_levels[_currentLevel]);
                ArkanoidEvent.OnLevelUpdatedEvent?.Invoke(_currentLevel);
            }
        }
    }

    private void SpawnPowerUp(Vector2 position, int powerupnum)
    {
        PowerUp powerUp = Instantiate<PowerUp>(powerUpPrefab, powerUpsParent);
        Powerups powerUpType = (Powerups)powerupnum;
        powerUp.Init(powerUpType, position);
    }
}
