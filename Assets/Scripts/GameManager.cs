using UnityEngine;
using Unity.Collections;
using Unity.Entities;
using Player;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

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
        player = FindObjectOfType<HumanoidPlayer>();
    }
}
