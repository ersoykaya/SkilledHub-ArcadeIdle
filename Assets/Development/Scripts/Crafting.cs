using UnityEngine;

public class Crafting : Interactables
{
    public GameObject craftPrefab;
    public Transform craftTransform;
    
    private bool _isCrafted = false;
    protected override void DoAction(GameObject interactingObj)
    {
        base.DoAction(interactingObj);

        if (!_isCrafted)
        {
            GameObject craft = Instantiate(craftPrefab, craftTransform.position, craftTransform.rotation);
            _isCrafted = true;
        }
    }
}
