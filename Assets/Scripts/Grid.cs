using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{

    public static Vector2Int PointToGrid(Vector3 Point)
    {
        int col = Mathf.FloorToInt(4 + Point.x / 2);//小数点切り捨て
        int row = Mathf.FloorToInt(4 + Point.z / 2);
        return new Vector2Int(col, row);
    }

    public static Vector2Int Gridpoint(int col, int row)
    {
        return new Vector2Int(col, row);
    }

    public static Vector3 GridToPoint(Vector2Int gridPoint)
    {
        int x = -7 + Mathf.FloorToInt(gridPoint.x)*2;//小数点切り上げ
        int z = -7 + Mathf.FloorToInt(gridPoint.y)*2;
        return new Vector3(x, 0, z);
    }

    public static GameObject GridToPiece(Vector2Int gridPoint)
    {
        Vector3 movePlace_Point = GridToPoint(gridPoint);

        Vector3 rayDirection = movePlace_Point;
        GameObject cam = Camera.main.gameObject;
        Ray ray = new Ray(cam.transform.position, rayDirection - cam.transform.position);
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.blue, 1, false);
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit)) return null;
        if (!hit.collider.gameObject.CompareTag("WhitePiece") && !hit.collider.gameObject.CompareTag("BlackPiece")) return null;
        return hit.collider.gameObject;
    }

    public static Vector2Int PieceToGrid(GameObject piece)
    {
        Vector2Int pieceGrid = PointToGrid(piece.transform.position);
        return pieceGrid;
    }
}

