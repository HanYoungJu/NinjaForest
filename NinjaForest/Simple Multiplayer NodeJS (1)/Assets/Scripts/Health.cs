using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class Health : MonoBehaviour {

	public const int maxHealth = 100;
	public bool destroyOnDeath;

	public int currentHealth = maxHealth;
	public bool isEnemy = false;

	public RectTransform healthBar;

	private bool isLocalPlayer;
    public GameObject Manager;

	// Use this for initialization
	void Start () {
        Controller pc = GetComponent<Controller>();
        isLocalPlayer = pc.isLocalPlayer;
	}

	public void TakeDamage(GameObject playerFrom, int amount) {
		currentHealth -= amount;
		OnChangeHealth();
		NetworkManager n = NetworkManager.instance.GetComponent<NetworkManager>();
		n.CommandHealthChange(playerFrom, this.gameObject, amount, isEnemy);
	}

	public void OnChangeHealth() {
		healthBar.sizeDelta = new Vector2(currentHealth, healthBar.sizeDelta.y);
		if(currentHealth <= 0 && isLocalPlayer) {
            NetworkManager n = NetworkManager.instance.GetComponent<NetworkManager>();
            n.localDisconnect();
            //n.disconnect();
            //n.reStart();
            //n.JoinGame();
            //currentHealth = 100;
        }
	}
	
	void Respawn() {
		if(isLocalPlayer) {
			Vector3 spawnPoint = Vector3.zero;
			Quaternion spawnRotation = Quaternion.Euler(0,180,0);
			transform.position = spawnPoint;
			transform.rotation = spawnRotation;
		}
	}
}
