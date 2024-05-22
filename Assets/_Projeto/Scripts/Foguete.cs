using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foguete : MonoBehaviour
{
    #region VARIAVEIS
    private WaitForSeconds timer = new WaitForSeconds(5);
    private bool temCombustivel = true;
    [SerializeField]
    private Rigidbody rigidbodyCorpo;
    [SerializeField]
    private Rigidbody rigidbodyCauda;
    [SerializeField]
    private GameObject cauda;
    [SerializeField]
    private GameObject paraquedas;
    [SerializeField]
    private ParticleSystem fogoCauda;
    private float alturaMaxima;
    private bool temAlturaMaxima = false;
    #endregion

    #region ONGUI
    GUIStyle style = new GUIStyle();
    int w = Screen.width, h = Screen.height;
    #endregion

    void Start()
    {
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = (int)(h * 0.1f);
        style.normal.textColor = Color.red;

        StartCoroutine(PararCombustivel());
    }

    void FixedUpdate()
    {
        if (temCombustivel)
        {
            rigidbodyCorpo.AddForce(transform.up * 700f * Time.fixedDeltaTime, ForceMode.Force);
            // Combustível está sendo queimado, afinal
            rigidbodyCorpo.mass -= 0.025f * Time.fixedDeltaTime;
        }
        else if(!temAlturaMaxima && rigidbodyCorpo.velocity.y <= 0f)
        {
            alturaMaxima = transform.position.y;
            paraquedas.SetActive(true);
            temAlturaMaxima = true;
        }
        // Vento
        rigidbodyCorpo.AddForce(Vector3.forward * 20f * Time.fixedDeltaTime, ForceMode.Force);
        if (rigidbodyCauda)
        {
            rigidbodyCauda.AddForce(Vector3.forward * 20f * Time.fixedDeltaTime, ForceMode.Force);
        }
    }

    void OnGUI()
    {
        int w = Screen.width, h = Screen.height;
        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        // Se tiver altura maxima, usa ela, caso contrário, usa a posição y
        string text = string.Format("{0:0.00}m de Altura", (temAlturaMaxima ? alturaMaxima : transform.position.y));
        GUI.Label(rect, text, style);
    }

    IEnumerator PararCombustivel()
    {
        yield return timer;
        temCombustivel = false;
        fogoCauda.Stop();
        cauda.transform.SetParent(null);
        rigidbodyCauda = cauda.AddComponent<Rigidbody>();
        // Estimo que cada estágio tenha metade da massa
        rigidbodyCorpo.mass /= 2;
        rigidbodyCauda.mass = rigidbodyCorpo.mass;
    }
}
