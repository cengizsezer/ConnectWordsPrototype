using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MyProject.Settings;
using MyProject.Core.Manager;

public class ChapterListElement : MonoBehaviour
{
    [SerializeField] RectTransform rectTransform, buttonPool,rectSeperator;
    [SerializeField] TextMeshProUGUI txtHeader;
    [SerializeField] LevelButton prefabButton;
    public ExpandableUISystem expandableUISystem;
    [SerializeField] TextParamGetter additionalText;
    public Image BG;
    public RectTransform GetRect() => rectTransform;
    public float GetTotalHeight() => totalHeight;

    float totalHeight = 0f;

    public void Init(string name,int chapterID, int totalLevel, float yPos, int startAtID)
    {
        //LanguageLoader.GetValue("chapter")
        string headerText = string.Join(' ', name/*, chapterID*/);
        txtHeader.SetText(headerText);

        float verticalPadding = Settings.ChapterPanel.buttonPanelVerticalPadding;
        float cellSpace = Settings.ChapterPanel.buttonCellSpace;
        float buttonHeight = Settings.ChapterPanel.buttonObjectHeight;

        float heightButtonPool = (verticalPadding * 2f) + (cellSpace * (totalLevel - 1)) + buttonHeight * totalLevel;

        totalHeight = heightButtonPool + rectSeperator.rect.height - rectSeperator.anchoredPosition.y * 2f;

        //rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, totalHeight);

        //rectTransform.anchoredPosition = new Vector2(0f, yPos);

        float crntPos = verticalPadding;

        //buttonPool.sizeDelta = new Vector2(buttonPool.sizeDelta.x, heightButtonPool);

        int maxLevelID = SaveLoadManager.GetMaxTotalLevel();

        additionalText.SetProps((chapterID + 1).ToString(), ResultType.Fixed, Position.After, " - ");

        for (int i = 0; i < totalLevel; i++)
        {
            int _mID = startAtID + i;

            LevelButton lb = Instantiate(prefabButton, buttonPool);
            lb.transform.position = Vector3.zero;
            lb.transform.position = expandableUISystem.resetPosition;
           
            //lb.GetRect().rect.Set(0f, -crntPos, lb.GetRect().rect.width, Settings.ChapterPanel.buttonObjectHeight);
            //lb.GetRect().anchoredPosition = Vector2.down * crntPos;

            bool _isAvailable = false;

            if (_mID <= maxLevelID)
                _isAvailable = true;

            lb.Init(_mID, _isAvailable,chapterID);

            crntPos += (buttonHeight + cellSpace);
        }
    }
}
