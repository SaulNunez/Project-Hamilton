using System;

[Serializable]
public class FunctionParameter
{
    public enum ParameterType
    {
        STRING,
        NUMBER
    }

    public ParameterType type;
    public string stringValue;
    public int numberValue;

    public object Value
    {
        get
        {
            switch(type)
            {
                case ParameterType.STRING: return stringValue;
                case ParameterType.NUMBER: return numberValue;
                // Solo para evitar que se enoje el linting
                default: return null;
            };
        }
    }
}
