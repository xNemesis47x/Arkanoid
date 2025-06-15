using System.Collections.Generic;
using UnityEngine;

public class Level2 : ILevel
{
    public List<Vector2Int> Layout()
    {
        List<Vector2Int> layout = new List<Vector2Int>();

        //martillo
        for (int i = 0; i < 5; i++)
        {
            layout.Add(new Vector2Int(5, i)); 
            layout.Add(new Vector2Int(i, 2)); 
            layout.Add(new Vector2Int(i, 3)); 
        }

        return layout;
    }
}
