﻿using UnityEngine;
using File = UnityEngine.Windows.File;

public class FileHelper {

    public static void Delete(string path)
    {
        File.Delete(path);
    }

    public static bool Exists(string path)
    {
        return File.Exists(path);
    }

    public static byte[] ReadAllBytes(string path)
    {
        return File.ReadAllBytes(path);
    }

    public static void WriteAllBytes(string path, byte[] bytes)
    {
        File.WriteAllBytes(path, bytes);
    }

    public static string ReadAllText(string path) 
    {
        byte[] bytes = File.ReadAllBytes(path);
        return System.Text.Encoding.UTF8.GetString(bytes, 0, bytes.Length);
    }

    public static void WriteAllText(string path, string text) 
    {
        File.WriteAllBytes(path, System.Text.Encoding.UTF8.GetBytes(text));
    }
}
