using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerObject : MonoBehaviour
{
    public CanvasGroup m_CanvasGroup;


    private void OnDestroy()
    {
        DOTween.Kill(transform);
    }
}
