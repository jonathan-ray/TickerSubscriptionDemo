namespace TickerSubscriptionDemo.Services.Exceptions;

public class UnattachedRpcClientServiceException : Exception
{
    public override string Message => "A method call was attempted on an RPC client service that hasn't been attached.";
}