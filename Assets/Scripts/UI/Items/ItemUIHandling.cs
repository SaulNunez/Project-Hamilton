using Assets.Scripts.UI.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUIHandling : MonoBehaviour
{
    public GameObject itemGrid;
    public GameObject itemPrefab;

    public static ItemUIHandling instance = null;

    public enum Motive {
        MOVEMENT_INTERACTION,
        DICE_THROW_MODIFICATION
    }

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        } else
        {
            Debug.LogError("Hay más de una instancia de la pantalla de items.");
        }
    }

    public async void OpenItems(Motive motive)
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
