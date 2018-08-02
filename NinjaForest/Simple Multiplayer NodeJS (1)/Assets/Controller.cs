using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

public class Controller : NetworkBehaviour
{
    [SerializeField] private float m_moveSpeed = 2;
    [SerializeField] private float m_turnSpeed = 200;
    [SerializeField] private float m_jumpForce = 4;
    [SerializeField] public Animator animator;
    [SerializeField] private Rigidbody m_rigidBody;

    private float m_currentV = 0;
    private float m_currentH = 0;
    private Vector3 playerPosition;
    private Quaternion playerRotation;
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    private readonly float m_interpolation = 10;
    private readonly float m_walkScale = 0.33f;
    private readonly float m_backwardsWalkScale = 0.16f;
    private readonly float m_backwardRunScale = 0.66f;

    private bool m_wasGrounded;
    private Vector3 m_currentDirection = Vector3.zero;

    private float m_jumpTimeStamp = 0;
    private float m_minJumpInterval = 0.25f;

    private bool m_isGrounded;
    private List<Collider> m_collisions = new List<Collider>();

    public GameObject playerCamera;
    public GameObject text;
    public GameObject gameobject;

    public bool isLocalPlayer = false;


    public void inputPosition(Vector3 position)
    {
        playerPosition = position;
    }

    public void inputRotation(Quaternion rotation)
    {
        playerRotation = rotation;
    }
   
    void Start()
    {
        if (isLocalPlayer == true) { playerCamera.SetActive(true); }
        else playerCamera.SetActive(false);
    }

    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        if (v < 0)
        {
            v *= m_backwardRunScale;
        }
        m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
        m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);
        Vector3 oldPosition = transform.position;
        Quaternion oldRotation = transform.rotation;
        this.transform.position += transform.forward * m_currentV * m_moveSpeed * Time.deltaTime;
        this.transform.Rotate(0, m_currentH * m_turnSpeed * Time.deltaTime, 0);
        if (oldPosition != transform.position || oldRotation != transform.rotation)
        {
            NetworkManager.instance.GetComponent<NetworkManager>().CommandMove(this.transform.position);
            NetworkManager.instance.GetComponent<NetworkManager>().CommandTurn(this.transform.rotation);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            NetworkManager n = NetworkManager.instance.GetComponent<NetworkManager>();
            n.CommandShoot();
            CmdFire();
        }

    }
    public void CmdFire()
    {
        var bullet = Instantiate(bulletPrefab,
                                 bulletSpawn.position,
                                 bulletSpawn.rotation) as GameObject;
        Bullet b = bullet.GetComponent<Bullet>();
        b.playerFrom = this.gameObject;
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * 10;
        Destroy(bullet, 1.0f);
    }
}
