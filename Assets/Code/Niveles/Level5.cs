using System.Collections.Generic;
using UnityEngine;

public class Level5 : ILevel
{
    public List<Vector2Int> Layout()
    {
        List<Vector2Int> layout = new List<Vector2Int>();

        // Letra M (columnas 0 a 2)
        layout.Add(new Vector2Int(0, 0));
        layout.Add(new Vector2Int(0, 1));
        layout.Add(new Vector2Int(0, 2));
        layout.Add(new Vector2Int(0, 3));
        layout.Add(new Vector2Int(0, 4));

        layout.Add(new Vector2Int(1, 1));
        layout.Add(new Vector2Int(2, 0));
        layout.Add(new Vector2Int(2, 1));
        layout.Add(new Vector2Int(2, 2));
        layout.Add(new Vector2Int(2, 3));
        layout.Add(new Vector2Int(2, 4));

        // Letra V (columnas 4 a 5)
        layout.Add(new Vector2Int(4, 0));
        layout.Add(new Vector2Int(4, 1));
        layout.Add(new Vector2Int(4, 2));
        layout.Add(new Vector2Int(5, 3));
        layout.Add(new Vector2Int(5, 4));
        layout.Add(new Vector2Int(6, 2));
        layout.Add(new Vector2Int(6, 1));
        layout.Add(new Vector2Int(6, 0));

        // Letra P (columnas 8 a 9)
        layout.Add(new Vector2Int(8, 0));
        layout.Add(new Vector2Int(8, 1));
        layout.Add(new Vector2Int(8, 2));
        layout.Add(new Vector2Int(8, 3));
        layout.Add(new Vector2Int(8, 4));

        layout.Add(new Vector2Int(9, 2));
        layout.Add(new Vector2Int(9, 1));
        layout.Add(new Vector2Int(9, 0));


        return layout;
    }
}
