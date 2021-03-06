﻿using UnityEngine;
using UnityEditor;
using System.Collections;
using PluginBuildConfig = MarkerMetro.Unity.WinShared.Editor.PluginConfigHelper.PluginBuildConfig;
using PluginSource = MarkerMetro.Unity.WinShared.Editor.PluginConfigHelper.PluginSource;

namespace MarkerMetro.Unity.WinShared.Editor
{
    internal sealed class ConfigureWindow : EditorWindow
    {
        enum DirType
        {
            WinLegacy,
            WinIntegration,
            NuGet,
            BuildLocal,
            VSCommonTool
        }

        string _winLegacyDir;
        string _winIntegrationDir;
        string _nugetDir;
        string _vsCommonToolDir;
        PluginSource _pluginSource;
        PluginBuildConfig _buildConfig;

        Vector2 _scrollPosition;

        void OnEnable()
        {
            PluginConfigHelper.SearchVSCommonToolsDir();
            _pluginSource = (PluginSource)PluginConfigHelper.CurrentPluginSource;
            _buildConfig = (PluginBuildConfig)PluginConfigHelper.CurrentBuildConfig;
            _winLegacyDir = PluginConfigHelper.WinLegacyDir;
            _winIntegrationDir = PluginConfigHelper.WinIntegrationDir;
            _nugetDir = PluginConfigHelper.BuildScriptsDir;
            _vsCommonToolDir = PluginConfigHelper.VSCommonToolDir;
        }

        void OnGUI()
        {
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, GUI.skin.scrollView);

            DrawUpdateSource();
            if (_pluginSource == PluginSource.Local)
            {
                DrawBuildConfig();
                DrawChooseDir(DirType.WinLegacy);
                DrawChooseDir(DirType.WinIntegration);
                DrawChooseDir(DirType.BuildLocal);
            }
            else
            {
                DrawChooseDir(DirType.NuGet);
            }
            DrawChooseDir(DirType.VSCommonTool);

            GUILayout.EndScrollView();
        }

        /// <summary>
        /// GUI for Update Source.
        /// </summary>
        void DrawUpdateSource()
        {
            GUILayout.Space(5f);
            GUI.changed = false;
            _pluginSource = (PluginSource)EditorGUILayout.EnumPopup("Plugin Source", _pluginSource, GUILayout.MaxWidth(250f));
            if (GUI.changed)
            {
                PluginConfigHelper.CurrentPluginSource = (int)_pluginSource;
            }
            GUILayout.Space(10f);
        }

        /// <summary>
        /// GUI for Build Config.
        /// </summary>
        void DrawBuildConfig()
        {
            GUI.changed = false;
            _buildConfig = (PluginBuildConfig)EditorGUILayout.EnumPopup("Build Local", _buildConfig, GUILayout.MaxWidth(250f));
            if (GUI.changed)
            {
                PluginConfigHelper.CurrentBuildConfig = (int)_buildConfig;
            }
            GUILayout.Space(10f);
        }

        /// <summary>
        /// GUI for selecting directory.
        /// </summary>
        void DrawChooseDir(DirType dirType)
        {
            string dir = GetDir(dirType);
            string dirLabel = GetDirLabel(dirType);

            EditorGUILayout.BeginVertical();
            GUILayout.Label(dirLabel + " Dir:");
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical();
            GUILayout.Space(4f);
            GUILayout.Label(dir, "AS TextArea", GUILayout.Height(20f));
            EditorGUILayout.EndVertical();
            GUILayout.Space(3f);
            if (GUILayout.Button("Choose", "LargeButtonMid", GUILayout.Height(20f), GUILayout.ExpandWidth(false)))
            {
                string defaultDir = string.IsNullOrEmpty(dir) ? Application.dataPath : dir;
                dir = EditorUtility.OpenFolderPanel("Choose Folder", defaultDir, "");
                if (!string.IsNullOrEmpty(dir))
                {
                    SetDir(dirType, dir);
                }
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(10f);
            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// Return cached dir based on DirType.
        /// </summary>
        string GetDir(DirType dirType)
        {
            string dir = string.Empty;
            switch (dirType)
            {
                case DirType.WinLegacy:
                    dir = _winLegacyDir;
                    break;
                case DirType.WinIntegration:
                    dir = _winIntegrationDir;
                    break;
                case DirType.NuGet:
                case DirType.BuildLocal:
                    dir = _nugetDir;
                    break;
                case DirType.VSCommonTool:
                    dir = _vsCommonToolDir;
                    break;
            }
            return dir;
        }

        /// <summary>
        /// Store dir of DirType to EditorPrefs.
        /// </summary>
        void SetDir(DirType dirType, string dir)
        {
            dir = System.IO.Path.GetFullPath(dir);
            switch (dirType)
            {
                case DirType.WinLegacy:
                    _winLegacyDir = dir;
                    PluginConfigHelper.WinLegacyDir = _winLegacyDir;
                    break;
                case DirType.WinIntegration:
                    _winIntegrationDir = dir;
                    PluginConfigHelper.WinIntegrationDir = _winIntegrationDir;
                    break;
                case DirType.NuGet:
                case DirType.BuildLocal:
                    _nugetDir = dir;
                    PluginConfigHelper.BuildScriptsDir = _nugetDir;
                    break;
                case DirType.VSCommonTool:
                    _vsCommonToolDir = dir;
                    PluginConfigHelper.VSCommonToolDir = _vsCommonToolDir;
                    break;
            }
        }

        /// <summary>
        /// Return dir GUI label based on DirType.
        /// </summary>
        string GetDirLabel(DirType dirType)
        {
            string dirLabel = string.Empty;
            switch (dirType)
            {
                case DirType.WinLegacy:
                    dirLabel = "WinLegacy Project";
                    break;
                case DirType.WinIntegration:
                    dirLabel = "WinIntegration Project";
                    break;
                case DirType.NuGet:
                case DirType.BuildLocal:
                    dirLabel = "Build Scripts";
                    break;
                case DirType.VSCommonTool:
                    dirLabel = "Visual Studio Common Tools";
                    break;
            }
            return dirLabel;
        }
    }
}