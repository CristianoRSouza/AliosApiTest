namespace Ailos.Shared.Common.Validators;

public static class CpfValidator
{
    public static bool IsValid(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf)) return false;
        
        cpf = cpf.Replace(".", "").Replace("-", "");
        
        if (cpf.Length != 11) return false;
        if (!cpf.All(char.IsDigit)) return false;
        if (cpf.All(c => c == cpf[0])) return false;

        var sum = 0;
        for (int i = 0; i < 9; i++)
            sum += int.Parse(cpf[i].ToString()) * (10 - i);
        
        var remainder = sum % 11;
        var digit1 = remainder < 2 ? 0 : 11 - remainder;
        
        if (int.Parse(cpf[9].ToString()) != digit1) return false;

        sum = 0;
        for (int i = 0; i < 10; i++)
            sum += int.Parse(cpf[i].ToString()) * (11 - i);
        
        remainder = sum % 11;
        var digit2 = remainder < 2 ? 0 : 11 - remainder;
        
        return int.Parse(cpf[10].ToString()) == digit2;
    }
}
