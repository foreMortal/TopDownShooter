using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollows : MonoBehaviour
{
    [SerializeField] private Transform player;
    private Camera cam;
    private Vector3 screenCoordinates;

    private void Awake()
    {
        screenCoordinates = new Vector3(Screen.width, Screen.height);
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {// ������� ������ ����������� ����� ������� ������
        // ������� ���������� ����� �� ������ ������� �� ������� �������� ���� 
        // ������ ����� �������� � �������
        Vector3 maxCords = cam.WorldToScreenPoint(player.transform.position) + screenCoordinates / 2;// �������� ������ ������� ����
        Vector3 minCords = cam.WorldToScreenPoint(player.transform.position) - screenCoordinates / 2;// �������� ����� ������ ����
        
        maxCords = cam.ScreenToWorldPoint(maxCords);
        minCords = cam.ScreenToWorldPoint(minCords);

        float newX;
        float newY;

        // ���� �������� ������� ������ ���������� � ���������� ������� �� �������� �
        if (maxCords.x <= 20f && minCords.x >= -20f)
            newX = player.position.x;
        else// ���� �������� ������� ������ �� ���������, �������� � ������� �������
            newX = transform.position.x;

        if(maxCords.y <= 15f && minCords.y >= -15f)
            newY = player.position.y;
        else
            newY = transform.position.y;

        transform.position = new Vector3(newX, newY, -10f);
    }
}
