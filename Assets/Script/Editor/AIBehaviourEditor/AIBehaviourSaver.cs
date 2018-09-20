using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public class AIBehaviourSaver {

    private static Dictionary<long, AIStateBase> m_nodeIdToAllStates = new Dictionary<long, AIStateBase>();
    private static Dictionary<long, IdleState> m_nodeIdToIdleStates = new Dictionary<long, IdleState>();
    private static Dictionary<long, AttackState> m_nodeIdToAttackStates = new Dictionary<long, AttackState>();
    private static Dictionary<long, MoveState> m_nodeIdToMoveStates = new Dictionary<long, MoveState>();
    private static Dictionary<long, List<DistanceCondition>> m_nodeIdToDistanceConditions = new Dictionary<long, List<DistanceCondition>>();
    private static Dictionary<long, List<NearestIsCondition>> m_nodeIdToNearestIsConditions = new Dictionary<long, List<NearestIsCondition>>();
    private static Dictionary<long, List<StatusCondition>> m_nodeIdToStatusConditions = new Dictionary<long, List<StatusCondition>>();

    private static Dictionary<DistanceCondition, int> m_distanceConditionToConditionListIndex = new Dictionary<DistanceCondition, int>();
    private static Dictionary<NearestIsCondition, int> m_nearestIsConditionToConditionListIndex = new Dictionary<NearestIsCondition, int>();
    private static Dictionary<StatusCondition, int> m_statusConditionToConditionListIndex = new Dictionary<StatusCondition, int>();

    private static List<AIStateBase> m_allState = new List<AIStateBase>();
    private static List<AIConditionBase> m_allCondition = new List<AIConditionBase>();

    public static void Save(string savePath, AIBehaviourData aiBehaviour, Action<AIBehaviourData> onSaved = null)
    {
        if (Directory.Exists(savePath))
        {
            List<BaseNode> _nodes = aiBehaviour.nodeDatas;

            string _statePath = savePath + "States/";
            string _conditionPath = savePath + "Conditions/";

            if (!Directory.Exists(_statePath))
            {
                Directory.CreateDirectory(_statePath);
            }
            else
            {
                Directory.Delete(_statePath, true);
                Directory.CreateDirectory(_statePath);
            }

            if (!Directory.Exists(_conditionPath))
            {
                Directory.CreateDirectory(_conditionPath);
            }
            else
            {
                Directory.Delete(_conditionPath, true);
                Directory.CreateDirectory(_conditionPath);
            }

            m_nodeIdToIdleStates = new Dictionary<long, IdleState>();
            m_nodeIdToAttackStates = new Dictionary<long, AttackState>();
            m_nodeIdToMoveStates = new Dictionary<long, MoveState>();
            m_nodeIdToDistanceConditions = new Dictionary<long, List<DistanceCondition>>();
            m_nodeIdToNearestIsConditions = new Dictionary<long, List<NearestIsCondition>>();
            m_nodeIdToStatusConditions = new Dictionary<long, List<StatusCondition>>();

            m_distanceConditionToConditionListIndex = new Dictionary<DistanceCondition, int>();
            m_nearestIsConditionToConditionListIndex = new Dictionary<NearestIsCondition, int>();
            m_statusConditionToConditionListIndex = new Dictionary<StatusCondition, int>();

            m_allState = new List<AIStateBase>();
            m_allCondition = new List<AIConditionBase>();

            for (int i = 0; i < _nodes.Count; i++)
            {
                if (_nodes[i] is StateNode)
                {
                    StateNode _stateNode = (StateNode)_nodes[i];
                    switch (_stateNode.stateType)
                    {
                        case StateNode.StateType.Idle:
                            {
                                CreateStateInstance(_statePath, _nodes[i].ID, ref m_nodeIdToIdleStates);
                                break;
                            }
                        case StateNode.StateType.Attack:
                            {
                                CreateStateInstance(_statePath, _nodes[i].ID, ref m_nodeIdToAttackStates);
                                break;
                            }
                        case StateNode.StateType.Move:
                            {
                                CreateStateInstance(_statePath, _nodes[i].ID, ref m_nodeIdToMoveStates);
                                break;
                            }
                    }
                }

                if (_nodes[i] is TransitionNode)
                {
                    TransitionNode _transitionNode = (TransitionNode)_nodes[i];

                    for (int _transitionDataIndex = 0; _transitionDataIndex < _transitionNode.transitionNodeDatas.Count; _transitionDataIndex++)
                    {
                        switch (_transitionNode.transitionNodeDatas[_transitionDataIndex].conditionType)
                        {
                            case TransitionNodeData.ConditionType.Distance:
                                {
                                    CreateConditionInstance(_conditionPath, _nodes[i].ID, _transitionDataIndex, ref m_nodeIdToDistanceConditions, ref m_distanceConditionToConditionListIndex);
                                    break;
                                }
                            case TransitionNodeData.ConditionType.NearestIs:
                                {
                                    CreateConditionInstance(_conditionPath, _nodes[i].ID, _transitionDataIndex, ref m_nodeIdToNearestIsConditions, ref m_nearestIsConditionToConditionListIndex);
                                    break;
                                }
                            case TransitionNodeData.ConditionType.Status:
                                {
                                    CreateConditionInstance(_conditionPath, _nodes[i].ID, _transitionDataIndex, ref m_nodeIdToStatusConditions, ref m_statusConditionToConditionListIndex);
                                    break;
                                }
                        }
                    }
                }
            }

            // Set Data
            foreach (KeyValuePair<long, IdleState> kvp in m_nodeIdToIdleStates)
            {
                AssignNextStatesToState(kvp.Key, kvp.Value);
            }

            foreach (KeyValuePair<long, AttackState> kvp in m_nodeIdToAttackStates)
            {
                StateNode _stateNode = AIBehaviourEditor.GetNode(kvp.Key) as StateNode;
                if (m_nodeIdToIdleStates.ContainsKey(_stateNode.defaultIdleStateNodeID))
                {
                    m_nodeIdToAttackStates[kvp.Key].SetData(m_nodeIdToAllStates[_stateNode.defaultIdleStateNodeID], _stateNode.attackTargetType);
                }
                else
                {
                    m_nodeIdToAttackStates[kvp.Key].SetData(null, _stateNode.attackTargetType);
                }
                AssignNextStatesToState(kvp.Key, kvp.Value);
            }

            foreach (KeyValuePair<long, MoveState> kvp in m_nodeIdToMoveStates)
            {
                StateNode _stateNode = AIBehaviourEditor.GetNode(kvp.Key) as StateNode;
                if (m_nodeIdToIdleStates.ContainsKey(_stateNode.defaultIdleStateNodeID))
                {
                    m_nodeIdToMoveStates[kvp.Key].SetData(m_nodeIdToAllStates[_stateNode.defaultIdleStateNodeID], _stateNode.moveTargetType, _stateNode.detctRangeData);
                }
                else
                {
                    m_nodeIdToMoveStates[kvp.Key].SetData(null, _stateNode.moveTargetType, _stateNode.detctRangeData);
                }
                AssignNextStatesToState(kvp.Key, kvp.Value);
            }

            foreach (KeyValuePair<long, List<DistanceCondition>> kvp in m_nodeIdToDistanceConditions)
            {
                TransitionNode _transitionNode = AIBehaviourEditor.GetNode(kvp.Key) as TransitionNode;

                for (int i = 0; i < kvp.Value.Count; i++)
                {
                    int _index = m_distanceConditionToConditionListIndex[kvp.Value[i]];
                    kvp.Value[i].SetData(_transitionNode.transitionNodeDatas[_index].distanceConditionTarget, _transitionNode.transitionNodeDatas[_index].compareCondition, _transitionNode.transitionNodeDatas[_index].distance);
                }
            }

            foreach (KeyValuePair<long, List<NearestIsCondition>> kvp in m_nodeIdToNearestIsConditions)
            {
                TransitionNode _transitionNode = AIBehaviourEditor.GetNode(kvp.Key) as TransitionNode;
                for (int i = 0; i < kvp.Value.Count; i++)
                {
                    int _index = m_nearestIsConditionToConditionListIndex[kvp.Value[i]];
                    kvp.Value[i].SetData(_transitionNode.transitionNodeDatas[_index].actorType);
                }
            }

            foreach (KeyValuePair<long, List<StatusCondition>> kvp in m_nodeIdToStatusConditions)
            {
                TransitionNode _transitionNode = AIBehaviourEditor.GetNode(kvp.Key) as TransitionNode;
                for (int i = 0; i < kvp.Value.Count; i++)
                {
                    int _index = m_statusConditionToConditionListIndex[kvp.Value[i]];
                    kvp.Value[i].SetData(_transitionNode.transitionNodeDatas[_index].statusType, _transitionNode.transitionNodeDatas[_index].compareCondition, _transitionNode.transitionNodeDatas[_index].statusConditionTarget, _transitionNode.transitionNodeDatas[_index].value);
                }
            }

            // Set Dirty

            for (int i = 0; i < _nodes.Count; i++)
            {
                EditorUtility.SetDirty(_nodes[i]);
            }
            for(int i = 0; i < m_allState.Count; i++)
            {
                EditorUtility.SetDirty(m_allState[i]);
            }
            for (int i = 0; i < m_allCondition.Count; i++)
            {
                EditorUtility.SetDirty(m_allCondition[i]);
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = aiBehaviour;

            if(onSaved != null)
            {
                onSaved(aiBehaviour);
            }
        }
        else
        {
            EditorUtility.DisplayDialog("Error", "Unexisting Path:" + savePath, "OK");
        }
    }

    private static void CreateStateInstance<T>(string path, long id, ref Dictionary<long, T> nodeIdToState) where T : AIStateBase
    {
        T _state = ScriptableObject.CreateInstance<T>();
        // Debug.Log("Creating " + typeof(T).Name + id + ".asset");
        AssetDatabase.CreateAsset(_state, path + typeof(T).Name + id + ".asset");
        if(!nodeIdToState.ContainsKey(id))
        {
            nodeIdToState.Add(id, _state);
        }
        else
        {
            nodeIdToState[id] = _state;
        }
        if(!m_nodeIdToAllStates.ContainsKey(id))
        {
            m_nodeIdToAllStates.Add(id, _state);
        }
        else
        {
            m_nodeIdToAllStates[id] = _state;
        }
        m_allState.Add(_state);
    }

    private static void CreateConditionInstance<T>(string path, long id, int transitionNodeConditionIndex, ref Dictionary<long, List<T>> nodeIdToCondition, ref Dictionary<T, int> conditionToListIndex) where T : AIConditionBase
    {
        T _condition = ScriptableObject.CreateInstance<T>();
        // Debug.Log("Creating " + typeof(T).Name + id + ".asset");
        AssetDatabase.CreateAsset(_condition, path + typeof(T).Name + id + "_" + transitionNodeConditionIndex + ".asset");
        if (nodeIdToCondition.ContainsKey(id))
        {
            nodeIdToCondition[id].Add(_condition);
        }
        else
        {
            nodeIdToCondition.Add(id, new List<T>() { _condition });
        }
        conditionToListIndex.Add(_condition, transitionNodeConditionIndex);
        m_allCondition.Add(_condition);
    }

    private static void AssignNextStatesToState(long stateNodeId, AIStateBase state)
    {
        List<TransitionNode> _transitionNodes = ((StateNode)AIBehaviourEditor.GetNode(stateNodeId)).transitions_out;
        NextAIState[] _nextAIStates = new NextAIState[_transitionNodes.Count];
        for (int _transitionNodeIndex = 0; _transitionNodeIndex < _transitionNodes.Count; _transitionNodeIndex++)
        {
            _nextAIStates[_transitionNodeIndex] = new NextAIState
            {
                conditions = new List<AIConditionBase>()
            };

            long _transitonNodeID = _transitionNodes[_transitionNodeIndex].ID;

            AssignConditionToNextState(_transitonNodeID, ref _nextAIStates[_transitionNodeIndex]);
            AssignStateToNextState(_transitonNodeID, ref _nextAIStates[_transitionNodeIndex]);

        }
        state.SetNextAIStates(_nextAIStates);
    }

    private static void AssignConditionToNextState(long transitionNodeId, ref NextAIState nextAIState)
    {
        if (m_nodeIdToDistanceConditions.ContainsKey(transitionNodeId))
        {
            for (int _conditionIndex = 0; _conditionIndex < m_nodeIdToDistanceConditions[transitionNodeId].Count; _conditionIndex++)
            {
                nextAIState.conditions.Add(m_nodeIdToDistanceConditions[transitionNodeId][_conditionIndex]);
            }
        }
        if (m_nodeIdToNearestIsConditions.ContainsKey(transitionNodeId))
        {
            for (int _conditionIndex = 0; _conditionIndex < m_nodeIdToNearestIsConditions[transitionNodeId].Count; _conditionIndex++)
            {
                nextAIState.conditions.Add(m_nodeIdToNearestIsConditions[transitionNodeId][_conditionIndex]);
            }
        }
        if (m_nodeIdToStatusConditions.ContainsKey(transitionNodeId))
        {
            for (int _conditionIndex = 0; _conditionIndex < m_nodeIdToStatusConditions[transitionNodeId].Count; _conditionIndex++)
            {
                nextAIState.conditions.Add(m_nodeIdToStatusConditions[transitionNodeId][_conditionIndex]);
            }
        }
    }

    private static void AssignStateToNextState(long transitionNodeId, ref NextAIState nextAIState)
    {
        TransitionNode _transitionNode = AIBehaviourEditor.GetNode(transitionNodeId) as TransitionNode;
        if (_transitionNode.ToStateNode != null)
        {
            switch (_transitionNode.ToStateNode.stateType)
            {
                case StateNode.StateType.Attack:
                    {
                        nextAIState.nextState = m_nodeIdToAttackStates[_transitionNode.ToStateNode.ID];
                        break;
                    }
                case StateNode.StateType.Idle:
                    {
                        nextAIState.nextState = m_nodeIdToIdleStates[_transitionNode.ToStateNode.ID];
                        break;
                    }
                case StateNode.StateType.Move:
                    {
                        nextAIState.nextState = m_nodeIdToMoveStates[_transitionNode.ToStateNode.ID];
                        break;
                    }
            }
        }
    }

}
