using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    // Start is called before the first frame update

    public int CurrentHealth;
    public int MaxHealth = 100;
    public float MoveSpeed;
    public float AttackSpeed;

    void Start()
    {
        CurrentHealth = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
