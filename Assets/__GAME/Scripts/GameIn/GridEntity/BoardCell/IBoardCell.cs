using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface IBoardCell:ICellBase
{
    BoardGenerator BoardGenerator { get; set; }
    bool IsHintCell { get; set; }
    bool isFirstLevelCondition { get; set; }
    List<WordTableCell> lsWhichWordToConnect { get; set; }
    HintController myHintLines { get; set; }
    int CorrectWord { get; set; }
    Lines mLine { get; set; }
    Tween _lastTween { get; set; }
    bool isTween { get; set; }

    bool HasLineOnMe();
    bool IsNeighbourOf(BoardCell bc);
    void SetWordCellText(int bcValue, BoardCell bc);
    void ResetWordCellText();
    void ParticlePlay(Color c);
    void CompleteLastTween();
    void AnimateShake(Action onComplate = null);
    Task AnimateEmptyCell();
    void AnimateBorn(Action onComplate = null);
}

