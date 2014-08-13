using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[SerializeField]
public class Trail{
	private int _Id = -1;
	private string _Name = "Default Trail Name";
	private float _Intensity = 10.0f;
	private List<Vector3> _TrailPoints = new List<Vector3>();

	#region Constructors
	/// <summary>
	/// Initializes a new instance of the <see cref="Trail"/> class.
	/// </summary>
	public Trail(){

	}

	/// <summary>
	/// Initializes a new instance of the <see cref="Trail"/> class with id and name.
	/// </summary>
	/// <param name="Id">Unique Id</param>
	/// <param name="Name">Name of the Trail</param>
	public Trail(int Id, string Name){
		this._Id = Id;
		this._Name = Name;
	}
	#endregion

	#region Getter/Setter
	/// <summary>
	/// Gets or sets the unique identifier.
	/// </summary>
	/// <value>The identifier.</value>
	public int Id {
		set{ this._Id = value;}
		get{ return this._Id; }
	}
	/// <summary>
	/// Gets or sets the name.
	/// </summary>
	/// <value>The name.</value>
	public string Name {
		set{ this._Name = value; }
		get{ return this._Name; }
	}
	/// <summary>
	/// Gets or sets the intensity.
	/// </summary>
	/// <value>The intensity.</value>
	public float Intensity {
		set{ this._Intensity = value; }
		get{ return this._Intensity; }
	}
	/// <summary>
	/// Gets or sets the trail points.
	/// </summary>
	/// <value>The trail points.</value>
	/*public ArrayList TrailPoints {
		set{ this._TrailPoints = value; }
		get{ return this._TrailPoints; }
	}*/
	#endregion

	#region Functions
	public void addPoint(Vector3 Point){
		this._TrailPoints.Add(Point);
	}
	public Vector3 getPoint(int Index){
		return this._TrailPoints[Index];
	}
	#endregion

    #region Debug / Test
    public void showPath() {

        //create a sphere
        foreach(Vector3 Point in this._TrailPoints){
            GameObject Sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Sphere.transform.position = Point;
        }
    }
    #endregion
}
