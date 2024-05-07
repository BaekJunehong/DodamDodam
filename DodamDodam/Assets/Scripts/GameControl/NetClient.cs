using System;
using System.Collections;
using System.Net.Sockets;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetClient : MonoBehaviour
{
    private TcpClient client;
    private string serverIP = "35.216.33.115";
    private int serverPort = 50001;

    void Start()
    {
        // TcpClient에서 서버로 소켓 연결
        try {
            client = new TcpClient(serverIP, serverPort);
        } catch (Exception e) {
            Debug.Log(e);
        }
    }

    // 로그인 팝업에서 로그인 버튼 클릭 시 로그인 기능 수행
    public void OnLoginButtonClicked()
    {
        if (client == null || !client.Connected) {
            // TcpClient에서 서버로 소켓 연결
            try {
                client = new TcpClient(serverIP, serverPort);
            } catch (Exception e) {
                Debug.Log(e);
                return;
            }
        }

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

    // 회원가입 팝업에서 회원가입 버튼 클릭 시 회원가입 기능 수행
    public void OnSignUpButtonClicked()
    {
        if (client == null || !client.Connected) {
            // TcpClient에서 서버로 소켓 연결
            try {
                client = new TcpClient(serverIP, serverPort);
            } catch (Exception e) {
                Debug.Log(e);
                return;
            }
        }

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

    // 결과 팝업에서 저장 버튼 클릭 시 점수 저장 기능 수행
    public void OnSaveButtonClicked()
    {
        if (client == null || !client.Connected) {
            // TcpClient에서 서버로 소켓 연결
            try {
                client = new TcpClient(serverIP, serverPort);
            } catch (Exception e) {
                Debug.Log(e);
                return;
            }
        }

        string name = GameObject.Find("name_text").GetComponent<TextMeshProUGUI>().text;
        string score = GameObject.Find("score_text").GetComponent<TextMeshProUGUI>().text;

        NetworkStream write_stream = client.GetStream();
        SendData(write_stream, "save", name, score);
        NetworkStream read_stream = client.GetStream();
        while (!read_stream.DataAvailable);
        ReadData(read_stream);
        write_stream.Close();
        read_stream.Close();
    }

    // 서버로 로그인 혹은 회원가입 데이터 전송
    private void SendData(NetworkStream write_stream, string command, string username = "", string password = "", 
    string name = "", string birthdate = "", string score = "")
    {
        try {
            if (write_stream.CanWrite) {
                string data = $"{command},{username},{password},{name},{birthdate},{score}";
                byte[] bytes = Encoding.UTF8.GetBytes(data);

                write_stream.Write(bytes, 0, bytes.Length);
            }
        } catch (SocketException s) {
            Debug.Log(s);
        }
    }

    // 서버로부터 로그인 혹은 회원가입 결과 메시지 수신 및 처리
    public void ReadData(NetworkStream read_stream)
    {
        try {
            string message = "";
            byte[] data = new byte[1024];

            int bytesRead = read_stream.Read(data, 0, data.Length);
            message = Encoding.UTF8.GetString(data, 0, bytesRead);
            Debug.Log(message);

            if (message == "Login successful\n") {
                SceneManager.LoadScene("Camera");
            } else if (message == "User created successfully\n") {
                GameObject.Find("signup_popup").SetActive(false);
            }
        } catch (SocketException s) {
            Debug.Log(s);
        }
    }

    // 애플리케이션 종료 시 소켓 연결 종료
    private void OnApplicationQuit() {
        if (client != null) {
            client.Close();
        }
    }
}