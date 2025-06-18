using System.Collections.Generic;
using UnityEngine;

public class Level8 : ILevel
{
    public List<Vector2Int> Layout()
    {
        List<Vector2Int> layout = new List<Vector2Int>();

        // Ojos
        layout.Add(new Vector2Int(3, 0));
        layout.Add(new Vector2Int(6, 0));

        // Boca
        layout.Add(new Vector2Int(2, 2));
        layout.Add(new Vector2Int(7, 2));
        layout.Add(new Vector2Int(3, 3));
        layout.Add(new Vector2Int(6, 3));
        layout.Add(new Vector2Int(4, 4));
        layout.Add(new Vector2Int(5, 4));


        return layout;
    }
}
