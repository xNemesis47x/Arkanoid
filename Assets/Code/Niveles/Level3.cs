using System.Collections.Generic;
using UnityEngine;

public class Level3 : ILevel
{
    //escalera
    public List<Vector2Int> Layout()
    {
        List<Vector2Int> layout = new List<Vector2Int>();

        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j <= i; j++)
            {
                layout.Add(new Vector2Int(j, i));
            }
        }

        return layout;
    }
}

