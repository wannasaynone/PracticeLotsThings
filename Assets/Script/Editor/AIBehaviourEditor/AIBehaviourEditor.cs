using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AIBehaviourEditor : EditorWindow {

    private const float TOP_BAR_HEIGHT = 20f;

    public enum UserAction
    {
        AddState,
        AddTranstionNode,
        TranstionNodeLinkToStateNode,
        DeleteNode
    }

    private static AIBehaviourData m_assignedAIBehaviour = null;
    private static AIBehaviourData m_currentAIBehaviour = null;
    private static string m_savePath = "Assets/";

    private Vector2 m_mousePosition = default(Vector2);
    private BaseNode m_selectedNode = null;
    private bool m_isLinkingState = false;

    [MenuItem("Tools/AI Behaviour Editor")]
    private static void Init()
    {
        GetWindow<AIBehaviourEditor>();
    }

    private void OnEnable()
    {
        ResetNode();
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        m_assignedAIBehaviour = EditorGUILayout.ObjectField("AI Behaviour Data:", m_assignedAIBehaviour, typeof(AIBehaviourData), false) as AIBehaviourData;

        if (m_assignedAIBehaviour != null && (m_currentAIBehaviour == null || (m_currentAIBehaviour != null && m_currentAIBehaviour != m_assignedAIBehaviour)))
        {
            if (GUILayout.Button("Apply"))
            {
                m_currentAIBehaviour = m_assignedAIBehaviour;
                m_savePath = m_currentAIBehaviour.savePath;
                Reassignodes();
            }
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();

        if(m_currentAIBehaviour == null)
        {
            m_savePath = EditorGUILayout.TextField("Save Path:", m_savePath);
            if (GUILayout.Button("Generate AI Behaviour Data"))
            {
                if (!Directory.Exists(m_savePath))
                {
                    EditorUtility.DisplayDialog("Error", "Unexisting Path:" + m_savePath, "OK");
                    return;
                }

                AIBehaviourData _aiBehaviour = CreateInstance<AIBehaviourData>();
                AssetDatabase.CreateAsset(_aiBehaviour, m_savePath + "AI Behaviour Data.asset");

                m_currentAIBehaviour = _aiBehaviour;
                m_assignedAIBehaviour = _aiBehaviour;

                m_currentAIBehaviour.savePath = m_savePath;

                SaveNodeData();
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = _aiBehaviour;
            }
        }
        else
        {
            if (GUILayout.Button("Generate AI Datas"))
            {
                AIBehaviourSaver.Save(m_savePath, m_currentAIBehaviour, OnSaved);
            }
            if(GUILayout.Button("Save Node Datas"))
            {
                SaveNodeData();
            }
            if (GUILayout.Button("Reset"))
            {
                ResetNode();
            }
        }

        EditorGUILayout.EndHorizontal();

        Event e = Event.current;
        m_mousePosition = e.mousePosition;
        DetectInput(e);
        DrawWindows();

        if (m_isLinkingState)
        {
            Handles.color = Color.red;
            Handles.DrawLine(m_selectedNode.OutPoint, m_mousePosition);
            Repaint();
        }
    }

    private void DetectInput(Event e)
    {
        if(m_currentAIBehaviour == null && e.type == EventType.MouseDown)
        {
            EditorUtility.DisplayDialog("Error", "Generate or Assign AI Behaviour Data First", "OK");
            return;
        }

        if (e.button == 1 && e.type == EventType.MouseDown)
        {
            m_isLinkingState = false;
            DetectIsClickedOnNode(e);
            CreateContextMenu();
        }

        if (e.button == 0 && e.type == EventType.MouseDown && m_isLinkingState)
        {
            m_isLinkingState = false;

            if(m_selectedNode is TransitionNode)
            {
                TransitionNode _transitionNode = (TransitionNode)m_selectedNode;
                DetectIsClickedOnNode(e);
                if (m_selectedNode != null && m_selectedNode is StateNode)
                {
                    _transitionNode.ToStateNode = (StateNode)m_selectedNode;
                }
            }
        }
    }

    private void ResetNode()
    {
        m_currentID = -1;
        m_currentAIBehaviour = null;
        m_assignedAIBehaviour = null;
        m_savePath = "Assets/";
    }

    private void DetectIsClickedOnNode(Event e)
    {
        for (int _nodeIndex = 0; _nodeIndex < m_currentAIBehaviour.nodeDatas.Count; _nodeIndex++)
        {
            if (m_currentAIBehaviour.nodeDatas[_nodeIndex].windowRect.Contains(e.mousePosition))
            {
                m_selectedNode = m_currentAIBehaviour.nodeDatas[_nodeIndex];
                return;
            }
        }
        m_selectedNode = null;
    }

    private void CreateContextMenu()
    {
        if(m_isLinkingState)
        {
            m_isLinkingState = false;
        }

        GenericMenu _menu = new GenericMenu();
        if (m_selectedNode == null)
        {
            _menu.AddItem(new GUIContent("Add State Node"), false, ContextCallbak, UserAction.AddState);
            _menu.ShowAsContext();
        }
        else
        {
            if(m_selectedNode is StateNode)
            {
                _menu.AddItem(new GUIContent("Add Transition Node"), false, ContextCallbak, UserAction.AddTranstionNode);
            }

            if (m_selectedNode is TransitionNode)
            {
                _menu.AddItem(new GUIContent("Link To"), false, ContextCallbak, UserAction.TranstionNodeLinkToStateNode);
            }

            _menu.AddItem(new GUIContent("Delete"), false, ContextCallbak, UserAction.DeleteNode);
            _menu.ShowAsContext();
        }
    }

    private long m_currentID = -1;
    private void ContextCallbak(object action)
    {
        switch ((UserAction)action)
        {
            case UserAction.AddState:
                {
                    AddStateNode(m_mousePosition);
                    break;
                }
            case UserAction.AddTranstionNode:
                {
                    AddTranstionNode(m_mousePosition, (StateNode)m_selectedNode);
                    break;
                }
            case UserAction.DeleteNode:
                {
                    if(m_selectedNode is TransitionNode)
                    {
                        TransitionNode _selectedTranstionNode = m_selectedNode as TransitionNode;

                        if(_selectedTranstionNode.FromStateNode != null)
                        {
                            _selectedTranstionNode.FromStateNode.transitions_out.Remove(_selectedTranstionNode);
                        }

                        if (_selectedTranstionNode.ToStateNode != null)
                        {
                            _selectedTranstionNode.ToStateNode.transitions_in.Remove(_selectedTranstionNode);
                        }
                    }

                    if(m_selectedNode is StateNode)
                    {
                        StateNode _selectedStateNode = m_selectedNode as StateNode;
                        List<TransitionNode> _waitForRemoveTransitionNode = new List<TransitionNode>();
                        for(int i = 0; i < m_currentAIBehaviour.nodeDatas.Count; i++)
                        {
                            if(m_currentAIBehaviour.nodeDatas[i] is TransitionNode)
                            {
                                TransitionNode _transtionNode = m_currentAIBehaviour.nodeDatas[i] as TransitionNode;

                                if (_transtionNode.ToStateNode != null && _transtionNode.ToStateNode.ID == _selectedStateNode.ID)
                                {
                                    _transtionNode.ToStateNode = null;
                                }

                                if (_transtionNode.FromStateNode != null && _transtionNode.FromStateNode.ID == _selectedStateNode.ID)
                                {
                                    _transtionNode.FromStateNode = null;
                                }

                                if(_transtionNode.ToStateNode == null && _transtionNode.FromStateNode == null)
                                {
                                    _waitForRemoveTransitionNode.Add(_transtionNode);
                                }
                            }
                        }

                        for (int i = 0; i < _waitForRemoveTransitionNode.Count; i++)
                        {
                            m_currentAIBehaviour.nodeDatas.Remove(_waitForRemoveTransitionNode[i]);
                        }
                    }

                    m_currentAIBehaviour.nodeDatas.Remove(m_selectedNode);
                    m_selectedNode = null;
                    break;
                }
            case UserAction.TranstionNodeLinkToStateNode:
                {
                    m_isLinkingState = true;
                    break;
                }
        }
    }

    private void AddStateNode(Vector2 position)
    {
        CreateNodeInstance<StateNode>(delegate (BaseNode node)
        {
            StateNode _stateNode = node as StateNode;

            _stateNode.ID = m_currentID;
            Rect _windowRect = new Rect(new Vector2(position.x, position.y), new Vector2(200, 150));
            _stateNode.windowRect = _windowRect;
        });
    }

    private void AddTranstionNode(Vector2 position, StateNode from)
    {
        CreateNodeInstance<TransitionNode>(
            delegate (BaseNode node)
            {
                TransitionNode _transitionNode = node as TransitionNode;

                _transitionNode.ID = m_currentID;
                _transitionNode.FromStateNode = from;
                _transitionNode.orgainHeight = 150f;
                Rect _windowRect = new Rect(new Vector2(position.x, position.y), new Vector2(200, 150));
                _transitionNode.windowRect = _windowRect;
                ((StateNode)m_selectedNode).transitions_out.Add(_transitionNode);
            });
    }

    private void CreateNodeInstance<T>(System.Action<BaseNode> onAssetCreated) where T : BaseNode
    {
        string _nodePath = m_savePath + "Nodes/";

        if (!Directory.Exists(_nodePath))
        {
            Directory.CreateDirectory(_nodePath);
        }

        m_currentID++;
        T _node = CreateInstance<T>();
        _node.ID = m_currentID;
        AssetDatabase.CreateAsset(_node, _nodePath + _node.Title + _node.ID + ".asset");

        if(onAssetCreated != null)
        {
            onAssetCreated(_node);
        }

        m_currentAIBehaviour.nodeDatas.Add(_node);

        EditorUtility.SetDirty(_node);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private void DrawWindows()
    {
        if(m_currentAIBehaviour == null)
        {
            return;
        }

        DrawLines();

        BeginWindows();
        for (int _nodeIndex = 0; _nodeIndex < m_currentAIBehaviour.nodeDatas.Count; _nodeIndex++)
        {
            m_currentAIBehaviour.nodeDatas[_nodeIndex].windowRect = GUI.Window(_nodeIndex, m_currentAIBehaviour.nodeDatas[_nodeIndex].windowRect, DrawNodeWindow, m_currentAIBehaviour.nodeDatas[_nodeIndex].Title);
        }
        EndWindows();
    }

    private Vector2 GetCenter(Vector2 p1, Vector2 p2)
    {
        return new Vector2((p1.x + p2.x) / 2, (p1.y + p2.y) / 2);
    }

    private Vector2 ConvertPositionToVertex(Vector2 point)
    {
        float _realHeight = position.height + TOP_BAR_HEIGHT;
        float _reverseY = _realHeight - (point.y + TOP_BAR_HEIGHT);
        return new Vector2(point.x / position.width, _reverseY / _realHeight);
    }

    private void DrawLines()
    {
        for (int _nodeIndex = 0; _nodeIndex < m_currentAIBehaviour.nodeDatas.Count; _nodeIndex++)
        {
            if(m_currentAIBehaviour.nodeDatas[_nodeIndex] is TransitionNode)
            {
                TransitionNode _transitionNode = m_currentAIBehaviour.nodeDatas[_nodeIndex] as TransitionNode;

                if (_transitionNode.FromStateNode != null)
                {
                    Handles.color = Color.black;
                    Handles.DrawLine(_transitionNode.FromStateNode.OutPoint, _transitionNode.EnterPoint);
                    DrawArrow(_transitionNode.FromStateNode.OutPoint, _transitionNode.EnterPoint, 15f, ArrowColor.Black);
                }
                
                if(_transitionNode.ToStateNode != null)
                {
                    Handles.color = Color.red;
                    Handles.DrawLine(_transitionNode.OutPoint, _transitionNode.ToStateNode.EnterPoint);
                    DrawArrow(_transitionNode.OutPoint, _transitionNode.ToStateNode.EnterPoint, 15f, ArrowColor.Red);
                }
            }
        }
    }
    
    private void DrawNodeWindow(int id)
    {
        if(id >= m_currentAIBehaviour.nodeDatas.Count)
        {
            return;
        }
        m_currentAIBehaviour.nodeDatas[id].DrawWindow();
        GUI.DragWindow();
    }

    private enum ArrowColor { Black, Red }
    private void DrawArrow(Vector2 startPoint, Vector2 endPoint, float length, ArrowColor color)
    {
        Vector2 _lineDir = endPoint - startPoint;
        _lineDir = _lineDir.normalized;

        float _verticalLength = Mathf.Sqrt((length * length) - ((length / 2) * (length / 2)));

        Vector2 _verticalDir_right = new Vector2(_lineDir.y / (Mathf.Sqrt(_lineDir.x * _lineDir.x + _lineDir.y * _lineDir.y)), -_lineDir.x / (Mathf.Sqrt(_lineDir.x * _lineDir.x + _lineDir.y * _lineDir.y)));
        Vector2 _verticalDir_left = new Vector2(-_lineDir.y / (Mathf.Sqrt(_lineDir.x * _lineDir.x + _lineDir.y * _lineDir.y)), _lineDir.x / (Mathf.Sqrt(_lineDir.x * _lineDir.x + _lineDir.y * _lineDir.y)));

        Vector2 _topPoint = GetCenter(startPoint, endPoint);
        Vector2 _downPoint_right = _topPoint - _lineDir * _verticalLength + _verticalDir_right.normalized * length / 2f;
        Vector2 _downPoint_left = _topPoint - _lineDir * _verticalLength + _verticalDir_left.normalized * length / 2f;

        if (Event.current.type == EventType.Repaint)
        {
            GL.PushMatrix();

            Material material = null;

            switch (color)
            {
                case ArrowColor.Black:
                    {
                        material = new Material(Shader.Find("Unlit/Color"))
                        {
                            color = Color.black
                        };
                        break;
                    }
                case ArrowColor.Red:
                    {
                        material = new Material(Shader.Find("Unlit/Color"))
                        {
                            color = Color.red
                        };
                        break;
                    }
                default:
                    {
                        material = new Material(Shader.Find("Unlit/Color"))
                        {
                            color = Color.black
                        };
                        break;
                    }
            }

            material.SetPass(0);
            GL.LoadOrtho();
            GL.Begin(GL.TRIANGLES);

            _topPoint = ConvertPositionToVertex(_topPoint);
            _downPoint_right = ConvertPositionToVertex(_downPoint_right);
            _downPoint_left = ConvertPositionToVertex(_downPoint_left);

            GL.Vertex3(_downPoint_left.x, _downPoint_left.y, 0);
            GL.Vertex3(_downPoint_right.x, _downPoint_right.y, 0);
            GL.Vertex3(_topPoint.x, _topPoint.y, 0);

            GL.End();
            GL.PopMatrix();
        }
    }

    private void OnSaved(AIBehaviourData data)
    {
        m_currentAIBehaviour = data;
        m_assignedAIBehaviour = data;
        SaveNodeData();
    }

    private void SaveNodeData()
    {
        for (int i = 0; i < m_currentAIBehaviour.nodeDatas.Count; i++)
        {
            EditorUtility.SetDirty(m_currentAIBehaviour.nodeDatas[i]);
        }
        EditorUtility.SetDirty(m_currentAIBehaviour);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private void Reassignodes()
    {
        m_currentID = -1;
        m_savePath = m_currentAIBehaviour.savePath;

        for(int i = 0; i < m_currentAIBehaviour.nodeDatas.Count; i++)
        {
            if(m_currentID < m_currentAIBehaviour.nodeDatas[i].ID)
            {
                m_currentID = m_currentAIBehaviour.nodeDatas[i].ID;
            }
        }
    }

    public static BaseNode GetNode(long id)
    {
        BaseNode _node = m_currentAIBehaviour.nodeDatas.Find(x => x.ID == id);
        if (_node == null)
        {
            Debug.LogError("Node ID " + id + " Not Existed");
            return null;
        }
        return _node;
    }
}
