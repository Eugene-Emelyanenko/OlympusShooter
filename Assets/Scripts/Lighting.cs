using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lighting : MonoBehaviour
{
    private float speed;
    private GameManager manager;

    private void Awake()
    {
        manager = FindObjectOfType<GameManager>();
    }

    public void SetSpeed(float lightingSpeed)
    {
        speed = lightingSpeed;
    }

    private void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            SoundManager.Instance.PlayClip("Enemy");
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        else if(collision.gameObject.CompareTag("Coin"))
        {
            int coins = Coins.GetCoins();
            coins++;
            Coins.SaveCoins(coins);
            manager.collectedCoins++;
            SoundManager.Instance.PlayClip("Coin");
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
