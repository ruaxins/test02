using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Single : MonoBehaviour
{

}
public class Value
{
    private string path;
    public string Path { get => path; set => path = value; }

    private string username;
    public string Username { get => username; set => username = value; }

    private string reponame;
    public string Reponame { get => reponame; set => reponame = value; }

    private string token;
    public string Token { get => token; set => token = value; }

    private static Value value;
    private Value() { }
    public static Value Instance
    {
        get
        {
            if (value == null)
            {
                value = new Value();
            }
            return value;
        }
    }
}
