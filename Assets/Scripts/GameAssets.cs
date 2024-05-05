using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets instance;
    public static GameAssets Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load<GameAssets>("GameAssets");
            }
            return instance;
        }
    }

    public Transform Enemy;
    public Transform EnemyDieParticles;
    public Transform ArrowProjectile;
    public Transform BuildingDestroyedParticles;
    public Transform BuildingConstruction;
    public Transform BuildingPlacedParticles;
}
