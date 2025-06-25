using Interfaces;
using UnityEngine;

namespace CityTrafficContent
{
    public abstract class AbstractNPC : MonoBehaviour
    {
        [SerializeField] private UnityEngine.AI.NavMeshAgent _navMeshAgent;
        [SerializeField] private Animator _animator;
        [SerializeField] private float _minDistance;

        private ICityTraffic  _npcTraffic;
        private Transform[] _points;
        private int _index = 0;

        private void Update()
        {
            Roam();
        }

        public abstract void InitUniqueData();
        
        public void Init(GameObject path, ICityTraffic npcTraffic)
        {
            _npcTraffic = npcTraffic;
            _points = new Transform[path.transform.childCount];
            for (int i = 0; i < _points.Length; i++)
                _points[i] = path.transform.GetChild(i);
        }
        
        /*public void Init<T>(GameObject path, CityTraffic<T> npcTraffic) where T : AbstractNPC
        {
            Debug.Log("npcTraffic " + npcTraffic);
            _npcTraffic = npcTraffic as CityTraffic<AbstractNPC>;
            _points = new Transform[path.transform.childCount];
            for (int i = 0; i < _points.Length; i++)
                _points[i] = path.transform.GetChild(i);
            /*ChoiceAccessories();
            ChoiceAppearance();#1#
        }*/

        private void Roam()
        {
            if (Vector3.Distance(transform.position, _points[_index].position) < _minDistance)
            {
                _index = (_index + 1) % _points.Length;

                if (_index == 0)
                {
                    _npcTraffic.DecreaseActiveNPC();
                    gameObject.SetActive(false);
                    return;
                }
            }

            _navMeshAgent.SetDestination(_points[_index].position);
            _animator.SetFloat("Vertical", !_navMeshAgent.isStopped ? 1 : 0);
        }

        /*private void ChoiceAccessories()
        {
            foreach (var accessory in _accessories)
                accessory.SetActive(false);

            _accessories[Random.Range(0, _accessories.Length)].SetActive(true);
        }

        private void ChoiceAppearance()
        {
            foreach (var body in _bodies)
                body.SetActive(false);

            foreach (var eyes in _eyes)
                eyes.SetActive(false);

            foreach (var tail in _tails)
                tail.SetActive(false);

            foreach (var mounth in _mounth)
                mounth.SetActive(false);

            _bodies[Random.Range(0, _bodies.Length)].SetActive(true);
            _eyes[Random.Range(0, _eyes.Length)].SetActive(true);
            _tails[Random.Range(0, _tails.Length)].SetActive(true);
            _mounth[Random.Range(0, _mounth.Length)].SetActive(true);
        }*/
    }
}