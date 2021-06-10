using System.Collections;
using UnityEngine;


public class TransformAnimator : ITransformAnimator
{

    private readonly AsyncProcessor asyncProcessor;
    private readonly PlayerSettingsSO playerSettings;


    public TransformAnimator(
        AsyncProcessor _asyncProcessor,
        PlayerSettingsSO _playerSettings)
    {
        asyncProcessor = _asyncProcessor;
        playerSettings = _playerSettings;
    }


    public void MoveObjToAnotherObj(Transform objToMove, Transform aimObj, float seconds)
    {
        if (!playerSettings.IsAnimationEnabled)
        {
            objToMove.position = aimObj.position;
        }

        asyncProcessor.StartCoroutine(MoveObjToAnotherObjCoroutine(objToMove, aimObj, seconds));
    }


    public void MoveObjToPoint(Transform obj, Vector3 newPos, float seconds)
    {
        if (!playerSettings.IsAnimationEnabled)
        {
            obj.position = newPos;
        }

        asyncProcessor.StartCoroutine(MoveObjToPointCoroutine(obj, newPos, seconds));
    }


    public void RotateObjToPoint(Transform obj, Vector3 newEulerRotation, float seconds)
    {
        if (!playerSettings.IsAnimationEnabled)
        {
            obj.rotation = Quaternion.Euler(newEulerRotation);
        }

        asyncProcessor.StartCoroutine(RotateObjToPointCoroutine(obj, newEulerRotation, seconds));
    }


    public IEnumerator MoveObjToAnotherObjCoroutine(Transform objToMove, Transform aimObj, float seconds)
    {
        asyncProcessor.StartCoroutine(SmoothMoveToObj(objToMove, aimObj, seconds));
        yield return new WaitForSeconds(seconds);
        objToMove.position = aimObj.position;
    }


    private IEnumerator MoveObjToPointCoroutine(Transform obj, Vector3 newPos, float seconds)
    {
        asyncProcessor.StartCoroutine(SmoothMoveToPoint(obj, newPos, seconds));
        yield return new WaitForSeconds(seconds);
        obj.position = newPos;
    }


    private IEnumerator RotateObjToPointCoroutine(Transform obj, Vector3 newEulerRotation, float seconds)
    {
        asyncProcessor.StartCoroutine(SmoothRotationToPoint(obj, newEulerRotation, seconds));
        yield return new WaitForSeconds(seconds);
        obj.rotation = Quaternion.Euler(newEulerRotation);
    }


    private IEnumerator SmoothRotationToPoint(Transform obj, Vector3 newEulerRotation, float seconds)
    {
        Quaternion startRotation = obj.rotation;
        Quaternion newRotation = Quaternion.Euler(newEulerRotation);

        float t = 0f;
        while (t <= 1f)
        {
            if (obj == null)
                break;

            t += Time.deltaTime / seconds;
            obj.rotation = Quaternion.Slerp(startRotation, newRotation, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
    }


    private IEnumerator SmoothMoveToPoint(Transform obj, Vector3 newPos, float seconds)
    {
        Vector3 startPos = obj.position;

        float t = 0f;
        while (t <= 1f)
        {
            if (obj == null)
                break;

            t += Time.deltaTime / seconds;
            obj.position = Vector3.Lerp(startPos, newPos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
    }


    private IEnumerator SmoothMoveToObj(Transform objToMove, Transform aimObj, float seconds)
    {
        Vector3 startPos = objToMove.position;

        float t = 0f;
        while (t <= 1f)
        {
            if (objToMove == null || aimObj == null)
                break;

            t += Time.deltaTime / seconds;
            objToMove.position = Vector3.Lerp(startPos, aimObj.position, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
    }
}
