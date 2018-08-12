using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public class AIBehaviourSaver {

    private static Dictionary<long, IdleState> nodeIdToIdleStates = new Dictionary<long, IdleState>();
    private static Dictionary<long, AttackState> nodeIdToAttackStates = new Dictionary<long, AttackState>();
    private static Dictionary<long, MoveState> nodeIdToMoveStates = new Dictionary<long, MoveState>();
    private static Dictionary<long, List<DistanceCondition>> nodeIdToDistanceConditions = new Dictionary<long, List<DistanceCondition>>();
    private static Dictionary<long, List<NearestIsCondition>> nodeIdToNearestIsConditions = new Dictionary<long, List<NearestIsCondition>>();
    private static Dictionary<long, List<StatusCondition>> nodeIdToStatusConditions = new Dictionary<long, List<StatusCondition>>();

    private static Dictionary<DistanceCondition, int> distanceConditionToConditionListIndex = new Dictionary<DistanceCondition, int>();
    private static Dictionary<NearestIsCondition, int> nearestIsConditionToConditionListIndex = new Dictionary<NearestIsCondition, int>();
    private static Dictionary<StatusCondition, int> statusConditionToConditionListIndex = new Dictionary<StatusCondition, int>();

    private static List<AIStateBase> allState = new List<AIStateBase>();
    private static List<AIConditionBase> allCondition = new List<AIConditionBase>();

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

            nodeIdToIdleStates = new Dictionary<long, IdleState>();
            nodeIdToAttackStates = new Dictionary<long, AttackState>();
            nodeIdToMoveStates = new Dictionary<long, MoveState>();
            nodeIdToDistanceConditions = new Dictionary<long, List<DistanceCondition>>();
            nodeIdToNearestIsConditions = new Dictionary<long, List<NearestIsCondition>>();
            nodeIdToStatusConditions = new Dictionary<long, List<StatusCondition>>();

            distanceConditionToConditionListIndex = new Dictionary<DistanceCondition, int>();
            nearestIsConditionToConditionListIndex = new Dictionary<NearestIsCondition, int>();
            statusConditionToConditionListIndex = new Dictionary<StatusCondition, int>();

            allState = new List<AIStateBase>();
            allCondition = new List<AIConditionBase>();

            for (int i = 0; i < _nodes.Count; i++)
            {
                if (_nodes[i] is StateNode)
                {
                    StateNode _stateNode = (StateNode)_nodes[i];
                    switch (_stateNode.stateType)
                    {
                        case StateNode.StateType.Idle:
                            {
                                CreateStateInstance(_statePath, _nodes[i].ID, ref nodeIdToIdleStates);
                                break;
                            }
                        case StateNode.StateType.Attack:
                            {
                                CreateStateInstance(_statePath, _nodes[i].ID, ref nodeIdToAttackStates);
                                break;
                            }
                        case StateNode.StateType.Move:
                            {
                                CreateStateInstance(_statePath, _nodes[i].ID, ref nodeIdToMoveStates);
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
                                    CreateConditionInstance(_conditionPath, _nodes[i].ID, _transitionDataIndex, ref nodeIdToDistanceConditions, ref distanceConditionToConditionListIndex);
                                    break;
                                }
                            case TransitionNodeData.ConditionType.NearestIs:
                                {
                                    CreateConditionInstance(_conditionPath, _nodes[i].ID, _transitionDataIndex, ref nodeIdToNearestIsConditions, ref nearestIsConditionToConditionListIndex);
                                    break;
                                }
                            case TransitionNodeData.ConditionType.Status:
                                {
                                    CreateConditionInstance(_conditionPath, _nodes[i].ID, _transitionDataIndex, ref nodeIdToStatusConditions, ref statusConditionToConditionListIndex);
                                    break;
                                }
                        }
                    }
                }
            }

            // Set Data
            foreach (KeyValuePair<long, IdleState> kvp in nodeIdToIdleStates)
            {
                AssignNextStatesToState(kvp.Key, kvp.Value);
            }

            foreach (KeyValuePair<long, AttackState> kvp in nodeIdToAttackStates)
            {
                StateNode _stateNode = AIBehaviourEditor.GetNode(kvp.Key) as StateNode;
                if (nodeIdToIdleStates.ContainsKey(_stateNode.defaultIdleStateNodeID))
                {
                    nodeIdToAttackStates[kvp.Key].SetData(nodeIdToIdleStates[_stateNode.defaultIdleStateNodeID], _stateNode.attackTargetType);
                }
                else
                {
                    nodeIdToAttackStates[kvp.Key].SetData(null, _stateNode.attackTargetType);
                }
                AssignNextStatesToState(kvp.Key, kvp.Value);
            }

            foreach (KeyValuePair<long, MoveState> kvp in nodeIdToMoveStates)
            {
                StateNode _stateNode = AIBehaviourEditor.GetNode(kvp.Key) as StateNode;
                if (nodeIdToIdleStates.ContainsKey(_stateNode.defaultIdleStateNodeID))
                {
                    nodeIdToMoveStates[kvp.Key].SetData(nodeIdToIdleStates[_stateNode.defaultIdleStateNodeID], _stateNode.moveTargetType, _stateNode.detctRangeData);
                }
                else
                {
                    nodeIdToMoveStates[kvp.Key].SetData(null, _stateNode.moveTargetType, _stateNode.detctRangeData);
                }
                AssignNextStatesToState(kvp.Key, kvp.Value);
            }

            foreach (KeyValuePair<long, List<DistanceCondition>> kvp in nodeIdToDistanceConditions)
            {
                TransitionNode _transitionNode = AIBehaviourEditor.GetNode(kvp.Key) as TransitionNode;

                for (int i = 0; i < kvp.Value.Count; i++)
                {
                    int _index = distanceConditionToConditionListIndex[kvp.Value[i]];
                    kvp.Value[i].SetData(_transitionNode.transitionNodeDatas[_index].distanceConditionTarget, _transitionNode.transitionNodeDatas[_index].compareCondition, _transitionNode.transitionNodeDatas[_index].distance);
                }
            }

            foreach (KeyValuePair<long, List<NearestIsCondition>> kvp in nodeIdToNearestIsConditions)
            {
                TransitionNode _transitionNode = AIBehaviourEditor.GetNode(kvp.Key) as TransitionNode;
                for (int i = 0; i < kvp.Value.Count; i++)
                {
                    int _index = nearestIsConditionToConditionListIndex[kvp.Value[i]];
                    kvp.Value[i].SetData(_transitionNode.transitionNodeDatas[_index].actorType);
                }
            }

            foreach (KeyValuePair<long, List<StatusCondition>> kvp in nodeIdToStatusConditions)
            {
                TransitionNode _transitionNode = AIBehaviourEditor.GetNode(kvp.Key) as TransitionNode;
                for (int i = 0; i < kvp.Value.Count; i++)
                {
                    int _index = statusConditionToConditionListIndex[kvp.Value[i]];
                    kvp.Value[i].SetData(_transitionNode.transitionNodeDatas[_index].statusType, _transitionNode.transitionNodeDatas[_index].compareCondition, _transitionNode.transitionNodeDatas[_index].statusConditionTarget, _transitionNode.transitionNodeDatas[_index].value);
                }
            }

            // Set Dirty

            for (int i = 0; i < _nodes.Count; i++)
            {
                EditorUtility.SetDirty(_nodes[i]);
            }
            for(int i = 0; i < allState.Count; i++)
            {
                EditorUtility.SetDirty(allState[i]);
            }
            for (int i = 0; i < allCondition.Count; i++)
            {
                EditorUtility.SetDirty(allCondition[i]);
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
        nodeIdToState.Add(id, _state);
        allState.Add(_state);
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
        allCondition.Add(_condition);
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
        if (nodeIdToDistanceConditions.ContainsKey(transitionNodeId))
        {
            for (int _conditionIndex = 0; _conditionIndex < nodeIdToDistanceConditions[transitionNodeId].Count; _conditionIndex++)
            {
                nextAIState.conditions.Add(nodeIdToDistanceConditions[transitionNodeId][_conditionIndex]);
            }
        }
        if (nodeIdToNearestIsConditions.ContainsKey(transitionNodeId))
        {
            for (int _conditionIndex = 0; _conditionIndex < nodeIdToNearestIsConditions[transitionNodeId].Count; _conditionIndex++)
            {
                nextAIState.conditions.Add(nodeIdToNearestIsConditions[transitionNodeId][_conditionIndex]);
            }
        }
        if (nodeIdToStatusConditions.ContainsKey(transitionNodeId))
        {
            for (int _conditionIndex = 0; _conditionIndex < nodeIdToStatusConditions[transitionNodeId].Count; _conditionIndex++)
            {
                nextAIState.conditions.Add(nodeIdToStatusConditions[transitionNodeId][_conditionIndex]);
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
                        nextAIState.nextState = nodeIdToAttackStates[_transitionNode.ToStateNode.ID];
                        break;
                    }
                case StateNode.StateType.Idle:
                    {
                        nextAIState.nextState = nodeIdToIdleStates[_transitionNode.ToStateNode.ID];
                        break;
                    }
                case StateNode.StateType.Move:
                    {
                        nextAIState.nextState = nodeIdToMoveStates[_transitionNode.ToStateNode.ID];
                        break;
                    }
            }
        }
    }

}
