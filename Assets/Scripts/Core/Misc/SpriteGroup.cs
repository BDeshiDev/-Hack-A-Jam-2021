using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Misc
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