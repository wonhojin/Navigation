using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Test : MonoBehaviour
{
    public GameObject AObjectPrefab;
    public GameObject BObjectPrefab;
    public GameObject CObjectPrefab;
    public GameObject UserPrefab;

    private GameObject AObject;
    private GameObject BObject;
    private GameObject CObject;
    private GameObject User;


    double x1;
    double y1;
    double x2;
    double y2;
    double x3;
    double y3;

    double r1;
    double r2;
    double r3;

    double S;
    double T;


    double x;
    double y;


    private Vector3 userLocation;

    public List<GameObject> BeconList = new List<GameObject>();

    public void BeconSetting()
    {
        if (BeconList.Count != 0)
        {
            for (int i = 0; i < 3; i++)
            {
                DestroyImmediate(BeconList[0]);
                BeconList.RemoveAt(0);
            }

            Destroy(User);
        }

        List<Dictionary<string, object>> data = CSVReader.Read("beacorn name");


        AObject = Instantiate(AObjectPrefab);
        AObject.transform.position = new Vector3(Convert.ToSingle(data[0]["x coor"]), Convert.ToSingle(data[0]["y coor"]), Convert.ToSingle(data[0]["z coor"]));
        BeconList.Add(AObject);

        BObject = Instantiate(BObjectPrefab);
        BObject.transform.position = new Vector3(Convert.ToSingle(data[1]["x coor"]), Convert.ToSingle(data[1]["y coor"]), Convert.ToSingle(data[1]["z coor"]));
        BeconList.Add(BObject);

        CObject = Instantiate(CObjectPrefab);
        CObject.transform.position = new Vector3(Convert.ToSingle(data[2]["x coor"]), Convert.ToSingle(data[2]["y coor"]), Convert.ToSingle(data[2]["z coor"]));
        BeconList.Add(CObject);

        User = Instantiate(UserPrefab);
        User.transform.position = new Vector3(UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(-10, 10), 0);

    }


    public void Trilateration()
    {
        x1 = AObject.transform.position.x;
        y1 = AObject.transform.position.y;

        x2 = BObject.transform.position.x;
        y2 = BObject.transform.position.y;

        x3 = CObject.transform.position.x;
        y3 = CObject.transform.position.y;


        r1 = Vector3.Distance(User.transform.position, AObject.transform.position);
        r2 = Vector3.Distance(User.transform.position, BObject.transform.position);
        r3 = Vector3.Distance(User.transform.position, CObject.transform.position);

        S = (Math.Pow(x3, 2) - Math.Pow(x2, 2) + Math.Pow(y3, 2) - Math.Pow(y2, 2)
            + Math.Pow(r2, 2) - Math.Pow(r3, 2)) / 2;

        T = (Math.Pow(x1, 2) - Math.Pow(x2, 2) + Math.Pow(y1, 2) - Math.Pow(y2, 2)
            + Math.Pow(r2, 2) - Math.Pow(r1, 2)) / 2;


        y = ((T * (x2 - x3)) - (S * (x2 - x1))) / (((y1 - y2) * (x2 - x3)) - ((y3 - y2) * (x2 - x1)));
        x = ((y * (y1 - y2)) - T) / (x2 - x1);

        userLocation = new Vector3((float)x, (float)y, 0);

        Debug.Log(userLocation);
    }
