namespace PropertyEditorLibrary
{
    public interface IResettableProperties
    {
        object GetResetValue(string propertyName);
    }
}
