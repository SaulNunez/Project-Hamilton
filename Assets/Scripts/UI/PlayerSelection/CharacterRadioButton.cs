using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Esto es una clase de ayuda.
/// Para evitar todos los problemas de buscar componentes y tener que conocer como esta implementado el prefab.   
/// Esta clase provee facil acceso a todas las cosas que puede buscar los scripts que lo necesiten.
/// </summary>
public class CharacterRadioButton : MonoBehaviour
{
    [SerializeField]
    private Toggle toggle;
    [SerializeField]
    private Text title;
    [SerializeField]
    private Text description;
    [SerializeField]
    private Text statsInfo;


    public Toggle Toggle => toggle;
    public Text CharacterNameText => title;
    public Text DescriptionText => description;
    public Text StatsText => statsInfo;
}
