using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ChartAndGraph;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System;

public class GraphChartFeed : MonoBehaviour
{
    private TcpClient client;
    private string serverIP = "35.216.111.151";
    private int serverPort = 50001;
    public List<(int, float)> tupleList = new List<(int, float)>{
    //    (4, 0),
    //    (5, 106.24f),
    //    (6, 123.4f)
    };
	void Start ()
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
        NetClient.SendData(write_stream, "save", name, score);
        NetworkStream read_stream = client.GetStream();
        while (!read_stream.DataAvailable);
        NetClient.ReadData(read_stream, score);
        write_stream.Close();
        read_stream.Close();

        List<int> monthList = NetClient.getMonth;
        List<float> valueList = NetClient.getValue;

        for (int i = 0; i < monthList.Count; i++) {
            tupleList.Add((monthList[i], valueList[i]));
        }

        for (int i = 0; i < tupleList.Count; i++) {
            Debug.Log(tupleList[i]);
        }

        var axis = GetComponent<HorizontalAxis>();
        axis.WithEdges = false;

        GraphChartBase graph = GetComponent<GraphChartBase>();
        if (graph != null)
        {
            
            graph.Scrollable = false;
            //graph.HorizontalValueToStringMap[0.0] = "Zero"; // example of how to set custom axis strings
            graph.DataSource.StartBatch();
            graph.DataSource.ClearCategory("Player 1");
            
            for (int i = 0; i < tupleList.Count; i++)
            {
                graph.DataSource.AddPointToCategory("Player 1", tupleList[i].Item1, tupleList[i].Item2);

                if( i == tupleList.Count-1)
                {
                    graph.HorizontalValueToStringMap[tupleList[i].Item1] = "현재";
                }
                else
                {
                    if (i == 0)
                    {
                        graph.HorizontalValueToStringMap[tupleList[i].Item1] = "0";
                    }

                    else
                    {
                        graph.HorizontalValueToStringMap[tupleList[i].Item1] = tupleList[i].Item1+"월";
                    }
                }
            }
            graph.DataSource.EndBatch();

            graph.DataSource.HorizontalViewSize = tupleList.Count-0.4f;
        }
       // StartCoroutine(ClearAll());
    }

    IEnumerator ClearAll()
    {
        yield return new WaitForSeconds(5f);
        GraphChartBase graph = GetComponent<GraphChartBase>();

        graph.DataSource.Clear();
    }
}
