using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonType : MonoBehaviour
{
    public PopUpType mPopUptype;
    [SerializeField] Button mButton;
    Image tutorialZoomImage;

    private void Start()
    {
        if(mPopUptype==PopUpType.Hint)
        {
            if(LevelManager.InTheFirstFourLevels())
            { 
                gameObject.SetActive(false);

            }else if (LevelManager.IsFiveLevel())
            {
                //Vector2 pos = GetComponent<RectTransform>().FromAnchoredPositionToAbsolutePosition(GetComponent<RectTransform>().pivot);
                tutorialZoomImage = TutorialManager.I.CreateBGObject(GetComponent<RectTransform>(), GameInUIManager.I.Hint.transform).GetComponent<Image>();
                var obj=TutorialManager.I.CreateFinger(transform.position, GameInUIManager.I.Hint.transform);
                //tutorialZoomImage.transform.position = pos;
                TutorialManager.I.ActiveFinger = obj.GetComponent<FingerObject>();
                TutorialManager.I.FingerTapAnimation(obj, transform.position);
                GameManager.I.isFiveLevelCondition = true;

            }
        }
    }
    public void OnEnable()
    {
        mButton.onClick.RemoveAllListeners();
        mButton.onClick.AddListener(Click);
    }


    private void OnDisable()
    {
        mButton.onClick.RemoveAllListeners();
    }


    public void Click()
    {
        if (LevelManager.IsFiveLevel())
        {

            if (mPopUptype == PopUpType.Hint)
            {
                GameManager.I.isFiveLevelCondition = false;
                IPopUp _pp = GameInManager.I.GameInController.PopUpController.GetPp(mPopUptype);
                TweenHelper.ButtonClickAnimation(mButton.transform);
                _pp.Send();
                tutorialZoomImage.gameObject.SetActive(false);
                DOTween.Kill(TutorialManager.I.ActiveFinger.transform);
                TutorialManager.I.ActiveFinger.gameObject.SetActive(false);
                //Destroy(TutorialManager.I.ActiveFinger.gameObject);

                return;
            }
        }

        IPopUp pp = GameInManager.I.GameInController.PopUpController.GetPp(mPopUptype);
        TweenHelper.ButtonClickAnimation(mButton.transform);
        pp.Send();
    }


    //PopUpHandler.I.popUpType = mPopUptype;
    //PopUpProcessor processor = new PopUpProcessor(PopUpHandler.I.GetPopUp());
    //processor.Process();

    //PopUpHandler.I.popUpType = mPopUptype;
    ////PopUpHandler.I.AddPopUpType(mPopUptype);
    //PopUpProcessor processor = new PopUpProcessor(PopUpHandler.I.GetPp(mPopUptype));
    //processor.Process();
}
