using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AIBehaviourEditor : EditorWindow {

    private static List<BaseNode> windows = new List<BaseNode>();

    private Vector3 m_mousePosition = default(Vector3);
    private bool m_makeTransition = false;
    private bool m_clickedOnWindow = false;
    private BaseNode m_selectedNode = null;

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
        m_selectedNode = null;
        m_clickedOnWindow = false;
        for (int _windowIndex = 0; _windowIndex < windows.Count; _windowIndex++)
        {
            if(windows[_windowIndex].windowRect.Contains(e.mousePosition))
            {
                m_selectedNode = windows[_windowIndex];
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

        if (m_selectedNode is StateNode)
        {
            StateNode _stateNode = (StateNode)m_selectedNode;
            if(_stateNode.currentState != null)
            {
                _menu.AddItem(new GUIContent("Add Transion"), false, ContextCallbak, UserAction.AddTranstionNode);
            }
            _menu.AddItem(new GUIContent("Delete"), false, ContextCallbak, UserAction.DeleteNode);
        }

        if(m_selectedNode is CommentNode)
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
                    if(m_selectedNode is StateNode)
                    {
                        StateNode _from = (StateNode)m_selectedNode;
                        Transition _transition = _from.AddTransition();
                        AddTransitionNode(_from.currentState.transitions.Count, _transition, _from);
                    }
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
                    windows.Remove(m_selectedNode);
                    break;
                }
        }
    }

    public static void DrawNodeCurve(Rect start, Rect end, bool left)
    {
        Vector3 _startPos = new Vector3((left ? start.x + start.width : start.x), (start.y + start.height * 0.5f));
        Vector3 _endPos = new Vector3(end.x + end.width * 0.5f, end.y + end.height * 0.5f);
        Vector3 _startTrans = _startPos + Vector3.right * 50;
        Vector3 _endTarns = _endPos + Vector3.left * 50;

        Color shadow = new Color(0, 0, 0, 0.06f);

        for(int i = 0; i < 3; i++)
        {
            Handles.DrawBezier(_startPos, _endPos, _startTrans, _endTarns, shadow, null, (i + 1) * 0.5f);
        }

        Handles.DrawBezier(_startPos, _endPos, _startTrans, _endTarns, Color.black, null, 1);
    }

    public static TransitionNode AddTransitionNode(int index, Transition transition, StateNode from)
    {
        Rect _fromRect = from.windowRect;
        _fromRect.x += 50;
        float _targetY = _fromRect.y - _fromRect.height;
        if(from.currentState != null)
        {
            _targetY += (index * 100);
        }

        _fromRect.y = _targetY;

        TransitionNode _transNode = CreateInstance<TransitionNode>();
        _transNode.Init(from, transition);
        _transNode.windowRect = new Rect(_fromRect.x + 200 + 100, _fromRect.y + _fromRect.height * 0.7f, 200, 80);
        _transNode.windowTitle = "Condition Check";
        windows.Add(_transNode);

        return _transNode;
    }

}
