using System.Collections.Generic;
using UnityEngine;

public class Level4 : ILevel
{
    //marco - anillo
    public List<Vector2Int> Layout()
    {
        List<Vector2Int> layout = new List<Vector2Int>();
        int width = 10;
        int height = 5;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    layout.Add(new Vector2Int(x, y));
                }
            }
        }

        return layout;
    }
}
