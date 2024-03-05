using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
    public void FirstScene()
    {
        Application.LoadLevel(0);
    }
    public void SecondScene()
    {
        Application.LoadLevel(1);
    }
}
