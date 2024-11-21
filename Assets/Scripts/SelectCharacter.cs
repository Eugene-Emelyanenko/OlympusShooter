using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectCharacter : MonoBehaviour
{
    [SerializeField] private Image bgImage;

    private void Start()
    {
        bgImage.sprite = Resources.Load<Sprite>($"Background/{PlayerPrefs.GetInt("Background", 1)}");
    }

    public void Select(int index)
    {
        PlayerPrefs.SetInt("Character", index);
        PlayerPrefs.Save();

        SceneManager.LoadScene("Game");
    }
}
