using UnityEngine;

public class Crafting : Interactables
{
    public GameObject craftPrefab;
    public Transform craftTransform;
    public float requiredPrice;
    
    private bool _isCrafted = false;
    protected override void DoAction(GameObject interactingObj)
    {
        base.DoAction(interactingObj);

        if (!_isCrafted)
        {
            if (EconomyManager.Instance.Purchase(requiredPrice) == true)
            {
                GameObject craft = Instantiate(craftPrefab, craftTransform.position, craftTransform.rotation);
                _isCrafted = true;
                Destroy(gameObject);
            }
        }
    }
}
