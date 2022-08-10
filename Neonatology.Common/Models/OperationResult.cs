namespace Neonatology.Common.Models;

public class OperationResult
{
    public bool Succeeded { get; private set; }
    
    public string Error { get; private set; }

    public bool Failed => !this.Succeeded;

    public static implicit operator OperationResult(bool succeeded)
        => new OperationResult() { Succeeded = succeeded };

    public static implicit operator OperationResult(string error)
        => new OperationResult()
        {
            Succeeded = false,
            Error = error,
        };
}