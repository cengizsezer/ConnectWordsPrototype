using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface IWordCell:ICellBase
{
    WordTableGenerator WordTableGenerator { get; set; }
    Tween _lastTween { get; set; }
    int DefaultValue { get; set; }
    bool IsCorrect { get; set; }
    bool IsFilled { get; set; }
    WordParticleType wordParicleType { get; set; }
    RowController myRow { get; set; }

    void ParticlePlay(WordParticleType type, Color c);
    void CompleteLastTween();
    void KillLastTween();
    void Emplacement(int value, char msg, Color c);
    void SetAnchroredPosWithY(float yPos);
    void AnimateShake();
    void ResetWordCell();
    void UpdateText(char msg);
    void UpdateColor(Color c);


}
