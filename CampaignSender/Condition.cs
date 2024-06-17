namespace CampaignSender
{

    public abstract class Condition
    {
        public abstract bool Evaluate(Customer customer);
    }

    public class MaleCondition : Condition
    {
        public override bool Evaluate(Customer customer)
        {
            return customer.Gender == "Male";
        }
    }

    public class AgeCondition : Condition
    {
        private readonly int _age;

        public AgeCondition(int age)
        {
            _age = age;
        }

        public override bool Evaluate(Customer customer)
        {
            return customer.Age >= _age;
        }
    }

    public class CityCondition : Condition
    {
        private readonly string _city;

        public CityCondition(string city)
        {
            _city = city;
        }

        public override bool Evaluate(Customer customer)
        {
            return customer.City == _city;
        }
    }

    public class DepositCondition : Condition
    {
        private readonly decimal _deposit;

        public DepositCondition(decimal deposit)
        {
            _deposit = deposit;
        }

        public override bool Evaluate(Customer customer)
        {
            return customer.Deposit >= _deposit;
        }
    }

    public class NewCustomerCondition : Condition
    {
        public override bool Evaluate(Customer customer)
        {
            return customer.NewCustomer == 1;
        }
    }
}
