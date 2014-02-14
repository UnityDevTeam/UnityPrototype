using System.Collections.Generic;

public class Symbol
{
	private bool _isTerm = false;
	private string _name;
	private Dictionary<string, float> _parameters = new Dictionary<string, float >();


	public bool isTerm
	{
		get {
			return this._isTerm;
		}
	}
	
	public string name
	{
		get {
			return this.name;
		}
	}
	
	public Dictionary<string, float> parameters
	{
		get {
			return this._parameters;
		}
	}

	public void setParameter( string paramName, float paramValue )
	{
		if (_parameters.ContainsKey (paramName))
		{
			_parameters[paramName] = paramValue;
		}
	}

	public void addParameter( string paramName, float paramValue )
	{
		_parameters.Add (paramName, paramValue);
	}

	public void removeParameter( string paramName)
	{
		if (_parameters.ContainsKey (paramName))
		{
			_parameters.Remove(paramName);
		}
	}

	public float getParameter( string paramName)
	{
		if (_parameters.ContainsKey (paramName))
		{
			return _parameters[paramName];
		}

		return 0.0f;
	}
}
