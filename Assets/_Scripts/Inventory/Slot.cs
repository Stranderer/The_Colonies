using UnityEngine;
using System.Collections;

public class Slot{
	#region private vars
	private int _slotId;
	private int _slotPosX;
	private int _slotPosY;
	private BaseItem _occupyingItem;
	#endregion

	#region constructor
	public Slot(){
	}
	public Slot (int _slotId, int _posX, int _posY)
	{
		this._slotId = _slotId;
		this._slotPosX = _posX;
		this._slotPosY = _posY;
	}
	#endregion

	#region getter
	public int getId(){
		return this._slotId;
	}
	public int getX(){
		return this._slotPosX;
	}
	public int getY(){
		return this._slotPosY;
	}
	public BaseItem getItem(){
		return this._occupyingItem;
	}
	#endregion

	#region setters
	public void setId(int id){
		this._slotId = id;
	}
	public void setX(int x){
		this._slotPosX = x;
	}
	public void setY(int y){
		this._slotPosY = y;
	}
	public void setItem(BaseItem newItem){
		this._occupyingItem = newItem;
	}
	#endregion
}
