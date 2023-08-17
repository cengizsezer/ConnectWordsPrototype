using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using MyProject.Core.Manager;
public class TutorialManager : Singleton<TutorialManager>
{
    public int Index = 0;
    public int buttonIndex = 0;
    public GameObject SwipeElement;

    public List<GameObject> lsBG = new List<GameObject>();
    [Space]
    [SerializeField] GameObject[] arrClosedUIElement;
    [Space]
    [SerializeField] GameObject[] arrInfoParents;
    [Space]
    [SerializeField] Image BGObjectPrefab;
    [Space]
    [SerializeField] FingerObject FingerObjectPrefab;
    [Space]
    [SerializeField] Button OkeyButton;
    public FingerObject ActiveFinger;
    public Vector3[] path;
    Tween activeTween = null;
    public bool IsTutorial = false;
  
    public  void Start()
    {
       
        OkeyButton.onClick.AddListener(ONOnClick);
        
    }

    public (float, float) GetSize(RectTransform rectTransform)
    {
        float sizeX = rectTransform.rect.width / 32;
        float sizeY = rectTransform.rect.height / 32;

        return (sizeX, sizeY);

    }

    public void InitialFingerAnimation(Range range,out Tween t)
    {
        Vector3[] arrPrev = new Vector3[2];
        arrPrev = path[range];
       
        var obj=CreateFinger(arrPrev[0],GameInUIManager.I.initialCanvas.transform);
        ActiveFinger = obj.GetComponent<FingerObject>();
        activeTween =StartHandAnimation(obj, arrPrev[0], arrPrev);
        t = activeTween;

    }


    public void AddBackGroundList(List<BoardCell> lsNumberCells,List<BoardCell> lsWordCells)
    {
        path = new Vector3[lsNumberCells.Count+ lsWordCells.Count];


        if (LevelManager.IsFirstLevel())
        {
            int idx = 0;

            for (int i = 0; i < lsNumberCells.Count; i++)
            {
                path[i + idx] = lsNumberCells[i].transform.position;

                idx++;
                Debug.Log(idx);

                path[i + idx] = lsWordCells[i].transform.position;
            }

        }
    }


    public void KillTweens()
    {
        DOTween.Kill(TweenIDs);
    }

    public  string[] TweenIDs;
    public Tween StartHandAnimation(CanvasGroup finger,Vector3 firstElement,Vector3[] path, float fadeInOutTime = .2f, float movementTime = .3f, bool repeat = true )
    {
       
       

         var guid1= new Guid().ToString();
        var guid2 = new Guid().ToString();
        TweenIDs = new string[] { guid1 ,guid2};
        finger.transform.position = Vector3.zero;
        finger.transform.position = firstElement;

        finger.transform.localScale = Vector3.one * 1.35f;

        finger.transform.DOScale(1f, fadeInOutTime);
        Tween t = finger.DOFade(1f, fadeInOutTime).OnComplete(() =>
        {
            finger.transform.DOPath(path, path.Length * movementTime)
            .OnUpdate(() =>
            {
                if (finger == null)
                {
                    KillTweens();
                }
            }).OnComplete(() =>
            {
                if (finger != null)
                {
                    finger.transform.DOScale(1.35f, fadeInOutTime).OnUpdate(() => 
                    {
                        if (finger == null)
                        {
                            KillTweens();
                        }
                    });

                    finger.DOFade(0f, fadeInOutTime).OnUpdate(() => 
                    {
                        if (finger == null)
                        {
                            KillTweens();
                        }
                    }).OnComplete(() =>
                    {
                        if (repeat && finger != null)
                        {
                            StartHandAnimation(finger, firstElement, path, fadeInOutTime, movementTime, repeat);
                        }
                    });
                }

            }).SetId(guid2);

        }).SetId(guid1);


        return t;
    }


    public void FingerTapAnimation(CanvasGroup finger,Vector3 from,float movementTime = .2f, bool repeat = true)
    {
        finger.transform.position = Vector3.zero;
        finger.transform.position = from;

        finger.transform.localScale = Vector3.one * 1.5f;
        finger.DOFade(1f, movementTime);
        finger.transform.DOScale(1f, movementTime).OnComplete(()=>
        {
            finger.transform.DOScale(1.5f,movementTime).OnComplete(()=> {

                if (repeat)
                {
                    FingerTapAnimation(finger, from , movementTime, repeat);
                }

            });
        }).SetId(Guid.NewGuid().ToString());

        
    }


    public void SecondFingerCondition()
    {

        if(ActiveFinger!=null)
        {
            
            KillTweens();
            ActiveFinger.gameObject.SetActive(false);
            //Destroy(ActiveFinger.gameObject);
            ActiveFinger = null;
        }

       


        Index += 1;

        if (Index >= 2) return;

        new DelayAction(()=>
        {
            
            Range range = new Range();
            range = 2..4;
            InitialFingerAnimation(range, out Tween t);
            activeTween = t;

        },1f).Execute(this);
    }

    public CanvasGroup CreateFinger(Vector3 _rect,Transform parent=null)
    {
        CanvasGroup bgObj = Instantiate(FingerObjectPrefab.m_CanvasGroup, parent);
        bgObj.transform.position = _rect;
        return bgObj;
    }

    public GameObject CreateBGObject(RectTransform _rect,Transform parent)
    {
        Image bgObj = Instantiate(BGObjectPrefab, parent);
        bgObj.SetNativeSize();
        float scaleX = GetSize(_rect).Item1;
        float scaleY = GetSize(_rect).Item2;
        bgObj.transform.localScale = new Vector2(scaleX, scaleY);
        lsBG.Add(bgObj.gameObject);
        bgObj.transform.position = _rect.transform.position;
        return bgObj.gameObject;
    }


    public void ONOnClick()
    {
        Range range = new Range();

        if (buttonIndex < 1)
        {
            buttonIndex += 1;
            lsBG[0].SetActive(false);
            lsBG[1].gameObject.SetActive(true);

            arrInfoParents[0].SetActive(false);
            arrInfoParents[1].SetActive(true);

            return;
        }

        buttonIndex += 1;
        lsBG.ForEach(n => n.gameObject.SetActive(false));

        foreach (var item in arrInfoParents)
        {
            item.gameObject.SetActive(false);
        }

        OkeyButton.gameObject.SetActive(false);
        IsTutorial = false;
        
        range = 0..2;
        InitialFingerAnimation(range,out Tween t);
    }

    private void OnDestroy()
    {
        OkeyButton.onClick.RemoveAllListeners();
    }


    public void UIElementActiveted(bool on)
    {

        foreach (var item in arrClosedUIElement)
        {
            item.SetActive(on);

        }

        //Button
        OkeyButton.gameObject.SetActive(!on);

        //BackGrounds
        lsBG[0].SetActive(!on);
        lsBG[1].SetActive(on);


        //texts
        arrInfoParents[0].SetActive(!on);
        arrInfoParents[1].SetActive(on);
      
    }
   

   

}
