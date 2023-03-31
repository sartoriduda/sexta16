using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float hp;
    public float maxHp = 100;
    public float moveSpeed;
    public Rigidbody2D rig2D;
    float moveX;
    float moveY;
    bool isMoving;
    Animator anim;
    public Image heart;
    public int enemiesDefeat = 0;
    public GameObject[] enemiesScene;
    public bool catchItem = false;
    GameObject door;
    
    void Start()
    {
        anim = GetComponent<Animator>();
        hp = maxHp;
        enemiesDefeat = 0;
        enemiesScene = GameObject.FindGameObjectsWithTag("Enemy");
        door = GameObject.FindGameObjectWithTag("Door");
        door.SetActive(false);
    }

    void Update()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");

        Move();
        Animation();
        Attack();
        UpdateUI();

        if(hp <= 0)
        {
            SceneManager.LoadScene("SceneGameOver");
        }

        if(enemiesDefeat >= enemiesScene.Length && catchItem)
        {
            door.SetActive(true);
        }
    }

    void Move()
    {
        rig2D.MovePosition(transform.position + new Vector3(moveX, moveY, 0) * Time.deltaTime * moveSpeed);
    }

    void Animation()
    {
        if (moveX == 0 && moveY == 0)
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }
        anim.SetBool("isMoving", isMoving);
        anim.SetFloat("Horizontal", moveX);
        anim.SetFloat("Vertical", moveY);
    }

    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetTrigger("isAttack");
        }
    }

    void UpdateUI()
    {
        heart.fillAmount = hp/maxHp;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Key")
        {
            catchItem = true;
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == "Potion")
        {
            hp = maxHp;
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == "Door")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
