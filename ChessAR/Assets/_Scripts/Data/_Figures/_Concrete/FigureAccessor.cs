using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FigureAccessor : MonoBehaviour, IFigureAccessor
{
    [SerializeField] private List<PositionData> figuresPosotionData;
    [SerializeField] private List<Collider> blackFiguresCollider;
    [SerializeField] private List<Collider> whiteFiguresCollider;


    public void DeleteFigure(PositionData figureToRemove)
    {
        figuresPosotionData.Remove(figureToRemove);
    }


    public PositionData GetFigureByIndexes(int firstIndex, int secondIndex)
    {
        return figuresPosotionData.Find(x =>
            (x.GetFirstIndex() == firstIndex && x.GetSecondIndex() == secondIndex));
    }


    public void SetAllBlackFiguresCollidersActive(bool isActive)
    {
        foreach (Collider collider in blackFiguresCollider)
        {
            if (collider != null)
            {
                collider.enabled = isActive;
            }
        }
    }


    public void SetAllWhiteFiguresCollidersActive(bool isActive)
    {
        foreach (Collider collider in whiteFiguresCollider)
        {
            if (collider != null)
            {
                collider.enabled = isActive;
            }
        }
    }

}
