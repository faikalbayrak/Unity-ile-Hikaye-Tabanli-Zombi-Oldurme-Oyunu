using UnityEngine;

public class MuzzleEffect : MonoBehaviour
{
    [SerializeField]
    public Transform muzzleEffect;
    public GameObject effectParticial;
    
    public void Effect()
    {
        GameObject _effect = Instantiate(effectParticial, muzzleEffect);
        _effect.transform.localPosition = new Vector3(0, 0, 0);
        _effect.transform.localRotation = Quaternion.Euler(0, 0, 0);
        Destroy(_effect, .2f);
    }
}
