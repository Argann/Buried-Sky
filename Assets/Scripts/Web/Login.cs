using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.SceneManagement;

namespace web_assets {

    public class Login : MonoBehaviour {

        [SerializeField]
        private InputField username;

        [SerializeField]
        private InputField password;

        private bool isLogged;

	    // Use this for initialization
	    void Start () {
            isLogged = false;
            ConnectionManager.Connect();
            addListeners();
        }

        void Update() {
            if (isLogged) {
                SceneManager.LoadSceneAsync("Lobby", LoadSceneMode.Single);
                isLogged = false;
            }
        }
	
	    public void TryToLogin() {
            Dictionary<string, string> credentials = new Dictionary<string, string>();

            credentials.Add("username", username.text);
            credentials.Add("password", password.text);

            ConnectionManager.Socket.Emit("login", JsonConvert.SerializeObject(credentials));
        }

        void OnApplicationQuit() {
            ConnectionManager.Disconnect();
        }

        private void addListeners() {
            ConnectionManager.Socket.On("login-response", (d) => {
                JObject data = (JObject)d;

                if ((bool)data["success"]) {
                    isLogged = true;
                } else {
                    Debug.Log("It's not OK");
                }
            });
        }
    }
}