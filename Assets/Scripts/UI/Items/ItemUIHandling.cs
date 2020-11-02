using Assets.Scripts.UI.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUIHandling : MonoBehaviour
{
    public GameObject itemGrid;
    public GameObject itemPrefab;

    async void OnEnable()
    {
        var items = await HamiltonHub.Instance.GetItemInfo();
        foreach (Transform gmTransform in itemGrid.GetComponentsInChildren<Transform>())
        {
            if (gmTransform != itemGrid.transform)
            {
                Destroy(gmTransform.gameObject);
            }
        }

        foreach(var item in items)
        {
            var itemGameObject = Instantiate(itemPrefab, itemGrid.transform);

            var itemModel = itemGameObject.GetComponent<ItemModel>();
            itemModel.Name = item.Name;
            itemModel.Description = item.Description;
        }
    }
}
