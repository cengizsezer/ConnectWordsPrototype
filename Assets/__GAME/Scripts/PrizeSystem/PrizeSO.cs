using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PrizeType
{
    SoftCurrency,
    PowerUp
}

[CreateAssetMenu(fileName ="New Prize Rule",menuName = "SBF/Prize/New Prize Rule")]
public class PrizeSO : ScriptableObject
{
    public PrizeRules[] rules;

    [System.Serializable]
    public class PrizeRules
    {
        public int stepCount;
        public Prizes[] prizes;
    }

    [System.Serializable]
    public struct Prizes
    {
        public PrizeType prizeType;
        //public int prizeCount;
    }
}
