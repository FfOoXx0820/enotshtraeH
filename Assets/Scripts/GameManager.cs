using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Minion sample;
    public Player Player1;
    public Player Player2;
    private void Start()
    {
        //Minion minion = Instantiate(sample, new Vector3(0.0f, 1.0f, 0.0f), Quaternion.identity);
        Player1.Fight(Player2);
    }
}
