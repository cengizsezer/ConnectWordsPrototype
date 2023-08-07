using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBarHandler : MonoBehaviour
{
    public List<Image> lsFirstLines = new();
    public List<Image> lsSecondLines = new();
    string firstLineColor = "5DDDBE";
    string secondLineColor = "3398C5";

    public bool IsComplate = false;
    int repeatCount = 0;
    int maxRepeatCount = 2;
    public float waitTime;
    [SerializeField] CanvasGroup cg_LoadingLines = new();
   
    private IEnumerator Start()
    {
        cg_LoadingLines.DOFade(1f, .3f);
        AllResetColor();
        yield return StartCoroutine( CalculateLineImageScale(true));
    }

    public void AllResetColor()
    {
        var c = Color.white;
        for (int i = 0; i < lsFirstLines.Count; i++)
        {
            lsFirstLines[i].color = c;
            lsFirstLines[i].transform.parent.GetComponent<Image>().color = c;
        }

        for (int i = 0; i < lsSecondLines.Count; i++)
        {
            lsSecondLines[i].color = c;
            lsSecondLines[i].transform.parent.GetComponent<Image>().color = c;
        }

        transform.GetChild(3).GetComponent<Image>().color = c;
        transform.GetChild(5).GetComponent<Image>().color = c;
    }
    public IEnumerator CalculateLineImageScale(bool IsRepeat)
    {
        
        SetFirstLines();
        yield return new WaitForSeconds(waitTime);
        FirstReset();
        SetSecondLines();
        yield return new WaitForSeconds(waitTime);
        SecondReset();

        if (IsRepeat  &&  repeatCount < maxRepeatCount)
        {
            repeatCount += 1;
            yield return StartCoroutine(CalculateLineImageScale(IsRepeat));
        }
        else
        {
            cg_LoadingLines.DOFade(0f, .3f).OnComplete(() =>
            {
                LoadingPage.I.LoadingComplate = true;
                IsComplate = true;
            });
        }
        
    }


    Color ToColorFromHex(string hexademical)
    {
        string s = "#" + hexademical;
        Color newCol = Color.white;
        if (ColorUtility.TryParseHtmlString(s, out newCol))
        {
            return newCol;
        }

        return newCol;
    }


    void SetFirstLines()
    {
        lsFirstLines[0].rectTransform.offsetMax = new Vector2(10f, 0f);
        lsFirstLines[1].rectTransform.offsetMax = new Vector2(10f, 0f);
        lsFirstLines[2].rectTransform.offsetMin = new Vector2(0f, -10f);

        var c = ToColorFromHex(firstLineColor);

        for (int i = 0; i < lsFirstLines.Count; i++)
        {
            lsFirstLines[i].color = c;
            lsFirstLines[i].transform.parent.GetComponent<Image>().color = c;
        }


        transform.GetChild(5).GetComponent<Image>().color = c;
    }


    void SetSecondLines()
    {
        lsSecondLines[0].rectTransform.offsetMax = new Vector2(0f, 10f);
        lsSecondLines[1].rectTransform.offsetMin = new Vector2(-10f, 0f);
        lsSecondLines[2].rectTransform.offsetMin = new Vector2(-10f, 0f);

        var c = ToColorFromHex(secondLineColor);

        for (int i = 0; i < lsSecondLines.Count; i++)
        {
            lsSecondLines[i].color = c;
            lsSecondLines[i].transform.parent.GetComponent<Image>().color = c;
        }

        transform.GetChild(3).GetComponent<Image>().color = c;
    }

    public void FirstReset()
    {
        lsFirstLines[0].rectTransform.offsetMax = new Vector2(0f, 0f);
        lsFirstLines[1].rectTransform.offsetMax = new Vector2(0f, 0f);
        lsFirstLines[2].rectTransform.offsetMin = new Vector2(0f, 0f);

        var c = Color.white;

        for (int i = 0; i < lsFirstLines.Count; i++)
        {
            lsFirstLines[i].color = c;
            lsFirstLines[i].transform.parent.GetComponent<Image>().color = c;
        }

        transform.GetChild(5).GetComponent<Image>().color = c;
    }


    public void SecondReset()
    {
        lsSecondLines[0].rectTransform.offsetMax = new Vector2(0f, 0f);
        lsSecondLines[1].rectTransform.offsetMin = new Vector2(0f, 0f);
        lsSecondLines[2].rectTransform.offsetMin = new Vector2(0f, 0f);

        var c = Color.white;

        for (int i = 0; i < lsSecondLines.Count; i++)
        {
            lsSecondLines[i].color = c;
            lsSecondLines[i].transform.parent.GetComponent<Image>().color = c;
        }

        transform.GetChild(3).GetComponent<Image>().color = c;
    }
}
