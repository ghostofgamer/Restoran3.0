using Enums;
using UI.Screens.ShopContent.WorkersContent;
using UnityEngine;

namespace WorkerContent
{
    public class Workers : MonoBehaviour
    {
        public const string Worker = "Worker";

        [SerializeField] private Worker[] _workers;
        [SerializeField] private WorkerUIProduct[] _workerUIProducts;

        private void OnEnable()
        {
            foreach (var workerUIProduct in _workerUIProducts)
            {
                workerUIProduct.WorkerBuyed += ActivateWorker;
                workerUIProduct.WorkerFired += DeactivateWorker;
            }
        }

        private void OnDisable()
        {
            foreach (var workerUIProduct in _workerUIProducts)
            {
                workerUIProduct.WorkerBuyed -= ActivateWorker;
                workerUIProduct.WorkerFired -= DeactivateWorker;
            }
        }

        private void Start()
        {
            Debug.Log("Уборщирк " + PlayerPrefs.GetInt(Worker + WorkerType.Cleaner, 0));

            foreach (var worker in _workers)
                worker.gameObject.SetActive(PlayerPrefs.GetInt(Worker + worker.WorkerType, 0) > 0);
        }

        private void ActivateWorker(WorkerType type)
        {
            Debug.Log("Купили работника себе " + type);
            var worker = System.Array.Find(_workers, w => w.WorkerType == type);

            if (worker != null)
                worker.gameObject.SetActive(true);
        }

        private void DeactivateWorker(WorkerType type)
        {
            Debug.Log("Продали работника себе " );
            var worker = System.Array.Find(_workers, w => w.WorkerType == type);

            if (worker != null)
                worker.gameObject.SetActive(false);
        }
    }
}