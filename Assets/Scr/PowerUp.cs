using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Powerups{
        MultiBall = 0,
        PaddleSizeSmall,
        PaddleSizeLarge,
        PaddleVelocitySlow,
        PaddleVelocityFast
    }
public class PowerUp : MonoBehaviour
{
    private const string POWERUP_PATH = "Sprites/PowerUps/PowerUp_{0}"; // path de donde cargaremos los sprites
    Powerups currentPowerUp;
    private Rigidbody2D _rb;
    private Collider2D _collider;
    private SpriteRenderer _renderer;
    
    [SerializeField]
    private float fallingSpeed = 0.1f;
    [SerializeField]
    private ArkanoidController gameManager;

    public void Init (Powerups powerup, Vector2 position)
    {
        currentPowerUp = powerup;
        switch(currentPowerUp)
        {
        case Powerups.MultiBall:
            _renderer.sprite = GetPowerUpSprite(1);

            break;

        case Powerups.PaddleSizeSmall:
            _renderer.sprite = GetPowerUpSprite(2);
            break;

        case Powerups.PaddleSizeLarge:
            _renderer.sprite = GetPowerUpSprite(3);
            break;
        
        case Powerups.PaddleVelocitySlow:
            _renderer.sprite = GetPowerUpSprite(4);
            break;
        
        case Powerups.PaddleVelocityFast:
            _renderer.sprite = GetPowerUpSprite(5);
            break;   
        }

        transform.position = position;
    }

    private void Awake()
    {
         _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();

        _collider.enabled = true;
        _renderer = GetComponentInChildren<SpriteRenderer>();

        gameManager = (ArkanoidController)FindObjectOfType(typeof(ArkanoidController));
    }

    private void Start()
    {
        ArkanoidEvent.OnGameOverEvent += DestroyObject;
        ArkanoidEvent.OnLevelUpdatedEvent += OnLevelUpdated;
        ArkanoidEvent.OnGameWinEvent += DestroyObject;
    }

       private void OnDestroy()
    {
        ArkanoidEvent.OnGameOverEvent -= DestroyObject;
        ArkanoidEvent.OnLevelUpdatedEvent -= OnLevelUpdated;
        ArkanoidEvent.OnGameWinEvent -= DestroyObject;
    }

    private void FixedUpdate()
    {
        _rb.velocity = new Vector2(0, -fallingSpeed); 
    }

    // POWERUP_PATH = "Sprites/PowerUps/PowerUp_{0}"
    static Sprite GetPowerUpSprite(int num_powerup)
    {
        string path = string.Empty;
        path = string.Format(POWERUP_PATH, num_powerup);

        if (string.IsNullOrEmpty(path)) // Si sí se llenó el path
        {
            return null;
        }

        // Cargamos y retornamos el sprite, usando la clase Resources de Unity
        return Resources.Load<Sprite>(path);
    }

    //Revisamos si un objeto toco al trigger collider del powerup
    private void OnTriggerEnter2D(Collider2D other)
    {
        Paddle paddleHit = other.GetComponentInParent<Paddle>();
        if (paddleHit == null)
        {
            return;
        }

        switch(currentPowerUp)
        {
        case Powerups.MultiBall:
            gameManager.MultiBall();
            break;

        case Powerups.PaddleSizeSmall:
            paddleHit.DecreaseSize();
            break;

        case Powerups.PaddleSizeLarge:
            paddleHit.IncrementSize();
            break;
        
        case Powerups.PaddleVelocitySlow:
            paddleHit.ChangeSpeed(-5);
            break;
        
        case Powerups.PaddleVelocityFast:
            paddleHit.ChangeSpeed(5);
            break;

        }
        Destroy (gameObject);
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }

    private void OnLevelUpdated(int level)
    {
        Destroy(gameObject);
    }
}
