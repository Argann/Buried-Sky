using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class ChatManager : MonoBehaviour {

    [Header("Links")]

    [SerializeField]
    private GameObject chatWindow;

    [SerializeField]
    private InputField input;

    [SerializeField]
    private GameObject chatText;

    [Header("Animation")]

    [SerializeField]
    private float visibleAlpha;

    [SerializeField]
    private float hiddenAlpha;

    [SerializeField]
    private float visibleHeight;

    [SerializeField]
    private float hiddenHeight;

    [SerializeField]
    private float speed;

    private CanvasGroup cg;

    private RectTransform r_cg;

    private ArrayList waitingMessages;

    private Coroutine lastRoutine;

	// Use this for initialization
	void Start () {

        waitingMessages = null;

        lastRoutine = null;

        input.enabled = false;

        cg = GetComponent<CanvasGroup>();
        cg.alpha = hiddenAlpha;

        r_cg = cg.GetComponent<RectTransform>();
        r_cg.sizeDelta = new Vector2(r_cg.sizeDelta.x, hiddenHeight);

        ConnectionManager.Socket.Emit("lobby-connection");

        ConnectionManager.Socket.On("s-message", (d) => {
            if (waitingMessages == null) {
                waitingMessages = new ArrayList();
            }
            JObject data = (JObject)d;
            waitingMessages.Add((string)data["message"]);
        });
	}

    public void FadeIn() {

        if (lastRoutine != null)
            StopCoroutine(lastRoutine);

        lastRoutine = StartCoroutine("FIn");

        input.enabled = true;
    }

    public void FadeOut() {
        if (lastRoutine != null)
            StopCoroutine(lastRoutine);

        lastRoutine = StartCoroutine("FOut");
        input.enabled = false;
    }

    private IEnumerator FIn() {
        while (cg.alpha < visibleAlpha || r_cg.sizeDelta.y < visibleHeight) {
            if (cg.alpha < visibleAlpha)
                cg.alpha += Time.deltaTime * speed;
            
            if (r_cg.sizeDelta.y < visibleHeight)
                r_cg.sizeDelta = new Vector2(r_cg.sizeDelta.x, r_cg.sizeDelta.y + (Time.deltaTime * speed * 400));

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator FOut() {
        while (cg.alpha > hiddenAlpha || r_cg.sizeDelta.y > hiddenHeight) {
            if (cg.alpha > hiddenAlpha)
                cg.alpha -= Time.deltaTime * speed;

            if (r_cg.sizeDelta.y > hiddenHeight)
                r_cg.sizeDelta = new Vector2(r_cg.sizeDelta.x, r_cg.sizeDelta.y - (Time.deltaTime * speed * 400));
            yield return new WaitForEndOfFrame();
        }
    }

    
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Return) && input.isActiveAndEnabled) {

            if (input.text.Trim().Length > 0) {
                Dictionary<string, string> message = new Dictionary<string, string>();

                message.Add("message", input.text);

                ConnectionManager.Socket.Emit("c-message", JsonConvert.SerializeObject(message));

                input.text = "";
            }
        }

        if (waitingMessages != null) {

            foreach (string item in waitingMessages) {
                GameObject go = Instantiate(chatText, chatWindow.transform);
                go.GetComponent<Text>().text = item;
            }
            

            waitingMessages = null;
        }
	}

    void OnApplicationQuit() {
        ConnectionManager.Disconnect();
    }
}
