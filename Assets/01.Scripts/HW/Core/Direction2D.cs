using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Direction2D
{
    public static List<Vector2> eightDirectionList = new List<Vector2>()
    {
        new Vector2(0, 1),
        new Vector2(0, -1),
        new Vector2(-1, 0),
        new Vector2(1, 0),
        new Vector2(1, 1),
        new Vector2(-1, 1),
        new Vector2(-1, -1),
        new Vector2(1, -1),
    };

    public static List<Vector2> nineDirectionList = new List<Vector2>()
    {
        new Vector2(0, 0),
        new Vector2(0, 1),
        new Vector2(0, -1),
        new Vector2(-1, 0),
        new Vector2(1, 0),
        new Vector2(1, 1),
        new Vector2(-1, 1),
        new Vector2(-1, -1),
        new Vector2(1, -1),
    };

    public static List<Vector2> fourDirectionList = new List<Vector2>()
    {
        new Vector2(0, 1),
        new Vector2(0, -1),
        new Vector2(-1, 0),
        new Vector2(1, 0)
    };

    public static List<Vector2> fiveDirectionList = new List<Vector2>()
    {
        new Vector2(0, 0),
        new Vector2(0, 1),
        new Vector2(0, -1),
        new Vector2(-1, 0),
        new Vector2(1, 0)
    };
}
