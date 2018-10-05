using System.Collections.Generic;
using UnityEngine;
using PracticeLotsThings.View.Actor;
using PracticeLotsThings.MainGameMonoBehaviour;

namespace PracticeLotsThings.Manager
{
    public class ActorFilter
    {
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
            Tower,
            All
        }

        public struct FilteCondition
        {
            public FilteBy filteBy;
            public CompareCondition compareCondition;
            public ActorType actorType;
            public float value;
        }

        private List<Actor> m_allActors = null;

        public List<Actor> GetActors(FilteCondition filteCondition, Actor compareTarget = null)
        {
            m_allActors = ActorManager.AllActors;
            List<Actor> _filtedActors = new List<Actor>();

            switch (filteCondition.filteBy)
            {
                case FilteBy.Distance:
                case FilteBy.Hp:
                    {
                        AddActorBySatus(filteCondition, m_allActors, ref _filtedActors, compareTarget);
                        break;
                    }
                case FilteBy.Type:
                    {
                        if (filteCondition.compareCondition != CompareCondition.Is)
                        {
                            Debug.LogWarning("FilteCondition MUST be \"Is\" when filting by ActorType");
                            break;
                        }

                        for (int i = 0; i < m_allActors.Count; i++)
                        {
                            if (m_allActors[i].GetCharacterStatus().HP <= 0)
                            {
                                continue;
                            }
                            switch (filteCondition.actorType)
                            {
                                case ActorType.All:
                                    {
                                        _filtedActors.Add(m_allActors[i]);
                                        break;
                                    }
                                case ActorType.Normal:
                                case ActorType.Shooter:
                                case ActorType.Zombie:
                                    {
                                        if (IsMatchNeededActorType(filteCondition.actorType, m_allActors[i]))
                                        {
                                            _filtedActors.Add(m_allActors[i]);
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

        private void AddActorBySatus(FilteCondition filteCondition, List<Actor> allActors, ref List<Actor> addTo, Actor compareTarget)
        {
            for (int i = 0; i < allActors.Count; i++)
            {
                float _targetValue = 0f;
                float _actorValue = 0f;

                switch (filteCondition.filteBy)
                {
                    case FilteBy.Distance:
                        {
                            if (compareTarget != null)
                            {
                                _targetValue = filteCondition.value;
                                _actorValue = Vector3.Distance(allActors[i].transform.position, compareTarget.transform.position);
                            }
                            else
                            {
                                Debug.LogError("Need to set a compare target when filting actors by distance");
                            }
                            break;
                        }
                    case FilteBy.Hp:
                        {
                            if (compareTarget != null)
                            {
                                _targetValue = compareTarget.GetCharacterStatus().HP + filteCondition.value;
                            }
                            else
                            {
                                _targetValue = filteCondition.value;
                            }
                            _actorValue = allActors[i].GetCharacterStatus().HP;
                            break;
                        }
                        // rest of Values....
                }

                switch (filteCondition.compareCondition)
                {
                    case CompareCondition.Is:
                        {
                            if (_actorValue == _targetValue)
                            {
                                if (IsMatchNeededActorType(filteCondition.actorType, allActors[i]))
                                {
                                    addTo.Add(allActors[i]);
                                }
                                break;
                            }
                            break;
                        }
                    case CompareCondition.More:
                        {
                            if (_actorValue >= _targetValue)
                            {
                                if (IsMatchNeededActorType(filteCondition.actorType, allActors[i]))
                                {
                                    addTo.Add(allActors[i]);
                                }
                                break;
                            }
                            break;
                        }
                    case CompareCondition.Less:
                        {
                            if (_actorValue <= _targetValue)
                            {
                                if (IsMatchNeededActorType(filteCondition.actorType, allActors[i]))
                                {
                                    addTo.Add(allActors[i]);
                                }
                                break;
                            }
                            break;
                        }
                }
            }
        }

        public static bool IsMatchNeededActorType(ActorType type, Actor actor)
        {
            switch (type)
            {
                case ActorType.All:
                    {
                        return true;
                    }
                case ActorType.Normal:
                    {
                        return actor is NormalActor && !(actor is ShooterActor) && !(actor is TowerActor);
                    }
                case ActorType.Shooter:
                    {
                        return actor is ShooterActor && !(actor is TowerActor);
                    }
                case ActorType.Tower:
                    {
                        return actor is TowerActor;
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
                    value = 0
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
                if (actors[i].GetCharacterStatus().HP <= 0)
                {
                    _waitToRemove.Add(actors[i]);
                }
            }

            for (int i = 0; i < _waitToRemove.Count; i++)
            {
                actors.Remove(_waitToRemove[i]);
            }

            if (actors.Count <= 0)
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
}