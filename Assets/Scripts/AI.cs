using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour {

    // public variables
    public float chaseDistance;
    public float attackDistance;
    public float visionAngle;
    public int totalBullets;
    public float chargingSeconds;
    public float minPointDistance;
    public float shootSeconds;
    public GameObject bulletGO;
    public Material[] materials;
    public NavMeshAgent navMeshAgent;

    private GameObject[] FleePoints;
    private GameObject[] patrollingPoints;
    private int bullets;
    private int searchingPoint = 0;
    private MeshRenderer mr;
    private Animator animator;
    private GameObject player;

    private void Awake()
    {
        player = GameObject.Find("Player");
        animator = gameObject.GetComponent<Animator>();
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        ChargeBullets();
        patrollingPoints = GameObject.FindGameObjectsWithTag("PatrolPoint");
        FleePoints = GameObject.FindGameObjectsWithTag("FleePoint");
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (player != null)
        {
            animator.SetBool("IsPlayerVisible", IsPlayerVisible());
            animator.SetBool("IsPlayerClose", IsPlayerClose());
            animator.SetBool("IsPlayerDetectable", IsPlayerDetectable());
            animator.SetBool("IsPlayerAttackable", IsPlayerAttackable());
            animator.SetBool("IsOnPoint", IsOnPoint());
            animator.SetBool("IsBulletsEmpty", isEmpty());
        }
    }

    public bool IsPlayerDetectable() {
        if(IsPlayerVisible()) {
            if(IsPlayerInAngle()) {
                return IsPlayerClose();
            }
        }
        return false;
    }

    public bool IsPlayerAttackable()
    {
        if (IsPlayerVisible())
        {
            return PlayerDistance() < attackDistance; 
        }
        return false;
    }

    public float PlayerDistance() {
		return Vector3.Magnitude (player.transform.position - transform.position);
	}

	public void SetMaterial(int index) {
		if (mr == null) {
			mr = GetComponent<MeshRenderer> ();
		}
		mr.material = materials [index];
	}

	public void SetNextPoint() {
		searchingPoint = Random.Range(0, patrollingPoints.GetLength(0));
        if(navMeshAgent == null) {
            navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        }
        navMeshAgent.SetDestination(patrollingPoints[searchingPoint].transform.position);
        navMeshAgent.stoppingDistance = 0f;
	}

    public void SetPlayerPosition() {
        navMeshAgent.SetDestination(player.transform.position);
    }

	public bool	IsOnPoint() {
		Vector3 diff = navMeshAgent.destination - transform.position;
		diff.y = 0f;
		return diff.magnitude <= minPointDistance;
	}

	public bool IsPlayerVisible() {
		RaycastHit hit;
		Vector3 direction = player.transform.position - transform.position;
        direction.y = 0; // make it horizontal
        direction = Vector3.Normalize(direction);
        if (Physics.Raycast (transform.position, direction, out hit, 100f)) {
            if (hit.transform.tag == "Player") {
                return true;
			}
		}
		return false;
	}

    public void ShowAngleLines() {
        Vector3 limit1 = Vector3.Slerp(transform.forward, transform.right, visionAngle / 180);
        Vector3 limit2 = Vector3.Slerp(transform.forward, -transform.right, visionAngle / 180);
        Debug.DrawRay(transform.position, limit1 * chaseDistance, Color.blue);
        Debug.DrawRay(transform.position, limit2 * chaseDistance, Color.blue);
    }

    public void ShowDestinationLine() {
        Debug.DrawRay(transform.position, navMeshAgent.destination - transform.position, Color.green);
    }

    public bool IsPlayerInAngle(){
        return Vector3.Angle(transform.forward, player.transform.position - transform.position) <= visionAngle / 2f;
    }

    public bool IsPlayerClose() {
        return PlayerDistance() < chaseDistance;
    }

    public void ChargeBullets () {
        bullets = totalBullets;
    }

    public void Shoot() {
        transform.LookAt(player.transform);
        Instantiate(bulletGO, transform.position + transform.forward, Quaternion.identity);
        if (bullets > 0) {
            bullets--;
        }
    }

    public void Stop() {
        navMeshAgent.stoppingDistance = 0f;
        navMeshAgent.SetDestination(this.transform.position);
    }

    public bool isEmpty() {
        return bullets <= 0;
    }

    public void SetFarDestination() {
        NavMeshPath pathE = new NavMeshPath();
        NavMeshPath pathP = new NavMeshPath();
        float diff = 0;
        int iPath = -1;
        for (int i = 0; i < FleePoints.Length; i++) {
            NavMesh.CalculatePath(transform.position, FleePoints[i].transform.position, NavMesh.AllAreas, pathE);
            NavMesh.CalculatePath(player.transform.position, FleePoints[i].transform.position, NavMesh.AllAreas, pathP);
            float newDiff = PathLength(pathP) - PathLength(pathE);
            if (newDiff > diff) {
                diff = newDiff;
                iPath = i;
            }
        }
        if(iPath != -1) {
            NavMesh.CalculatePath(transform.position, FleePoints[iPath].transform.position, NavMesh.AllAreas, pathE);
            for (int i = 0; i < pathE.corners.Length - 1; i++) {
                Debug.DrawLine(pathE.corners[i], pathE.corners[i + 1], Color.red);
            }
            navMeshAgent.SetDestination(FleePoints[iPath].transform.position);
        }

        //Vector3 direction = Vector3.Normalize(transform.position - player.transform.position) * 5f;
        //navMeshAgent.SetDestination(transform.position + direction);
    }

    public void TurnAround() {
        transform.Rotate(new Vector3(0, 2f, 0));
    }

    public float PathLength(NavMeshPath path)
    {
        if (path.corners.Length < 2)
            return 0;

        Vector3 previousCorner = path.corners[0];
        float lengthSoFar = 0.0F;
        int i = 1;
        while (i < path.corners.Length)
        {
            Vector3 currentCorner = path.corners[i];
            lengthSoFar += Vector3.Distance(previousCorner, currentCorner);
            previousCorner = currentCorner;
            i++;
        }
        return lengthSoFar;
    }

}
