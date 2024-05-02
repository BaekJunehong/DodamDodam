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

    public GameObject signup_popup;

    void Start()
    {
        // 서버의 IP 주소와 포트 번호를 입력
        string serverIP = "35.216.33.115";
        int serverPort = 3000;

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

        signup_popup.SetActive(false);
    }

    // 서버로 로그인 혹은 회원가입 데이터 전송
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

    // 서버로부터 로그인 혹은 회원가입 결과 메시지 수신 및 처리
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