using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorFilter : Manager {

	public enum FilteBy
    {
        Distance,
        Hp,
        Type
    }

    public enum CompareCondition
    {
        More,
        Less,
        Is
    }

    public enum ActorType
    {
        Shooter,
        Zombie,
        Normal,
        All
    }

    public struct FilteCondition
    {
        public FilteBy filteBy;
        public CompareCondition compareCondition;
        public ActorType actorType;
        public float value;
    }

    private ActorManager m_actorManager = null;

    public ActorFilter(ActorManager actorManager)
    {
        m_actorManager = actorManager;
    }

    public List<Actor> GetActors(FilteCondition filteCondition, Actor compareTarget = null)
    {
        List<View> _allActors = GetViews<Actor>();
        List<Actor> _filtedActors = new List<Actor>();

        switch (filteCondition.filteBy)
        {
            case FilteBy.Distance:
                {
                    if (compareTarget == null)
                    {
                        Debug.LogWarning("Need to set a compare target when filting actors by distance");
                        break;
                    }

                    for (int i = 0; i < _allActors.Count; i++)
                    {
                        Actor _actor = _allActors[i] as Actor;
                        float _distance = Vector3.Distance(_actor.transform.position, compareTarget.transform.position);
                        switch(filteCondition.compareCondition)
                        {
                            case CompareCondition.Is:
                                {
                                    if(_distance == filteCondition.value)
                                    {
                                        if(IsMatchNeededActorType(filteCondition, _actor))
                                        {
                                            _filtedActors.Add(_actor);
                                        }
                                    }
                                    break;
                                }
                            case CompareCondition.More:
                                {
                                    if (_distance >= filteCondition.value)
                                    {
                                        if (IsMatchNeededActorType(filteCondition, _actor))
                                        {
                                            _filtedActors.Add(_actor);
                                        }
                                    }
                                    break;
                                }
                            case CompareCondition.Less:
                                {
                                    if (_distance <= filteCondition.value)
                                    {
                                        if (IsMatchNeededActorType(filteCondition, _actor))
                                        {
                                            _filtedActors.Add(_actor);
                                        }
                                    }
                                    break;
                                }
                        }
                    }

                    break;
                }
            case FilteBy.Hp:
                {
                    AddActorBySatus(filteCondition, _allActors, ref _filtedActors, compareTarget);
                    break;
                }
            case FilteBy.Type:
                {
                    if (filteCondition.compareCondition != CompareCondition.Is)
                    {
                        Debug.LogWarning("FilteCondition MUST be \"Is\" when filting by ActorType");
                        break;
                    }

                    for (int i = 0; i < _allActors.Count; i++)
                    {
                        Actor _actor = _allActors[i] as Actor;

                        switch (filteCondition.actorType)
                        {
                            case ActorType.All:
                                {
                                    _filtedActors.Add(_actor);
                                    break;
                                }
                            case ActorType.Normal:
                                {
                                    // TODO: not completed yet
                                    break;
                                }
                            case ActorType.Shooter:
                                {
                                    if(_actor is ShooterActor)
                                    {
                                        _filtedActors.Add(_actor);
                                    }
                                    break;
                                }
                            case ActorType.Zombie:
                                {
                                    if (_actor is ZombieActor)
                                    {
                                        _filtedActors.Add(_actor);
                                    }
                                    break;
                                }
                        }
                    }
                    break;
                }
        }

        return _filtedActors;
    }

    private void AddActorBySatus(FilteCondition filteCondition, List<View> allActors, ref List<Actor> addTo, Actor compareTarget)
    {
        for (int i = 0; i < allActors.Count; i++)
        {
            Actor _actor = allActors[i] as Actor;
            float _targetValue = 0f;
            float _actorValue = 0f;

            switch(filteCondition.filteBy)
            {
                case FilteBy.Hp:
                    {
                        if(compareTarget != null)
                        {
                            _targetValue = m_actorManager.GetCharacterStatus(compareTarget).HP;
                        }

                        _actorValue = m_actorManager.GetCharacterStatus(_actor).HP;
                        break;
                    }
            }

            switch(filteCondition.compareCondition)
            {
                case CompareCondition.Is:
                    {
                        if (compareTarget != null)
                        {
                            if(_actorValue == _targetValue)
                            {
                                if (IsMatchNeededActorType(filteCondition, _actor))
                                {
                                    addTo.Add(_actor);
                                }
                                break;
                            }
                        }
                        else
                        {
                            if (_actorValue == filteCondition.value)
                            {
                                if (IsMatchNeededActorType(filteCondition, _actor))
                                {
                                    addTo.Add(_actor);
                                }
                                break;
                            }
                        }
                        break;
                    }
                case CompareCondition.More:
                    {
                        if (compareTarget != null)
                        {
                            if (_actorValue >= _targetValue)
                            {
                                if (IsMatchNeededActorType(filteCondition, _actor))
                                {
                                    addTo.Add(_actor);
                                }
                                break;
                            }
                        }
                        else
                        {
                            if (_actorValue >= filteCondition.value)
                            {
                                if (IsMatchNeededActorType(filteCondition, _actor))
                                {
                                    addTo.Add(_actor);
                                }
                                break;
                            }
                        }
                        break;
                    }
                case CompareCondition.Less:
                    {
                        if (compareTarget != null)
                        {
                            if (_actorValue <= _targetValue)
                            {
                                if (IsMatchNeededActorType(filteCondition, _actor))
                                {
                                    addTo.Add(_actor);
                                }
                                break;
                            }
                        }
                        else
                        {
                            if (_actorValue <= filteCondition.value)
                            {
                                if (IsMatchNeededActorType(filteCondition, _actor))
                                {
                                    addTo.Add(_actor);
                                }
                                break;
                            }
                        }
                        break;
                    }
            }
        }
    }

    private bool IsMatchNeededActorType(FilteCondition filteCondition, Actor actor)
    {
        switch(filteCondition.actorType)
        {
            case ActorType.All:
                {
                    return true;
                }
            case ActorType.Normal:
                {
                    return false; // TODO: not completed yet
                }
            case ActorType.Shooter:
                {
                    return actor is ShooterActor;
                }
            case ActorType.Zombie:
                {
                    return actor is ZombieActor;
                }
        }
        return false;
    }

    public static Actor GetNearestActor(List<Actor> actors, Actor ai)
    {
        if (actors.Count <= 0)
        {
            return null;
        }

        Actor _actor = actors[0];
        for (int i = 1; i < actors.Count; i++)
        {
            if (Vector3.Distance(actors[i].transform.position, ai.transform.position) < Vector3.Distance(_actor.transform.position, ai.transform.position))
            {
                _actor = actors[i];
            }
        }

        return _actor;
    }

}
