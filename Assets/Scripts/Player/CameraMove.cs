using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform target; // ����, �� ������� ������� ������
    public Vector3 offset = new Vector3(0f, 8f, -5.65f); // �������� ������ ������������ ����
    public float smoothSpeed = 0.5f; // �������� ����������� ������

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset; // �������� ������� ������ � ������ ��������
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed); // ��������� ����������� � ������� ������
            transform.position = smoothedPosition; // ������������� ����� ������� ������
            transform.LookAt(target); // ���������� ������ �� ����
        }
    }
}
