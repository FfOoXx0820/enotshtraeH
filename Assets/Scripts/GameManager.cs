using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Minion minion_sample;
    private void Start()
    {
        minion_sample = new Minion("sample", 0, 3, 4, 1, new bool[] { false, false, false, false, false, false });
    }
}
