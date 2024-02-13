interface IListener {}

interface IListener<TInput> : IListener where TInput : IInput
{
    void OnAction();
}
