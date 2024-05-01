using System;
using System.Collections;
using System.Net.Sockets;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loginRegister : MonoBehaviour
{
    private TcpClient client;
    private string message;

    void Start()
    {
        // 서버의 IP 주소와 포트 번호를 입력합니다.
        string serverIP = "35.216.33.115";
        int serverPort = 3000;

        try {
            client = new TcpClient(serverIP, serverPort);
        } catch (Exception e) {
            Debug.Log(e);
        }
    }

    public void OnLoginButtonClicked()
    {
        string username = GameObject.Find("ID_inputField").GetComponent<TMP_InputField>().text;
        string password = GameObject.Find("PW_inputField").GetComponent<TMP_InputField>().text;

        NetworkStream write_stream = client.GetStream();
        SendData(write_stream, "login", username, password);
        NetworkStream read_stream = client.GetStream();
        while (!read_stream.DataAvailable);
        ReadData(read_stream);
        write_stream.Close();
        read_stream.Close();
    }

    public void OnSignUpButtonClicked()
    {
        string username = GameObject.Find("newID_inputField").GetComponent<TMP_InputField>().text;
        string password = GameObject.Find("newPW_inputField").GetComponent<TMP_InputField>().text;
        string name = GameObject.Find("NAME_inputField").GetComponent<TMP_InputField>().text;
        string birthdate = GameObject.Find("BIRTH_inputField").GetComponent<TMP_InputField>().text;

        NetworkStream write_stream = client.GetStream();
        SendData(write_stream, "signup", username, password, name, birthdate);
        NetworkStream read_stream = client.GetStream();
        while (!read_stream.DataAvailable);
        ReadData(read_stream);
        write_stream.Close();
        read_stream.Close();
    }

    private void SendData(NetworkStream write_stream, string command, string username, string password, string name = "", string birthdate = "")
    {
        if (client == null || !client.Connected) {
            return;
        }

        try {
            if (write_stream.CanWrite) {
                string data = $"{command},{username},{password},{name},{birthdate}";
                byte[] bytes = Encoding.UTF8.GetBytes(data);

                write_stream.Write(bytes, 0, bytes.Length);
            }
        } catch (SocketException s) {
            Debug.Log(s);
        }
    }

    public void ReadData(NetworkStream read_stream)
    {
        if (client == null || !client.Connected) {
            return;
        }

        try {
            string message = "";
            byte[] data = new byte[1024];

            int bytesRead = read_stream.Read(data, 0, data.Length);
            message = Encoding.UTF8.GetString(data, 0, bytesRead);
            Debug.Log(message);

            if (message == "Login successful\n") {
                SceneManager.LoadScene("Success");
            } else if (message == "Sign up successful\n") {
                GameObject.Find("signup_popup").SetActive(false);
            }
        } catch (SocketException s) {
            Debug.Log(s);
        }
    }

    private void OnApplicationQuit() {
        if (client != null) {
            client.Close();
        }
    }
}