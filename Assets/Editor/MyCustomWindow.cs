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

    // 定义一个菜单项，用于打开窗口
    [MenuItem("Tools/Git")]
    public static void ShowWindow()
    {
        // 显示自定义窗口
        GetWindow<MyCustomWindow>("Git");
    }

    // 窗口的GUI内容
    private void OnGUI()
    {
        Tools tools = new Tools();
        GUILayout.Label("创建分支", EditorStyles.boldLabel);
        new_branchName = EditorGUILayout.TextField("创建分支名称:", new_branchName);
        original_branchName = EditorGUILayout.TextField("原分支名称:", original_branchName);
        if (GUILayout.Button("创建"))
        {
            bool isConfirmed = EditorUtility.DisplayDialog(
                "Confirmation", // 弹窗标题
                "Are you sure you want to submit: " + new_branchName + "?", // 弹窗消息
                "Yes", // 确认按钮文本
                "No" // 取消按钮文本
            );
            if(!isConfirmed) return;
            if (!string.IsNullOrEmpty(new_branchName) && !string.IsNullOrEmpty(original_branchName))
            {
                try
                {
                    //创建分支
                    string baseSha = tools.GetBranchSha(original_branchName);
                    if (!string.IsNullOrEmpty(baseSha))
                    {
                        tools.CreateBranch(new_branchName, baseSha);
                        UnityEngine.Debug.Log("创建成功");
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

        GUILayout.Label("\n删除分支", EditorStyles.boldLabel);
        delete_branchName = EditorGUILayout.TextField("删除分支名称:", delete_branchName);
        if (GUILayout.Button("删除"))
        {
            bool isConfirmed = EditorUtility.DisplayDialog(
                "Confirmation", // 弹窗标题
                "Are you sure you want to submit: " + new_branchName + "?", // 弹窗消息
                "Yes", // 确认按钮文本
                "No" // 取消按钮文本
            );
            if (!isConfirmed) return;
            if (!string.IsNullOrEmpty(delete_branchName))
            {
                try
                {
                    //删除分支
                    tools.DeleteBranch(delete_branchName);
                    UnityEngine.Debug.Log("删除成功");
                }
                catch
                {

                }
            }
        }

        GUILayout.Label("\n更新分支", EditorStyles.boldLabel);
        //now_branchName = EditorGUILayout.TextField("更新分支名称:", now_branchName);
        updatemessage = EditorGUILayout.TextField("更新信息:", updatemessage);
        if (GUILayout.Button("更新"))
        {
            if (!string.IsNullOrEmpty(updatemessage))
            {
                bool isConfirmed = EditorUtility.DisplayDialog(
                    "Confirmation", // 弹窗标题
                    "Are you sure you want to submit: " + new_branchName + "?", // 弹窗消息
                    "Yes", // 确认按钮文本
                    "No" // 取消按钮文本
                );
                if (!isConfirmed) return;
                try
                {
                    //更新分支
                    tools.CommitChanges(updatemessage);
                }
                catch
                {

                }
            }
        }

        GUILayout.Label("\n合并分支", EditorStyles.boldLabel);
        merge_title = EditorGUILayout.TextField("合并标题:", merge_title);
        merge_branchName = EditorGUILayout.TextField("合并分支名称:", merge_branchName);
        merged_branchName = EditorGUILayout.TextField("合并到分支名称:", merged_branchName);
        merge_body = EditorGUILayout.TextField("合并备注:", merge_body);
        if (GUILayout.Button("合并"))
        {
            if (!string.IsNullOrEmpty(merge_title) && !string.IsNullOrEmpty(merge_branchName) && 
                !string.IsNullOrEmpty(merged_branchName) && !string.IsNullOrEmpty(merge_body))
            {
                bool isConfirmed = EditorUtility.DisplayDialog(
                    "Confirmation", // 弹窗标题
                    "Are you sure you want to submit: " + new_branchName + "?", // 弹窗消息
                    "Yes", // 确认按钮文本
                    "No" // 取消按钮文本
                );
                if (!isConfirmed) return;
                try
                {
                    //合并分支
                    int num = tools.CreatePullRequest(merge_title, merge_branchName, merged_branchName, merge_body);
                    tools.MergePullRequest(num, merge_title, merge_body);
                    UnityEngine.Debug.Log("合并成功");
                }
                catch
                {

                }
            }
        }

        GUILayout.Label("\n导入分支", EditorStyles.boldLabel);
        load_branchName = EditorGUILayout.TextField("导入分支名称:", load_branchName);
        load_path = EditorGUILayout.TextField("导入到的空文件夹路径::", load_path);
        if (GUILayout.Button("导入"))
        {
            if (!string.IsNullOrEmpty(load_branchName))
            {
                bool isConfirmed = EditorUtility.DisplayDialog(
                    "Confirmation", // 弹窗标题
                    "Are you sure you want to submit: " + new_branchName + "?", // 弹窗消息
                    "Yes", // 确认按钮文本
                    "No" // 取消按钮文本
                );
                if (!isConfirmed) return;
                try
                {
                    //导入分支
                    tools.DownloadBranch(load_branchName, load_path);
                }
                catch
                {

                }
            }
        }
    }
}
