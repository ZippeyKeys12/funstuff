using UnityEngine;
using Unity.Collections;
using Unity.Entities;

using Player;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    EntityManager manager;
    HumanoidPlayer player;

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

    void Start()
    {
        manager = World.Active.EntityManager;
        player = FindObjectOfType<HumanoidPlayer>();
    }

    public static Entity CreateEntity(params ComponentType[] types)
    {
        return Instance.manager.CreateEntity(types);
    }

    public static NativeArray<Entity> GetAllEntities(params ComponentType[] types)
    {
        return Instance.manager.GetAllEntities();
    }
}
