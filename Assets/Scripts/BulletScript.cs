using Photon.Pun;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float speed = 30f;
    public int damage = 10; 
    private float timer = 2f; 

    private PhotonView photonView;
    private Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        photonView = GetComponent<PhotonView>();
    }
    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            photonView.RPC("DestroyObject", RpcTarget.All);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            {
                player.TakeDamage(damage);
            }
            photonView.RPC("DestroyObject", RpcTarget.All);
        }
    }
    [PunRPC]
    private void DestroyObject()
    {
        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
