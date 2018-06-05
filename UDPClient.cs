using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System;
using System.Text;
using SimpleJSON;

public class UDPClient : MonoBehaviour {
    UdpClient myClient;
    EndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 20000);
    Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
    IPEndPoint sendIP = new IPEndPoint(System.Net.IPAddress.Parse("172.31.216.111"), 9891);
      public int clientNumber = 0;
    public static List<GameObject> ObjectsToSend;
    
    List<string> receivedText;
    
    int setClient;
    GameObject set;
	// Use this for initialization
	void Start () {
        Application.runInBackground = true;
        ObjectsToSend = new List<GameObject>();
        receivedText = new List<string>();
        int receiverPort = 20000;
        myClient = new UdpClient(receiverPort);
        myClient.BeginReceive(DataReceived, myClient);
        //myClient.Connect("localhost", 9891);
        Time.fixedDeltaTime = 0.05f;
	}

    // Update is called once per frame
    void FixedUpdate()
    {
        if (ObjectsToSend.Capacity > 0)
        foreach (GameObject g in ObjectsToSend) // get all gameobjects i need to send to the server.
        {
                //Debug.Log(g.transform.position);
            ServerObject send;
            if (g.GetComponent<Rigidbody>())
            {
                 send = new ServerRigidBody(g.GetComponent<Rigidbody>(), clientNumber);
            }
            else
            {
                 send = new ServerObject(g, clientNumber);
                
            }
            send.pos = g.transform.position;
               
            string json;
            
            json = JsonUtility.ToJson(send);

            //Debug.Log(json);
            byte[] json_bytes = System.Text.Encoding.ASCII.GetBytes(json);
            
            //socket.Connect(sendIP);
            socket.SendTo(json_bytes, sendIP);
             Debug.Log(json);
        }
        else
        {
            Debug.Log("Connect");
            ServerObject send = new ServerObject();
            send.clientNumber = 0;
            string json;
            json = JsonUtility.ToJson(send);
            byte[] json_bytes = System.Text.Encoding.ASCII.GetBytes(json);
            socket.SendTo(json_bytes, sendIP);
        }
        
        
        
        

    }
    private void Update()
    {
        
        checkData();
    }
    private void DataReceived(IAsyncResult ar)
    {
        Debug.Log("Receiving message");
        UdpClient c = (UdpClient)ar.AsyncState;
        
        IPEndPoint receivedIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
        Byte[] receivedBytes = c.EndReceive(ar, ref receivedIpEndPoint);

        // Convert data to ASCII and print in console
        lock (receivedText)
        {
            receivedText.Add(ASCIIEncoding.ASCII.GetString(receivedBytes));
            //c.BeginReceive(DataReceived, myClient);
        }
        
    }
    public void checkData()
    {
        lock (receivedText)
        {
            if (receivedText.Count > 0)
            {
                foreach (string s in receivedText)
                {
                    string temp = s;
                    if (temp.Contains("SetClient: "))
                    {
                        //IPEndPoint receivedIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                        myClient.Close();
                        temp = temp.Remove(0, 10);
                        clientNumber = Convert.ToInt32(temp);
                        //Debug.Log(temp);
                        clientFinder.finder.setMe(clientNumber);
                        //myClient.Close();
                        myClient = new UdpClient(20000 + clientNumber);
                        myClient.BeginReceive(DataReceived, myClient);
                        //RemoteIpEndPoint =  new IPEndPoint(IPAddress.Any, 20000 + clientNumber);

                    }
                    else
                    {

                        Debug.Log("here now");
                        JsonUtility.FromJson<int>(temp);
                        JSONNode n = JSON.Parse(temp);
                        //n = n["RigidBody"];
                        Debug.Log(n);
                        //Rigidbody r = ObjectsToSend[0].GetComponent<Rigidbody>();
                        ServerRigidBody r = new ServerRigidBody();
                        Vector3 pos = new Vector3(n["position"]["x"], n["position"]["y"], n["position"]["z"]);
                        //Vector3 pos = Vector3.up;
                        Vector3 euler = new Vector3(n["euler"]["x"], n["euler"]["y"], n["euler"]["z"]);
                        Vector3 velocity = new Vector3(n["vel"]["x"], n["vel"]["y"], n["vel"]["z"]);
                        r.pos = pos;
                        r.euler = euler;
                        r.vel = velocity;
                        r.clientNumber = n["clientNumber"];
                        clientFinder.finder.setPlayer(r.clientNumber, r);
                        //Debug.Log(receivedIpEndPoint + ": " + receivedText + Environment.NewLine);
                        myClient.BeginReceive(DataReceived, myClient);
                        // Restart listening for udp data packages

                    }
                }
                
                 receivedText = new List<string>();
                
            }
            
        }
    }
}
[System.Serializable]
public class ServerObject
{
    
    public int clientNumber;
    public Vector3 pos;
    public Vector3 euler;
    public ServerObject(GameObject g, int client)
    {
        clientNumber = client;
        pos = g.transform.position;
        euler = g.transform.eulerAngles;
    }
    public ServerObject(int client)
    {
        clientNumber = client;
    }
    public ServerObject()
    {

    }
}
[System.Serializable]
public class ServerRigidBody : ServerObject
{
    public float Mass, Drag, Angular_Drag;
    public bool useGravity = true, isKinematic = false;
    public RigidbodyInterpolation interpolate;
    public CollisionDetectionMode collision_detection;
    public Vector3 vel;
    //public RigidbodyConstraints contraints;
    public ServerRigidBody(Rigidbody r, int client)
    {
        Mass = r.mass;
        Drag = r.drag;
        Angular_Drag = r.angularDrag;

        useGravity = r.useGravity;
        isKinematic = r.isKinematic;

        interpolate = r.interpolation;
        collision_detection = r.collisionDetectionMode;
        //contraints = r.constraints;
        vel = r.velocity;
        pos = r.gameObject.transform.position;
        euler = r.gameObject.transform.eulerAngles;
        clientNumber = client; 
    }
    public ServerRigidBody()
    {

    }
    
}

