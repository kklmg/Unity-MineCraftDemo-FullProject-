using UnityEngine;

public static class Direction
{
    public const byte UP = 0;
    public const byte DOWN = 1;
    public const byte LEFT = 2;
    public const byte RIGHT = 3;
    public const byte FORWARD = 4;
    public const byte BACKWARD = 5;

    public static byte Opposite(byte dir)
    {
        Debug.Assert(dir < 6);
        return ((dir & 1) == 0) ? ++dir : --dir;
    }

    public static Vector3 DirToVector(byte dir)
    {
        Debug.Assert(dir < 6);
        switch (dir)
        {
            case UP: return Vector3.up;
            case DOWN: return Vector3.down;
            case LEFT: return Vector3.left;
            case RIGHT: return Vector3.right;
            case FORWARD: return new Vector3(0, 0, 1);
            case BACKWARD: return new Vector3(0, 0, -1);
            default: return Vector3.zero;
        }

    }
    public static Vector3Int DirToVectorInt(byte dir)
    {
        Debug.Assert(dir < 6);
        switch (dir)
        {
            case UP: return Vector3Int.up;
            case DOWN: return Vector3Int.down;
            case LEFT: return Vector3Int.left;
            case RIGHT: return Vector3Int.right;
            case FORWARD: return new Vector3Int(0, 0, 1);
            case BACKWARD: return new Vector3Int(0, 0, -1);
            default: return Vector3Int.zero;
        }
    }

    public static byte VectorToDir(Vector3 vt3)
    {
        if (vt3 == Vector3.up)
            return Direction.UP;
        else
        if (vt3 == Vector3.down)
            return Direction.DOWN;
        else
        if (vt3 == Vector3.left)
            return Direction.LEFT;
        else
        if (vt3 == Vector3.right)
            return Direction.RIGHT;
        else
        if (vt3 == Vector3.forward)
            return Direction.FORWARD;
        else
        if (vt3 == Vector3.back)
            return Direction.BACKWARD;
        else
        {
            Debug.LogWarning("invalid Dir!!");
            return byte.MaxValue;
        }
    }
}