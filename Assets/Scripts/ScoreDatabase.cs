using System;
using UnityEngine;

#if !UNITY_WEBGL || UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

public static class ScoreDatabase
{
#if !UNITY_WEBGL || UNITY_EDITOR

    const string LIB = "sqlite3";
    const int SQLITE_ROW = 100;

    [DllImport(LIB, CallingConvention = CallingConvention.Cdecl)]
    static extern int sqlite3_open(string filename, out IntPtr db);

    [DllImport(LIB, CallingConvention = CallingConvention.Cdecl)]
    static extern int sqlite3_close(IntPtr db);

    [DllImport(LIB, CallingConvention = CallingConvention.Cdecl)]
    static extern int sqlite3_exec(IntPtr db, string sql, IntPtr cb, IntPtr arg, out IntPtr errmsg);

    [DllImport(LIB, CallingConvention = CallingConvention.Cdecl)]
    static extern int sqlite3_prepare_v2(IntPtr db, string sql, int nBytes, out IntPtr stmt, out IntPtr tail);

    [DllImport(LIB, CallingConvention = CallingConvention.Cdecl)]
    static extern int sqlite3_step(IntPtr stmt);

    [DllImport(LIB, CallingConvention = CallingConvention.Cdecl)]
    static extern int sqlite3_column_int(IntPtr stmt, int col);

    [DllImport(LIB, CallingConvention = CallingConvention.Cdecl)]
    static extern int sqlite3_finalize(IntPtr stmt);

    [DllImport(LIB, CallingConvention = CallingConvention.Cdecl)]
    static extern int sqlite3_bind_int(IntPtr stmt, int index, int value);

    [DllImport(LIB, CallingConvention = CallingConvention.Cdecl)]
    static extern int sqlite3_bind_text(IntPtr stmt, int index, string value, int len, IntPtr destructor);

    static readonly IntPtr SQLITE_TRANSIENT = new IntPtr(-1);
    static string DbPath => Application.persistentDataPath + "/savethestars.db";

    public static void Initialize()
    {
        try
        {
            IntPtr db;
            if (sqlite3_open(DbPath, out db) != 0) return;
            try
            {
                IntPtr err;
                sqlite3_exec(db,
                    "CREATE TABLE IF NOT EXISTS scores (" +
                    "id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                    "score INTEGER NOT NULL, " +
                    "date TEXT NOT NULL)",
                    IntPtr.Zero, IntPtr.Zero, out err);
            }
            finally { sqlite3_close(db); }
        }
        catch (Exception e) { Debug.LogWarning("ScoreDatabase.Initialize failed: " + e.Message); }
    }

    public static void SaveScore(int score)
    {
        try
        {
            IntPtr db;
            if (sqlite3_open(DbPath, out db) != 0) return;
            try
            {
                IntPtr stmt, tail;
                if (sqlite3_prepare_v2(db,
                    "INSERT INTO scores (score, date) VALUES (?, ?)",
                    -1, out stmt, out tail) == 0)
                {
                    sqlite3_bind_int(stmt, 1, score);
                    sqlite3_bind_text(stmt, 2,
                        DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                        -1, SQLITE_TRANSIENT);
                    sqlite3_step(stmt);
                    sqlite3_finalize(stmt);
                }
            }
            finally { sqlite3_close(db); }
        }
        catch (Exception e) { Debug.LogWarning("ScoreDatabase.SaveScore failed: " + e.Message); }
    }

    public static int GetHighScore()
    {
        try
        {
            IntPtr db;
            if (sqlite3_open(DbPath, out db) != 0) return 0;
            try
            {
                IntPtr stmt, tail;
                int best = 0;
                if (sqlite3_prepare_v2(db,
                    "SELECT COALESCE(MAX(score), 0) FROM scores",
                    -1, out stmt, out tail) == 0)
                {
                    if (sqlite3_step(stmt) == SQLITE_ROW)
                        best = sqlite3_column_int(stmt, 0);
                    sqlite3_finalize(stmt);
                }
                return best;
            }
            finally { sqlite3_close(db); }
        }
        catch (Exception e) { Debug.LogWarning("ScoreDatabase.GetHighScore failed: " + e.Message); return 0; }
    }

#else
    const string HighScoreKey = "SaveStars_HighScore";

    public static void Initialize()
    {
        
    }

    public static void SaveScore(int score)
    {
        try
        {
            int current = PlayerPrefs.GetInt(HighScoreKey, 0);
            if (score > current)
            {
                PlayerPrefs.SetInt(HighScoreKey, score);
                PlayerPrefs.Save();
            }
        }
        catch (Exception e) { Debug.LogWarning("ScoreDatabase.SaveScore (WebGL) failed: " + e.Message); }
    }

    public static int GetHighScore()
    {
        try { return PlayerPrefs.GetInt(HighScoreKey, 0); }
        catch (Exception e) { Debug.LogWarning("ScoreDatabase.GetHighScore (WebGL) failed: " + e.Message); return 0; }
    }
#endif
}