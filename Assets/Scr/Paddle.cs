using UnityEngine;

public enum PaddleSize
{
    Small = 0,
    Normal,
    Large
}

public class Paddle : MonoBehaviour
{

    [SerializeField]
    private float _speed = 5; // rapidez del movimiento
    [SerializeField]
    private float _minimumSpeed = 1;

    [SerializeField]
    private float _movementLimit = 7; // limite de la pantalla

    private Vector3 _targetPosition;

    private PaddleSize currentPaddleSize = PaddleSize.Normal;

    [SerializeField]
    GameObject smallPaddle;

    [SerializeField]
    GameObject NormalPaddle;

    [SerializeField]
    GameObject BigPaddle;


    private Camera _cam;
    private Camera Camera
    {
        get
        {
            if (_cam == null)
            {
                _cam = Camera.main;
            }
            return _cam;
        }
    }
    void Start()
    {
        ArkanoidEvent.OnGameStartEvent += OnGameStart;
    }

    void OnDestroy()
    {
         ArkanoidEvent.OnGameStartEvent += OnGameStart;
    }

    void Update()
    {
        _targetPosition.x = Camera.ScreenToWorldPoint(Input.mousePosition).x; // Obyiene la posicion del mouse en el mundo 3D
        _targetPosition.x = Mathf.Clamp(_targetPosition.x, -_movementLimit, _movementLimit); // Acota la x de acuerdo al Movement Limit
        _targetPosition.y = this.transform.position.y; //En y será la misma posición del paddle

        transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * _speed); // Para obtener un movimiento más suavizado (Lerp)
    }

    private void OnGameStart()
    {
        smallPaddle.SetActive(false);
        BigPaddle.SetActive(false);
        NormalPaddle.SetActive(true);
        _speed = 5;
    }

    public void ChangeSpeed(float speedDelta)
    {
        _speed = Mathf.Max(_minimumSpeed, _speed + speedDelta);
    }

    public void IncrementSize()
    {
        if (!(currentPaddleSize == PaddleSize.Large))
        {
            if(currentPaddleSize == PaddleSize.Small)
            {
                smallPaddle.SetActive(false);
                NormalPaddle.SetActive(true);
                BigPaddle.SetActive(false);
                currentPaddleSize = PaddleSize.Normal;
            }else
            {
                smallPaddle.SetActive(false);
                NormalPaddle.SetActive(false);
                BigPaddle.SetActive(true);
                currentPaddleSize = PaddleSize.Large;
            }
        }
    }

    public void DecreaseSize()
    {
        if (!(currentPaddleSize == PaddleSize.Small))
        {
            if(currentPaddleSize == PaddleSize.Large)
            {
                smallPaddle.SetActive(false);
                NormalPaddle.SetActive(true);
                BigPaddle.SetActive(false);
                currentPaddleSize = PaddleSize.Normal;
            }else
            {
                smallPaddle.SetActive(true);
                NormalPaddle.SetActive(false);
                BigPaddle.SetActive(false);
                currentPaddleSize = PaddleSize.Small;
            }
        }
    }
}