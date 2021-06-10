using UnityEngine;
using System;

[Serializable]
public class MaterialElement
{
    public MaterialIndex Index => index;
    public Material Material => material;

    [SerializeField] private MaterialIndex index;
    [SerializeField] private Material material;
}
