using FluentValidation;

namespace Receiver.Validators
{
    public class OrderSubmittedValidator : AbstractValidator<OrderSubmitted>
    {
        public OrderSubmittedValidator()
        {
            RuleFor(x => x.Value).GreaterThan(1);
        }
    }
}
