using UnityEngine;

public interface IMaterialAccessor
{
    Material GetByIndex(MaterialIndex index);

    Mesh GetWhiteQueen();

    Mesh GetBlackQueen();
}
