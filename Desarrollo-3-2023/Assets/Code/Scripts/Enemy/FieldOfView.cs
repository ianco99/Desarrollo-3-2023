using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code.FOV
{
    public class FieldOfView : MonoBehaviour
    {
        public float viewRadius;
        [Range(0, 360)]
        public float viewAngle;

        public LayerMask targetMask;
        public LayerMask obstacleMask;

        public List<Transform> visibleTargets = new List<Transform>();
        public float meshResolution;

        public MeshFilter viewMeshFilter;
        private Mesh viewMesh;

        public bool searching = false;

        private void Start()
        {
            viewMesh = new Mesh();
            viewMesh.name = "View Mesh";
            viewMeshFilter.mesh = viewMesh;
        }

        public void ToggleFindingTargets(bool searching = false)
        {
            this.searching = searching;


            if (searching)
                StartCoroutine(nameof(FindTargetsWithDelay), 0.1f);
            else
                StopCoroutine(nameof(FindTargetsWithDelay));
        }
        public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
        {
            if (!angleIsGlobal)
            {
                angleInDegrees += transform.eulerAngles.z;
            }

            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0.0f, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }

        IEnumerator FindTargetsWithDelay(float delay)
        {
            while (searching)
            {
                yield return new WaitForSeconds(delay);
                FindVisibleTargets();
            }
        }

        private void LateUpdate()
        {
            DrawFieldOfView();
        }

        private void FindVisibleTargets()
        {
            visibleTargets.Clear();
            Collider2D[] targetsInViewRadius;

            targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, targetMask);

            for (int i = 0; i < targetsInViewRadius.Length; i++)
            {
                Transform target = targetsInViewRadius[i].transform;
                Vector3 dirToTarget = (target.position - transform.position).normalized;

                if (Vector3.Angle(transform.right, dirToTarget) < viewAngle / 2.0f)
                {
                    float distToTarget = Vector3.Distance(transform.position, target.position);

                    if (!Physics2D.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask))
                    {
                        visibleTargets.Add(target);
                    }
                }
            }
        }

        private void DrawFieldOfView()
        {
            int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
            float stepAngleSize = viewAngle / stepCount;
            List<Vector3> viewPoints = new List<Vector3>();


            for (int i = 0; i <= stepCount; i++)
            {
                float angle = transform.eulerAngles.z - viewAngle / 2.0f + stepAngleSize * i;
                ViewCastInfo newViewCast = ViewCast(angle);
                viewPoints.Add(newViewCast.point);
            }

            int vertexCount = viewPoints.Count + 1;
            Vector3[] vertices = new Vector3[vertexCount];
            int[] triangles = new int[(vertexCount-2) * 3];

            vertices[0] = Vector3.zero;

            for (int i = 0; i < vertexCount-1; i++)
            {
                vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

                if(i < vertexCount -2)
                {
                    triangles[i + 3] = 0;
                    triangles[i * 3 + 1] = i + 1;
                    triangles[i * 3 + 2] = i + 2;
                }
            }
            Debug.Log(vertexCount);
            viewMesh.Clear();
            viewMesh.vertices = vertices;
            viewMesh.triangles = triangles;
            viewMesh.RecalculateNormals();
        }

        private ViewCastInfo ViewCast(float globalAngle)
        {
            Vector3 dirFromAngle = DirFromAngle(globalAngle, true);
            Vector3 rotatedDir = Quaternion.Euler(new Vector3(0.0f, transform.eulerAngles.y + 90.0f, transform.eulerAngles.z + 90.0f)) * dirFromAngle;
            RaycastHit2D hit;
            //Debug.DrawLine(transform.position, transform.position + rotatedDir * viewRadius, Color.red);
            Debug.DrawRay(transform.position, rotatedDir, Color.red);
            hit = Physics2D.Raycast(transform.position, rotatedDir, viewRadius, obstacleMask);
            if (hit)
            {
                return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
            }
            else
            {
                return new ViewCastInfo(false, transform.position + rotatedDir * viewRadius, viewRadius, globalAngle);
            }
        }

        public struct ViewCastInfo
        {
            public bool hit;
            public Vector3 point;
            public float dst;
            public float angle;

            public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
            {
                hit = _hit;
                point = _point;
                dst = _dst;
                angle = _angle;
            }
        }
    }
}
