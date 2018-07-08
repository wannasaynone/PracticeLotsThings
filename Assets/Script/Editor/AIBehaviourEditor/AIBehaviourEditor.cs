using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AIBehaviourEditor : EditorWindow {

    private static List<BaseNode> windows = new List<BaseNode>();

    private Vector3 m_mousePosition = default(Vector3);
    private bool m_makeTransition = false;
    private bool m_clickedOnWindow = false;
    private BaseNode m_selectedWindow = null;

    public enum UserAction
    {
        AddState,
        AddTranstionNode,
        DeleteNode,
        CommentNode
    }

    [MenuItem("Tools/AI Behaviour Editor")]
    private static void Init()
    {
        windows.Clear(); // TESTING
        AIBehaviourEditor editor = GetWindow<AIBehaviourEditor>();
        editor.minSize = new Vector2(800f, 600f);
    }

    private void OnGUI()
    {
        Event e = Event.current;
        m_mousePosition = e.mousePosition;
        UserInput(e);
        DrawWindows();
    }

    private void DrawWindows()
    {
        BeginWindows();
        for(int _windowIndex = 0; _windowIndex < windows.Count; _windowIndex++)
        {
            windows[_windowIndex].DrawCurve();
            windows[_windowIndex].windowRect = GUI.Window(_windowIndex, windows[_windowIndex].windowRect, DrawNodeWindow, windows[_windowIndex].windowTitle);
        }
        EndWindows();
    }

    private void DrawNodeWindow(int id)
    {
        windows[id].DrawWindow();
        GUI.DragWindow();
    }

    private void UserInput(Event e)
    {
        if(e.button == 1 && !m_makeTransition)
        {
            if(e.type == EventType.mouseDown)
            {
                RightClick(e);
            }
        }

        if (e.button == 0 && !m_makeTransition)
        {
            if (e.type == EventType.mouseDown)
            {
                LeftClick(e);
            }
        }
    }

    private void RightClick(Event e)
    {
        m_selectedWindow = null;
        m_clickedOnWindow = false;
        for (int _windowIndex = 0; _windowIndex < windows.Count; _windowIndex++)
        {
            if(windows[_windowIndex].windowRect.Contains(e.mousePosition))
            {
                m_selectedWindow = windows[_windowIndex];
                m_clickedOnWindow = true;
                break;
            }
        }

        if(m_clickedOnWindow)
        {
            ModifyNodes(e);
        }
        else
        {
            AddNewNodes(e);
        }
    }

    private void LeftClick(Event e)
    {

    }

    private void AddNewNodes(Event e)
    {
        GenericMenu _menu = new GenericMenu();
        _menu.AddItem(new GUIContent("Add State"), false, ContextCallbak, UserAction.AddState);
        _menu.AddItem(new GUIContent("Add Comment"), false, ContextCallbak, UserAction.CommentNode);
        _menu.ShowAsContext();
        e.Use();
    }

    private void ModifyNodes(Event e)
    {
        GenericMenu _menu = new GenericMenu();

        if (m_selectedWindow is StateNode)
        {
            _menu.AddItem(new GUIContent("Add Transion"), false, ContextCallbak, UserAction.AddTranstionNode);
            _menu.AddItem(new GUIContent("Delete"), false, ContextCallbak, UserAction.DeleteNode);
        }

        if(m_selectedWindow is CommentNode)
        {
            _menu.AddItem(new GUIContent("Delete"), false, ContextCallbak, UserAction.DeleteNode);
        }

        _menu.ShowAsContext();
        e.Use();
    }

    private void ContextCallbak(object action)
    {
        switch((UserAction)action)
        {
            case UserAction.AddState:
                {
                    StateNode _stateNode = new StateNode
                    {
                        windowRect = new Rect(new Vector2(m_mousePosition.x, m_mousePosition.y), new Vector2(200, 300)),
                        windowTitle = "State"
                    };
                    windows.Add(_stateNode);
                    break;
                }
            case UserAction.AddTranstionNode:
                {
                    break;
                }
            case UserAction.CommentNode:
                {
                    CommentNode _commentNode = new CommentNode
                    {
                        windowRect = new Rect(new Vector2(m_mousePosition.x, m_mousePosition.y), new Vector2(200, 150)),
                        windowTitle = "Comment"
                    };
                    windows.Add(_commentNode);
                    break;
                }
            case UserAction.DeleteNode:
                {
                    windows.Remove(m_selectedWindow);
                    break;
                }
        }
    }

}
