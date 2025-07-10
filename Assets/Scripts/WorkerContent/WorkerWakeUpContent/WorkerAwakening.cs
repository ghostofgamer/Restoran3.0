using ADSContent;
using AttentionHintContent;
using EnergyContent;
using Enums;
using Io.AppMetrica;
using UnityEngine;

namespace WorkerContent.WorkerWakeUpContent
{
    public class WorkerAwakening : MonoBehaviour
    {
        [SerializeField] private Worker _worker;
        [SerializeField] private int _energyPrice;
        [SerializeField] private ADS _ads;
        [SerializeField] private Energy _energy;

        public void Init()
        {
            gameObject.SetActive(_worker.gameObject.activeSelf);
        }

        public void Wake(bool adsValue)
        {
            if (adsValue)
            {
                if (_worker.CurrentWorkerStateType == WorkerStateType.Relax)
                    _ads.ShowRewarded(() =>
                    {
                        AppMetrica.ReportEvent("RewardAD", "{\"" + "WakeUpWorkerADS" + "\":null}");
                        _worker.WakeUp();
                    });
                else
                    Debug.Log("Он и так в состоянии работы");
            }
            else
            {
                if (_energy.EnergyValue < _energyPrice)
                {
                    Debug.Log("недостаточно енергии");
                    AttentionHintActivator.Instance.ShowHint("недостаточно енергии");
                    return;
                }

                if (_worker.CurrentWorkerStateType == WorkerStateType.Work)
                {
                    Debug.Log("Он и так в состоянии работы");
                    return;
                }

                AppMetrica.ReportEvent("Energy", "{\"" + "WakeUpWorkerEnergy" + "\":null}");
                _energy.DecreaseEnergy(_energyPrice);
                _worker.WakeUp();
            }
        }
    }
}