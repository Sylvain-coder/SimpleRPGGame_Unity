using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Control : MonoBehaviour
{
    public GameObject win_text_GO;
    public Text win_text;

    public GameObject defeat_text_GO;

    public Text defeat_text;

    public GameObject player;
    public CharacterMotor characterMotor;
    
    // Start is called before the first frame update
    void Start()
    {
        win_text_GO = GameObject.Find("Wintext");
        win_text = win_text_GO.GetComponent<Text>();

        defeat_text_GO = GameObject.Find("Defeattext");
        defeat_text = defeat_text_GO.GetComponent<Text>();

        player = GameObject.Find("Player");
        characterMotor = player.GetComponent<CharacterMotor>();
    }

    // Update is called once per frame
    void Update()
    {
        // Si le casque a été récupéré (il n'existe plus dans la scène) alors le texte correspondant à 
        // la victoire s'affiche
        if (GameObject.FindWithTag("Helmet") == null)
        {
            win_text.enabled = true;
        }

        // Si le joueur est mort alors le texte correspondant à la défaite s'affiche
        if (characterMotor.isDead)
        {
            defeat_text.enabled = true;
        }
    }

    // Recommence le jeu
    public void ResetTheGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        print("Le bouton \"Recommencer le jeu\" fonctionne correctement.");
    }
}
