namespace operation_OLX.CustomValidators
{
    public class ValidUserNameAttribute :ValidationAttribute
    {
        public readonly SecurityServices _SecurityServices;
        public ValidUserNameAttribute(SecurityServices _SecurityServices)
        {
            this._SecurityServices = _SecurityServices;
        }

        public override bool IsValid(object? value)
        {
            
            return _SecurityServices.ValidateUserName(value.ToString()).Result;
        }

    }
}
