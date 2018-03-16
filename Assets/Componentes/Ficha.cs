using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ficha : MonoBehaviour {
    public Vector2 positionInMatrix;

    // Use this for initialization
    void Start () {
		
	}

	void OnMouseDown(){
        GameManager.instance.SetFichaSeleccionada(this.gameObject);
    }

    public void SetPositionInMatrix(Vector2 position) {      
        positionInMatrix = position;
    }

    public Vector2 GetPositionInMatrix()
    {
        return positionInMatrix;
    }

	public void SetNewTransform(int x, int y, string tag) {
		GameObject nuevo = GameObject.FindGameObjectWithTag (tag);
		nuevo.transform.position = new Vector3 (x, y, 0);
		SetPositionInMatrix (new Vector2(x, -y));
	}

    
}
