namespace Services.Exceptions
{
    public class ObjectHasBeenModifiedException : CustomBaseException
    {
        public ObjectHasBeenModifiedException(string msg)
            : base(msg)
        {

        }
    }
}