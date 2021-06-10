using UnityEngine;

public interface ICamerasAccessor
{
    Camera DefaultCamera { get; }

    Camera ARcamera { get; }
}
