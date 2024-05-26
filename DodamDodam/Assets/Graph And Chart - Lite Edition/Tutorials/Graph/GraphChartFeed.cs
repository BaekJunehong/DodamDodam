using UnityEngine;
using ChartAndGraph;
using System.Collections;
using System.Collections.Generic;

public class GraphChartFeed : MonoBehaviour
{
    public List<(int, int)> tupleList = new List<(int, int)>
    {
        (7, 2),
        (8, 3),
        (9, 4)
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
                    graph.HorizontalValueToStringMap[tupleList[i].Item1] = tupleList[i].Item1+"월";
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
