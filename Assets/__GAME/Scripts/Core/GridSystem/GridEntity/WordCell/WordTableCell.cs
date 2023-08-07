using DG.Tweening;
using MyProject.Settings;
using System;
using UnityEngine;
using UnityEngine.UI.Extensions;

public enum WordParticleType
{
    connection,
    disconnection,
    None

}
public class WordTableCell : Cell,IWordCell
{
    [SerializeField] private ParticleSystem[] psParticle;
    [SerializeField] public RectTransform rectTransform;

    public WordTableGenerator WordTableGenerator { get; set; }
    public Tween _lastTween { get; set; }= null;
    public int DefaultValue { get; set; }= 0;
    public bool IsCorrect { get; set; }=false;
    public bool IsFilled { get; set; }= false;
    public WordParticleType wordParicleType { get; set; } = WordParticleType.None;
    public RowController myRow { get; set; }

    public override void SetText(int value)
    {

        char mChar = Convert.ToChar(value);

        if (!IsEmpty())
        {
            txt.SetText(mChar.ToString());
            if (IsNumber())
            {

                WordTableGenerator.GetWordTable().SetObjColors(this);
            }
            else if (IsWord())
            {

                WordTableGenerator.GetWordTable().SetConnectionColors(this);
            }

        }
        else
        {
            txt.SetText("");

        }

        transform.localScale = Vector3.one * Settings.BoardCell.unselectedScale;
    }
    public override void SetAvailable(bool isAvailable)
    {
        img.enabled = isAvailable;
        txt.enabled = isAvailable;
        txtPlaceholder.enabled = isAvailable;
    }
    public override void SetFixed()
    {

        txtPlaceholder.enabled = false;
        txt.enabled = true;
    }

    #region Particle
    public void ParticlePlay(WordParticleType type, Color c)
    {
        ParticleSystem.MainModule settings = psParticle[(int)type].main;
        settings.startColor = new ParticleSystem.MinMaxGradient(c);
        UIParticleSystem[] psUI = psParticle[(int)type].gameObject.GetComponentsInChildren<UIParticleSystem>();
        foreach (var item in psUI)
        {
            item.StartParticleEmission();
        }

    }
    #endregion

    #region Tween

    public void CompleteLastTween()
    {
        if (_lastTween != null) _lastTween.Complete(true);
    }

    public void KillLastTween()
    {
        if (_lastTween == null) return;
        _lastTween.Kill(true);
    }

    #endregion

    #region Animate
    public void Emplacement(int value, char msg, Color c)
    {
        UpdateText(msg);
        VALUE = value;
        KillLastTween();
        AnimateShake();

    }

    public void SetAnchroredPosWithY(float yPos)
    {
        Vector2 tempAnchoredPos = rectTransform.anchoredPosition;
        tempAnchoredPos.y += yPos;
        rectTransform.anchoredPosition = tempAnchoredPos;
    }

    public void AnimateShake()
    {
        CompleteLastTween();
        _lastTween = TweenHelper.ShakeRotation(transform);
    }
    #endregion

    #region Setter
    public void ResetWordCell()
    {
        char mChar = Convert.ToChar(DefaultValue);
        UpdateText(mChar);
        VALUE = DefaultValue;
        ParticlePlay(WordParticleType.disconnection, defaultImgColor);
        //UpdateColor(defaultImgColor);
        //txt.color = Color.black;
        IsCorrect = false;
        IsFilled = false;

    }
    public void UpdateText(char msg)
    {
        txt.SetText(msg.ToString());
        IsFilled = true;
    }
    public void UpdateColor(Color c)
    {
        img.color = c;
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
        WordTableGenerator = GameInManager.I.GameInController.GeneratorController.WordGenerator;
        transform.SetParent(WordTableGenerator.GetPoolParent());
        OnDeactivate();
    }

    #endregion

}
