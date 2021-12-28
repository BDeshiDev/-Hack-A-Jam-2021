using BDeshi.Utility.Extensions;
using UnityEngine;

namespace Core.Combat.Visuals.UI
{
    public class WorldPosTracker : MonoBehaviour
    {
        public Renderer target;
    
        public Vector3 directlyVisibleStateOffset = Vector3.up * 1.85f;
        public float inVisibleStateDist = .5f;
        public Vector3 directlyVisibleStateFacingDir = Vector3.down;
        private Camera cam;

        public Transform directlyVisibleStateHideTarget;
    

        private void Start()
        {
            cam = Camera.main;
        }

        public Vector3 dirToPointBoundedByScreen(Vector3 origin, Vector3 direction)
        {
            var ray = new Ray(origin, direction);

            var currentMinDistance = float.MaxValue;
            var hitPoint = Vector3.zero;
            var planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
            for(var i = 0; i < 4; i++)
            {
                // Raycast against the plane
                if(planes[i].Raycast(ray, out var distance))
                {
                    // Since a plane is mathematical infinite
                    // what you would want is the one that hits with the shortest ray distance
                    if(distance < currentMinDistance)
                    {
                        hitPoint = ray.GetPoint(distance);
                        currentMinDistance = distance;
                    } 
                }
            }

            return hitPoint;
        }

        public void setWorldPos()
        {
            if (target.isVisible)
            {
                directlyVisibleStateHideTarget.gameObject.SetActive(false);
                transform.position = target.transform.position + directlyVisibleStateOffset;
                transform.allignToDir(directlyVisibleStateFacingDir);
            }
            else
            {
                //hackity hack to prevent the other sprite from rotating. 
                directlyVisibleStateHideTarget.gameObject.SetActive(true);

                Vector3 origin = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height)* .5f) ;
                Vector3 dirToTarget = (target.transform.position - origin);
                transform.position = dirToPointBoundedByScreen( origin, dirToTarget) - dirToTarget.normalized * inVisibleStateDist;
                transform.allignToDir(dirToTarget.normalized);
            }
        }

        void Update()
        {
            setWorldPos();
        }

        public void init(Renderer newTarget)
        {
            target = newTarget;
        }
    }
}
