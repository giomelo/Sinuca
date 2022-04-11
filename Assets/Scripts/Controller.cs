using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public static Controller Instance;

    public int Score { get; private set; }

    public Taco taco;

    private void Awake()
    {
        Instance = this;
    }

    public void BackWhite()
    {
        Erros();
        taco.ResetWhite();
    }

    private void Erros()
    {
        Score--;
    }

    public void Acertos()
    {
        Score++;
    }
}
