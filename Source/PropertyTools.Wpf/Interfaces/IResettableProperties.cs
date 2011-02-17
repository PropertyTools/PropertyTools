namespace PropertyTools.Wpf
{
    public interface IResettableProperties
    {
        object GetResetValue(string propertyName);
    }
}
