using Zenject;
using UnityEngine;

public class SelectionManager : ISelectionManager
{
    private readonly ISignalSystem signalSystem;
    private readonly IRayProvider rayProvider;
    private readonly ISelector selector;
    private readonly ISelectionResponse selectionResponse;

    private Transform currentSelection;
    private Transform selectedFigure;
    private bool isFigureSelected = false;


    public SelectionManager(
        ISignalSystem _signalSystem,
        IRayProvider _rayProvider,
        ISelector _selector,
        ISelectionResponse _selectionResponse)
    {
        signalSystem = _signalSystem;
        rayProvider = _rayProvider;
        selector = _selector;
        selectionResponse = _selectionResponse;
    }


    public void SelectionUpdate()
    {
        if (currentSelection != null && !isFigureSelected)
        {
            selectionResponse.OnDeselect(currentSelection);
        }

        selector.Check(rayProvider.CreateRay());
        currentSelection = selector.GetSelection();

        if (currentSelection != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                SelectionPressed();
            }
            else if (!isFigureSelected)
            {
                selectionResponse.OnPoint(currentSelection);
            }
        }
        else
        {
            NothingWasSelected();
        }
    }


    private void NothingWasSelected()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (selectedFigure != null)
            {
                selectionResponse.OnDeselect(selectedFigure);
                if (selectedFigure.TryGetComponent(out PositionData _figureData))
                {
                    signalSystem.FireSignal(new FigureDeselectedSignal()
                    {
                        figureTransform = selectedFigure,
                        figureData = _figureData
                    });
                }
            }
            isFigureSelected = false;
            selectedFigure = null;

            signalSystem.FireSignal(new NothingWasSelectedSignal());
        }
    }


    private void SelectionPressed()
    {
        if (selectedFigure != null)
        {
            DeselectOldFigure();
        }

        if (currentSelection.CompareTag(Tags.BoardSquare))
        {
            BoardSquareWasSelected();
            return;
        }

        isFigureSelected = true;

        selectedFigure = currentSelection;
        selectionResponse.OnSelect(currentSelection);

        if (selectedFigure.TryGetComponent(out PositionData _figureData))
        {
            signalSystem.FireSignal(new FigureSelectedSignal()
            {
                figureTransform = selectedFigure,
                figureData = _figureData
            });

            return;
        }
    }


    private void BoardSquareWasSelected()
    {
        isFigureSelected = false;
        signalSystem.FireSignal(new SquareSelectedSignal()
        {
            squareTransform = currentSelection
        });
    }


    private void DeselectOldFigure()
    {
        selectionResponse.OnDeselect(selectedFigure);
        if (selectedFigure.TryGetComponent(out PositionData _figureData_))
        {
            signalSystem.FireSignal(new FigureDeselectedSignal()
            {
                figureTransform = selectedFigure,
                figureData = _figureData_
            });
        }
    }
}
