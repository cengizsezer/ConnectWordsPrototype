using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyController
{
    protected Tween _lastTween = null;
    private Sprite CoinSprite;
    private RectTransform[] target;
    private RectTransform[] spawnPosition;
    public CurrencyController(CurrencyControllerSceneReferences references)
    {
        CoinSprite = references.CoinSprite;
        target = references.TargetsRectTransform;
        spawnPosition = references.SpawnPositions;

    }
   
    public void CacheTween(Tween tween)
    {
        _lastTween = tween;
    }

    protected void CompleteLastTween()
    {
        if (_lastTween != null) _lastTween.Complete(true);
    }

    protected void KillLastTween()
    {
        if (_lastTween == null) return;
        _lastTween.Kill(true);
    }

    public void AddCoin(int add)
    {
        SaveLoadManager.AddCoin(add);
    }


    public CoinBehaviour GetCoin()
    {
        return PoolHandler.I.GetObject<CoinBehaviour>();
    }


    public CoinBehaviour CreateCoin(Vector2 spawnPosition, UIEffectsManager.CanvasLayer type)
    {
        CoinBehaviour flyingObj = PoolHandler.I.GetObject<CoinBehaviour>();
        flyingObj.transform.position = spawnPosition;
        flyingObj.transform.SetParent(UIEffectsManager.I.GetLayerParent(type));
        flyingObj.GetComponent<RectTransform>().sizeDelta = new Vector2(.4f, .4f);
        flyingObj.transform.DOScale(Vector3.zero, 0f);
        flyingObj.transform.DOScale(new Vector2(250f, 250f), .2f).SetDelay(.1f).SetEase(Ease.OutBack);
        flyingObj.GetComponent<Image>().sprite = CoinSprite;
        return flyingObj;
    }


    public void OnComplate(Transform rect)
    {
        Sequence s = DOTween.Sequence();
        Vector2 YPos = rect.transform.position.WithZ(0f);
        YPos.y = YPos.y + 0.1f;

        s.Append(TweenHelper.BouncyMoveTo(rect, YPos));

        KillLastTween();
    }


    public async Task GetPrize(int Count, float addCoinValue, RectTransform spawnTarget = null)
    {
        int defaultValue = 200;
        List<CoinBehaviour> lsCoin = new List<CoinBehaviour>();
        Index idx = new Index();


        var type = UIEffectsManager.I.GetTypeLayer(LevelManager.IsLastLevelOfChapter);
        idx = (int)type - 2;
        Vector2 targetPosition = target[idx].transform.position;

        var delay = 0f;
        lsCoin.Clear();

        Vector3 pos = spawnTarget != null ? new Vector3(spawnTarget.position.x, spawnTarget.position.y + 1f, spawnTarget.position.z) : Vector3.zero;

        for (int i = 0; i < Count; i++)
        {

            var position = new Vector3(UnityEngine.Random.Range(-.3f, .3f),
                UnityEngine.Random.Range(-.3f, .3f), 0f) + (spawnTarget == null ? spawnPosition[idx].position : (pos));

            CoinBehaviour c = CreateCoin(position, type);
            c.transform.SetAsLastSibling();
            var q = new Vector3(UnityEngine.Random.Range(-30f, 30f), UnityEngine.Random.Range(-30f, 30f), 0f);
            c.transform.rotation = Quaternion.Euler(q);
            c.transform.DORotate(Vector3.zero, 0.5f).SetDelay(delay + 0.5f).SetEase(Ease.Flash);

            lsCoin.Add(c);
            delay += 0.1f;
            await Task.Delay(100);
        }

        await Task.Delay(300);
       
        for (int i = 0; i < lsCoin.Count; i++)
        {

            if (spawnTarget != null)
            {
                CoinBehaviour c = lsCoin[i];
                Tween flyingTween = TweenHelper.CurvingMoveTo(c.transform, targetPosition, () => SaveLoadManager.AddCoin(addCoinValue), 1f, .2f, Ease.InOutCubic, Ease.InBack);

            }
            else
            {
                CoinBehaviour c = lsCoin[i];
                Vector3 downPos = c.transform.position;
                downPos.y -= 1f;

                c.transform.DOMoveY(downPos.y, .2f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    Tween flyingTween = TweenHelper.CurvingMoveTo(c.transform, targetPosition, () => SaveLoadManager.AddCoin(addCoinValue), 1f, .2f, Ease.InOutCubic, Ease.InBack);
                });
            }



            await Task.Delay(150);
        }


        await Task.Delay(defaultValue * Count);

        await Task.Yield();
        lsCoin.ForEach(n => n.OnDeactivate());
        lsCoin.Clear();
    }
}
