using UI.Screens.ShopContent.WorkersContent;
using UnityEngine;

namespace UI.Buttons.WorkerContent
{
    public class DismissWorkerButton : AbstractButton
    {
        [SerializeField] private WorkerUIProduct _workerUIProduct;
    
        public override void OnClick()
        {
            _workerUIProduct.DismissWorker();
        }
    }
}
