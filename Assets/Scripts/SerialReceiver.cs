using System;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class SerialReceiver : MonoBehaviour
{
    private SerialPort stream;
    public World world;

    //[SerializeField] public List<GameObject> objects;
    [SerializeField] public List<Transform> objectTransforms;
    [SerializeField] public List<Light> objectLights;

    [SerializeField] public float windSpeed;

    // Start is called before the first frame update
    void Start()
    {
        stream = new SerialPort(SerialPort.GetPortNames()[0], 9600);
        stream.ReadTimeout = 100;
        stream.Open();

        //foreach(GameObject obj in objects)
        //{
        //    objectTransforms.Add(obj.GetComponent<Transform>());
        //    print(obj.transform.GetChild(0));
        //    objectLights.Add(obj.transform.GetChild(0).GetComponent<Light>());
        //}
    }

    // Update is called once per frame
    void Update()
    {
        string[] serialString = stream.ReadLine().Split(',');
        if (serialString != null) UpdateWorldStruct(serialString);

        if (world.windBlowing)
        {
            foreach (Transform tr in objectTransforms)
            {
                tr.position += new Vector3(world.direction.x * windSpeed * Time.deltaTime, 0, world.direction.y * windSpeed * Time.deltaTime);
            }
        }

        foreach (Light light in objectLights)
        {
            light.intensity = world.lightStrength / 30;
        }
    }

    public void UpdateWorldStruct(string[] values)
    {
        // Set wind direction
        float angle = int.Parse(values[0]) / 3.75f;
        angle += 45;
        world.angle = angle;
        world.direction = DegreeToVector2(angle);

        // Set wind bool
        world.windBlowing = int.Parse(values[1]) == 1 ? true : false;

        // Set light strength
        world.lightStrength = int.Parse(values[2]);
    }

    public void PrintWorldValues()
    {
        print("Direction " + world.direction);
        print("Wind is Blowing " + world.windBlowing);
        print("Light Strength " + world.lightStrength);
    }

    public static Vector2 DegreeToVector2(float degree)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }

    public static Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }
}

public struct World
{
    public Vector2 direction;
    public float angle;
    public bool windBlowing;
    public int lightStrength;
}
