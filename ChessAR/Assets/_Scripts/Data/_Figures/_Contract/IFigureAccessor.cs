public interface IFigureAccessor
{
    PositionData GetFigureByIndexes(int firstIndex, int secondIndex);

    void DeleteFigure(PositionData positionData);

    void SetAllWhiteFiguresCollidersActive(bool isActive);

    void SetAllBlackFiguresCollidersActive(bool isActive);
}
