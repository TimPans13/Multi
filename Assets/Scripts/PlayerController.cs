using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    public PhotonView photonView;//private
    private int playerCoinCounter = 0;
    private CoinManager coinManager;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    public int maxHealth = 100;
    private int currentHealth;//private
    private bool isAlive;
    GameController gameController;
    Camera mainCamera;
    float cameraHeight;
    float cameraWidth;
    private Joystick joystick; 
    private Vector2 joystickDirection; 
    private Button fireButton;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        photonView = GetComponent<PhotonView>();
        coinManager = FindObjectOfType<CoinManager>();
        currentHealth = maxHealth;
        UIManager uiManager = FindObjectOfType<UIManager>();
        gameController = FindObjectOfType<GameController>();
        isAlive = true;
        photonView.RPC("UpdateHealth", RpcTarget.AllBuffered, currentHealth);

        mainCamera = Camera.main;
        cameraHeight = mainCamera.orthographicSize;
        cameraWidth = cameraHeight * mainCamera.aspect;
        joystick = FindObjectOfType<Joystick>();
        fireButton= FindObjectOfType<Button>();
        fireButton.onClick.AddListener(Shoot);

        if (uiManager != null && photonView.IsMine)
        {
            //uiManager.healthText.text = "Здоровье: " + currentHealth.ToString();
            uiManager.healthBar.fillAmount=currentHealth*0.01f;
            uiManager.coinText.text = "Монеты: " + playerCoinCounter.ToString();
        }
    }

    private void Update()
    {
        if (!photonView.IsMine)
            return;

        //float moveHorizontal = Input.GetAxis("Horizontal");
        //float moveVertical = Input.GetAxis("Vertical");

        float moveHorizontal = joystick.Horizontal;
        float moveVertical = joystick.Vertical;

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        movement.Normalize();

        if (movement != Vector2.zero)
        {
            float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90f));
        }

        Vector2 newPosition = rb.position + movement * moveSpeed * Time.fixedDeltaTime;
        newPosition.x = Mathf.Clamp(newPosition.x, -cameraWidth + 1, cameraWidth - 1);
        newPosition.y = Mathf.Clamp(newPosition.y, -cameraHeight + 1, cameraHeight - 1);
        rb.MovePosition(newPosition);

        rb.velocity = movement * moveSpeed;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            GameObject coinManagerObject = GameObject.FindWithTag("CoinManager");
            CoinManager coinManager = coinManagerObject.GetComponent<CoinManager>();
            coinManager.DestroyCoin(other.gameObject);

            if (!photonView.IsMine)
                return;

            playerCoinCounter++;
            UIManager uiManager = FindObjectOfType<UIManager>();
            if (uiManager != null)
            {
                uiManager.coinText.text = "Монеты: " + playerCoinCounter.ToString();
            }
        }
    }

    private void Shoot()
    {
        if (!photonView.IsMine)
            return;

        Vector3 spawnPosition = bulletSpawnPoint.position;
        GameObject bullet = PhotonNetwork.Instantiate(bulletPrefab.name, spawnPosition, transform.rotation);
        //BulletScript bulletScript = bullet.GetComponent<BulletScript>();
        //bulletScript.speed = 30f;
        //bulletScript.damage = 33;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        photonView.RPC("UpdateHealth", RpcTarget.All, currentHealth);
        if (currentHealth <= 0)  Death(); 
    }

    private void Death()
    {
        isAlive=false;
        photonView.RPC("DestroyObject", RpcTarget.All);
        gameController.IncreaseDeadPlayersCounter();
    }

    [PunRPC]
    private void DestroyObject()
    {
        if (photonView.IsMine) PhotonNetwork.Destroy(gameObject);        
    }

    public int GetPlayerCoinCounter()
    {
        //int i = playerCoinCounter;
        return playerCoinCounter;
    }
    public bool IsAlive()
    {
        return isAlive;
        //if (currentHealth > 0) return true;
        //else return false;
    }

    [PunRPC]
    private void UpdateHealth(int newHealth)
    {
        currentHealth = newHealth;

        UIManager uiManager = FindObjectOfType<UIManager>();
        if (photonView.IsMine && uiManager != null && uiManager.healthText != null)
        {
            //uiManager.healthText.text = "Здоровье: " + currentHealth.ToString();
            uiManager.healthBar.fillAmount = currentHealth * 0.01f;
        }
    }

}
