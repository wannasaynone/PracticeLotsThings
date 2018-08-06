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
                        switch (filteCondition.actorType)
                        {
                            case ActorType.All:
                                {
                                    _filtedActors.Add((Actor)_allActors[i]);
                                    break;
                                }
                            case ActorType.Normal:
                                {
                                    if (_allActors[i] is NormalActor && !(_allActors[i] is ShooterActor))
                                    {
                                        _filtedActors.Add((NormalActor)_allActors[i]);
                                    }
                                    break;
                                }
                            case ActorType.Shooter:
                                {
                                    if(_allActors[i] is ShooterActor)
                                    {
                                        _filtedActors.Add((ShooterActor)_allActors[i]);
                                    }
                                    break;
                                }
                            case ActorType.Zombie:
                                {
                                    if (_allActors[i] is ZombieActor)
                                    {
                                        _filtedActors.Add((ZombieActor)_allActors[i]);
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
                case FilteBy.Distance:
                    {
                        if (compareTarget != null)
                        {
                            _targetValue = filteCondition.value;
                            _actorValue = Vector3.Distance(_actor.transform.position, compareTarget.transform.position);
                        }
                        else
                        {
                            Debug.LogError("Need to set a compare target when filting actors by distance");
                        }
                        break;
                    }
                case FilteBy.Hp:
                    {
                        if(compareTarget != null)
                        {
                            _targetValue = m_actorManager.GetCharacterStatus(compareTarget).HP;
                        }
                        else
                        {
                            _targetValue = filteCondition.value;
                        }
                        _actorValue = m_actorManager.GetCharacterStatus(_actor).HP;
                        break;
                    }
                    // rest of Values....
            }

            switch(filteCondition.compareCondition)
            {
                case CompareCondition.Is:
                    {
                        if (_actorValue == _targetValue)
                        {
                            if (IsMatchNeededActorType(filteCondition, _actor))
                            {
                                addTo.Add(_actor);
                            }
                            break;
                        }
                        break;
                    }
                case CompareCondition.More:
                    {
                        if (_actorValue >= _targetValue)
                        {
                            if (IsMatchNeededActorType(filteCondition, _actor))
                            {
                                addTo.Add(_actor);
                            }
                            break;
                        }
                        break;
                    }
                case CompareCondition.Less:
                    {
                        if (_actorValue <= _targetValue)
                        {
                            if (IsMatchNeededActorType(filteCondition, _actor))
                            {
                                addTo.Add(_actor);
                            }
                            break;
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
                    return actor is NormalActor && !(actor is ShooterActor);
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

    public static Actor GetNearestActor(ActorType type, Actor compareWith)
    {
        List<Actor> _actors = Engine.ActorFilter.GetActors(
            new FilteCondition()
            {
                filteBy = FilteBy.Type,
                compareCondition = CompareCondition.Is,
                actorType = type,
            });

        return GetNearestActor(_actors, compareWith);
    }

    public static Actor GetNearestActor(List<Actor> actors, Actor compareWith)
    {
        if (actors.Count <= 0)
        {
            return null;
        }

        actors.Remove(compareWith);
        List<Actor> _waitToRemove = new List<Actor>();

        for (int i = 0; i < actors.Count; i++)
        {
            if(Engine.ActorManager.GetCharacterStatus(actors[i]).HP <= 0)
            {
                _waitToRemove.Add(actors[i]);
            }
        }

        for (int i = 0; i < _waitToRemove.Count; i++)
        {
            actors.Remove(_waitToRemove[i]);
        }

        if(actors.Count <= 0)
        {
            return null;
        }

        Actor _actor = actors[0];
        for (int i = 1; i < actors.Count; i++)
        {
            if (Vector3.Distance(actors[i].transform.position, compareWith.transform.position) < Vector3.Distance(_actor.transform.position, compareWith.transform.position))
            {
                _actor = actors[i];
            }
        }

        return _actor;
    }

}
