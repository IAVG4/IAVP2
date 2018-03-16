using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;



public class GameManager : MonoBehaviour {

	public static GameManager instance;
	public enum tipoCasilla {Normal, Embarrada, Bloqueada, Ficha, end_tipoCasilla};
	tipoCasilla [,] tablero = new tipoCasilla[10,10];
    string [,] tagCasillas = new string[10, 10];

    //////////////////////////////////////////////////
    ///                Para el A*                  ///
    //////////////////////////////////////////////////
    public int [,] tablero01 = new int [10,10];
    Vector2 posicionInicial;
    Vector2 posicionFinal;

	List<Vector2> roja;
	List<Vector2> verde;
	List<Vector2> azul;
    //////////////////////////////////////////////////
    ///                                            ///
    //////////////////////////////////////////////////

    public GameObject casilla;
    public Text fichaSelect;

    public Sprite normal;
    public Sprite embarrada;
    public Sprite bloqueada;

    public GameObject fichaRoja;
    public GameObject fichaVerde;
    public GameObject fichaAzul;

    public GameObject flechaRoja;
    public GameObject flechaVerde;
    public GameObject flechaAzul;

    GameObject fichaSeleccionada;
    GameObject flechaSeleccionada;
    GameObject RojaSeleccionada;
    GameObject VerdeSeleccionada;
    GameObject AzulSeleccionada;

    const int MaxNumCasillasEmbarradas = 12, MaxNumCasillasBloqueadas = 10;
	int numCasillasEmbarradas = MaxNumCasillasEmbarradas;
	int numCasillasBloqueadas = MaxNumCasillasBloqueadas;
	// Use this for initialization
	void Start () {
		instance = this;
        fichaSeleccionada = null;
        
        CreaFichas();
		CreaTablero ();

		roja = new List<Vector2>();
		verde = new List<Vector2>();
		azul = new List<Vector2>();

	}
	
	// Update is called once per frame
	void Update () {
        
    }

    public void SetFichaSeleccionada(GameObject ficha) {
		Debug.Log (posicionInicial);
        if (fichaSeleccionada == null)
        {
            fichaSeleccionada = ficha;
            posicionInicial = ficha.GetComponent<Ficha>().GetPositionInMatrix();
            ActualizaTableros(posicionInicial);
            tablero[(int)posicionInicial.x, (int)posicionInicial.y] = tipoCasilla.Ficha;
            switch (fichaSeleccionada.tag)
            {
                case "fichaRoja":
                    fichaSelect.text = "Rojo Seleccionado";
                    fichaSelect.color = Color.red;
                    break;
                case "fichaVerde":
                    fichaSelect.text = "Verde Seleccionado";
                    fichaSelect.color = Color.green;
                    break;
                case "fichaAzul":
                    fichaSelect.text = "Azul Seleccionado";
                    fichaSelect.color = Color.blue;
                    break;
            }
        }
        else
        {
            fichaSeleccionada = ficha;
            posicionInicial = ficha.GetComponent<Ficha>().GetPositionInMatrix();
            ActualizaTableros(posicionInicial);
            tablero[(int)posicionInicial.x, (int)posicionInicial.y] = tipoCasilla.Ficha;
            switch (fichaSeleccionada.tag)
            {
                case "fichaRoja":
                    fichaSeleccionada = RojaSeleccionada;
                    fichaSelect.text = "Rojo Seleccionado";
                    fichaSelect.color = Color.red;
                    break;
                case "fichaVerde":
                    fichaSeleccionada = VerdeSeleccionada;
                    fichaSelect.text = "Verde Seleccionado";
                    fichaSelect.color = Color.green;
                    break;
                case "fichaAzul":
                    fichaSeleccionada = AzulSeleccionada;
                    fichaSelect.text = "Azul Seleccionado";
                    fichaSelect.color = Color.blue;
                    break;
            }

        }
    }

    public void OnClick(GameObject casillaPulsada)
    {
        if (fichaSeleccionada != null)
        {

            if (casillaPulsada.tag == "CasillaBloqueada")
            {
                switch (fichaSeleccionada.tag)
                {
                    case "fichaRoja":
                        Destroy(RojaSeleccionada);
                        break;
                    case "fichaVerde":
                        Destroy(VerdeSeleccionada);
                        break;
                    case "fichaAzul":
                        Destroy(AzulSeleccionada);
                        break;
                }

                fichaSeleccionada = null;

                fichaSelect.text = "Ningun Seleccionado";
                fichaSelect.color = Color.white;
            }

            else
            {

                if (fichaSeleccionada.tag == "fichaRoja")
                {
					if (RojaSeleccionada == null) {
						RojaSeleccionada = Instantiate (flechaRoja, casillaPulsada.transform);
						flechaSeleccionada = RojaSeleccionada;
						posicionFinal = casillaPulsada.GetComponent<Casilla> ().positionInMatrix;
						roja = GetComponent<AStar> ().calculatePath (traducirMatriz (tablero01), traducirVector (posicionInicial), traducirVector (posicionFinal));
						if (roja != null) {
							MoverRoja ();
						} else
							Debug.Log ("No solution!!");
                        
					} 
					else {
						Destroy (RojaSeleccionada);
						RojaSeleccionada = Instantiate(flechaRoja, casillaPulsada.transform);
						flechaSeleccionada = RojaSeleccionada;
						posicionFinal = casillaPulsada.GetComponent<Casilla>().positionInMatrix;
						roja = GetComponent<AStar> ().calculatePath (traducirMatriz(tablero01), traducirVector(posicionInicial), traducirVector(posicionFinal));
						if (roja != null) {
							MoverRoja ();

						}
						else
							Debug.Log("No solution!!");
					}
						  
                }

                else if (fichaSeleccionada.tag == "fichaVerde")
                {
                    if (VerdeSeleccionada == null)
                    {
                        VerdeSeleccionada = Instantiate(flechaVerde, casillaPulsada.transform);
                        flechaSeleccionada = VerdeSeleccionada;
                        posicionFinal = casillaPulsada.GetComponent<Casilla>().positionInMatrix;
						verde = GetComponent<AStar> ().calculatePath (traducirMatriz(tablero01), traducirVector(posicionInicial), traducirVector(posicionFinal));
						if (verde != null) {
							MoverVerde ();
						} else
							Debug.Log ("No solution!!");
                    }
                    else
                    {
						Destroy (VerdeSeleccionada);
						VerdeSeleccionada = Instantiate(flechaVerde, casillaPulsada.transform);
						flechaSeleccionada = VerdeSeleccionada;
						posicionFinal = casillaPulsada.GetComponent<Casilla>().positionInMatrix;
						verde = GetComponent<AStar> ().calculatePath (traducirMatriz(tablero01), traducirVector(posicionInicial), traducirVector(posicionFinal));
						if (verde != null) {
							MoverVerde ();
						} else
							Debug.Log ("No solution!!");

                    }
                }

                else if (fichaSeleccionada.tag == "fichaAzul")
                {
                    if (AzulSeleccionada == null)
                    {
                        AzulSeleccionada = Instantiate(flechaAzul, casillaPulsada.transform);
                        flechaSeleccionada = AzulSeleccionada;
                        posicionFinal = casillaPulsada.GetComponent<Casilla>().positionInMatrix;
						azul = GetComponent<AStar> ().calculatePath (traducirMatriz(tablero01), traducirVector(posicionInicial), traducirVector(posicionFinal));
						if (azul != null) {
							MoverAzul ();
						} else
							Debug.Log ("No solution!!");
                    }
                    else
                    {
						Destroy (AzulSeleccionada);
						AzulSeleccionada = Instantiate(flechaAzul, casillaPulsada.transform);
						flechaSeleccionada = AzulSeleccionada;
						posicionFinal = casillaPulsada.GetComponent<Casilla>().positionInMatrix;
						azul = GetComponent<AStar> ().calculatePath (traducirMatriz(tablero01), traducirVector(posicionInicial), traducirVector(posicionFinal));
						if (azul != null) {
							MoverAzul ();
						} else
							Debug.Log ("No solution!!");
                    }
                }
                fichaSeleccionada = null;
            }
        }

        // Cambio de las casillas de estados
        else if (tablero[(int)casillaPulsada.GetComponent<Casilla>().positionInMatrix.x,
            (int)casillaPulsada.GetComponent<Casilla>().positionInMatrix.y] != tipoCasilla.Ficha)
        {
            casillaPulsada.GetComponent<BoxCollider2D>().enabled = true;
            switch (casillaPulsada.tag)
            {
                case "CasillaNormal":
                    casillaPulsada.tag = "CasillaEmbarrada";
                    tagCasillas[(int)casillaPulsada.GetComponent<Casilla>().positionInMatrix.x,
                        (int)casillaPulsada.GetComponent<Casilla>().positionInMatrix.y] = "CasillaEmbarrada";
                    tablero01[(int)casillaPulsada.GetComponent<Casilla>().positionInMatrix.x,
                        (int)casillaPulsada.GetComponent<Casilla>().positionInMatrix.y] = 2;
                    tablero[(int)casillaPulsada.GetComponent<Casilla>().positionInMatrix.x,
                        (int)casillaPulsada.GetComponent<Casilla>().positionInMatrix.y] = tipoCasilla.Embarrada;
                    casillaPulsada.GetComponent<SpriteRenderer>().sprite = embarrada;
                    break;

                case "CasillaEmbarrada":
                    tablero01[(int)casillaPulsada.GetComponent<Casilla>().positionInMatrix.x,
                        (int)casillaPulsada.GetComponent<Casilla>().positionInMatrix.y] = 0;
                    casillaPulsada.tag = "CasillaBloqueada";
                    tagCasillas[(int)casillaPulsada.GetComponent<Casilla>().positionInMatrix.x,
                        (int)casillaPulsada.GetComponent<Casilla>().positionInMatrix.y] = "CasillaBloqueada";
                    tablero[(int)casillaPulsada.GetComponent<Casilla>().positionInMatrix.x,
                        (int)casillaPulsada.GetComponent<Casilla>().positionInMatrix.y] = tipoCasilla.Bloqueada;
                    casillaPulsada.GetComponent<SpriteRenderer>().sprite = bloqueada;
                    break;

                case "CasillaBloqueada":
                    casillaPulsada.tag = "CasillaNormal";
                    tagCasillas[(int)casillaPulsada.GetComponent<Casilla>().positionInMatrix.x,
                        (int)casillaPulsada.GetComponent<Casilla>().positionInMatrix.y] = "CasillaNormal";
                    tablero01[(int)casillaPulsada.GetComponent<Casilla>().positionInMatrix.x,
                        (int)casillaPulsada.GetComponent<Casilla>().positionInMatrix.y] = 1;
                    tablero[(int)casillaPulsada.GetComponent<Casilla>().positionInMatrix.x,
                        (int)casillaPulsada.GetComponent<Casilla>().positionInMatrix.y] = tipoCasilla.Normal;
                    casillaPulsada.GetComponent<SpriteRenderer>().sprite = normal;
                    break;
            }
        }

        else if (tablero[(int)casillaPulsada.GetComponent<Casilla>().positionInMatrix.x,
            (int)casillaPulsada.GetComponent<Casilla>().positionInMatrix.y] == tipoCasilla.Ficha)
        {
            tablero01[(int)casillaPulsada.GetComponent<Casilla>().positionInMatrix.x,
                        (int)casillaPulsada.GetComponent<Casilla>().positionInMatrix.y] = 0;
            casillaPulsada.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    void CreaFichas()
    {
        int randomIndexCoordX = Random.Range(0, tablero.GetLength(1));
        int randomIndexCoordY = Random.Range(0, tablero.GetLength(1));

        for (int i = 0; i < 3; i++)
        {

            randomIndexCoordX = Random.Range(0, tablero.GetLength(1));
            randomIndexCoordY = Random.Range(0, tablero.GetLength(1));

            while (tablero[randomIndexCoordX, randomIndexCoordY] != tipoCasilla.Ficha)
            {

                switch (i)
                {

                    case 0: //Ficha roja
                        fichaRoja.GetComponent<Ficha>().SetPositionInMatrix(new Vector2(randomIndexCoordX, randomIndexCoordY));
                        fichaRoja.transform.position = new Vector2(randomIndexCoordX, -randomIndexCoordY);
                        Instantiate(fichaRoja, this.transform);
                        tablero[randomIndexCoordX, randomIndexCoordY] = tipoCasilla.Ficha;
                        break;

                    case 1: //Ficha verde
                        fichaVerde.GetComponent<Ficha>().SetPositionInMatrix(new Vector2(randomIndexCoordX, randomIndexCoordY));
                        fichaVerde.transform.position = new Vector2(randomIndexCoordX, -randomIndexCoordY);
                        Instantiate(fichaVerde, this.transform);
                        tablero[randomIndexCoordX, randomIndexCoordY] = tipoCasilla.Ficha;
                        break;

                    case 2: //Ficha azul
                        fichaAzul.GetComponent<Ficha>().SetPositionInMatrix(new Vector2(randomIndexCoordX, randomIndexCoordY));
                        fichaAzul.transform.position = new Vector2(randomIndexCoordX, -randomIndexCoordY);
                        Instantiate(fichaAzul, this.transform);
                        tablero[randomIndexCoordX, randomIndexCoordY] = tipoCasilla.Ficha;
                        break;
                }
            }
        }
    }


    void CreaTablero() {
		int y = 0; //Coordenadas de UNITY
		int cientoUno = tablero.GetLength(0) * tablero.GetLength(0);

		int randomIndex = Random.Range(0, cientoUno);

		for(int filas = 0; filas < tablero.GetLength(0); ++filas) {
			for(int columnas = 0; columnas < tablero.GetLength(1); ++columnas) {

                casilla.name = "Casilla_" + (columnas + (filas * tablero.GetLongLength(1)));
                casilla.GetComponent<BoxCollider2D>().enabled = true;

				randomIndex = Random.Range(0, cientoUno);
				if (randomIndex < cientoUno - MaxNumCasillasEmbarradas - MaxNumCasillasBloqueadas){ //Casilla normal
                    casilla.GetComponent<SpriteRenderer>().sprite = normal;
                    casilla.tag = "CasillaNormal";
                    tagCasillas[columnas, filas] = "CasillaNormal";
					casilla.GetComponent<Casilla> ().positionInMatrix.x = columnas;
					casilla.GetComponent<Casilla> ().positionInMatrix.y = filas;
					casilla.transform.position = new Vector2 (columnas, y);
                    tablero01[columnas, filas] = 1;
                    if (tablero[columnas, filas] != tipoCasilla.Ficha)
                        tablero[columnas, filas] = tipoCasilla.Normal;
                    
                    else
                    {
                        tablero01[columnas, filas] = 0;
                        casilla.GetComponent<BoxCollider2D>().enabled = false;
                    }
					Instantiate (casilla, this.transform);
                    
				}

				else if (randomIndex >= cientoUno - MaxNumCasillasEmbarradas - MaxNumCasillasBloqueadas && randomIndex < cientoUno - MaxNumCasillasBloqueadas) { //Casilla embarrada
					if (numCasillasEmbarradas < 0) {
                        casilla.GetComponent<SpriteRenderer>().sprite = normal;
                        casilla.tag = "CasillaNormal";
                        tagCasillas[columnas, filas] = "CasillaNormal";
						casilla.GetComponent<Casilla> ().positionInMatrix.x = columnas;
						casilla.GetComponent<Casilla> ().positionInMatrix.y = filas;						
						casilla.transform.position = new Vector2 (columnas, y);
                        tablero01[columnas, filas] = 1;
                        if (tablero[columnas, filas] != tipoCasilla.Ficha)
                            tablero[columnas, filas] = tipoCasilla.Normal;
                        else
                        {
                            tablero01[columnas, filas] = 0;
                            casilla.GetComponent<BoxCollider2D>().enabled = false;
                        }
						Instantiate (casilla, this.transform);

                        
					} 
					else {
                        casilla.GetComponent<SpriteRenderer>().sprite = embarrada;
                        casilla.tag = "CasillaEmbarrada";
                        tagCasillas[columnas, filas] = "CasillaEmbarrada";
						casilla.GetComponent<Casilla> ().positionInMatrix.x = columnas;
						casilla.GetComponent<Casilla> ().positionInMatrix.y = filas;
						casilla.transform.position = new Vector2 (columnas, y);
                        tablero01[columnas, filas] = 2;
                        if (tablero[columnas, filas] != tipoCasilla.Ficha)
                            tablero[columnas, filas] = tipoCasilla.Embarrada;
                        else
                        {
                            tablero01[columnas, filas] = 0;
                            casilla.GetComponent<BoxCollider2D>().enabled = false;
                        }
						Instantiate (casilla, this.transform);
                        
						numCasillasEmbarradas--;
					}
				}

				else { //Casilla bloqueada
					if (numCasillasBloqueadas < 0 || tablero[columnas, filas] == tipoCasilla.Ficha) {
                        casilla.GetComponent<SpriteRenderer>().sprite = normal;
                        casilla.tag = "CasillaNormal";
                        tagCasillas[columnas, filas] = "CasillaEmbarrada";
						casilla.GetComponent<Casilla> ().positionInMatrix.x = columnas;
						casilla.GetComponent<Casilla> ().positionInMatrix.y = filas;
						casilla.transform.position = new Vector2 (columnas, y);
                        tablero01[columnas, filas] = 1;
                        if (tablero[columnas, filas] != tipoCasilla.Ficha)
                            tablero[columnas, filas] = tipoCasilla.Normal;
                        else
                        {
                            tablero01[columnas, filas] = 0;
                            casilla.GetComponent<BoxCollider2D>().enabled = false;
                        }
						Instantiate (casilla, this.transform);

                        
					} 
                    
                    else {
                        casilla.GetComponent<SpriteRenderer>().sprite = bloqueada;
                        casilla.tag = "CasillaBloqueada";
                        tagCasillas[columnas, filas] = "CasillaBloqueada";
						casilla.GetComponent<Casilla> ().positionInMatrix.x = columnas;
						casilla.GetComponent<Casilla> ().positionInMatrix.y = filas;
						casilla.transform.position = new Vector2 (columnas, y);
                        tablero01[columnas, filas] = 0;
						Instantiate (casilla, this.transform);

						tablero[columnas, filas] = tipoCasilla.Bloqueada;
						numCasillasBloqueadas--;
					}
				}
                
                
			}
			y = -filas - 1;
		}

	}

	int [,] traducirMatriz(int [,] original){
		int[,] traduccion = new int[original.GetLength (0), original.GetLength (1)];
		for (int i = 0; i < original.GetLength (0); ++i) {
			for (int j = 0; j < original.GetLength (1); ++j) {
				traduccion [j, i] = original [i, j];
			}
		}
		return traduccion;
	}

	void MoverRoja() {
		// En cada paso que da hay que comprobar que no haya pared o ficha y que no sea la siguiente posicion del A*
        if (roja.Count > 0)
        {
            Vector2 siguienteCasilla = traducirVector(roja[0]);
            roja.RemoveAt(0);
            if (siguienteCasilla != (Vector2)fichaRoja.transform.position && tablero01[(int)siguienteCasilla.x, (int)siguienteCasilla.y] == 0)
            {
                roja.Clear();
                Destroy(RojaSeleccionada);
                fichaSeleccionada = null;

                fichaSelect.text = "Ningun Seleccionado";
                fichaSelect.color = Color.white;
            }

            else
            {
				ActualizaTableros(siguienteCasilla);
				fichaRoja.GetComponent<Ficha>().SetNewTransform((int)siguienteCasilla.x, (int)-siguienteCasilla.y, "fichaRoja");
                if (roja.Count == 0)
                {
                    Destroy(RojaSeleccionada);
                    fichaSeleccionada = null;

                    fichaSelect.text = "Ningun Seleccionado";
                    fichaSelect.color = Color.white;
                }

                else
                {
                    Invoke("MoverRoja", tablero01[(int)siguienteCasilla.x, (int)siguienteCasilla.y]);
                }
            }
        }

	}

	void MoverVerde() {
		// En cada paso que da hay que comprobar que no haya pared o ficha y que no sea la siguiente posicion del A*
		if (verde.Count > 0)
		{
			Vector2 siguienteCasilla = traducirVector(verde[0]);
			verde.RemoveAt(0);
			if (siguienteCasilla != (Vector2)fichaVerde.transform.position && tablero01[(int)siguienteCasilla.x, (int)siguienteCasilla.y] == 0)
			{
				verde.Clear();
				Destroy(VerdeSeleccionada);
				fichaSeleccionada = null;

				fichaSelect.text = "Ningun Seleccionado";
				fichaSelect.color = Color.white;
			}

			else
			{
				ActualizaTableros(siguienteCasilla);
				fichaVerde.GetComponent<Ficha>().SetNewTransform((int)siguienteCasilla.x, (int)-siguienteCasilla.y, "fichaVerde");
				if (verde.Count == 0)
				{
					Destroy(VerdeSeleccionada);
					fichaSeleccionada = null;

					fichaSelect.text = "Ningun Seleccionado";
					fichaSelect.color = Color.white;
				}

				else
				{
					Invoke("MoverVerde", tablero01[(int)siguienteCasilla.x, (int)siguienteCasilla.y]);
				}
			}
		}

	}

	void MoverAzul() {
		// En cada paso que da hay que comprobar que no haya pared o ficha y que no sea la siguiente posicion del A*
		if (azul.Count > 0)
		{
			Vector2 siguienteCasilla = traducirVector(azul[0]);
			azul.RemoveAt(0);
			if (siguienteCasilla != (Vector2)fichaAzul.transform.position && tablero01[(int)siguienteCasilla.x, (int)siguienteCasilla.y] == 0)
			{
				azul.Clear();
				Destroy(AzulSeleccionada);
				fichaSeleccionada = null;

				fichaSelect.text = "Ningun Seleccionado";
				fichaSelect.color = Color.white;
			}

			else
			{
				ActualizaTableros(siguienteCasilla);
				fichaAzul.GetComponent<Ficha>().SetNewTransform((int)siguienteCasilla.x, (int)-siguienteCasilla.y, "fichaAzul");
				if (roja.Count == 0)
				{
					Destroy(RojaSeleccionada);
					fichaSeleccionada = null;

					fichaSelect.text = "Ningun Seleccionado";
					fichaSelect.color = Color.white;
				}

				else
				{
					Invoke("MoverAzul", tablero01[(int)siguienteCasilla.x, (int)siguienteCasilla.y]);
				}
			}
		}

	}

    void ActualizaTableros(Vector2 position)
    {
        switch (tagCasillas[(int)position.x, (int)position.y])
        {
            case "CasillaNormal":
                tablero[(int)position.x, (int)position.y] = tipoCasilla.Normal;
                tablero01[(int)position.x, (int)position.y] = 1;
                break;
            case "CasillaEmbarrada":
                tablero[(int)position.x, (int)position.y] = tipoCasilla.Embarrada;
                tablero01[(int)position.x, (int)position.y] = 2;
                break;
            case "CasillaBloqueada":
                tablero[(int)position.x, (int)position.y] = tipoCasilla.Bloqueada;
                tablero01[(int)position.x, (int)position.y] = 0;
                break;
        }
    }

	Vector2 traducirVector(Vector2 vec){
		return new Vector2 (vec.y, vec.x);
	}


	void printResult(List<Vector2> list){
		foreach (Vector2 elem in list)
			Debug.Log (elem);
	}
}
