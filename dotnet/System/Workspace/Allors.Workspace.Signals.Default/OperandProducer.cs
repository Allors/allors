namespace Allors.Workspace.Signals.Default;

public class OperandProducer : IProducer
{
    public IOperand Operand { get; }

    public OperandProducer(IOperand operand)
    {
        this.Operand = operand;
    }
}
