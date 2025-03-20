using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using UnityEditor.PackageManager;
using UnityEngine;

public class Tool : MonoBehaviour
{

}
public class Tools
{
    private string token;
    private string repoOwner;
    private string repoName;
    private string repoPath;
    private string repoUrl;
    private string downloadPath;

    public Tools()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "token.txt");
        try
        {
            if (File.Exists(filePath))
            {
                // ʹ�� File.ReadAllLines ��ȡ������
                string[] lines = File.ReadAllLines(filePath);
                Value.Instance.Token = lines[0];
                Value.Instance.Username = lines[1];
                Value.Instance.Reponame = lines[2];
                Value.Instance.Path = lines[3];
            }
            else
            {
                UnityEngine.Debug.Log("�Ҳ����ļ�");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
        UnityEngine.Debug.Log(Value.Instance.Token);
        UnityEngine.Debug.Log(Value.Instance.Username);
        UnityEngine.Debug.Log(Value.Instance.Reponame);
        UnityEngine.Debug.Log(Value.Instance.Path);

        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
        token = Value.Instance.Token; // ���GitHub���˷�������
        repoOwner = Value.Instance.Username; // ���GitHub�û���
        repoName = Value.Instance.Reponame; // ���GitHub�ֿ���
        repoPath = Value.Instance.Path;
        repoUrl = "https://github.com/{repoOwner}/{repoName}.git";
        downloadPath = Value.Instance.Path;
    }


    //������ȡ����:title_������⣬head_��ǰ��֧��baseBranch_Ŀ���֧��body_ע��
    //��������
    public int CreatePullRequest(string title, string head, string baseBranch, string body)
    {
        string url = $"https://api.github.com/repos/{repoOwner}/{repoName}/pulls";
        string json = $"{{\"title\": \"{title}\", \"head\": \"{head}\", \"base\": \"{baseBranch}\", \"body\": \"{body}\"}}";

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "POST";
        request.ContentType = "application/json";
        request.Accept = "application/vnd.github.v3+json";
        request.Headers.Add("Authorization", $"token {token}");
        request.UserAgent = "UnityGitHubPlugin";

        byte[] data = Encoding.UTF8.GetBytes(json);
        request.ContentLength = data.Length;

        using (Stream stream = request.GetRequestStream())
        {
            stream.Write(data, 0, data.Length);
        }

        try
        {
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                string responseText = reader.ReadToEnd();
                // ����JSON��Ӧ����ȡ��ȡ������
                return ParsePRNumber(responseText);
            }
        }
        catch (WebException ex)
        {
            using (var stream = ex.Response.GetResponseStream())
            using (var reader = new StreamReader(stream))
            {
                string responseText = reader.ReadToEnd();
                return -1;
            }
        }
    }
    //�ϲ���ȡ����:pullNumber_��ȡ����ı�ţ�commitTitle_�ϲ����⣬commitMessage_�ϲ�ע��
    //ͬ������
    public void MergePullRequest(int pullNumber, string commitTitle, string commitMessage)
    {
        string url = $"https://api.github.com/repos/{repoOwner}/{repoName}/pulls/{pullNumber}/merge";
        string json = $"{{\"commit_title\": \"{commitTitle}\", \"commit_message\": \"{commitMessage}\"}}";

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "PUT";
        request.ContentType = "application/json";
        request.Accept = "application/vnd.github.v3+json";
        request.Headers.Add("Authorization", $"token {token}");
        request.UserAgent = "UnityGitHubPlugin";

        byte[] data = Encoding.UTF8.GetBytes(json);
        request.ContentLength = data.Length;

        using (Stream stream = request.GetRequestStream())
        {
            stream.Write(data, 0, data.Length);
        }

        try
        {
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                string responseText = reader.ReadToEnd();
            }
        }
        catch
        {
        }
    }
    //��ȡ��ȡ������
    private int ParsePRNumber(string jsonResponse)
    {
        // ʹ�ü򵥵��ַ������ҷ�������JSON
        int startIndex = jsonResponse.IndexOf("\"number\":") + 9;
        int endIndex = jsonResponse.IndexOf(",", startIndex);
        string numberStr = jsonResponse.Substring(startIndex, endIndex - startIndex);
        return int.Parse(numberStr);
    }
    //branchName_�·�֧���ƣ�baseSha_ԭ��֧�Ĺ�ϣֵ
    //������֧
    public void CreateBranch(string branchName, string baseSha)
    {
        string url = $"https://api.github.com/repos/{repoOwner}/{repoName}/git/refs";
        string json = $"{{\"ref\": \"refs/heads/{branchName}\", \"sha\": \"{baseSha}\"}}";

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "POST";
        request.ContentType = "application/json";
        request.Accept = "application/vnd.github.v3+json";
        request.Headers.Add("Authorization", $"token {token}");
        request.UserAgent = "UnityGitHubPlugin";

        byte[] data = Encoding.UTF8.GetBytes(json);
        request.ContentLength = data.Length;

        using (Stream stream = request.GetRequestStream())
        {
            stream.Write(data, 0, data.Length);
        }

        try
        {
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                string responseText = reader.ReadToEnd();

            }
        }
        catch { }
    }
    //branchName_��ȡ��ϣֵ�ķ�֧����
    //��ȡ��ϣֵ
    public string GetBranchSha(string branchName)
    {
        string url = $"https://api.github.com/repos/{repoOwner}/{repoName}/git/ref/heads/{branchName}";

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "GET";
        request.Accept = "application/vnd.github.v3+json";
        request.Headers.Add("Authorization", $"token {token}");
        request.UserAgent = "UnityGitHubPlugin";

        try
        {
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                string responseText = reader.ReadToEnd();


                // ����JSON
                GitHubRefResponse refResponse = JsonUtility.FromJson<GitHubRefResponse>(responseText);
                if (refResponse != null && refResponse.@object != null)
                {
                    return refResponse.@object.sha; // ���ط�֧�Ĺ�ϣֵ
                }
                else
                {
                    return null;
                }
            }
        }
        catch (WebException ex)
        {
            using (var stream = ex.Response.GetResponseStream())
            using (var reader = new StreamReader(stream))
            {
                UnityEngine.Debug.LogError("API Error: " + reader.ReadToEnd());
            }
            //UnityEngine.Debug.Log(token);
            //UnityEngine.Debug.Log(ex);
            return null;
        }
    }

    [System.Serializable]
    public class GitHubRefResponse
    {
        public string @ref;
        public string node_id;
        public string url;
        public GitHubObject @object;
    }

    [System.Serializable]
    public class GitHubObject
    {
        public string sha;
        public string type;
        public string url;
    }
    //branchName_ɾ���ķ�֧����
    //ɾ����֧
    public void DeleteBranch(string branchName)
    {
        string url = $"https://api.github.com/repos/{repoOwner}/{repoName}/git/refs/heads/{branchName}";

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "DELETE";
        request.Accept = "application/vnd.github.v3+json";
        request.Headers.Add("Authorization", $"token {token}");
        request.UserAgent = "UnityGitHubPlugin";

        try
        {
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        }
        catch
        {

        }
    }
    //�ݽ��޸�
    public void CommitChanges(string commitMessage)
    {
        // ��������޸ĵ��ļ�
        RunGitCommand("add .");

        // �ύ�޸�
        RunGitCommand($"commit -m \"{commitMessage}\"");

        // �����ύ��Զ�ֿ̲�
        RunGitCommand("push origin " + GetBranch());
    }
    //��ȡ��֧
    public void DownloadBranch(string branchName)
    {
        // ȷ������·������
        if (!Directory.Exists(downloadPath))
        {
            Directory.CreateDirectory(downloadPath);
        }

        // ��¡�ֿ���ض���֧
        RunGitCommand($"clone --branch {branchName} --single-branch {repoUrl} {downloadPath}");
    }
    //Gitָ��
    private void RunGitCommand(string command)
    {
        Process process = new Process();
        ProcessStartInfo startInfo = new ProcessStartInfo();

        // ����Git����
        startInfo.FileName = "git";
        startInfo.Arguments = command;
        startInfo.WorkingDirectory = repoPath; // ����Git�ֿ�·��
        startInfo.RedirectStandardOutput = true;
        startInfo.RedirectStandardError = true;
        startInfo.UseShellExecute = false;
        startInfo.CreateNoWindow = true;

        process.StartInfo = startInfo;

        // ��������
        process.Start();

        // ��ȡ���
        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();

        process.WaitForExit();

        // �ж��Ƿ�ɹ�
        if (process.ExitCode == 0)
        {
            UnityEngine.Debug.Log("ִ�гɹ�");
            UnityEngine.Debug.Log("Output: " + output);
        }
        else
        {
            UnityEngine.Debug.Log("ִ��ʧ��");
            UnityEngine.Debug.Log("Error: " + error);
        }
    }

    private string GetBranch()
    {
        // ���� ProcessStartInfo
        ProcessStartInfo processStartInfo = new ProcessStartInfo("git", "rev-parse --abbrev-ref HEAD")
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        // ��������
        using (Process process = new Process())
        {
            process.StartInfo = processStartInfo;
            process.Start();

            // ��ȡ��׼���
            string branchName = process.StandardOutput.ReadToEnd().Trim();
            // ��ȡ��׼����
            string error = process.StandardError.ReadToEnd();

            // �ȴ����̽���
            process.WaitForExit();

            // �ж��Ƿ�ɹ�
            if (process.ExitCode == 0)
            {
                return branchName;
            }
            else
            {
                UnityEngine.Debug.Log("����֧ʧ��");
                UnityEngine.Debug.Log("Error: " + error);
                return null;
            }
        }
    }
}
