using MyProject.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class Cell : PoolObject, ICellBase
{
    [SerializeField] private int _value;
    public int VALUE
    {
        get
        {
            return _value;
        }
        set
        {
            _value = value;
            SetText(_value);
        }
    }

    public int i { get; set; }
    public int j { get; set; }
    public Image img;
    public Image lineImg;
    public TextMeshProUGUI txt;
    public TextMeshProUGUI txtPlaceholder;
    public Color defaultImgColor { get; set; }
    public Color defaultTextColor { get; set; }
    public bool IsActive() => img.enabled;
    public bool IsEmpty() => VALUE == 0;
    public bool IsNumber()
    {
        char mChar = Convert.ToChar(VALUE);
        return char.IsNumber(mChar);
    }
    public bool IsWord()
    {

        char mChar = Convert.ToChar(VALUE);

        if (char.IsNumber(mChar))
        {
            return false;
        }

        if (IsEmpty())
        {
            return false;
        }



        return true;
    }
    public abstract void SetText(int value);
    public abstract void SetAvailable(bool isAvailable);
    public abstract void SetFixed();

    public virtual void SetImageAlpha(float alpha)
    {
        var tempColor = img.color;
        tempColor.a = alpha;
        img.color = tempColor;
    }

    public virtual void SetTextAlpha(float alpha)
    {
        var tempColor = txt.color;
        tempColor.a = alpha;
        txt.color = tempColor;
    }

    public virtual void SetImageColor(Color c)
    {
        img.color = c;
    }

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
}
