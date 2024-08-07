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
    {// позиция камеры обновляется после позиции игрока
        // сначала просчитать будет ли камера выходит за пределы игрового поля 
        // только затем обновить её позицию
        Vector3 maxCords = cam.WorldToScreenPoint(player.transform.position) + screenCoordinates / 2;// будующий правый верхний угол
        Vector3 minCords = cam.WorldToScreenPoint(player.transform.position) - screenCoordinates / 2;// будующий левый нижний угол
        
        maxCords = cam.ScreenToWorldPoint(maxCords);
        minCords = cam.ScreenToWorldPoint(minCords);

        float newX;
        float newY;

        // если будующая позиция камеры находиться в допустимой позиции то обновить её
        if (maxCords.x <= 20f && minCords.x >= -20f)
            newX = player.position.x;
        else// если будующая позиция камеры не допустима, оставить её текущую позицию
            newX = transform.position.x;

        if(maxCords.y <= 15f && minCords.y >= -15f)
            newY = player.position.y;
        else
            newY = transform.position.y;

        transform.position = new Vector3(newX, newY, -10f);
    }
}
