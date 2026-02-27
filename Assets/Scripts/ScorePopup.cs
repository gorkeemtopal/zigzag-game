using TMPro;
using UnityEngine;

public class ScorePopup : MonoBehaviour
{
    public float lifeTime = 0.8f;
    public float moveUpSpeed = 50f;

    private TMP_Text text;
    private float timer;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    public void Show(string value)
    {
        text.text = value;
        timer = lifeTime;
    }

    private void Update()
    {
        transform.position += Vector3.up * moveUpSpeed * Time.deltaTime;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
