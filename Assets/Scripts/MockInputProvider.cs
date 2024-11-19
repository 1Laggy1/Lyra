public class MockInputProvider : IInputProvider
{
    private float horizontal;

    public MockInputProvider(float horizontal)
    {
        this.horizontal = horizontal;
    }

    public float GetHorizontal()
    {
        return horizontal;
    }
}
