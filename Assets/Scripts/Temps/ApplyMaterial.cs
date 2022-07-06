using UnityEngine;

namespace Temps
{
    public class ApplyMaterial : MonoBehaviour
    {
        [SerializeField] private Material material;
        public void Apply(Transform pTransfrom)
        {
            var renderer = pTransfrom.GetComponent<MeshRenderer>();

            if (renderer != null)
            {
                renderer.sharedMaterial = material;
            }
        

            int childCount = pTransfrom.childCount;

            for (int i = 0; i < childCount; i++)
            {
                Apply(pTransfrom.GetChild(i));
                
            }
        }
    }
}
