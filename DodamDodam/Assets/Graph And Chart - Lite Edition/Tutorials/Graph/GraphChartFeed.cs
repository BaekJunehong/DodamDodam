using UnityEngine;
using ChartAndGraph;
using System.Collections;
using System.Collections.Generic;

public class GraphChartFeed : MonoBehaviour
{
    public List<(int, float)> tupleList = new List<(int, float)>
    {
        (5, 5),
        (6, 150),
        (7, 120),
        (8, 130),
        (9, 106.24f),
        (10, 130)
    };
	void Start ()
    {

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
                graph.DataSource.AddPointToCategory("Player 1",tupleList[i].Item1, tupleList[i].Item2);

                if( i == tupleList.Count-1)
                {
                    graph.HorizontalValueToStringMap[tupleList[i].Item1] = "현재";
                }
                else
                {
                    if ( i == 0)
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
