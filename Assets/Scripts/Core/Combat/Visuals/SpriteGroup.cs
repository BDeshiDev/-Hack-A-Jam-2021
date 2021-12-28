using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Combat.Visuals
{
    [Serializable]
    public class SpriteGroup
    {
        [SerializeField] List<SpriteRenderer> spriterRenderers;
        public Color Color {
            set
            {
                foreach (var spriterRenderer in spriterRenderers)
                {
                    spriterRenderer.color = value;
                }
            }
        }


    }
}