using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour
{
    public static Controller Instance;
    [SerializeField]
    private Text score;
    public int Score { get; private set; }

    public Taco taco;

    private int amountOfBalls = 15;

    private int currentAmountOfBalls = 0;
    [SerializeField]
    private GameObject winScreen;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        score.text = Score.ToString();
        currentAmountOfBalls = 0;
        amountOfBalls = 0;
    }

    public void BackWhite()
    {
        Erros();
        taco.ResetWhite();
    }

    private void Erros()
    {
        Score--;
        score.text = Score.ToString();
    }

    public void Acertos()
    {
        Score++;
        score.text = Score.ToString();
        currentAmountOfBalls++;

        if (currentAmountOfBalls == amountOfBalls)
        {
            //ganhou
            winScreen.SetActive(true);
        }
    }

    public void Reiniciar()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
