using System.Collections.Generic;
using UnityEngine;

public class CityTraffic : MonoBehaviour
{
    [SerializeField] private float _speed;
    
    public List<GameObject> cars; 
    public List<PathTraffic> paths;

    private int currentCarIndex = -1;
    private List<Vector3> currentPath;
    private int currentPathIndex = 0;
    private bool isMoving = false;

    void Start()
    {
        if (cars.Count == 0 || paths.Count == 0)
        {
            Debug.LogError("Cars or pathPoints list is empty!");
            return;
        }

        StartNextCar();
    }

    void Update()
    {
        if (isMoving)
        {
            MoveCar();
        }
    }

    void StartNextCar()
    {
        if (currentCarIndex != -1)
        {
            cars[currentCarIndex].SetActive(false);
        }

        currentCarIndex = Random.Range(0, cars.Count);
        int pathIndex = Random.Range(0, paths.Count);
        
        Debug.Log("pathIndex " + pathIndex);
        currentPath = ConvertPathToVector3(paths[pathIndex].points);

        cars[currentCarIndex].SetActive(true);
        cars[currentCarIndex].transform.position = currentPath[0];
        currentPathIndex = 0;
        isMoving = true;
    }

    void MoveCar()
    {
        GameObject currentCar = cars[currentCarIndex];
        Vector3 targetPosition = currentPath[currentPathIndex + 1];
        float step = _speed * Time.deltaTime;

        currentCar.transform.position = Vector3.MoveTowards(currentCar.transform.position, targetPosition, step);

        Vector3 direction = targetPosition - currentCar.transform.position;
        
        if (direction != Vector3.zero)
        {
            currentCar.transform.rotation = Quaternion.LookRotation(direction);
        }
        
        if (Vector3.Distance(currentCar.transform.position, targetPosition) < 0.001f)
        {
            currentPathIndex++;

            if (currentPathIndex + 1 >= currentPath.Count)
            {
                isMoving = false; 
                currentCar.SetActive(false);
                StartNextCar();
            }
        }
    }

    List<Vector3> ConvertPathToVector3(List<Transform> path)
    {
        List<Vector3> vectorPath = new List<Vector3>();
        foreach (var point in path)
        {
            vectorPath.Add(point.position);
        }
        return vectorPath;
    }
}


[System.Serializable]
public class PathTraffic
{
    public List<Transform> points = new List<Transform>();
}
