using UnityEngine;

public interface ITransformAnimator
{
    public void MoveObjToAnotherObj(Transform objToMove, Transform aimObj, float seconds);

    public void MoveObjToPoint(Transform objTransfrom, Vector3 newPos, float seconds);

    public void RotateObjToPoint(Transform objTransfrom, Vector3 newEulerRotation, float seconds);
}
