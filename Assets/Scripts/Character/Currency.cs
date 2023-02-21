public class Currency : Singleton<Currency>
{
    public int Blood;

    public void ModifyCurrencyAmount(int amount)
    {
        Blood += amount;
    }
}
