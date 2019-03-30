using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingManager : MonoBehaviour {

    public class FlockingGroup
    {
        public List<GameObject> flockers;
        public GameObject leader;
        public float repulsionForce;
        public Vector3 center;
        public int nextPosition;

        public FlockingGroup(GameObject _leader, GameObject firstFollower)
        {
            nextPosition = 0;
            leader = _leader;
            flockers = new List<GameObject>();
            flockers.Add(firstFollower);
            repulsionForce = 3.0f;
        }

        public int AddFlocker(GameObject flocker)
        {
            flockers.Add(flocker);
            return ++nextPosition;
        }

        public void ComputeCenter()
        {
            Vector3 theCenter = Vector3.zero;
            theCenter += leader.transform.position;
            foreach(GameObject flocker in flockers)
            {
                theCenter += flocker.transform.position;
            }
            center = theCenter / (flockers.Count + 1);
        }
    }

    private List<FlockingGroup> groups;

	void Start ()
    {
        groups = new List<FlockingGroup>();
	}

    public void AddFlockingGroup(FlockingGroup group)
    {
        groups.Add(group);
    }
}
