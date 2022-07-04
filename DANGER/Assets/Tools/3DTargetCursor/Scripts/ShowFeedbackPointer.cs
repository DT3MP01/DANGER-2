using UnityEngine;
using UnityEngine.AI;

namespace Graphics.Feedback.Scripts

{
    public class ShowFeedbackPointer : MonoBehaviour
    {
        private readonly FeedbackPointer _feedbackPointer = new FeedbackPointer();

        [SerializeField]
        private float _feedbackPointerScale = 1f;

        [SerializeField]
        private GameObject _moveToIndicator;

        private CharacterMovment canMove;

        [SerializeField]
        private int _walkableNavMeshMask = -1;

        public void InteractOnPerformed(Vector2 position)
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(position.x, position.y, Camera.main.nearClipPlane));

            const float maxDistance = 300f;
            if (!Physics.Raycast(ray.origin, ray.direction, out RaycastHit hitInfo, maxDistance))
                return;

            NavMesh.SamplePosition(hitInfo.point, out NavMeshHit navHit, maxDistance, _walkableNavMeshMask);

            if (!navHit.hit)
                return;

            _feedbackPointer.ShowPointer(navHit.position);

        }

        private void Start()
        {
            _feedbackPointer.PreparePointer(_moveToIndicator, _feedbackPointerScale);
             canMove = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovment>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && canMove.underControl==true)
                InteractOnPerformed(Input.mousePosition);
        }
    }
}