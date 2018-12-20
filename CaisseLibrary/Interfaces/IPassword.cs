using System.Collections;

namespace CaisseLibrary.Interfaces
{
    public interface IPassword
    {

        string Generate(int length);

        string GenerateNoDuplicate(int length, IList existing);

    }
}
