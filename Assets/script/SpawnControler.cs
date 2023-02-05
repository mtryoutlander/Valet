using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnControler : MonoBehaviour
{
    public GameObject prefabDrivableCar;
    public GameObject spawnPoint;

    

    private void OnEnable()
    {
        PlayerControl.spawnDrivableCar += SpawnCar;
    }
    private void OnDisable()
    {
        PlayerControl.spawnDrivableCar -= SpawnCar;
    }

    public void SpawnCar()
    {
        Instantiate(prefabDrivableCar,spawnPoint.transform);
    }
}
