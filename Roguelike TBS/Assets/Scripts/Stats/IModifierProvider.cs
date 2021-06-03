using System.Collections.Generic;

namespace RPG.Stats {
    public interface IModifierProvider{
        IEnumerable<int> GetAdditiveModifiers(Stat stat);
        IEnumerable<int> GetMultiplicativeModifiers(Stat stat);
    }
}
