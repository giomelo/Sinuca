using System;
using System.Diagnostics;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Scripts
{
    public class Hole : MonoBehaviour
    {
        private void OnTriggerEnter(Collider col)
        {
            switch (col.tag)
            {
                case "Ball":
                    Destroy(col.gameObject);
                    Controller.Instance.Acertos();
                    break;
                case "Branca":
                    Controller.Instance.BackWhite();
                    break;
            }
        }
    }
}