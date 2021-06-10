using UnityEngine;

public class CameraStabilizationUsingCallbacks : MonoBehaviour
{
    private Vector3 _lastPosition;
    private Quaternion _lastRotation;

    private void OnEnable()
    {
        _lastPosition = transform.localPosition;
        _lastRotation = transform.localRotation;
    }

    private void LateUpdate()
    {
        transform.localPosition = Vector3.Lerp(_lastPosition, transform.localPosition, Time.deltaTime);
        transform.localRotation = Quaternion.Lerp(_lastRotation, transform.localRotation, Time.deltaTime);

        _lastPosition = transform.localPosition;
        _lastRotation = transform.localRotation;
    }
}
