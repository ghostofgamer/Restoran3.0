using UI.Buttons;
using UI.Screens.ShopContent.WorkersContent;
using UnityEngine;

public class BuyWorkerButton :AbstractButton
{
    [SerializeField] private WorkerUIProduct _workerUIProduct;
    
    public override void OnClick()
    {
        _workerUIProduct.BuyWorker();
    }
}