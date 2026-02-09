namespace app;

public class Result
{
  public bool IsSuccess { get; }
  public string Error { get; }
  public bool IsFailure => !IsSuccess;

  protected Result(bool isSuccess, string error)
  {
    if (isSuccess && error != string.Empty)
      throw new InvalidOperationException();
    if (!isSuccess && error == string.Empty)
      throw new InvalidOperationException();

    IsSuccess = isSuccess;
    Error = error;
  }

}


public class Result<T> : Result
{
  private readonly T? _value;
  public T Value
  {
    get
    {
      if (!IsSuccess)
        throw new InvalidOperationException();

      return _value!;
    }
  }

  private Result(T? value, bool isSuccess, string error)
      : base(isSuccess, error)
  {
    _value = value;
  }

  public static Result<T> Fail(string message)
  {
    return new Result<T>(default, false, message);
  }
  public static Result<T> Ok(T value)
  {
    return new Result<T>(value, true, string.Empty);
  }
}

