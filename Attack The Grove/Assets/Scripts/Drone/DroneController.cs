using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class DroneController : MonoBehaviour
{
    FSM<StatesEnum> _fsm;
    ITreeNode _root;
    [SerializeField] DroneModel _model;
    //[SerializeField] GameObject target;
    private Rigidbody _targetRigidbody;

    [SerializeField] bool isDrone;
    [SerializeField] float closeToLeader;
    [SerializeField] GameObject target;

    public float attackRange;
    public float currentHealth;
    public LeaderBehaviour leaderBehaviour;
    ISteering _pursuit;
    ISteering _steering;
    ObstacleAvoidanceV2 _obstacleAvoidance;
    ILineOfSight _los;

    #region Steering
    public float timePrediction;
    public float angle;
    public float radius;
    public float personalArea;
    public LayerMask maskObs;
    #endregion

    private void Awake()
    {
        _model = GetComponent<DroneModel>();
        _los = GetComponent<ILineOfSight>();

    }
    void Start()
    {
        InitializeSteerings();
        InitializedTree();
        InitializeFSM();

    }

    void InitializeFSM()
    {
        var idle = new DroneIdleState<StatesEnum>(_model);
        var follow = new DroneFollowState<StatesEnum>(_model, _steering, _obstacleAvoidance);
        var flee = new DroneFleeState<StatesEnum>(_model, target.transform, _steering, _obstacleAvoidance);
        var death = new DroneDeathState<StatesEnum>(_model);
        var attack = new DroneAttackState<StatesEnum>(_model);
        var chase = new DroneChaseState<StatesEnum>(_model, target.transform, _steering, _obstacleAvoidance);

        idle.AddTransition(StatesEnum.Follow, follow);
        idle.AddTransition(StatesEnum.Death, death);
        idle.AddTransition(StatesEnum.Flee, flee);
        idle.AddTransition(StatesEnum.Attack, attack);
        idle.AddTransition(StatesEnum.Chase, chase);

        follow.AddTransition(StatesEnum.Idle, idle);
        follow.AddTransition(StatesEnum.Flee, flee);
        follow.AddTransition(StatesEnum.Death, death);
        follow.AddTransition(StatesEnum.Attack, attack);
        follow.AddTransition(StatesEnum.Chase, chase);

        chase.AddTransition(StatesEnum.Idle, idle);
        chase.AddTransition(StatesEnum.Attack, attack);
        chase.AddTransition(StatesEnum.Death, death);
        chase.AddTransition(StatesEnum.Flee, flee);


        attack.AddTransition(StatesEnum.Idle, idle);
        attack.AddTransition(StatesEnum.Death, death);
        attack.AddTransition(StatesEnum.Flee, flee);
        attack.AddTransition(StatesEnum.Follow, follow);
        attack.AddTransition(StatesEnum.Chase, chase);

        flee.AddTransition(StatesEnum.Flee, flee);
        flee.AddTransition(StatesEnum.Idle, idle);
        flee.AddTransition(StatesEnum.Death, death);
        flee.AddTransition(StatesEnum.Attack, attack);
        flee.AddTransition(StatesEnum.Chase, chase);

        _fsm = new FSM<StatesEnum>(idle);
    }



    void InitializeSteerings()
    {
        _steering = GetComponent<FlockingManager>();

        _obstacleAvoidance = new ObstacleAvoidanceV2(_model.transform, angle, radius, maskObs, personalArea);

    }

    void InitializedTree()
    {
        // Actions
        var idle = new ActionNode(() => _fsm.Transition(StatesEnum.Idle));
        var follow = new ActionNode(() => _fsm.Transition(StatesEnum.Follow));
        var death = new ActionNode(() => _fsm.Transition(StatesEnum.Death)); //lefe
        var attack = new ActionNode(() => _fsm.Transition(StatesEnum.Attack));
        var flee = new ActionNode(() => _fsm.Transition(StatesEnum.Flee)); //lefe
        var chase = new ActionNode(() => _fsm.Transition(StatesEnum.Chase));

        // Questions
        QuestionNode distanceToLead = new QuestionNode(QuestionTooClose, idle, follow);

        var qAttackRange = new QuestionNode(QuestionAttackRange, attack, chase);
        var qLoS = new QuestionNode(() => _model.fight == true, qAttackRange, follow);
        var qFlee = new QuestionNode(() => currentHealth >= 10, qLoS, flee);
        var qHealth = new QuestionNode(() => currentHealth >= 0, qFlee, death);

        _root = distanceToLead;
    }

    bool QuestionLoS()
    {
        var seen = _los.CheckRange(target.transform)
                //&& _los.CheckAngle(target.transform)
                && _los.CheckView(target.transform);

        return seen;
    }

    bool QuestionTooClose()
    {
        return Vector3.Distance(transform.position, leaderBehaviour.target.position) < closeToLeader;
    }

    bool QuestionPatrol()
    {
        return true;
    }

    bool QuestionAttackRange()
    {
        return _los.CheckRange(target.transform, attackRange);
    }

    void Update()
    {
        _fsm.OnUpdate();
        _root.Execute();
        currentHealth = _model.health;
    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == 10 || other.gameObject.layer == 11)
        {
            currentHealth--;
        }

    }
}