using UnityEngine;

public class CamerasAccessor : MonoBehaviour, ICamerasAccessor
{
    [SerializeField] Camera defaultCamera;
    [SerializeField] Camera arCamera;

    public Camera DefaultCamera => defaultCamera;

    public Camera ARcamera => arCamera;
}
