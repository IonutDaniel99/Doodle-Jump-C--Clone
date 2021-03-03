using UnityEngine;

public class DestroyerScript : MonoBehaviour
{

    private CanvasManager canvasManager;
    public bool ExtraLife;

    void Start()
    {
        canvasManager = GameObject.Find("CanvasManager").GetComponent<CanvasManager>();
        ExtraLife = true;

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Platforms"))
        {
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("Player"))
        {
            if (ExtraLife == true && SaveManager.Instance.state.activeUpgrade == 1)
            {
                LevelGeneratorScript.Instance.ExtraLife();
                ExtraLife = false;
            }
            else
            {
                Destroy(collision.gameObject);
                canvasManager.OnDeathShowCanvas();
            }
        }
        if (collision.CompareTag("Rocket"))
        {
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("Coin"))
        {
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
        }

    }
}
