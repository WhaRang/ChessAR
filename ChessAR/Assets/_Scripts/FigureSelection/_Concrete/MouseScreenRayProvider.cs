using UnityEngine;

public class MouseScreenRayProvider : IRayProvider
{

    private readonly ICamerasAccessor camerasAccessor;
    private readonly PlayerSettingsSO playerSettings;


    public MouseScreenRayProvider(
        ICamerasAccessor _camerasAccessor,
        PlayerSettingsSO _playerSettings)
    {
        camerasAccessor = _camerasAccessor;
        playerSettings = _playerSettings;
    }


    public Ray CreateRay()
    {
        return Camera.main.ScreenPointToRay(Input.mousePosition);
    }
}
