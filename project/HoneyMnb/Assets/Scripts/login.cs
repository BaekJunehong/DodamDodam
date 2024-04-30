using System;
using System.Collections;
using System.Net.Sockets;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TCPClient : MonoBehaviour
{
    private TcpClient client;
    private NetworkStream stream;
    private string message;

    void Start()
    {
        // 서버의 IP 주소와 포트 번호를 입력합니다.
        string serverIP = "35.216.33.115";
        int serverPort = 3000;

        try {
            client = new TcpClient(serverIP, serverPort);
            stream = client.GetStream();
        } catch (Exception e) {
            Debug.Log(e);
        }

        StartCoroutine(ReadData());
    }

    IEnumerator ReadData()
    {
        byte[] data = new byte[1024];
        while (true)
        {
            if (stream.DataAvailable)
            {
                int bytesRead = stream.Read(data, 0, data.Length);
                message = Encoding.UTF8.GetString(data, 0, bytesRead);
                Debug.Log(message);

                if (message == "Login successful\n") {
                    SceneManager.LoadScene("Success");
                } else if (message == "Sign up successful\n") {
                    GameObject.Find("signup_popup").SetActive(false);
                }
            }
            yield return null;
        }
    }

    public void OnLoginButtonClicked()
    {
        string username = GameObject.Find("ID_inputField").GetComponent<TMP_InputField>().text;
        string password = GameObject.Find("PW_inputField").GetComponent<TMP_InputField>().text;

        SendData("login", username, password);
    }

    public void OnSignUpButtonClicked()
    {
        string username = GameObject.Find("newID_inputField").GetComponent<TMP_InputField>().text;
        string password = GameObject.Find("newPW_inputField").GetComponent<TMP_InputField>().text;
        string name = GameObject.Find("NAME_inputField").GetComponent<TMP_InputField>().text;
        string birthdate = GameObject.Find("BIRTH_inputField").GetComponent<TMP_InputField>().text;

        SendData("signup", username, password, name, birthdate);
    }

    private void SendData(string command, string username, string password, string name = "", string birthdate = "")
    {
        string data = $"{command},{username},{password},{name},{birthdate}";
        byte[] bytes = Encoding.UTF8.GetBytes(data);

        stream.Write(bytes, 0, bytes.Length);
    }

    void OnApplicationQuit()
    {
        stream.Close();
        client.Close();
    }
}