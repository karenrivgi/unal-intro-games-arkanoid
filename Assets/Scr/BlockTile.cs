using UnityEngine;

public enum BlockType
{
    Small,
    Big
}

public enum BlockColor
{
    Green,
    Blue,
    Orange,
    Red,
    Purple
}

public class BlockTile : MonoBehaviour
{
    private const string BLOCK_BIG_PATH = "Sprites/BlockTiles/Big/Big_{0}_{1}"; // path de donde cargaremos los sprites
    
    [SerializeField] 
    private BlockType _type = BlockType.Big;

    [SerializeField]
    private int _score = 10;
    
    public int Score => _score;
    
    private int _id;
    private BlockColor _color = BlockColor.Blue;
    private SpriteRenderer _renderer;
    private Collider2D _collider;
    
    private int _totalHits = 1;
    private int _currentHits = 0;

    //Le asigna la info pasada en la construccion del Grid
    public void SetData(int id, BlockColor color)
    {
        _id = id;
        _color = color;
    }
    
    public void Init()
    {
        _currentHits = 0;

        // determinamos que se requieren dos golpes para destruir un bloque grande
        _totalHits = _type == BlockType.Big ? 2 : 1;

        _collider = GetComponent<Collider2D>();
        _collider.enabled = true;
        
        _renderer = GetComponentInChildren<SpriteRenderer>();

        // Cargamos el sprite correspondiente, el inicial
        _renderer.sprite =GetBlockSprite(_type, _color, 0);
    }
    
    // Será llamada cuando ball impacte con un bloque
    public void OnHitCollision(ContactPoint2D contactPoint)
    {
        _currentHits++;
        if (_currentHits >= _totalHits)
        {
            _collider.enabled = false; // para evitar colisiones no deseadas  
            gameObject.SetActive(false);
            // Invocamos el event BlockDestroyedAction cuando es destruido.
            ArkanoidEvent.OnBlockDestroyedEvent?.Invoke(_id);
        }
        else
        {
            _renderer.sprite = GetBlockSprite(_type, _color, _currentHits);
        }
    }
    
    static Sprite GetBlockSprite(BlockType type, BlockColor color, int state)
    {
        string path = string.Empty;
        if (type == BlockType.Big)
        {
            path = string.Format(BLOCK_BIG_PATH, color, state);
        }

        if (string.IsNullOrEmpty(path)) // Si sí se llenó el path
        {
            return null;
        }

        // Cargamos y retornamos el sprite, usando la clase Resources de Unity
        return Resources.Load<Sprite>(path);
    }
}

