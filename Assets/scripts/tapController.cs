using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class tapController : MonoBehaviour
{
    public delegate void PlayerDelegate();
    public static event PlayerDelegate OnPlayerDied;
    public static event PlayerDelegate OnPlayerScored;
   

    public float tapForce = 10;
    public float tiltSmooth = 5;
    public Vector3 startPos;

    public AudioSource tapSound;
    public AudioSource scoreSound;
    public AudioSource dieSound;

    Rigidbody2D rigidbody;
    Quaternion downRotation;
    Quaternion forwardRotation;
    GameManager game;

    void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
        downRotation = Quaternion.Euler(0, 0, -100);
        forwardRotation = Quaternion.Euler(0, 0, 40);
        game = GameManager.Instance;
        rigidbody.simulated = false;
        transform.localPosition = startPos;
    }
    void OnEnable()
    {
        GameManager.OnGameStarted += OnGameStarted;
        GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
    }
    void OnDisable()
    {
        GameManager.OnGameStarted -= OnGameStarted;
        GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;
    }
    void OnGameStarted()
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.simulated = true;
    }
    void OnGameOverConfirmed()
    {
        transform.localPosition = startPos;
        transform.rotation = Quaternion.identity;
    }
    void Update() {

        if (game.GameOver) return;

        if (Input.GetMouseButtonDown(0))
        {
            transform.rotation = forwardRotation;
            rigidbody.velocity= Vector3.zero;
            rigidbody.AddForce(Vector2.up * tapForce, ForceMode2D.Force);
            tapSound.Play();
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, downRotation, tiltSmooth * Time.deltaTime);

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "ScoreZone") {

            OnPlayerScored();
            scoreSound.Play();//score sound
        }
        if (col.gameObject.tag == "DeadZone") {
            rigidbody.simulated = false;
            OnPlayerDied();//event sent to GameMananger
            dieSound.Play();//gameover sound
        }
    }
}
