namespace PropertyTools.Wpf.Operators
{
    public interface ICustomLocalizableOperator
    {
        /// <summary>
        /// Sets custom localizable operator
        /// </summary>        
        void UseLocalizableOperator(ILocalizableOperator customLocalizableOperator);
    }
}