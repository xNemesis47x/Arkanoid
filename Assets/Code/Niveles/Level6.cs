using System.Collections.Generic;
using UnityEngine;

public class Level6 : ILevel
{
    public List<Vector2Int> Layout()
    {
        //Cruz

        List<Vector2Int> layout = new List<Vector2Int>();

        int centerX = 5;
        int centerY = 2;

        for (int x = 0; x < 10; x++)
            layout.Add(new Vector2Int(x, centerY));

        for (int y = 0; y < 5; y++)
            layout.Add(new Vector2Int(centerX, y));

        return layout;
    }
}
