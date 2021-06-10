using UnityEngine;

public class OutlineSelectionResponse : ISelectionResponse
{
    private const float POINT_WIDTH = 7.0f;
    private const float DEFAULT_WIDTH = 0.0f;
    private const float SELECTED_WIDTH = 6.0f;

    private readonly Color pointColor;
    private readonly Color defaultColor;
    private readonly Color selectedColor;

    public OutlineSelectionResponse()
    {
        pointColor = new Color(1.0f, 1.0f, 1.0f, 0.42f);
        defaultColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        selectedColor = new Color(0.31f, 0.64f, 0.27f, 1.0f);
    }

    public void OnPoint(Transform selection)
    {
        var outline = selection.GetComponent<Outline>();
        if (outline != null)
        {
            outline.OutlineColor = pointColor;
            outline.OutlineWidth = POINT_WIDTH;
        }
    }

    public void OnDeselect(Transform selection)
    {
        var outline = selection.GetComponent<Outline>();
        if (outline != null)
        {
            outline.OutlineColor = defaultColor;
            outline.OutlineWidth = DEFAULT_WIDTH;
        }
    }


    public void OnSelect(Transform selection)
    {
        var outline = selection.GetComponent<Outline>(); 
        if (outline != null)
        {
            outline.OutlineColor = selectedColor;
            outline.OutlineWidth = SELECTED_WIDTH;
        }
    }
}
