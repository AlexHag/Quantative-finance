namespace MarketSim.Core.Exceptions;

[System.Serializable]
public class PriceNotFoundExceptionException : System.Exception
{
    public PriceNotFoundExceptionException() { }
    public PriceNotFoundExceptionException(string message) : base(message) { }
    public PriceNotFoundExceptionException(string message, System.Exception inner) : base(message, inner) { }
    protected PriceNotFoundExceptionException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}