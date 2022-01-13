using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Data/Level Data")]
//Acá almacenaremos todas esas variables que definen el comportamiento de la grid
//Hereda de ScriptableObject, que permite crear archivos que se guardan en disco
public class LevelData : ScriptableObject
{
    [Range(1, 10)]

    // Número de filas, Espacio entre filas y la Lista de filas
    public int RowCount = 7;
    public float rowSpacing = 0.1f;
    public List<GridRowData> Rows = new List<GridRowData>();
}

    [System.Serializable]

//Esta clase contendrá la información de los bloques en una fila: Cuantos bloques, Espaciado, Tipo y Color.
public class GridRowData
{
    [Range(1, 24)]
    public int BlockAmount = 7;
    public float blockSpacing = 0.1f;
    public BlockType BlockType = BlockType.Big;
    public BlockColor BlockColor = BlockColor.Green;
}
