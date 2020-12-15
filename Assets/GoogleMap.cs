using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoogleMap : MonoBehaviour
{
    public RawImage mapRawImage;

    string url;

    public float lat = 35.885100f;
    public float lon = 128.626949f;

    public int zoom = 18;
    public int mapWidth = 300;
    public int mapHeight = 300;

    public enum mapType { road, satellite, hybrid, terrain }
    public mapType mapSelected;
    public int scale;

    float monsterLat = 35.885778f;
    float monsterLon = 128.627866f;

    public GameObject monsterPrefab;

    static float UPDATE_TIME = 3f;
    float updateTime = UPDATE_TIME;

    IEnumerator Map()
    {
        url = "https://maps.googleapis.com/maps/api/staticmap?center=" + lat + "," + lon +
            "&zoom=" + zoom + "&size" + mapWidth + "x" + mapHeight + "&scale" + scale
            + "&maptype" + mapSelected +
            "&key=AIzaSyAHA_IVgU-ZXH0iF3rd2gqZQvWf-mTww-E" +
            "&markers=color:red%7Clabel:C%7C" + lat + "," + lon +
            "&markers=color:green%7Clabel:C%7C" + monsterLat + "," + monsterLon;

        WWW www = new WWW(url);
        yield return www;
        mapRawImage.texture = www.texture;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartGPS());

        mapRawImage = gameObject.GetComponent<RawImage>();
        StartCoroutine(Map());
    }

    // Update is called once per frame
    private void Update()
    {
        if(updateTime >= 0)
        {
            updateTime = updateTime - Time.deltaTime;
        } else
        {
            updateTime = UPDATE_TIME;

            float latitude = Input.location.lastData.latitude;
            float longitude = Input.location.lastData.longitude;

            if(latitude != 0f && longitude != 0)
            {
                if(lat != latitude || lon != longitude)
                {
                    lat = latitude;
                    lon = longitude;

                    StartCoroutine(Map());

                    if(Mathf.Abs(lat - monsterLat) < 0.0001f && Mathf.Abs(lon - monsterLon) < 0.0001f)
                    {
                        GameObject monster = Instantiate(monsterPrefab);
                    }
                }
            }
        }
    }

    IEnumerator StartGPS()
    {
        if (!Input.location.isEnabledByUser)
            yield break;

        Input.location.Start();

        int maxWait = 20;
        while(Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if(maxWait < 1)
        {
            print("Time Out");
            yield break;
        }

        if(Input.location.status == LocationServiceStatus.Failed)
        {
            print("Unable to determine device location");
            yield break;
        } else
        {
            print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);

        }
    }

    public void exitGame()
    {
        Application.Quit();
    }

    public void resetGPS()
    {
        lat = 35.885100f;
        lon = 128.626949f;
    }
}
