using UnityEngine;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour
{
    private const float BALL_VELOCITY_MIN_AXIS_VALUE = 0.5f; //Valor minimo del vector de velocidad

    [SerializeField]
    private float _initSpeed = 5;
    [SerializeField]
    private float _minSpeed = 4;
    [SerializeField]
    private float _maxSpeed = 7;


    private Rigidbody2D _rb;
    private Collider2D _collider;
    

    public void Init()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();

        _collider.enabled = true;
        _rb.velocity = Random.insideUnitCircle.normalized * _initSpeed;

    }

    void FixedUpdate()
    {
        CheckVelocity();
    }

    private void CheckVelocity()
    {
        Vector2 velocity = _rb.velocity;
        float currentSpeed = velocity.magnitude;

        // Validamos la velocidad mínima y máxima. En caso de no cumplir, obligamos a que utilice la velocidad correspondiente.
        if (currentSpeed < _minSpeed)
        {
            velocity = velocity.normalized * _minSpeed; // para conservar la dirección del movimiento, pero con la rapidez deseada.
        }
        else if (currentSpeed > _maxSpeed)
        {
            velocity = velocity.normalized * _maxSpeed;
        }

        // Verificamos cada componente de la velocidad, para evitar movimiento solo horizontales o solo verticales.
        // (cuando una componente es casi 0)

        if(Mathf.Abs(velocity.x) < BALL_VELOCITY_MIN_AXIS_VALUE) 
        {
            // para obtener el signo, la direccion del movimiento
            float sign = velocity.x == 0 ? Mathf.Sign(-transform.position.x) : Mathf.Sign(velocity.x);
            velocity.x += sign * BALL_VELOCITY_MIN_AXIS_VALUE * Time.deltaTime;
            // En caso de que el valor sea menor al deseado, sumamos pequeñas cantidades cada frame en la dirección del movimiento
        }
        else if (Mathf.Abs(velocity.y) < BALL_VELOCITY_MIN_AXIS_VALUE)
        {   
            float sign = velocity.y == 0 ? Mathf.Sign(-transform.position.y) : Mathf.Sign(velocity.y);   
            velocity.y += sign * BALL_VELOCITY_MIN_AXIS_VALUE * Time.deltaTime;
        }

        _rb.velocity = velocity;

    }

    //Es invocada por Unity cuando se produce una colisión.
    private void OnCollisionEnter2D(Collision2D other) // Recibe un objeto que contiene toda la info de la colision.
    {
        BlockTile blockTileHit;

        // Verificamos si el objeto que impactamos, contiene un componente de tipo BlockTile
        if (!other.collider.TryGetComponent(out blockTileHit))
        {
            return;
        }

        //Si sí, llamamos la funcion OnHitCollision y e enviamos un objeto de tipo ContactPoint2D, proveniente de los datos de la colision
        ContactPoint2D contactPoint = other.contacts[0];
        blockTileHit.OnHitCollision(contactPoint);

    }

    //Desactivamos el collider y apagamos el objeto para evitar colisiones no deseadas.
    public void Hide()
    {
        _collider.enabled = false;
        gameObject.SetActive(false);
    }

}
