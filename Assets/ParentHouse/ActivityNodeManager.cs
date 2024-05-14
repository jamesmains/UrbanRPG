using System.Collections.Generic;
using System.Linq;
using ParentHouse.Utils;
using UnityEngine;

namespace ParentHouse {
    public class ActivityNodeManager : MonoBehaviour {
        // TODO: may need to serialize if the world isn't loaded at all times

        [SerializeField] private List<ActivityTrigger> Activities = new();
        [SerializeField] private float ReactivationTime;
        private List<Node> Nodes = new();
        private float timer;

        private void Start() {
            Nodes.Clear();
            Activities.Clear();

            Activities = GetComponentsInChildren<ActivityTrigger>().ToList();
            foreach (var activity in Activities) {
                var newNode = new Node(activity, ReactivationTime);
                foreach (var activityAction in activity.Activities) {
                    if (activityAction.signature.ActivityType != ActivityType.Consume) continue;
                    activityAction.worldActions.AddListener(delegate { newNode.Deactivate(); });
                    Nodes.Add(newNode);
                }
            }
        }

        private void Update() {
            foreach (var node in Nodes)
                if (node.Timer > 0) {
                    node.Timer -= Time.deltaTime * TimeManager.TimeMultiplier;
                    if (node.Timer <= 0) node.Activate();
                }
        }
    }

    public class Node {
        public ActivityTrigger activityTriggerNode;
        public float ReactivationTime;
        public float Timer;

        public Node(ActivityTrigger activityTrigger, float reactivationTime) {
            activityTriggerNode = activityTrigger;
            ReactivationTime = reactivationTime;
        }

        public void Activate() {
            Timer = -1;
            activityTriggerNode.gameObject.SetActive(true);
        }

        public void Deactivate() {
            Timer = ReactivationTime;
            activityTriggerNode.gameObject.SetActive(false);
        }
    }
}