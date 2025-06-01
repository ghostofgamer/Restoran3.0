using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class TransferItems : MonoBehaviour
{
    public void TransferListItems(int value,GameObject[] objects,Transform[] targetPositions,Action callback)
    {
        Debug.Log("VALUE " + value);
        Debug.Log("objects " + objects.Length);
        Debug.Log("targetPositions " + targetPositions.Length);

        List<GameObject> activeItems = objects.Where(p => p.activeSelf).ToList();

        if (value > activeItems.Count)
            value = activeItems.Count;

        Sequence sequence = DOTween.Sequence();

        for (int i = 0; i < value; i++)
        {
            int index = i;
            
            if (index < activeItems.Count && index < targetPositions.Length)
            {
                sequence.Join(activeItems[index].transform.DOMove(targetPositions[index].position, 1f)
                    .SetEase(Ease.InOutQuad));
            }
        }

        sequence.OnComplete(() =>
        {
            /*foreach (var item in activeItems)
                item.transform.localPosition = Vector3.zero;*/
            
            callback?.Invoke();
        });
        
        
        
        /*List<GameObject> activeItems = objects.Where(p => p.activeSelf).ToList();

        if (value > activeItems.Count)
            value = activeItems.Count;

        for (int i = objects.Length - 1; i >= objects.Length - value; i--)
        {
            int index = i;

            activeItems[index].transform.DOMove(targetPositions[index].transform.position, 1f)
                .SetEase(Ease.InOutQuad)
                .OnComplete(() =>
                {
                    activeItems[index].transform.localPosition = Vector3.zero;
                    callback?.Invoke();
                });
        }*/
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        /*Sequence sequence = DOTween.Sequence();
        
        sequence.Append(currentObject.transform.DOMove(targetPos.position, 0.5f)
            .SetEase(Ease.InOutQuad));

        sequence.Join(currentObject.transform
                .DOLocalRotate(new Vector3(0, 0, 0), 0.5f, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear))
            .OnComplete(() => callback?.Invoke());*/
    }
}