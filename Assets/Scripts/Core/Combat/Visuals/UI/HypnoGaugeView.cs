using Core.Combat.CombatResources;
using UnityEngine;

namespace Core.Combat.Visuals.UI
{
    public class HypnoGaugeView : MonoBehaviour
    {
        public HypnoComponent hypnoComponent;
        public SpriteRenderer hypnoGaugeFillSpriter;
        private MaterialPropertyBlock hypnGaugeFillPropertyBlock;
        private static readonly int FillHeight = Shader.PropertyToID("FillHeight");
        public Color hypnotizedColor;
        public Color normalColor;

        public void updateHypnoGaugeFill(ResourceComponent resourceComponent)
        {
            hypnoGaugeFillSpriter.GetPropertyBlock(hypnGaugeFillPropertyBlock);
            hypnGaugeFillPropertyBlock.SetFloat(FillHeight,resourceComponent.Ratio );

        
            hypnoGaugeFillSpriter.SetPropertyBlock(hypnGaugeFillPropertyBlock);
        }

        private void Awake()
        {   
            hypnoComponent = GetComponentInParent<HypnoComponent>();
            hypnGaugeFillPropertyBlock = new MaterialPropertyBlock();
            hypnoGaugeFillSpriter.color = normalColor;
    
        
            handleHypnosisRecovery(hypnoComponent);
        }

        private void Start()
        {
            updateHypnoGaugeFill(hypnoComponent);
        }

        private void OnEnable()
        {
            if (hypnoComponent != null)
            {
                hypnoComponent.RatioChanged += updateHypnoGaugeFill;
                hypnoComponent.Hypnotized += handleHypnotized;
                hypnoComponent.HypnosisRecovered += handleHypnosisRecovery;
            }
        }

        private void handleHypnosisRecovery(HypnoComponent obj)
        {
            hypnoGaugeFillSpriter.color = normalColor;
        }

        private void handleHypnotized(HypnoComponent obj)
        {
            hypnoGaugeFillSpriter.color = hypnotizedColor;
        }

        private void OnDisable()
        {
            if (hypnoComponent != null)
            {
                hypnoComponent.RatioChanged -= updateHypnoGaugeFill;
                hypnoComponent.Hypnotized -= handleHypnotized;
                hypnoComponent.HypnosisRecovered -= handleHypnosisRecovery;
            }
        }
    }
}
