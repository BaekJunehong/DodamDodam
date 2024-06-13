using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ChartAndGraph;

public class BarFeedChart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start () {
        BarChart barChart = GetComponent<BarChart>();
        if (barChart != null)
        {
            barChart.DataSource.SetValue("Player 1", "Value 1", Random.value * 20);
            barChart.DataSource.SlideValue("Player 2","Value 1", Random.value * 20, 40f);
        }
    }
}
