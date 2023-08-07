using UnityEngine;
using MyBox;

public enum ResultType
{
    Int,
    String,
    Float,
    Fixed,
    ChapterName
}
public enum Position
{
    Before,
    After
}

public enum SeperatorType
{
    String,
    Enter,
    Tab
}

[RequireComponent(typeof(TextKey))]
public class TextParamGetter : MonoBehaviour
{
    public SeperatorType seperatorType;
    [ConditionalField(nameof(resultType),true,ResultType.ChapterName)]
    [SerializeField] string key;

    public string seperator;

    [SerializeField] ResultType resultType;
    public Position position;

    [ConditionalField(nameof(resultType), false, ResultType.Int)]
    [SerializeField] int addInt;

    [ConditionalField(nameof(resultType), false, ResultType.Float)]
    [SerializeField] int addFloat;

    public void SetProps(string key)
    {
        this.key = key;
        GetComponent<TextKey>().SetText();
    }

    public void SetProps(string key,string seperator)
    {
        this.seperator = seperator;
        this.key = key;
        GetComponent<TextKey>().SetText();
    }

    public void SetProps(string key, ResultType resultType)
    {
        this.key = key;
        this.resultType = resultType;
        GetComponent<TextKey>().SetText();
    }

    public void SetProps(string key, ResultType resultType, string seperator)
    {
        this.key = key;
        this.resultType = resultType;
        this.seperator = seperator;
        GetComponent<TextKey>().SetText();
    }

    public void SetProps(string key, ResultType resultType,Position position)
    {
        this.key = key;
        this.resultType = resultType;
        this.position = position;
        GetComponent<TextKey>().SetText();
    }

    public void SetProps(string key, ResultType resultType, Position position, string seperator)
    {
        this.key = key;
        this.resultType = resultType;
        this.position = position;
        this.seperator = seperator;
        GetComponent<TextKey>().SetText();
    }

    public void SetProps(string key,Position position)
    {
        this.key = key;
        this.position = position;
        GetComponent<TextKey>().SetText();
    }

    public void SetProps(string key, Position position,string seperator)
    {
        this.key = key;
        this.position = position;
        this.seperator = seperator;
        GetComponent<TextKey>().SetText();
    }

    public string GetResult()
    {
        string res = "NULL";
        switch(resultType)
        {
            case ResultType.Int:
                res = GetSavedInt();
                break;
            case ResultType.String:
                res = GetSavedString();
                break;
            case ResultType.Float:
                res = GetSavedFloat();
                break;
            case ResultType.Fixed:
                res = key;
                break;
            case ResultType.ChapterName:
                res = GetChapterName();
                break;
        }
        return res;
    }

    public string GetChapterName()
    {
        return (LevelManager.GetLevelInfo().chapters[LevelManager.CurrentChapterID].ChapterName);
    }
    public string GetSavedString()
    {
        return PlayerPrefs.GetString(key, "N/A");
    }

    public string GetSavedInt()
    {
        int val = PlayerPrefs.GetInt(key, 0) + addInt;
        return val.ToString();
    }

    public string GetSavedFloat()
    {
        float val = (PlayerPrefs.GetFloat(key, 0.0f) + addFloat);
        return val.ToString();
    }
}
