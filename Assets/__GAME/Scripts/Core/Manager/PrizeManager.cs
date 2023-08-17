using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject.Core.Manager
{
    public class PrizeManager : PersistentSingleton<PrizeManager>
    {
        [SerializeField]
        PrizeSO prizeRules;


        public int normalizedTotalStepID = 0, normalizedCrntStpID = 0;

        PrizeSO.PrizeRules crntPrizeRule;

        public int CurrentStep => normalizedCrntStpID;
        public int TotalStep => normalizedTotalStepID;

        public PrizeSO.PrizeRules GetCurrentPrizeRule
        {
            get
            {
                if (crntPrizeRule == null)
                {
                    SetCurrentPrize();
                }
                return crntPrizeRule;
            }
            private set
            {
                crntPrizeRule = value;
            }
        }

        [HideInInspector]
        public bool IsPrizeAvailable = false;

        private void Start()
        {
            if (prizeRules != null)
                SetCurrentPrize();
        }

        public float GetCurrentPrizeProgress()
        {
            bool resault = (normalizedCrntStpID - 1f == 0);
            float progress = resault ? 0 : Mathf.Clamp01((normalizedCrntStpID - 1f) / normalizedTotalStepID);
            return progress;
        }

        public float GetPrizeProgress()
        {
            float progress = Mathf.Clamp01((float)normalizedCrntStpID / (float)normalizedTotalStepID);
            return progress;
        }

        public void SetCurrentPrize()
        {
            int crntTotalStepID = SaveLoadManager.GetTotalPrizeID();
            //Debug.Log("PRIZE ID :" + crntTotalStepID);
            int totalSteps = 0;

            normalizedCrntStpID = crntTotalStepID;
            normalizedTotalStepID = 0;

            bool prizeFound = false;



            for (int i = 0; i < prizeRules.rules.Length; i++)
            {
                totalSteps += prizeRules.rules[i].stepCount;


                if (crntTotalStepID <= totalSteps)
                {

                    crntPrizeRule = prizeRules.rules[i];
                    normalizedTotalStepID = prizeRules.rules[i].stepCount;
                    normalizedCrntStpID = crntTotalStepID - totalSteps + normalizedTotalStepID;
                    prizeFound = true;
                    IsPrizeAvailable = true;
                    break;
                }

            }

            if (!prizeFound)
            {
                IsPrizeAvailable = false;
            }
        }
    }
}

