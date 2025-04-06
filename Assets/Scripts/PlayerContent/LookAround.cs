using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class LookAround : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Transform playerBody; // Ссылка на тело персонажа
    public float lookSpeed = 2f; // Скорость вращения камеры
    public float verticalLookLimit = 80f; // Лимит вращения камеры по вертикали

    private float rotationX = 0;
    private bool isRotating = false;

    private void Update()
    {
        if (isRotating)
        {
            // Вращение камеры по вертикали и горизонтали
            rotationX -= Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -verticalLookLimit, verticalLookLimit);

            float rotationY = Input.GetAxis("Mouse X") * lookSpeed;

            // Применение вращения камеры
            transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            playerBody.Rotate(Vector3.up * rotationY);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
        isRotating = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("OnPointerUp");
        isRotating = false;
    }
    
    public void StartRotation()
    {
        isRotating = true;
        Debug.Log("StartRotation called");
    }

    public void StopRotation()
    {
        isRotating = false;
        Debug.Log("StopRotation called");
    }
}