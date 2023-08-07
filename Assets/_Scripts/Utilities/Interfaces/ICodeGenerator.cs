namespace _Scripts.Utilities.Interfaces
{
    public interface ICodeGenerator
    {
        /// <summary>
        /// Возвращает true если был сгенерирован код, false если нет
        /// </summary>
        bool GenerateCode();
    }
}
