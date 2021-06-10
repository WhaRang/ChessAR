using UnityEngine;

public class RayCastBasedTagSelector : ISelector
{
    private Transform _selection;

    public void Check(Ray ray)
    {
        _selection = null;

        if (!Physics.Raycast(ray, out var hit)) {
            return; 
        }

        var selection = hit.transform;

        if (selection.CompareTag(Tags.SelectableFigure) || selection.CompareTag(Tags.BoardSquare))
        {
            _selection = selection;
        }
    }

    public Transform GetSelection()
    {
        return _selection;
    }
}
