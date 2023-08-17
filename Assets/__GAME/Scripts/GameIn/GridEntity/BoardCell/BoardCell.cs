using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using MyProject.Settings;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using MyProject.Core.Manager;
using MyProject.Extentions;

[Serializable]
public class BoardCell : Cell, IBoardCell
{
    public BoardGenerator BoardGenerator { get; set; }
    public bool IsHintCell { get; set; }
    public bool isFirstLevelCondition { get; set; }
    public List<WordTableCell> lsWhichWordToConnect { get; set; } = new ();
    public int CorrectWord { get; set; }
    public Lines mLine { get; set; }
    public Tween _lastTween { get; set; }
    public bool isTween { get; set; }
    public HintController myHintLines { get; set; } = null;

    //Referances
    [SerializeField] private ParticleSystem psParticle;
    [SerializeField] private GameObject spinObject;
    [SerializeField] private ShineEffect shine;



    #region Predicate

    public bool HasLineOnMe()
    {
        return mLine != null;
    }

    public bool IsNeighbourOf(BoardCell bc)
    {
        if (i == bc.i)
        {
            if (Mathf.Abs(j - bc.j) == 1)
                return true;
        }
        else if (j == bc.j)
        {
            if (Mathf.Abs(i - bc.i) == 1)
                return true;
        }

        return false;
    }
    #endregion

    #region Setter

    public void SetWordCellText(int bcValue, BoardCell bc)
    {
        var tcs = new TaskCompletionSource<bool>();
        char mChar = Convert.ToChar(bcValue);
        if (lsWhichWordToConnect.Count > 0 && this.IsNumber())
        {

            SoundManager.I.PlayOneShot(Sounds.WordConnection, 0.5f);
            VibrationManeger.Haptic(MoreMountains.NiceVibrations.HapticTypes.MediumImpact);
            for (int i = 0; i < lsWhichWordToConnect.Count; i++)
            {
                WordTableCell wtc = lsWhichWordToConnect[i];
                GameInManager.I.GameInController.ConnectionController.CreateFlyingSpriteToGoal(bc, wtc, () =>
                {
                    wtc.Emplacement(bcValue, mChar, Color.white);

                });
            }
        }

    }

    public void ResetWordCellText()
    {
        lsWhichWordToConnect.ForEach(n => n.ResetWordCell());

        if (this.IsNumber())
        {
            if (mLine != null)
            {
                if (mLine.connectedCell != null)
                {
                    mLine.connectedCell = null;
                }
            }
        }
    }

    public override void SetText(int value)
    {
        char mChar = Convert.ToChar(value);

        if (!IsEmpty())
        {
            txt.SetText(mChar.ToString());
            if (char.IsNumber(mChar))
            {
                AnimateBorn();
                BoardGenerator.GetBoard().SetObjColors(this);
            }
            else
            {
                AnimateBorn();
                txt.color = BoardGenerator.GetBoard().GetEmptyTextColor();
                SetImageColor(BoardGenerator.GetBoard().GetEmptyImageColor().SetAlpha(1f));
            }
        }
        else
        {
            txt.SetText("");
            txt.color = BoardGenerator.GetBoard().GetEmptyTextColor();
            SetImageColor(BoardGenerator.GetBoard().GetEmptyImageColor().SetAlpha(1f));
        }

        transform.localScale = Vector3.one * Settings.BoardCell.unselectedScale;
    }

    public override void SetAvailable(bool isAvailable)
    {
       
        txt.enabled = isAvailable;
        txtPlaceholder.enabled = isAvailable;
    }

    public override void SetFixed()
    {
        txtPlaceholder.enabled = false;
        txt.enabled = true;
    }
    #endregion

    public void ParticlePlay(Color c)
    {
        ParticleSystem.MainModule settings = psParticle.main;
        settings.startColor = new ParticleSystem.MinMaxGradient(c);
        var psUI = psParticle.gameObject.GetComponent<UIParticleSystem>();
        psUI.StartParticleEmission();
    }

    public void CompleteLastTween()
    {
        if (_lastTween != null) _lastTween.Complete(true);
    }

    #region Animate

    public void AnimateShake(Action onComplate = null)
    {
        CompleteLastTween();
        isTween = true;
        _lastTween = TweenHelper.ShakeRotation(transform);
        _lastTween.OnComplete(() => onComplate?.Invoke());
    }

    public async Task AnimateEmptyCell()
    {
        Sequence s = DOTween.Sequence();
        Vector3 targetPos = transform.localPosition;
        targetPos.y = transform.position.y - (+1000f);
        img.enabled = false;
        Image spinObjImg = spinObject.GetComponent<Image>();
        spinObjImg.enabled = true;
        s.AppendInterval(.5f);
        s.Append(TweenHelper.LinearLocalMoveTo(transform, targetPos, null, .9f));
        s.Join(TweenHelper.Spin(spinObject.transform, null, 1f));
        s.Join(spinObjImg.DOFade(0f, 1.1f).From(1f));
        await Task.CompletedTask;
    }

    public virtual void AnimateBorn(Action onComplate = null)
    {
        //CompleteLastTween();
        //shine.gameObject.SetActive(true);
        //Sequence tween = DOTween.Sequence();
        //tween.Append(TweenHelper.PunchScale(transform, null, .1f, .3f));
        //tween.AppendCallback(() => shine.MoveEffectObject(() => shine.gameObject.SetActive(false)));
        //if (onComplate != null) tween.onComplete = () => { onComplate(); };
        //_lastTween = tween;
    }
    #endregion

    #region Pool Object Functions

    public override void OnDeactivate()
    {
        PoolHandler.I.EnqueObject(this);
        transform.SetParent(null);
        gameObject.SetActive(false);
    }

    public override void OnSpawn()
    {
        gameObject.SetActive(true);
        transform.localScale = Vector3.one * Settings.BoardCell.unselectedScale;
        SetAvailable(false);
    }

    public override void OnCreated()
    {
        OnDeactivate();
    }

    #endregion
}


