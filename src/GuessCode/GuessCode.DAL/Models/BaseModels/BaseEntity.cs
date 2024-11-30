namespace GuessCode.DAL.Models.BaseModels;

public abstract class BaseEntity<T>
{
    public T Id { get; set; }
}