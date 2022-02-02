using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MyPaintWindow : EditorWindow
{
   private Color _leftButtonColor = new Color(0,0,0);
   private Color _rightButtonColor = new Color(0,0,0);
   private GameObject _gameObject;
   private Texture2D _texture;
   private Color[,] _colors;
   
   [MenuItem("NewWindow/MyPaintWindow")]
   private static void OpenMyWindow()
   {
       GetWindow<MyPaintWindow>();
   }
   private void OnEnable()
   {
       _colors = new Color[8, 8];
       _texture = new Texture2D(8, 8);
      for (int i = 0; i < 8; i++)
      {
         for (int j = 0; j < 8; j++)
         {
             _texture.SetPixel(j, i, Color.white);
            _colors[j,i] = Color.white;
         }
      }
   }
   private void OnGUI()
    {
        Event _event = Event.current;
        Texture _startTexture = EditorGUIUtility.whiteTexture;

        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical(GUILayout.Width(350f));
        GUILayout.Label("Toolbar");
        GUILayout.BeginHorizontal();
        GUILayout.Label("Color for left button");
        GUILayout.Space(10f);
        _leftButtonColor = EditorGUILayout.ColorField(_leftButtonColor);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Color for right button");
        GUILayout.Space(3f);
        _rightButtonColor = EditorGUILayout.ColorField(_rightButtonColor);
        GUILayout.EndHorizontal();
        GUILayout.Space(20f);
        if (GUILayout.Button("Fill All"))
        {
            FillAllGrids();
        }
        GUILayout.Space(330f);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Output Renderer");
        _gameObject = (GameObject)EditorGUILayout.ObjectField(_gameObject, typeof(GameObject));
        GUILayout.EndHorizontal();
        if (GUILayout.Button("Save to Object"))
        {
            SaveToObject();
        }
        GUILayout.EndVertical();
        
        for (int j = 0; j < 8; j++)
        {
            for (int i = 0; i < 8; i++)
            {
                Rect _rect = new Rect(400 + 60 * i, 0 + 60 * j, 50, 50);

                if (_event.isMouse)
                {
                  if (_event.button == 0 && _rect.Contains(_event.mousePosition))
                  {
                      _colors[i, j] = _leftButtonColor;
                      _event.Use();
                  }
                  if (_event.button == 1 && _rect.Contains(_event.mousePosition))
                  {
                      _colors[i, j] = _rightButtonColor;
                      _event.Use();
                  }  
                }

                GUI.color = _colors[i, j];
                GUI.DrawTexture(_rect,_startTexture);
            }
        }
        
        GUILayout.EndHorizontal();
    }

    private void FillAllGrids()
    {
        for (int j = 0; j < 8; j++)
        {
            for (int i = 0; i < 8; i++)
            {
                _colors[i, j] = _leftButtonColor;
            }
        }
    }

    private void SaveToObject()
    {
        MeshRenderer _meshRenderer = _gameObject.GetComponent<MeshRenderer>();
        _texture = new Texture2D(8,8);
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                _texture.SetPixel(i,j,_colors[i,j]);
            }
         
        }
        _texture.filterMode = FilterMode.Point;
        _texture.Apply();
        _meshRenderer.sharedMaterial.mainTexture = _texture;

    }
}
