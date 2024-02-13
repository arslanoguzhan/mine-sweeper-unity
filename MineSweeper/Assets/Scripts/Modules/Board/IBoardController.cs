using System.Collections.Generic;

interface IBoardController
{
    void CreateBoard();

    List<Box> GetNearBoxes(int r, int c);
}