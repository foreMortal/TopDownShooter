using System.Collections.Generic;
using UnityEngine;

public class VoidZones : MonoBehaviour
{
    [SerializeField] private GameObject slowZone, deathZone;
    private List<GameObject> zones = new List<GameObject>();
    private List<Vector3> positions = new();
    private float xRange = 20f, yRange = 15f;

    private void Awake()
    {
        positions.Add(new(Random.Range(-xRange + 3f, xRange - 3f), Random.Range(-yRange + 3f, yRange - 3f)));
        for (int i = 0; i < 4; i++)
        {
            AddNewPosition();
        }

        for(int i = 0; i < 2; i++)
        {
            zones.Add(Instantiate(slowZone));
        }
        zones.Add(Instantiate(deathZone));

        zones.Add(slowZone);
        zones.Add(deathZone);

        for(int i = 0; i < zones.Count; i++)
        {
            zones[i].transform.position = positions[i];
        }
    }

    private void AddNewPosition()
    {
        Vector3 newPos = new(Random.Range(-xRange + 3f, xRange - 3f), Random.Range(-yRange + 3f, yRange - 3f));
        foreach(var position in positions)
        {
            if ((newPos - position).sqrMagnitude < 9f)
            {
                AddNewPosition();
                return;
            }
        }
        positions.Add(newPos);
    }
}
