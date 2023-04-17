using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkWeapon : MonoBehaviour
{
    // ID de l'arme actuelle
    private int weaponID;

    // Liste de nos armes (Objets se trouvant dans la main du personnage)
    [SerializeField]
    public List<GameObject> weaponList = new List<GameObject>();

    // // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount > 0)
        {
            weaponID = gameObject.GetComponentInChildren<ItemOnObject>().item.itemID;
        }
        else
        {
            weaponID = 0;

            for (int i = 0 ; i < weaponList.Count; i++)
            {
                weaponList[i].SetActive(false);
            }
        }
        // WeaponID correspond à l'ID de l'arme dans la base de données (ItemDatabase)
        // i = X correspond à l'ID (ou index) de l'arme dans la liste
        if (weaponID == 1 && transform.childCount > 0)
        {
            for (int i = 0 ; i < weaponList.Count; i++)
            {
                if (i == 0)
                {
                    weaponList[i].SetActive(true);
                }
            }
        }
    }
}
