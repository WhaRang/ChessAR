using UnityEngine;

public class PositionData : MonoBehaviour
{
    [SerializeField] private PositionNumber firstNumber;
    [SerializeField] private PositionNumber secondNumber;


    public PositionData(int _firstNumber, int _secondNumber)
    {
        SetFirstIndex(_firstNumber);
        SetSecondIndex(_secondNumber);
    }


    public int GetFirstIndex()
    {
        return (int)firstNumber;
    }


    public int GetSecondIndex()
    {
        return (int)secondNumber;
    }


    public void SetFirstIndex(int index)
    {
        firstNumber = (PositionNumber)index;
    }


    public void SetSecondIndex(int index)
    {
        secondNumber = (PositionNumber)index;
    }


    public bool SameWith(PositionData dataToCompare)
    {
        return (GetFirstIndex() == dataToCompare.GetFirstIndex()
            && GetSecondIndex() == dataToCompare.GetSecondIndex());
    }
}
