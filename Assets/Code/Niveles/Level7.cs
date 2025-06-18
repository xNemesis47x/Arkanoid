using System.Collections.Generic;
using UnityEngine;

public class Level7 : ILevel
{
    //zigzag

    public List<Vector2Int> Layout()
    {
        List<Vector2Int> layout = new List<Vector2Int>();

        for (int y = 0; y < 5; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                if ((x + y) % 2 == 0)
                    layout.Add(new Vector2Int(x, y));
            }
        }

        return layout;
    }
}
