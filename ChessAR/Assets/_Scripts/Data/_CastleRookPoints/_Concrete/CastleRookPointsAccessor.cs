using System.Collections.Generic;
using UnityEngine;


public class CastleRookPointsAccessor : MonoBehaviour, ICastleRookPointsAccessor
{
    [SerializeField] private List<Transform> points;


    public Transform GetPointByPosition(Vector3 position)
    {
        int closestIndex = 0;
        float closestDiff = (position - points[0].position).sqrMagnitude;

        for (int i = 1; i < points.Count; i++)
        {
            if ((position - points[i].position).sqrMagnitude < closestDiff)
            {
                closestDiff = (position - points[i].position).sqrMagnitude;
                closestIndex = i;
            }
        }

        return points[closestIndex];
    }
}
