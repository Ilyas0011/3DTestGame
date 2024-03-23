using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform target; // Цель, за которой следует камера
    public Vector3 offset = new Vector3(0f, 8f, -5.65f); // Смещение камеры относительно цели
    public float smoothSpeed = 0.5f; // Скорость сглаживания камеры

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset; // Желаемая позиция камеры с учетом смещения
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed); // Применяем сглаживание к позиции камеры
            transform.position = smoothedPosition; // Устанавливаем новую позицию камеры
            transform.LookAt(target); // Направляем камеру на цель
        }
    }
}
