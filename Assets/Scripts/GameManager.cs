using UnityEngine;
using Unity.Collections;
using Unity.Entities;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static Entity CreateEntity(params ComponentType[] types)
    {
        return World.Active.EntityManager.CreateEntity(types);
    }

    public static NativeArray<Entity> GetAllEntities(params ComponentType[] types)
    {
        return World.Active.EntityManager.GetAllEntities();
    }
}
