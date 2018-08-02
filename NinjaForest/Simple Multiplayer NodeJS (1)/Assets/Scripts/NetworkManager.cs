using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using SocketIO;

public class NetworkManager : MonoBehaviour {

	public static NetworkManager instance;
	public Canvas canvas;
	public SocketIOComponent socket;
	public InputField playerNameInput;
    public GameObject Chara_4Hero;

    public Text chat1, chat2, chat3, chat4, chat5, chat6, chat7, chat8, chat9, chat10, chat11, chat12, chat13, chat14, chat15;
    public InputField textInput;

    void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);
		//DontDestroyOnLoad(gameObject);
	}

	// Use this for initialization
	void Start () {
		socket.On("other player connected", OnOtherPlayerConnected);
		socket.On("play", OnPlay);
		socket.On("player move", OnPlayerMove);
		socket.On("player turn", OnPlayerTurn);
		socket.On("health", OnHealth);
		socket.On("other player disconnected", OnOtherPlayerDisconnect);
        socket.On("newMsg", OnNewMsg);
        socket.On("animation", OnAnimation);
        socket.On("player shoot", OnPlayerShoot);
    }
    public void disconnect()
    {
        socket.Off("other player connected", OnOtherPlayerConnected);
        socket.Off("play", OnPlay);
        socket.Off("player move", OnPlayerMove);
        socket.Off("player turn", OnPlayerTurn);
        socket.Off("health", OnHealth);
        socket.Off("other player disconnected", OnOtherPlayerDisconnect);
        socket.Off("newMsg", OnNewMsg);
        socket.Off("animation", OnAnimation);
        socket.Off("player shoot", OnPlayerShoot);
    }
    public void reStart()
    {
        socket.On("other player connected", OnOtherPlayerConnected);
        socket.On("play", OnPlay);
        socket.On("player move", OnPlayerMove);
        socket.On("player turn", OnPlayerTurn);
        socket.On("health", OnHealth);
        socket.On("other player disconnected", OnOtherPlayerDisconnect);
        socket.On("newMsg", OnNewMsg);
        socket.On("animation", OnAnimation);
        socket.On("player shoot", OnPlayerShoot);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if(textInput.text != string.Empty){
                string text = textInput.text;
                string name = playerNameInput.text;
                NewMsgJSON NewMsgJSON = new NewMsgJSON(name, text);
                ChattingUpdate(name, text);
                socket.Emit("newMsg", new JSONObject(JsonUtility.ToJson(NewMsgJSON)));
                textInput.text = string.Empty;
            }
        }
    }
    void ChattingUpdate(string p_name, string p_text)
    {
        string chat = p_text;
        string play1 = p_name;
        string playerchat = play1 + ">" + chat;
        chat15.text = chat14.text;
        chat14.text = chat13.text;
        chat13.text = chat12.text;
        chat12.text = chat11.text;
        chat11.text = chat10.text;
        chat10.text = chat9.text;
        chat9.text = chat8.text;
        chat8.text = chat7.text;
        chat7.text = chat6.text;
        chat6.text = chat5.text;
        chat5.text = chat4.text;
        chat4.text = chat3.text;
        chat3.text = chat2.text;
        chat2.text = chat1.text;
        chat1.text = playerchat;
    }

    public void JoinGame()
	{
		StartCoroutine(ConnectToServer());
	}

	#region Commands

	IEnumerator ConnectToServer()
	{
        canvas.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.5f);
		yield return new WaitForSeconds(1f);

		string playerName = playerNameInput.text;
		List<SpawnPoint> playerSpawnPoints = GetComponent<PlayerSpawner>().playerSpawnPoints;
		PlayerJSON playerJSON = new PlayerJSON(playerName, playerSpawnPoints);
		string data = JsonUtility.ToJson(playerJSON);
        socket.Emit("player connect");
        socket.Emit("play", new JSONObject(data));
		canvas.gameObject.SetActive(false);
    }
    public void CommandAnimation(string motion)
    {
        string data = JsonUtility.ToJson(new AnimationJSON(playerNameInput.text, motion));
        socket.Emit("animation", new JSONObject(data));
    }

    public void CommandMove(Vector3 vec3)
	{
		string data = JsonUtility.ToJson(new PositionJSON(vec3));
		socket.Emit("player move", new JSONObject(data));
	}

	public void CommandTurn(Quaternion quat)
	{
		string data = JsonUtility.ToJson(new RotationJSON(quat));
		socket.Emit("player turn", new JSONObject(data));
	}

	public void CommandHealthChange(GameObject playerFrom, GameObject playerTo, int healthChange, bool isEnemy)
	{
		print("health change cmd");
		HealthChangeJSON healthChangeJSON = new HealthChangeJSON(playerTo.name, healthChange, playerFrom.name, isEnemy);
		socket.Emit("health", new JSONObject(JsonUtility.ToJson(healthChangeJSON)));
	}
    public void CommandShoot()
    {
        print("Shoot");
        socket.Emit("player shoot");
    }
    #endregion

    #region Listening
    void OnNewMsg(SocketIOEvent socketIOEvent)
    {
        NewMsgJSON msgJSON = NewMsgJSON.CreateFromJSON(socketIOEvent.data.ToString());
        ChattingUpdate(msgJSON.name, msgJSON.text);
    }

    void OnAnimation(SocketIOEvent socketIOEvent)
    {
        AnimationJSON aniJSON = AnimationJSON.CreateFromJSON(socketIOEvent.data.ToString());
        Debug.Log(aniJSON.name);
        GameObject o = GameObject.Find(aniJSON.name) as GameObject;
        Debug.Log(aniJSON.motion);
        Debug.Log(o ==null);
        if (o == null)
        {
            return;
        }
        SampleAnimation pc = o.GetComponent<SampleAnimation>();
        Debug.Log(aniJSON.motion);
        switch (aniJSON.motion)
        {
            case "run":
                pc.setRun();
                break;
            case "unrun":
                pc.unRun();
                break;
            case "attack1":
                pc.setAttack1();
                break;
            case "unattack1":
                pc.unAttack1();
                break;
            case "attack2":
                pc.setAttack2();
                break;
            case "unattack2":
                pc.unAttack2();
                break;
            case "jump":
                pc.setJump();
                break;
            case "unjump":
                pc.unJump();
                break;
        }

    }

    void OnOtherPlayerConnected(SocketIOEvent socketIOEvent)
	{
		print("Someone else joined");
		string data = socketIOEvent.data.ToString();
		UserJSON userJSON = UserJSON.CreateFromJSON(data);
		Vector3 position = new Vector3(userJSON.position[0], userJSON.position[1], userJSON.position[2]);
		Quaternion rotation = Quaternion.Euler(userJSON.rotation[0], userJSON.rotation[1], userJSON.rotation[2]);
		GameObject o = GameObject.Find(userJSON.name) as GameObject;
		if (o != null)
		{
			return;
		}
        GameObject p = Instantiate(Chara_4Hero, position, rotation) as GameObject;
        Controller pc = p.GetComponent<Controller>();
        SampleAnimation pa = p.GetComponent<SampleAnimation>();
        pc.isLocalPlayer = false;
        pa.isLocal = false;


        Transform t = p.transform.Find("Healthbar Canvas");
		Transform t1 = t.transform.Find("Player Name");
		Text playerName = t1.GetComponent<Text>();
		playerName.text = userJSON.name;
		p.name = userJSON.name;

		Health h = p.GetComponent<Health>();
		h.currentHealth = userJSON.health;
		h.OnChangeHealth();
    }
	void OnPlay(SocketIOEvent socketIOEvent)
	{
		print("you joined");
		string data = socketIOEvent.data.ToString();
		UserJSON currentUserJSON = UserJSON.CreateFromJSON(data);
		Vector3 position = new Vector3(currentUserJSON.position[0], currentUserJSON.position[1], currentUserJSON.position[2]);
		Quaternion rotation = Quaternion.Euler(currentUserJSON.rotation[0], currentUserJSON.rotation[1], currentUserJSON.rotation[2]);
        GameObject p = Instantiate(Chara_4Hero, position, rotation) as GameObject;
        Controller pc = p.GetComponent<Controller>();
        SampleAnimation pa = p.GetComponent<SampleAnimation>();
        pa.isLocal = true;
        pc.isLocalPlayer = true;

        Transform t = p.transform.Find("Healthbar Canvas");
		Transform t1 = t.transform.Find("Player Name");
		Text playerName = t1.GetComponent<Text>();
		playerName.text = currentUserJSON.name;
		p.name = currentUserJSON.name;
        playerNameInput.text = currentUserJSON.name;
    }
	void OnPlayerMove(SocketIOEvent socketIOEvent)
	{
		string data = socketIOEvent.data.ToString();
		UserJSON userJSON = UserJSON.CreateFromJSON(data);
		Vector3 otherPosition = new Vector3(userJSON.position[0], userJSON.position[1], userJSON.position[2]);

        GameObject p = GameObject.Find(userJSON.name) as GameObject;
        Controller pc = p.GetComponent<Controller>();
        pc.inputPosition(otherPosition);

        if (p != null)
        {
            if(p.transform.position != otherPosition)
            {
                p.transform.position = otherPosition;
            }
        }
    }
	void OnPlayerTurn(SocketIOEvent socketIOEvent)
	{
		string data = socketIOEvent.data.ToString();
		UserJSON userJSON = UserJSON.CreateFromJSON(data);
		Quaternion otherRotation = Quaternion.Euler(userJSON.rotation[0], userJSON.rotation[1], userJSON.rotation[2]);

        GameObject p = GameObject.Find(userJSON.name) as GameObject;
        Controller pc = p.GetComponent<Controller>();
        pc.inputRotation(otherRotation);
        
        if (p != null)
        {
            if (p.transform.rotation != otherRotation)
            {
                p.transform.rotation = otherRotation;
            }
        }
    }

	void OnHealth(SocketIOEvent socketIOEvent)
	{
		print("changing the health");
		string data = socketIOEvent.data.ToString();
		UserHealthJSON userHealthJSON = UserHealthJSON.CreateFromJSON(data);
		GameObject p = GameObject.Find(userHealthJSON.name);
		Health h = p.GetComponent<Health>();
		h.currentHealth = userHealthJSON.health;
		h.OnChangeHealth();
	}

	void OnOtherPlayerDisconnect(SocketIOEvent socketIOEvent)
	{
		string data = socketIOEvent.data.ToString();
		UserJSON userJSON = UserJSON.CreateFromJSON(data);
        print(userJSON.name+" is disconnected");
        Destroy(GameObject.Find(userJSON.name));
	}

    public void localDisconnect()
    {
        Destroy(GameObject.Find(playerNameInput.text));
        disconnect();
        reStart();
        Destroy(GameObject.Find(playerNameInput.text));
        JoinGame();
        Destroy(GameObject.Find(playerNameInput.text));
    }

    void OnPlayerShoot(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        ShootJSON userHealthJSON = ShootJSON.CreateFromJSON(data);
        GameObject p = GameObject.Find(userHealthJSON.name);
        Controller pc = p.GetComponent<Controller>();
        pc.CmdFire();
    }

    #endregion

    #region JSONMessageClasses
    [Serializable]
    public class NewMsgJSON
    {
        public string name;
        public string text;

        public NewMsgJSON(string _name, string _text)
        {
            name = _name;
            text = _text;
        }
        public static NewMsgJSON CreateFromJSON(string data)
        {
            return JsonUtility.FromJson<NewMsgJSON>(data);
        }
    }

    [Serializable]
    public class AnimationJSON
    {
        public string name;
        public string motion;

        public AnimationJSON(string name, string motion)
        {
            this.name = name;
            this.motion = motion;
        }
        public static AnimationJSON CreateFromJSON(string data)
        {
            return JsonUtility.FromJson<AnimationJSON>(data);
        }
    }

    [Serializable]
	public class PlayerJSON
	{
		public string name;
		public List<PointJSON> playerSpawnPoints;

		public PlayerJSON(string _name, List<SpawnPoint> _playerSpawnPoints)
		{
			playerSpawnPoints = new List<PointJSON>();
			name = _name;
			foreach (SpawnPoint playerSpawnPoint in _playerSpawnPoints)
			{
				PointJSON pointJSON = new PointJSON(playerSpawnPoint);
				playerSpawnPoints.Add(pointJSON);
			}
		}
	}

	[Serializable]
	public class PointJSON
	{
		public float[] position;
		public float[] rotation;
		public PointJSON(SpawnPoint spawnPoint)
		{
			position = new float[] {
				spawnPoint.transform.position.x,
				spawnPoint.transform.position.y,
				spawnPoint.transform.position.z
			};
			rotation = new float[] {
				spawnPoint.transform.eulerAngles.x,
				spawnPoint.transform.eulerAngles.y,
				spawnPoint.transform.eulerAngles.z
			};
		}
	}

	[Serializable]
	public class PositionJSON
	{
		public float[] position;

		public PositionJSON(Vector3 _position)
		{
			position = new float[] { _position.x, _position.y, _position.z };
		}
	}

	[Serializable]
	public class RotationJSON
	{
		public float[] rotation;

		public RotationJSON(Quaternion _rotation)
		{
			rotation = new float[] { _rotation.eulerAngles.x,
				_rotation.eulerAngles.y, 
				_rotation.eulerAngles.z };
		}
	}

	[Serializable]
	public class UserJSON
	{
		public string name;
		public float[] position;
		public float[] rotation;
		public int health;

		public static UserJSON CreateFromJSON(string data)
		{
			return JsonUtility.FromJson<UserJSON>(data);
		}
	}

	[Serializable]
	public class HealthChangeJSON
	{
		public string name;
		public int healthChange;
		public string from;
		public bool isEnemy;

		public HealthChangeJSON(string _name, int _healthChange, string _from, bool _isEnemy)
		{
			name = _name;
			healthChange = _healthChange;
			from = _from;
			isEnemy = _isEnemy;
		}
	}

	[Serializable]
	public class EnemiesJSON
	{
		public List<UserJSON> enemies;

		public static EnemiesJSON CreateFromJSON(string data)
		{
			return JsonUtility.FromJson<EnemiesJSON>(data);
		}
	}

	[Serializable]
	public class ShootJSON
	{
		public string name;

		public static ShootJSON CreateFromJSON(string data)
		{
			return JsonUtility.FromJson<ShootJSON>(data);
		}
	}

	[Serializable]
	public class UserHealthJSON
	{
		public string name;
		public int health;

		public static UserHealthJSON CreateFromJSON(string data)
		{
			return JsonUtility.FromJson<UserHealthJSON>(data);
		}
	}

	#endregion
}
