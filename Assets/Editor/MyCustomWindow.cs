using System.Diagnostics;
using System;
using UnityEditor;
using UnityEngine;

public class MyCustomWindow : EditorWindow
{
    string new_branchName;
    string original_branchName;
    string delete_branchName;
    string updatemessage;
    string merge_title;
    string merge_branchName;
    string merged_branchName;
    string merge_body;
    string load_branchName;
    string load_path;

    // ����һ���˵�����ڴ򿪴���
    [MenuItem("Tools/Git")]
    public static void ShowWindow()
    {
        // ��ʾ�Զ��崰��
        GetWindow<MyCustomWindow>("Git");
    }

    // ���ڵ�GUI����
    private void OnGUI()
    {
        Tools tools = new Tools();
        GUILayout.Label("������֧", EditorStyles.boldLabel);
        new_branchName = EditorGUILayout.TextField("������֧����:", new_branchName);
        original_branchName = EditorGUILayout.TextField("ԭ��֧����:", original_branchName);
        if (GUILayout.Button("����"))
        {
            bool isConfirmed = EditorUtility.DisplayDialog(
                "Confirmation", // ��������
                "Are you sure you want to submit: " + new_branchName + "?", // ������Ϣ
                "Yes", // ȷ�ϰ�ť�ı�
                "No" // ȡ����ť�ı�
            );
            if(!isConfirmed) return;
            if (!string.IsNullOrEmpty(new_branchName) && !string.IsNullOrEmpty(original_branchName))
            {
                try
                {
                    //������֧
                    string baseSha = tools.GetBranchSha(original_branchName);
                    if (!string.IsNullOrEmpty(baseSha))
                    {
                        tools.CreateBranch(new_branchName, baseSha);
                        UnityEngine.Debug.Log("�����ɹ�");
                    }
                    else
                    {
                        UnityEngine.Debug.Log(baseSha);
                        UnityEngine.Debug.LogError("Failed to get base branch SHA.");
                    }
                    
                }
                catch
                {

                }
            }
        }

        GUILayout.Label("\nɾ����֧", EditorStyles.boldLabel);
        delete_branchName = EditorGUILayout.TextField("ɾ����֧����:", delete_branchName);
        if (GUILayout.Button("ɾ��"))
        {
            bool isConfirmed = EditorUtility.DisplayDialog(
                "Confirmation", // ��������
                "Are you sure you want to submit: " + new_branchName + "?", // ������Ϣ
                "Yes", // ȷ�ϰ�ť�ı�
                "No" // ȡ����ť�ı�
            );
            if (!isConfirmed) return;
            if (!string.IsNullOrEmpty(delete_branchName))
            {
                try
                {
                    //ɾ����֧
                    tools.DeleteBranch(delete_branchName);
                    UnityEngine.Debug.Log("ɾ���ɹ�");
                }
                catch
                {

                }
            }
        }

        GUILayout.Label("\n���·�֧", EditorStyles.boldLabel);
        //now_branchName = EditorGUILayout.TextField("���·�֧����:", now_branchName);
        updatemessage = EditorGUILayout.TextField("������Ϣ:", updatemessage);
        if (GUILayout.Button("����"))
        {
            if (!string.IsNullOrEmpty(updatemessage))
            {
                bool isConfirmed = EditorUtility.DisplayDialog(
                    "Confirmation", // ��������
                    "Are you sure you want to submit: " + new_branchName + "?", // ������Ϣ
                    "Yes", // ȷ�ϰ�ť�ı�
                    "No" // ȡ����ť�ı�
                );
                if (!isConfirmed) return;
                try
                {
                    //���·�֧
                    tools.CommitChanges(updatemessage);
                }
                catch
                {

                }
            }
        }

        GUILayout.Label("\n�ϲ���֧", EditorStyles.boldLabel);
        merge_title = EditorGUILayout.TextField("�ϲ�����:", merge_title);
        merge_branchName = EditorGUILayout.TextField("�ϲ���֧����:", merge_branchName);
        merged_branchName = EditorGUILayout.TextField("�ϲ�����֧����:", merged_branchName);
        merge_body = EditorGUILayout.TextField("�ϲ���ע:", merge_body);
        if (GUILayout.Button("�ϲ�"))
        {
            if (!string.IsNullOrEmpty(merge_title) && !string.IsNullOrEmpty(merge_branchName) && 
                !string.IsNullOrEmpty(merged_branchName) && !string.IsNullOrEmpty(merge_body))
            {
                bool isConfirmed = EditorUtility.DisplayDialog(
                    "Confirmation", // ��������
                    "Are you sure you want to submit: " + new_branchName + "?", // ������Ϣ
                    "Yes", // ȷ�ϰ�ť�ı�
                    "No" // ȡ����ť�ı�
                );
                if (!isConfirmed) return;
                try
                {
                    //�ϲ���֧
                    int num = tools.CreatePullRequest(merge_title, merge_branchName, merged_branchName, merge_body);
                    tools.MergePullRequest(num, merge_title, merge_body);
                    UnityEngine.Debug.Log("�ϲ��ɹ�");
                }
                catch
                {

                }
            }
        }

        GUILayout.Label("\n�����֧", EditorStyles.boldLabel);
        load_branchName = EditorGUILayout.TextField("�����֧����:", load_branchName);
        load_path = EditorGUILayout.TextField("���뵽�Ŀ��ļ���·��::", load_path);
        if (GUILayout.Button("����"))
        {
            if (!string.IsNullOrEmpty(load_branchName))
            {
                bool isConfirmed = EditorUtility.DisplayDialog(
                    "Confirmation", // ��������
                    "Are you sure you want to submit: " + new_branchName + "?", // ������Ϣ
                    "Yes", // ȷ�ϰ�ť�ı�
                    "No" // ȡ����ť�ı�
                );
                if (!isConfirmed) return;
                try
                {
                    //�����֧
                    tools.DownloadBranch(load_branchName, load_path);
                }
                catch
                {

                }
            }
        }
    }
}
