using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private Image bgImage;
    [SerializeField] private Transform backgroundButtons;
    [SerializeField] private Button musicButton;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Sprite[] musicSprites;
    [SerializeField] private TextMeshProUGUI coinsText;
    public int superGamePrice = 50;
    private bool musicOn = true;
    private float volume = 1f;
    private int selectedBg = 1;

    private void Start()
    {
        selectedBg = PlayerPrefs.GetInt("Background", 1);      

        foreach (Transform t in backgroundButtons)
        {
            Button button = t.GetComponent<Button>();
            button.onClick.AddListener(() => SelectBackground(int.Parse(t.gameObject.name)));
        }

        musicOn = PlayerPrefs.GetInt("Music", 1) == 1;
        volume = PlayerPrefs.GetFloat("Volume", 1f);
        volumeSlider.value = volume;
        musicButton.onClick.AddListener(SwitchMusic);
        volumeSlider.onValueChanged.AddListener(OnValueCahnged);

        UpdateSettings();

        UpdateCoinsText();
    }

    private void SelectBackground(int index)
    {
        selectedBg = index;

        PlayerPrefs.SetInt("Background", selectedBg);
        PlayerPrefs.Save();

        UpdateSettings();
    }

    private void OnValueCahnged(float value)
    {
        volume = value;

        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.Save();

        UpdateSettings();
    }

    private void SwitchMusic()
    {
        musicOn = !musicOn;      

        PlayerPrefs.SetInt("Music", musicOn ? 1 : 0);
        PlayerPrefs.Save();

        UpdateSettings();
    }

    private void UpdateSettings()
    {
        bgImage.sprite = Resources.Load<Sprite>($"Background/{selectedBg}");

        foreach (Transform t in backgroundButtons)
        {
            int index = int.Parse(t.gameObject.name);
            GameObject isSelected = t.Find("IsSelected").gameObject;

            if (index != selectedBg)
                isSelected.SetActive(false);
            else
                isSelected.SetActive(true);
        }

        SoundManager.Instance.audioSource.volume = volume;

        if (musicOn)
        {
            SoundManager.Instance.PlayMusic();
            musicButton.image.sprite = musicSprites[1];
            volumeSlider.transform.parent.gameObject.SetActive(true);
        }          
        else
        {
            SoundManager.Instance.PauseMusic();
            musicButton.image.sprite = musicSprites[0];
            volumeSlider.transform.parent.gameObject.SetActive(false);
        }           
    } 

    public void Game()
    {
        PlayerPrefs.SetInt("SelectedLevel", 1);
        SceneManager.LoadScene("SelectCharacter");
    }

    public void SuperGame()
    {
        int coins = Coins.GetCoins();
        if(coins >= superGamePrice)
        {
            coins -= superGamePrice;
            Coins.SaveCoins(coins);
            UpdateCoinsText();
            SoundManager.Instance.PlayClip("Buy");
            PlayerPrefs.SetInt("SelectedLevel", 2);
            SceneManager.LoadScene("SelectCharacter");
        }
    }

    private void UpdateCoinsText()
    {
        coinsText.text = Coins.GetCoins().ToString();
    }
}
