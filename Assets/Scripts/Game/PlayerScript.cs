using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerScript : MonoBehaviour, IPointerUpHandler
{
    public static PlayerScript Instance { set; get; }

    private CanvasManager canvasManager;
    public float movementSpeed = 15f;
    public float movement = 0f;
    public bool m_FacingRight = false;
    private Animator _animator = null;
    private BoxCollider2D _bx;
    public Vector2 LastPlatformHit;
    public Rigidbody2D rb;
    public GameObject sliderMover;
    public bool IsDead = false;
    public bool IsInRocket = false;

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        canvasManager = GameObject.Find("CanvasManager").GetComponent<CanvasManager>();
        if (SaveManager.Instance.state.usingAccelerometer)
        {
            sliderMover.SetActive(false);
        }
        rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _bx = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDead == false)
        {
            if (SaveManager.Instance.state.usingAccelerometer)
            {
                movement = Input.acceleration.x * movementSpeed;
            }
            else
            {
                movement = (sliderMover.GetComponent<Slider>().value);
            }
        }
        else
        {
            movement = 0f;
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        sliderMover.GetComponent<Slider>().value = 0;
    }
    private void FixedUpdate()
    {
        Vector2 velocity = rb.velocity;
        velocity.x = movement;
        rb.velocity = velocity;

        if (movement > 0 && !m_FacingRight)
        {
            Flip();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (movement < 0 && m_FacingRight)
        {
            Flip();
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Rocket") && IsInRocket == false)
        {
            Destroy(collision.gameObject);
            StartCoroutine(RocketBoost(2f));
            _animator.Play("Jump");
        }
        if (collision.gameObject.CompareTag("Coin"))
        {
            Destroy(collision.gameObject);
            print("PlaySound");
            canvasManager.UpdateMoneyInGameText();
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            IsDead = true;
            _bx.enabled = false;
            print("PlaySound Enemy");
            canvasManager.OnDeathShowCanvas();
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platforms"))
        {
            _animator.Play("Idle");
            LastPlatformHit = other.gameObject.transform.position;

        }
    }
    private void Flip()
    {
        m_FacingRight = !m_FacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    private IEnumerator RocketBoost(float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            IsInRocket = true;
            elapsed += Time.deltaTime;
            rb.AddForce(transform.up * 15);
            yield return null;
        }
        IsInRocket = false;
    }

}
