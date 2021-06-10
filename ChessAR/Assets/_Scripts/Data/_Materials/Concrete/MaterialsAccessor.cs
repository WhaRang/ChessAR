using System.Collections.Generic;
using UnityEngine;

public class MaterialsAccessor : MonoBehaviour, IMaterialAccessor
{
    [SerializeField] private List<MaterialElement> materialsList = null;
    [SerializeField] private Mesh whiteQueen = null;
    [SerializeField] private Mesh blackQueen = null;

    public Material GetByIndex(MaterialIndex index)
    {
        return materialsList.Find(i => i.Index == index).Material;
    }

    public Mesh GetWhiteQueen()
    {
        return whiteQueen;
    }

    public Mesh GetBlackQueen()
    {
        return blackQueen;
    }
}
