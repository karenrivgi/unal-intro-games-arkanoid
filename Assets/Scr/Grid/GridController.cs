using UnityEngine;
using System.Collections.Generic;

public class GridController : MonoBehaviour
{
    [SerializeField]
    private Vector2 _offset = new Vector2(-5.45f, 4); //Donde empieza la cuadricula de bloques (esq sup-izq)
    
    private LevelData _currentLevelData; // Almacena la informacion del nivel actual
    
    //Guardar los bloques según su id
    private Dictionary<int, BlockTile> _blockTiles = new Dictionary<int, BlockTile>();

    // Devuelve la cantidad de bloques que quedn activos
    public int GetBlocksActive()
    {
        int totalActiveBlocks = 0;
        foreach (BlockTile block in _blockTiles.Values)
        {
            if (block.gameObject.activeSelf)
            {
                totalActiveBlocks++;
            }
        }

        return totalActiveBlocks;
    }
    
    // Actualiza los datos del nivel, limpia la grid actual y crea la grid en base a la nueva información
    public void BuildGrid(LevelData levelData)
    {
        _currentLevelData = levelData;
        ClearGrid();
        BuildGrid();
    }
    
    
    private void BuildGrid()
    {
        int id = 0;
        
        //Datos del nivel actual
        int rowCount = _currentLevelData.RowCount;
        float verticalSpacing = _currentLevelData.rowSpacing;

         // Iteramos sobre el número de filas y luego sobre la cantidad de bloques en cada fila

        for (int j = 0; j < rowCount; j++)
        {
            //Obtiene la fila actual
            GridRowData rowData = _currentLevelData.Rows[j];
            
            int blockCount = rowData.BlockAmount;
            float horizontalSpacing = rowData.blockSpacing;

            Vector2 blockSize = GetBlockSize(rowData.BlockType);
            BlockTile blockTilePerfab = Resources.Load<BlockTile>(GetBlockPath(rowData.BlockType));

            BlockColor blockColor = rowData.BlockColor;

            if (blockTilePerfab == null)
            {
                return;
            }
            
            for (int i = 0; i < blockCount; i++)
            {
                BlockTile blockTile = Instantiate<BlockTile>(blockTilePerfab, transform);
                // Punto inicial + mitad del tamaño del bloque (centro) + tamaño de todos los bloques anteriores + el espaciado entre ellos
                float x = _offset.x + blockSize.x/2 + (blockSize.x + horizontalSpacing) * i;
                // Punto inicial + tamaño de todas las filas anteriores + el espaciado entre ellas
                float y = _offset.y - (blockSize.y + verticalSpacing) * j;
                blockTile.transform.position = new Vector3(x, y, 0);
                
                blockTile.SetData(id, blockColor);
                blockTile.Init();
                
                _blockTiles.Add(id, blockTile);
                id++;
            }
        }
    }

    private void ClearGrid()
    {
        // Recorre todos los hijos de la Grid y los destruye
        int totalChildren = transform.childCount;
        for (int i = totalChildren - 1; i >= 0; i--)
        {
            if (Application.isPlaying)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
            else
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }
        
        _blockTiles.Clear();
    }

    public BlockTile GetBlockBy(int id)
    {
        if (_blockTiles.TryGetValue(id, out BlockTile block))
        {
            return block;
        }

        return null;
    }
    
    private Vector2 GetBlockSize(BlockType type)
    {
        if (type == BlockType.Big)
        {
            return new Vector2(1.5f, 0.5f);

        }
        // else if(type == BlockType.Small)
        // {
        //     return new Vector2();
        // }

        return Vector2.zero;
    }

    private string GetBlockPath(BlockType type)
    {
        if (type == BlockType.Big)
        {
            return "Prefabs/BigBlockTile";
        }
        // else if (type == BlockType.Small)
        // {
        //     return "Prefabs/SmallBlockTile";
        // }

        return string.Empty;
    }
}
