using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BDeshi.Utility.Extensions
{
    public static class Unity
    {
        public static void allignToDir(this Transform transform, Vector2 dir)
        {
            float angle = get2dAngle(dir);
            transform.set2dRotation(angle);
        }

        public static void allignToDir(this Transform transform, Vector2 dir, float angleOffsetInDegrees)
        {
            float angle = get2dAngle(dir) + angleOffsetInDegrees;
            transform.set2dRotation(angle);
        }

        //requires T to be a class and NOT a struct FIX: use.equals which may be slower...

        public static void allignToDir(this Rigidbody2D rb2D, Vector2 dir)
        {
            rb2D.rotation = get2dAngle(dir);
        }

        public static bool exceedSqrDist(this Vector3 vec, float dist)
        {
            return vec.sqrMagnitude > (dist * dist);
        }

        public static void set2dRotation(this Transform transform, float angle)
        {
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        public static void addAngleOffset(this Transform transform, float angleOffset)
        {
            transform.rotation = Quaternion.AngleAxis(transform.rotation.eulerAngles.z + angleOffset, Vector3.forward);
        }

        public static float get2dAngle(this Vector2 dir)
        {
            return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        }

        public static void lookAlongTopDown(this Transform transform, Vector3 dir)
        {
            transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
        }

        public static Vector3 toTopDown(this Vector2 dir)
        {
            return new Vector3(dir.x, 0, dir.y);
        }

        public static Vector2 Rotate(this Vector2 v, float degrees)
        {
            float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
            float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

            float tx = v.x;
            float ty = v.y;
            v.x = (cos * tx) - (sin * ty);
            v.y = (sin * tx) + (cos * ty);
            return v;
        }

        public static bool Contains(this LayerMask mask, int layer)
        {
            return mask == (mask | (1 << layer));
        }

        public static bool Contains(this LayerMask mask, GameObject obj)
        {
            return mask == (mask | (1 << obj.layer));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mask"></param>
        /// <param name="hit">ASSUME THAT the raycast has HIT SOMETHING</param>
        /// <returns></returns>
        public static bool Contains(this LayerMask mask, RaycastHit2D hit)
        {
            return mask == (mask | (1 << hit.collider.gameObject.layer));
        }
        public static bool Contains(this LayerMask mask, RaycastHit hit)
        {
            return mask == (mask | (1 << hit.collider.gameObject.layer));
        }

        public static void reparentAndReset(this Transform transform, Transform parent)
        {
            transform.parent = parent;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }
        
        public static float distanceBetween(this Transform transform, Transform t)
        {
            return (transform.position - t.position).magnitude;
        }

        public static RaycastHit2D raycastFromInsideCollider2D(Vector2 origin, Vector2 direction, float length, LayerMask layer)
        {
            bool usedToHit = Physics2D.queriesStartInColliders;
            Physics2D.queriesStartInColliders = true;
            var result = Physics2D.Raycast(origin, direction, length, layer);
            Physics2D.queriesStartInColliders = usedToHit;

            return result;
        }

        public static Vector2 getRaycastEndpoint2D(Vector2 origin, Vector2 dir, float length, LayerMask layer, out RaycastHit2D hit)
        {
            dir.Normalize();
            hit = raycastFromInsideCollider2D(origin, dir, length, layer);
            return hit ? (hit.point) : (origin + dir * length);
        }


        public static Vector2 multiplyDimensions(this Vector2 v, Vector2 other)
        {
            return new Vector2(v.x * other.x, v.y * other.y);
        }

        public static Vector3 multiplyDimensions(this Vector3 v, Vector3 other)
        {
            return new Vector3(v.x * other.x, v.y * other.y, v.z * v.z);
        }

#if UNITY_EDITOR
        public static void DrawWireCapsule(Vector3 _pos, Quaternion _rot, float _radius, float _height, Color _color = default(Color))
        {
            if (_color != default(Color))
                Handles.color = _color;
            Matrix4x4 angleMatrix = Matrix4x4.TRS(_pos, _rot, Handles.matrix.lossyScale);
            using (new Handles.DrawingScope(angleMatrix))
            {
                var pointOffset = (_height - (_radius * 2)) / 2;

                //draw sideways
                Handles.DrawWireArc(Vector3.up * pointOffset, Vector3.left, Vector3.back, -180, _radius);
                Handles.DrawLine(new Vector3(0, pointOffset, -_radius), new Vector3(0, -pointOffset, -_radius));
                Handles.DrawLine(new Vector3(0, pointOffset, _radius), new Vector3(0, -pointOffset, _radius));
                Handles.DrawWireArc(Vector3.down * pointOffset, Vector3.left, Vector3.back, 180, _radius);
                //draw frontways
                Handles.DrawWireArc(Vector3.up * pointOffset, Vector3.back, Vector3.left, 180, _radius);
                Handles.DrawLine(new Vector3(-_radius, pointOffset, 0), new Vector3(-_radius, -pointOffset, 0));
                Handles.DrawLine(new Vector3(_radius, pointOffset, 0), new Vector3(_radius, -pointOffset, 0));
                Handles.DrawWireArc(Vector3.down * pointOffset, Vector3.back, Vector3.left, -180, _radius);
                //draw center
                Handles.DrawWireDisc(Vector3.up * pointOffset, Vector3.up, _radius);
                Handles.DrawWireDisc(Vector3.down * pointOffset, Vector3.up, _radius);

            }
        }

        public static void DrawPathGizmos(List<Vector3> path)
        {
            for (int i = 1; i < path.Count; i++)
            {
                Gizmos.DrawLine(path[i-1], path[i ]);
            }
        }
#endif
    }
}