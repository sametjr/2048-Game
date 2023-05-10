using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants : MonoBehaviour
{
    #region Singleton
    private static Constants _instance;
    public static Constants Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<Constants>();
            }
            return _instance;
        }
    }
    #endregion

    public Color[] colors;
    public float blockSize = 2;
    public Vector2 startPos = new Vector2(-3, 3);
}
