using System.Collections.Generic;
using UnityEngine;

public class Level1 : ILevel
{
    public List<Vector2Int> Layout()
    {
        List<Vector2Int> layout = new List<Vector2Int>();

        int rows = 5;
        int startX, endX;

        for (int y = 0; y < rows; y++)
        {
            // A medida que subís, la fila tiene menos ladrillos centrados
            startX = y;
            endX = 10 - y;

            for (int x = startX; x < endX; x++)
            {
                layout.Add(new Vector2Int(x, y));
            }
        }

        return layout;
    }
}
