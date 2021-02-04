using KTEngine;

namespace Chip
{
    public interface IShootable
    {
        void Hurt(int damage = 0, Entity from = null);
    }
}