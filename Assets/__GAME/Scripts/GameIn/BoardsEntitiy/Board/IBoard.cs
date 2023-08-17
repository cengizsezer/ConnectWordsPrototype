using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IBoard : IGenericBase<BoardCell>
{

    List<BoardCell> activeCells { get; set; }
    List<BoardCell> lsAllCells { get; set; }
    List<BoardCell> lsNumberCell { get; set; }
    List<BoardCell> lsWordCell { get; set; }
    List<HintController> lsHintCells { get; set; }
    Task MixBoard(Dictionary<int, int> asciiPairs);
    void SetCorrectValues(List<BoardCell> lsNumberCell);

    int GetCorrectValue(int id);

}

